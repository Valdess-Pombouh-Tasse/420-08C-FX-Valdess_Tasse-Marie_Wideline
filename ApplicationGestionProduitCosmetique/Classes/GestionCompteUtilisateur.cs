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

        /// <summary>
        /// Permet de créer une connection
        /// </summary>
        /// <returns>Connection sql</returns>
        public static MySqlConnection CreerConnection()
        {
            string connectionString = _config.GetConnectionString(CONNECTION_STRING);
            return new MySqlConnection(connectionString);
        }

        /// <summary>
        /// Constructeur static permettant de charger les configurations de l'application
        /// </summary>
        static GestionCompteUtilisateur()
        {
            _config = new ConfigurationBuilder().AddJsonFile(APPSETTINGS_FILE, false, true).Build();
        }

        public static List<Utilisateur> ChargerUtilisateurs()
        {
            List<Utilisateur> utilisateurs = new List<Utilisateur>();

            MySqlConnection cn = CreerConnection();

            cn.Open();

            string requete = "SELECT CodeUtilisateur, NomUtilisateur, Role FROM utilisateurs";

            MySqlCommand cmd = new MySqlCommand(requete, cn);
            MySqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                string codeUtilisateur = dr.GetString(0);  
                string nomUtilisateur = dr.GetString(1);   
                string role = dr.GetString(2);   

                Utilisateur utilisateur;

                if (role == "Client")
                {
                    utilisateur = new Client(nomUtilisateur, codeUtilisateur, role);
                }
                else if (role == "Vendeur")
                {
                    utilisateur = new Vendeur(nomUtilisateur, codeUtilisateur, role);
                }
                else if (role == "Manager")
                {
                    utilisateur = new Manager(nomUtilisateur, codeUtilisateur, role);
                }
                else
                {
                    throw new InvalidOperationException("Type utilisateur inconnu : " + role);
                }

                utilisateurs.Add(utilisateur);
            }

            dr.Close();
            cn.Close();

            return utilisateurs;
        }


        /// <summary>
        /// Permet d'ajouter un ajouter un 
        /// </summary>
        /// <param name="utilisateur"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AjouterUtilisateur(Utilisateur utilisateur)
        {
            MySqlConnection cn = CreerConnection();

            try
            {
                cn.Open();
                // Ajout des informations du vendeur
                string requeteUtilisateur = $"INSERT INTO vendeurs (CodeUtilisateur,Nom,Role) " +
                 $"VALUES(@code,@nom,@role)";

                MySqlCommand cmdFacture = new MySqlCommand(requeteUtilisateur, cn);
                cmdFacture.Parameters.AddWithValue("@code", utilisateur.CodeUtilisateur);
                cmdFacture.Parameters.AddWithValue("@nom", utilisateur.NomUtilisateur);
                //cmdFacture.Parameters.AddWithValue("@role", utilisateur.Role);
               
                cmdFacture.ExecuteNonQuery();

                // Fermeture de la connection
                cn.Close();

            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de l'ajout du vendeur", ex);
            }
            finally
            {
                if (cn is not null && cn.State == ConnectionState.Open)
                    cn.Close();
            }

        }
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

        public static void ModifierUtilisateur(Utilisateur utilisateur)
        {
            // Création de la connexion
            MySqlConnection cn = CreerConnection();

            try
            {
                // Ouverture de la connexion
                cn.Open();

                // Requête de mise à jour
                string requete = "UPDATE utilisateurs SET Nom = @nom, Role = @role WHERE CodeUtilisateur = @code";
                MySqlCommand cmd = new MySqlCommand(requete, cn);

                // Ajout des paramètres
                cmd.Parameters.AddWithValue("@nom", utilisateur.NomUtilisateur);
                cmd.Parameters.AddWithValue("@role", utilisateur.Role.ToString());
                cmd.Parameters.AddWithValue("@code", utilisateur.CodeUtilisateur);

                // Exécution de la commande
                int rowsAffected = cmd.ExecuteNonQuery();

                // Vérification si la mise à jour a affecté des lignes
                if (rowsAffected == 0)
                {
                    throw new Exception("Aucun utilisateur trouvé avec le CodeUtilisateur spécifié.");
                }
            }
            catch (Exception ex)
            {
                // Gestion des exceptions
                throw new Exception("Erreur lors de la modification de l'utilisateur", ex);
            }
            finally
            {
                // Fermeture de la connexion si elle est ouverte
                if (cn != null && cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }
        }
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
                if (cn != null && cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }
        }
        /// <summary>
        /// Méthode de connexion d'un utilisateur (client, vendeur ou manager).
        /// </summary>
        public static bool ConnexionUtilisateur(string codeUtilisateur, string motDePasse, out string role)
        {
            role = string.Empty;

            using (MySqlConnection cn = CreerConnection())
            {
                cn.Open();

                string requete = "SELECT MotDePasse, Role FROM utilisateurs WHERE CodeUtilisateur = @code";
                using (MySqlCommand cmd = new MySqlCommand(requete, cn))
                {
                    cmd.Parameters.AddWithValue("@code", codeUtilisateur);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string motDePasseHashEnBase = reader.GetString("MotDePasse");
                            role = reader.GetString("Role");

                            string hashSaisi = HasherMotDePasse(motDePasse);

                            if (hashSaisi == motDePasseHashEnBase)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
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
