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

        /// <summary>
        /// Permet d'ajouter un ajouter un 
        /// </summary>
        /// <param name="vendeur"></param>
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

        ///// <summary>
        ///// Méthode de connexion d'un utilisateur (client, vendeur ou manager).
        ///// </summary>
        //public static bool ConnexionUtilisateur(string codeUtilisateur, string motDePasse, out string role)
        //{
        //    role = string.Empty;

        //    using (MySqlConnection cn = CreerConnection())
        //    {
        //        cn.Open();

        //        string requete = "SELECT MotDePasse, Role FROM utilisateurs WHERE CodeUtilisateur = @code";
        //        using (MySqlCommand cmd = new MySqlCommand(requete, cn))
        //        {
        //            cmd.Parameters.AddWithValue("@code", codeUtilisateur);

        //            using (var reader = cmd.ExecuteReader())
        //            {
        //                if (reader.Read())
        //                {
        //                    string motDePasseHashEnBase = reader.GetString("MotDePasse");
        //                    role = reader.GetString("Role");

        //                    string hashSaisi = HasherMotDePasse(motDePasse);

        //                    if (hashSaisi == motDePasseHashEnBase)
        //                    {
        //                        return true;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return false;
        //}



        //public static bool ConnexionUtilisateur(string nomdeUtilisateur, string motDePasse, out string role, out string messageErreur)
        //{
        //    role = string.Empty;
        //    messageErreur = "Nom d'utilisateur ou mot de passe incorrect.";

        //    try
        //    {
        //        using (MySqlConnection cn = CreerConnection())
        //        {
        //            cn.Open();

        //            // Vérifier si l'utilisateur existe
        //            string requete = "SELECT NomUtilisateur, MotDePasse, Role FROM utilisateurs WHERE MotDePasse = @motDePasse";
        //            using (MySqlCommand cmd = new MySqlCommand(requete, cn))
        //            {
        //                cmd.Parameters.Add("@motDePasse", MySqlDbType.VarChar).Value = motDePasse;

        //                using (var reader = cmd.ExecuteReader())
        //                {
        //                    if (reader.Read()) // L'utilisateur existe
        //                    {
        //                        string nomUtilisateurEnBase = reader["NomUtilisateur"] as string ?? string.Empty;
        //                        string motDePasseEnBase = reader["MotDePasse"] as string ?? string.Empty;
        //                        role = reader["Role"] as string ?? string.Empty;

        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        messageErreur = "Erreur de connexion : " + ex.Message;
        //    }

        //    return false;
        //}

        /// <summary>
        /// Méthode de connexion d'un utilisateur (client, vendeur ou manager).
        /// </summary>
        /// <param name="nomUtilisateur"></param>
        /// <param name="motDePasse"></param>
        /// <param name="role"></param>
        /// <param name="messageErreur"></param>
        /// <returns></returns>
        public static bool ConnexionUtilisateur(string nomUtilisateur, string motDePasse, out string role, out string messageErreur)
        {
            role = string.Empty;
            messageErreur = "Nom d'utilisateur ou mot de passe incorrect.";

            try
            {
                using (MySqlConnection cn = CreerConnection())
                {
                    cn.Open();

                    string requete = "SELECT NomUtilisateur, MotDePasse, Role FROM utilisateurs WHERE NomUtilisateur = @nom";
                    using (MySqlCommand cmd = new MySqlCommand(requete, cn))
                    {
                        cmd.Parameters.AddWithValue("@nom", nomUtilisateur);

                        using (var reader = cmd.ExecuteReader())
                        {
                            // Vérifie si un utilisateur avec ce nom existe
                            if (reader.Read()) 
                            {
                                string motDePasseEnBase = reader["MotDePasse"] as string ?? string.Empty;
                                role = reader["Role"] as string ?? string.Empty;

                                // Vérification du mot de passe
                                if (motDePasse == motDePasseEnBase)
                                {
                                    return true; 
                                }
                               
                            }
                       
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                messageErreur = "Erreur de connexion : " + ex.Message;
            }

            return false;
        }


        public static bool ConnexionVendeur(string nomUtilisateur, string motDePasse, string codeVendeur, out string role, out string messageErreur)
        {
            role = string.Empty;
            messageErreur = "Nom d'utilisateur ou mot de passe incorrect.";

            using (MySqlConnection cn = CreerConnection())
            {
                cn.Open();

                string requete = "SELECT MotDePasse, Role, Code FROM utilisateurs WHERE NomUtilisateur = @nom AND Role = 'Vendeur'";
                using (MySqlCommand cmd = new MySqlCommand(requete, cn))
                {
                    cmd.Parameters.AddWithValue("@nom", nomUtilisateur);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read()) // Vérifie si un vendeur a été trouvé
                        {
                            string motDePasseEnBase = reader.GetString("MotDePasse");
                            string codeVendeurEnBase = reader.GetString("Code"); // Récupération du code vendeur en base
                            role = reader.GetString("Role");

                            // Vérification du mot de passe et du code vendeur
                            if (motDePasse == motDePasseEnBase && codeVendeur == codeVendeurEnBase)
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
