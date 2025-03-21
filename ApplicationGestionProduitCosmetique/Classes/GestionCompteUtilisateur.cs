using ApplicationGestionProduitCosmetique.enums;
using ApplicationGestionProduitCosmetique.Models;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ApplicationGestionProduitCosmetique.Classes
{
    public static class GestionCompteUtilisateur
    {
        static IConfiguration _config;
        public const string APPSETTINGS_FILE = "appsettings.json";
        public static string CONNECTION_STRING = "DefaultConnection";
        public const string IMAGE_PATH = "images:path";
        


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
        /// Constructeur static permettant de charger les configurations de l'application
        /// </summary>
        /// * @author Marie
        static GestionCompteUtilisateur()
        {
            _config = new ConfigurationBuilder().AddJsonFile(APPSETTINGS_FILE, false, true).Build();
        }

        /// <summary>
        /// Permet de charger les utilisateurs (avec le mot de passe)
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// * @author : Marie
        public static List<Utilisateur> ChargerUtilisateurs()
        {
            List<Utilisateur> utilisateurs = new List<Utilisateur>();

            MySqlConnection cn = CreerConnection();

            cn.Open();

            string requete = "SELECT ID, CodeUtilisateur, NomUtilisateur, MotDePasse, Role FROM utilisateurs";
            MySqlCommand cmd = new MySqlCommand(requete, cn);
            MySqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                string codeUtilisateur = dr.GetString(1);
                string nomUtilisateur = dr.GetString(2);
                string motDePasse = dr.GetString(3);
                string roleString = dr.GetString(4);

                if (!Enum.TryParse(roleString, true, out RoleUtilisateur roleEnum))
                {
                    throw new InvalidOperationException($"Type utilisateur inconnu : {roleString}");
                }

                Utilisateur utilisateur;

                switch (roleEnum)
                {
                    case RoleUtilisateur.Client:
                        utilisateur = new Client(codeUtilisateur, nomUtilisateur, motDePasse, roleEnum);
                        break;

                    case RoleUtilisateur.Vendeur:
                        utilisateur = new Vendeur(codeUtilisateur, nomUtilisateur, motDePasse, roleEnum);
                        break;

                    case RoleUtilisateur.Manager:
                        utilisateur = new Manager(codeUtilisateur, nomUtilisateur, motDePasse, roleEnum);
                        break;

                    default:
                        throw new InvalidOperationException($"Type utilisateur inconnu : {roleEnum}");
                }

                utilisateurs.Add(utilisateur);
            }

            dr.Close();
            FermerConnexion(cn);

            return utilisateurs;
        }

        public static Utilisateur UtilisateurConnecte { get; set; }

        /// <summary>
        /// Permet d'ajouter un ajouter un 
        /// </summary>
        /// <param name="utilisateur"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// * @author Marie
        public static void AjouterUtilisateur(Utilisateur utilisateur)
        {
            MySqlConnection cn = CreerConnection();

            try
            {
                cn.Open();
                // Ajout des informations de l'utilisateur
                string requeteUtilisateur = $"INSERT INTO utilisateurs (CodeUtilisateur,NomUtilisateur,MotDePasse,Role) " +
                 $"VALUES(@code,@nom,@motDePasse,@role)";

                MySqlCommand cmdFacture = new MySqlCommand(requeteUtilisateur, cn);
                cmdFacture.Parameters.AddWithValue("@code", utilisateur.CodeUtilisateur);
                cmdFacture.Parameters.AddWithValue("@nom", utilisateur.NomUtilisateur);
                cmdFacture.Parameters.AddWithValue("@motDePasse", utilisateur.MotDePasse);
                cmdFacture.Parameters.AddWithValue("@role", utilisateur.Role.ToString());

                cmdFacture.ExecuteNonQuery();

                FermerConnexion(cn);

            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de l'ajout du vendeur", ex);
            }
            finally
            {
                FermerConnexion(cn);
            }

        }

        /// <summary>
        /// Permet de vérifier si un utilisateur existe
        /// </summary>
        /// <param name="codeUtilisateur"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        /// * @author Marie
        public static bool UtilisateurExiste(int codeUtilisateur)
        {
            using (MySqlConnection cn = CreerConnection())
            {
                try
                {
                    cn.Open();
                    string requete = "SELECT COUNT(*) FROM utilisateurs WHERE IdUtilisateur = @id";
                    using (MySqlCommand cmd = new MySqlCommand(requete, cn))
                    {
                        cmd.Parameters.AddWithValue("@id", codeUtilisateur);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Erreur lors de la vérification de l'utilisateur : " + ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// Permet de modifier un utilisateur
        /// </summary>
        /// <param name="utilisateur"></param>
        /// <exception cref="Exception"></exception>
        /// * @author Marie
        public static void ModifierUtilisateur(Utilisateur utilisateur)
        {

            // Création de la connexion
            MySqlConnection cn = CreerConnection();

            try
            {
                // Ouverture de la connexion
                cn.Open();

                // Requête de mise à jour
                string requete = "UPDATE utilisateurs SET NomUtilisateur = @nom, Role = @role, MotDePasse = @motDePasse WHERE CodeUtilisateur = @code";
                MySqlCommand cmd = new MySqlCommand(requete, cn);

                // Ajout des paramètres
                cmd.Parameters.AddWithValue("@code", utilisateur.CodeUtilisateur);
                cmd.Parameters.AddWithValue("@nom", utilisateur.NomUtilisateur);
                cmd.Parameters.AddWithValue("@motDePasse", utilisateur.MotDePasse);
                cmd.Parameters.AddWithValue("@role", utilisateur.Role.ToString());



                // Exécution de la commande
                int ligneAffectee = cmd.ExecuteNonQuery();

                // Vérification si la mise à jour a affecté des lignes
                if (ligneAffectee == 0)
                {
                    throw new Exception($"Aucun utilisateur trouvé avec le CodeUtilisateur spécifié.{utilisateur.CodeUtilisateur}");
                }
            }
            catch (Exception ex)
            {
                // Gestion des exceptions
                throw new Exception("Erreur lors de la modification de l'utilisateur", ex);
            }
            finally
            {
                FermerConnexion(cn);
            }
        }

        /// <summary>
        /// Permet de supprimer un utilisateur
        /// </summary>
        /// <param name="codeUtilisateur"></param>
        /// <exception cref="Exception"></exception>
        /// * @author Marie
        public static void SupprimerUtilisateur(string codeUtilisateur)
        {
            MySqlConnection cn = CreerConnection();

            try
            {
                cn.Open();

                string requete = "DELETE FROM utilisateurs WHERE CodeUtilisateur = @code";
                MySqlCommand cmd = new MySqlCommand(requete, cn);

                cmd.Parameters.AddWithValue("@code", codeUtilisateur);

                int ligne = cmd.ExecuteNonQuery();

                if (ligne == 0)
                {
                    throw new Exception("Aucun utilisateur trouvé avec le CodeUtilisateur spécifié.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la suppression de l'utilisateur", ex);
            }
            finally
            {
                FermerConnexion(cn);
            }
        }
        /// <summary>
        /// Méthode de connexion d'un utilisateur (client, vendeur ou manager).
        /// </summary>
        /// <param name="nomUtilisateur"></param>
        /// <param name="motDePasse"></param>
        /// <param name="role"></param>
        /// <param name="messageErreur"></param>
        /// <returns></returns>
        public static bool ConnexionUtilisateur(string nom, string motDePasse, string? codeUtilisateur, out RoleUtilisateur role, out string messageErreur)
        {
            messageErreur = "";
            role = RoleUtilisateur.Client;

            try
            {
                using (MySqlConnection connection = CreerConnection())
                {
                    connection.Open();

                    string requeteConnection = "SELECT Role FROM Utilisateurs WHERE NomUtilisateur = @Nom AND MotDePasse = @MotDePasse";

                    if (!string.IsNullOrEmpty(codeUtilisateur))
                    {
                        requeteConnection += " AND CodeUtilisateur = @Code";
                    }

                    using (MySqlCommand command = new MySqlCommand(requeteConnection, connection))
                    {
                        command.Parameters.AddWithValue("@Nom", nom);
                        command.Parameters.AddWithValue("@MotDePasse", motDePasse);

                        if (!string.IsNullOrEmpty(codeUtilisateur))
                        {
                            command.Parameters.AddWithValue("@Code", codeUtilisateur);
                        }

                        string result = command.ExecuteScalar() as string;
                        if (result != null)
                        {
                            switch (result)
                            {
                                case "Vendeur":
                                    role = RoleUtilisateur.Vendeur;
                                    break;
                                case "Manager":
                                    role = RoleUtilisateur.Manager;
                                    break;
                                default:
                                    role = RoleUtilisateur.Client;
                                    break;
                            }
                            return true;
                        }
                        else
                        {
                            messageErreur = "Nom ou mot de passe incorrect.";
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                messageErreur = "Erreur de connexion à la base : " + ex.Message;
                return false;
            }
        }

    
        /// <summary>
        /// Permet d'obtenir liste des catégories provenant de la base de données triée par nom croissant
        /// </summary>
        /// <returns>Liste de Categorie</returns>
        /// * @author Valdess
        public static List<Categorie> ObtenirListeCategories()
        {
            List<Categorie> lstCategorie = new List<Categorie>();

            MySqlConnection cn = CreerConnection();

            try
            {
                cn.Open();
                string requete = $"SELECT IDCategorie, NomCategorie FROM categories ORDER BY NomCategorie";
                MySqlCommand cmdCategorie = new MySqlCommand(requete, cn);

                MySqlDataReader rdCategorie = cmdCategorie.ExecuteReader();

                while (rdCategorie.Read())
                {
                    Categorie categorie = new Categorie(rdCategorie.GetUInt32(0), rdCategorie.GetString(1));
                    lstCategorie.Add(categorie);
                }
                rdCategorie.Close();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                FermerConnexion(cn);

            }
            return lstCategorie;
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

                string requete = @"
                        SELECT p.IdProduit, p.Code, p.Nom, p.Prix, p.Image, p.IdCategorie, 
                               c.NomCategorie, p.Description, p.Quantite
                        FROM produit p
                        INNER JOIN categories c ON p.IdCategorie = c.IDCategorie
                        WHERE p.Code = @code";

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
                string requete = "SELECT p.IdProduit,p.Code,p.Nom,p.IdCategorie,p.Prix,p.Image, c.NomCategorie, p.Description, p.Quantite FROM produit p INNER JOIN categories c ON p.IdCategorie = c.IDCategorie";

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
                        Produit produit = new Produit(idProduit, codeProduit, nomProd, prix, _config[IMAGE_PATH] + image, objCategorie,description, quantite);
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
        /// Hashage sécurisé du mot de passe.
        /// </summary>
        private static string HasherMotDePasse(string motDePasse)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(motDePasse));
                return Convert.ToBase64String(bytes);
            }
        }
    }
}

