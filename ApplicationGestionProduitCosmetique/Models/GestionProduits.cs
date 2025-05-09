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
        
        public const string APPSETTINGS_FILE = "appsettings.json";
        public static string CONNECTION_STRING = "DefaultConnection";
        public const string IMAGE_PATH = "images:path";

        private static List<Produit> produits;

        static IConfiguration _config;
        /// <summary>
        /// Constructeur static permettant de charger les configurations de l'application
        /// </summary>
        static GestionProduits()
        {
            _config = new ConfigurationBuilder().AddJsonFile(APPSETTINGS_FILE, false, true).Build();
        }

        /// <summary>
        /// Permet de créer une connection
        /// </summary>
        /// <returns>Connection sql</returns>
        /// * @author Marie
        public static MySqlConnection CreerConnection()
        {
            string? connectionString = _config.GetConnectionString(CONNECTION_STRING);
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
            Produit? produit = null;

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
        /// Autohor Valdess
        /// </summary>
        /// <param name="idVendeur"></param>
        /// <returns></returns>
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

                if (!dr.HasRows)
                {
                    MessageBox.Show("Aucun produit trouvé en base pour cet ID vendeur.", "DEBUG SQL");
                }

                while (dr.Read())
                {

                    uint idProduit = dr.GetUInt32(0);
                    string codeProduit = dr.GetString(1);
                    string nom = dr.GetString(2);
                    uint idCategorie = dr.GetUInt32(3);
                    decimal prix = dr.GetDecimal(4);
                    string image = dr.GetString(5);
                    string nomCategorie = dr.GetString(6);
                    string description = dr.GetString(7);
                    int quantite = dr.GetInt32(8);

                    Categorie categorieObj = new Categorie(idCategorie, nomCategorie);
                    Produit produit = new Produit(idProduit, codeProduit, nom, prix, image, categorieObj, description, quantite);
                    produitsVendeur.Add(produit);
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur SQL : " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                FermerConnexion(cn);
            }

            return produitsVendeur;
        }


        /// <summary>
        /// Autohor Marie
        /// </summary>
        /// <param name="codeUtilisateur"></param>
        /// <returns></returns>
        public static int ObtenirIdVendeurParCode(string codeUtilisateur)
        {
            MySqlConnection cn = CreerConnection();
            int idVendeur = -1;

            try
            {
                cn.Open();
                string requete = "SELECT ID FROM utilisateurs WHERE Code = @CodeUtilisateur AND Role = 'Vendeur'";
                MySqlCommand cmd = new MySqlCommand(requete, cn);
                cmd.Parameters.AddWithValue("@CodeUtilisateur", codeUtilisateur);

                object? result = cmd.ExecuteScalar();
                if (result != null)
                {
                    idVendeur = Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message, "Erreur SQL", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                FermerConnexion(cn);
            }

            return idVendeur;
        }      

        /// <summary>
        /// Autohor Marie
        /// Permet d'ajouter une facture dans la base de données
        /// </summary>
        /// <param name="facture">facture à ajouter</param>
        /// <exception cref="ArgumentNullException">Lancée si la facture est nulle.</exception>
        /// <exception cref="ArgumentNullException">Lancée si la liste des produitsFacture est nulle ou vide.</exception>
        public static void AjouterFacture(Facture facture)
        {
            if (facture == null)
                throw new ArgumentNullException(nameof(facture), "La facture ne doit pas être nulle.");

            if (facture.ProduitsFacture == null || facture.ProduitsFacture.Count == 0)
                throw new ArgumentNullException(nameof(facture), "Cette facture ne contient aucun produit.");

            MySqlConnection cn = CreerConnection();

            try
            {
                cn.Open();
                string requeteAddFacture = @"INSERT INTO factures 
                       (Id,Date,MontantSousTotal,MontantTPS,MontantTVQ,MontantTotal)
                            VALUES (null,@date,@montantSousTotal,@montantTPS,@montantTVQ,@montantTotal)";

                MySqlCommand cmdInsertFacture = new MySqlCommand(requeteAddFacture, cn);;
                cmdInsertFacture.Parameters.AddWithValue("@montantSousTotal", facture.MontantSousTotal);
                cmdInsertFacture.Parameters.AddWithValue("@montantTPS", facture.MontantTPS);
                cmdInsertFacture.Parameters.AddWithValue("@montantTVQ", facture.MontantTVQ);
                cmdInsertFacture.Parameters.AddWithValue("@montantTotal", facture.MontantTotal);

                if (cmdInsertFacture.ExecuteNonQuery() > 0)
                {
                    uint idLastFacture = (uint)cmdInsertFacture.LastInsertedId;

                    string requeteAjoutProduits = @"INSERT INTO produitsfactures 
                                        (IdFacture, IdProduit,PrixUnitaire,Quantite)
                                            VALUES(@idFacture, @idProduit,@prixUnitaire,@quantite)";

                    MySqlCommand cmdAjoutProduits = new MySqlCommand(requeteAjoutProduits, cn);

                    cmdAjoutProduits.Parameters.Add("@idFacture ", MySqlDbType.UInt32);
                    cmdAjoutProduits.Parameters.Add("@idProduit ", MySqlDbType.UInt32);
                    cmdAjoutProduits.Parameters.Add("@prixUnitaire ", MySqlDbType.Decimal);
                    cmdAjoutProduits.Parameters.Add("@quantite ", MySqlDbType.UInt32);

                    cmdAjoutProduits.Prepare();

                    foreach (ProduitFacture prodF in facture.ProduitsFacture)
                    {
                        cmdAjoutProduits.Parameters["@idFacture"].Value = idLastFacture;
                        cmdAjoutProduits.Parameters["@idProduit"].Value = prodF.Produit.Id;
                        cmdAjoutProduits.Parameters["@prixUnitaire"].Value = prodF.PrixUnitaire;
                        cmdAjoutProduits.Parameters["@quantite"].Value = prodF.Quantite;

                        cmdAjoutProduits.ExecuteNonQuery();
                    }

                    facture.Id = idLastFacture;
                }

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                FermerConnexion(cn);
            }
        }

    }
}
