using ApplicationGestionProduitCosmetique.Classes;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using static ApplicationGestionProduitCosmetique.Views.FormApplication;

namespace ApplicationGestionProduitCosmetique.Models
{
    public static class GestionProduits
    {
        static IConfiguration _config;
        public const string APPSETTINGS_FILE = "appsettings.json";
        public static string CONNECTION_STRING = "DefaultConnection";
        public const string IMAGE_PATH = "images:path";

        private static List<Produit> produits;

        /// <summary>
        /// Permet de créer une connection
        /// </summary>
        /// <returns>Connection sql</returns>
        /// * @author Marie
        public static MySqlConnection CreerConnection()
        {
            string connectionString = _config.GetConnectionString(CONNECTION_STRING);
            return new MySqlConnection(connectionString);
        }

        /// <summary>
        /// Permet la fermeture de la connexion si ouverte.
        /// </summary>
        /// * @author Valdess
        private static void FermerConnexion(MySqlConnection cn)
        {
            if (cn.State == System.Data.ConnectionState.Open)
            {
                cn.Close();
            }
        }

    
    

        /// <summary>
        /// Permet d'obtenir un produit à partir de son code
        /// </summary>
        /// <param name="code">Code unique du produit</param>
        /// <returns>Le produit trouvé. Si non null.</returns>
        /// * @author Valdess
        public static Produit ObtenirProduit(string code)
        {
            Produit produit = null;

            MySqlConnection cn = CreerConnection();

            try
            {
                cn.Open();

                string requete = "SELECT produit.IdProduit,produit.Code,produit.Nom,produit.IdCategorie,produit.Prix,produit.Image, categories.NomCategorie FROM produit INNER JOIN categories ON produit.IdCategorie = categories.IDCategorie WHERE produit.Code = @code";

                MySqlCommand cmd = new MySqlCommand(requete, cn);
                cmd.Parameters.AddWithValue("@code", code);

                MySqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    dr.Read();

                    uint idProduit = dr.GetUInt32(0);
                    string codeProduit = dr.GetString(1);
                    string nomProduit = dr.GetString(2);
                    //decimal prix = dr.GetDecimal(3);
                    uint idCategorie = dr.GetUInt32(3);
                    int prixInt = dr.GetInt32(4);
                    decimal prix = (decimal)prixInt;
                    string image = dr.GetString(5);
                    string NomCategorie = dr.GetString(6);
                    string description = dr.GetString(7);
                    int quantite = dr.GetInt32(8);


                    Categorie categorie = new Categorie(idCategorie, NomCategorie);

                    if (categorie != null)
                    {
                        produit = new Produit(idProduit, codeProduit, nomProduit, prix, _config[IMAGE_PATH] + image, categorie, description, quantite);
                    }
                }
                dr.Close();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                FermerConnexion(cn);
            }

            return produit;
        }

        /// <summary>
        /// Permet d'obtenir la liste des produits dans la base de données selon certains critères de recherche.
        /// triés par nom croissant
        /// </summary>
        /// <param name="nomProduit">Partie du nom du produit à chercher</param>
        /// <param name="categorie">Catégorie du produit</param>
        /// <returns>La liste des produits trouvés selon les critères de recherche. Si aucun critère n'est spécifié alors on retourne tous les produits</returns>
        /// <remarks>La liste des produits est toujours triée en ordre croissant de nom.</remarks>
        /// * @author Valdess
        public static List<Produit> ObtenirListeProduits(string nomProduit = "", Categorie categorie = null)
        {
            List<Produit> ListProduits = new List<Produit>();

            MySqlConnection cn = CreerConnection();

            try
            {
                cn.Open();
                string requete = "SELECT p.IdProduit,p.Code,p.Nom,p.IdCategorie,p.Prix,p.Image, c.NomCategorie FROM produit p INNER JOIN categories c ON p.IdCategorie = c.IDCategorie";

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = cn;

                if (!string.IsNullOrWhiteSpace(nomProduit) && categorie != null)
                {
                    requete += " WHERE p.IdCategorie = @idCat AND p.Nom LIKE @nomProd";
                    cmd.Parameters.AddWithValue("@idCat", categorie.Id);
                    cmd.Parameters.AddWithValue("@nomProd", "%" + nomProduit + "%");
                }
                else if (!string.IsNullOrWhiteSpace(nomProduit) && categorie == null)
                {
                    requete += " WHERE p.Nom LIKE @nomProd";
                    cmd.Parameters.AddWithValue("@nomProd", "%" + nomProduit + "%");
                }
                else if (string.IsNullOrWhiteSpace(nomProduit) && categorie != null)
                {
                    requete += " WHERE p.IdCategorie = @idCat";
                    cmd.Parameters.AddWithValue("@idCat", categorie.Id);
                }

                requete += " ORDER BY p.Nom ASC";

                cmd.CommandText = requete;
                MySqlDataReader dr = cmd.ExecuteReader();


                while (dr.Read())
                {
                    uint idProduit = dr.GetUInt32(0);
                    string codeProduit = dr.GetString(1);
                    string nomProd = dr.GetString(2);
                    uint idCategorie = dr.GetUInt32(3);
                    int prixInt = dr.GetInt32(4);
                    decimal prix = (decimal)prixInt;
                    string image = dr.GetString(5);
                    string NomCategorie = dr.GetString(6);
                    string description = dr.GetString(7);
                    int quantite = dr.GetInt32(8);

                    Categorie objCategorie = new Categorie(idCategorie, NomCategorie);
                    if (objCategorie != null)
                    {
                        Produit produit = new Produit(idProduit, codeProduit, nomProd, prix, _config[IMAGE_PATH] + image, objCategorie, description, quantite);
                        ListProduits.Add(produit);
                    }
                }

                dr.Close();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                FermerConnexion(cn);

            }

            return ListProduits;
        }

        /// <summary>
        /// Autohor Valdess
        /// Permet d'obtenir un produit dans la base de données à partir de son identifiant unique
        /// </summary>
        /// <param name="id">Identifiant unique du produit</param>
        /// <returns>Le produit trouvé ou Null si aucun produit trouvé.</returns>
        public static Produit ObtenirProduit(uint id)
        {
            Produit produit = null;

            MySqlConnection cn = CreerConnection();

            try
            {
                cn.Open();
                string requeteProduit = "SELECT p.IdProduit,p.Code,p.Nom,p.IdCategorie,p.Prix,p.Image,c.NomCategorie FROM produit p INNER JOIN categories c ON p.IdCategorie = c.IDCategorie WHERE p.IdProduit = @id";

                MySqlCommand cmdProduit = new MySqlCommand(requeteProduit, cn);
                cmdProduit.Parameters.AddWithValue("@id", id);

                MySqlDataReader dr = cmdProduit.ExecuteReader();

                if (dr.HasRows)
                {
                    dr.Read();

                    uint idProduit = dr.GetUInt32(0);
                    string codeProduit = dr.GetString(1);
                    string nomProduit = dr.GetString(2);
                    uint idCategorie = dr.GetUInt32(3);
                    decimal prix = dr.GetDecimal(4);
                    string image = dr.GetString(5);
                    string NomCategorie = dr.GetString(6);
                    string description = dr.GetString(7);
                    int quantite = dr.GetInt32(8);

                    Categorie categorie = new Categorie(idCategorie, NomCategorie);

                    if (categorie != null)
                    {
                        produit = new Produit(idProduit, codeProduit, nomProduit, prix, _config[IMAGE_PATH] + image, categorie, description, quantite);
                    }
                }
                dr.Close();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                FermerConnexion(cn);
            }
            return produit;
        }

        /// <summary>
        /// Permet de charger la liste des produits d'un vendeur spécifique
        /// </summary>
        /// <param name="idVendeur">Identifiant unique du vendeur</param>
        /// <returns>Liste des produits associés au vendeur</returns>
        /// <remarks>* @author Marie</remarks>
        public static List<Produit> ObtenirProduitsVendeur(int idVendeur)
        {

            MySqlConnection cn = CreerConnection();
            List<Produit> produitsVendeur = new List<Produit>();

            try
            {
                cn.Open();

                string requete = @"
            SELECT p.IdProduit, p.Code, p.Nom, p.IdCategorie, p.Prix, p.Image, 
                   c.NomCategorie, p.Description, p.Quantite
            FROM produit p
            INNER JOIN categories c ON p.IdCategorie = c.IDCategorie
            INNER JOIN utilisateurs u ON p.IdVendeur = u.ID
            WHERE u.ID = @idVendeur";

                MySqlCommand cmd = new MySqlCommand(requete, cn);
                cmd.Parameters.AddWithValue("@idVendeur", idVendeur);
                MySqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    uint idProduit = dr.GetUInt32(0);
                    string codeProduit = dr.GetString(1);
                    string nom = dr.GetString(2);
                    uint idCategorie = dr.GetUInt32(3);
                    decimal prix = dr.GetDecimal(4);
                    string image = dr.GetString(5);
                    string nomCategorie = dr.GetString(6);
                    string description =  dr.GetString(7);
                    int quantite = dr.GetInt32(8);

                    Categorie categorieObj = new Categorie(idCategorie, nomCategorie);
                    Produit produit = new Produit(idProduit, codeProduit, nom, prix, image, categorieObj, description, quantite);
                    produitsVendeur.Add(produit);
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message, "Erreur SQL", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                FermerConnexion(cn);
            }

            return produitsVendeur;
        }

        public static List<ListePanier> ChargerListePanier()
        {
            List<ListePanier> listePanier = new List<ListePanier>();
            List<string> vectReferencePanier = new List<string>();

            MySqlConnection cn = CreerConnection();

            try
            {
                cn.Open();

                // Récupération des références de panier uniques pour l'utilisateur connecté
                string requeteReferences = "SELECT DISTINCT ReferencePanier FROM panier WHERE NomUtilisateur = @nomUtilisateur";
                MySqlCommand cmdReferences = new MySqlCommand(requeteReferences, cn);
                cmdReferences.Parameters.AddWithValue("@nomUtilisateur", accueil.utilisateur.NomUtilisateur);

                MySqlDataReader drReferences = cmdReferences.ExecuteReader();
                while (drReferences.Read())
                {
                    vectReferencePanier.Add(drReferences.GetString(0));
                }
                drReferences.Close();

                // Pour chaque référence de panier, récupérer les produits associés
                foreach (string reference in vectReferencePanier)
                {
                    List<Items> listItems = new List<Items>();

                    string requeteProduits = "SELECT IdProduit, Quantite FROM panier WHERE ReferencePanier = @reference AND NomUtilisateur = @nomUtilisateur";
                    MySqlCommand cmdProduits = new MySqlCommand(requeteProduits, cn);
                    cmdProduits.Parameters.AddWithValue("@reference", reference);
                    cmdProduits.Parameters.AddWithValue("@nomUtilisateur", accueil.utilisateur.NomUtilisateur);

                    MySqlDataReader drProduits = cmdProduits.ExecuteReader();

                    while (drProduits.Read())
                    {
                        int idProduit = drProduits.GetInt32(0);
                        int quantite = drProduits.GetInt32(1);

                        // Récupérer les détails du produit
                        Produit produit = ObtenirProduit((uint)idProduit);

                        if (produit is not null)
                        {
                            Items item = new Items(idProduit, produit.Nom, produit, quantite, accueil.utilisateur.NomUtilisateur);
                            listItems.Add(item);
                        }
                    }
                    drProduits.Close();

                    // Créer la liste de panier avec les articles
                    ListePanier panier = new ListePanier(reference, listItems);
                    listePanier.Add(panier);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la récupération du panier : " + ex.Message);
                throw;
            }
            finally
            {
                FermerConnexion(cn);
            }

            return listePanier;
        }

        public static ListePanier ObtenirPanier(string referencePanier)
        {
            ListePanier panier = null;
            MySqlConnection cn = CreerConnection();
            try
            {
                cn.Open();
                List<Items> listItems = new List<Items>();
                string requeteProduits = "SELECT IdProduit, Quantite FROM panier WHERE ReferencePanier = @reference AND NomUtilisateur = @nomUtilisateur";
                MySqlCommand cmdProduits = new MySqlCommand(requeteProduits, cn);
                cmdProduits.Parameters.AddWithValue("@reference", referencePanier);
                cmdProduits.Parameters.AddWithValue("@nomUtilisateur", accueil.utilisateur.NomUtilisateur);
                MySqlDataReader drProduits = cmdProduits.ExecuteReader();
                while (drProduits.Read())
                {
                    int idProduit = drProduits.GetInt32(0);
                    int quantite = drProduits.GetInt32(1);
                    // Récupérer les détails du produit
                    Produit produit = ObtenirProduit((uint)idProduit);
                    if (produit is not null)
                    {
                        Items item = new Items(idProduit, produit.Nom, produit, quantite, accueil.utilisateur.NomUtilisateur);
                        listItems.Add(item);
                    }
                }
                drProduits.Close();
                // Créer la liste de panier avec les articles
                panier = new ListePanier(referencePanier, listItems);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la récupération du panier : " + ex.Message);
                throw;
            }
            finally
            {
                FermerConnexion(cn);
            }
            return panier;
        }

        public static void AjouterProduitPanier(Items item)
        {
            MySqlConnection cn = CreerConnection();
            try
            {
                cn.Open();
                string requete = "INSERT INTO panier (ReferencePanier, IdProduit, Quantite, NomUtilisateur) VALUES (@reference, @idProduit, @quantite, @nomUtilisateur)";
                MySqlCommand cmd = new MySqlCommand(requete, cn);
                cmd.Parameters.AddWithValue("@reference", item.NomItem);
                cmd.Parameters.AddWithValue("@idProduit", item.IdItems);
                cmd.Parameters.AddWithValue("@quantite", item.NbItem);
                cmd.Parameters.AddWithValue("@nomUtilisateur", item.NomUtilisateurs);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de l'ajout du produit au panier : " + ex.Message);
                throw;
            }
            finally
            {
                FermerConnexion(cn);
            }
        }

        public static void SupprimerProduitPanier(Items item)
        {
            MySqlConnection cn = CreerConnection();
            try
            {
                cn.Open();
                string requete = "DELETE FROM panier WHERE ReferencePanier = @reference AND IdProduit = @idProduit AND NomUtilisateur = @nomUtilisateur";
                MySqlCommand cmd = new MySqlCommand(requete, cn);
                cmd.Parameters.AddWithValue("@reference", item.NomItem);
                cmd.Parameters.AddWithValue("@idProduit", item.IdItems);
                cmd.Parameters.AddWithValue("@nomUtilisateur", item.NomUtilisateurs);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la suppression du produit du panier : " + ex.Message);
                throw;
            }
            finally
            {
                FermerConnexion(cn);
            }
        }

    }
}
