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
        /// * @author Marie
        public static MySqlConnection CreerConnection()
        {
            string connectionString = _config.GetConnectionString(CONNECTION_STRING);
            return new MySqlConnection(connectionString);
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
        /// Permet de charger les utilisateurs
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// * @author : Marie
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
        /// * @author Marie
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
                if (cn != null && cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
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
        //public static bool ConnexionUtilisateur(string nomUtilisateur, string motDePasse, out string role, out string messageErreur)
        //{
        //    role = string.Empty;
        //    messageErreur = "Nom d'utilisateur ou mot de passe incorrect.";

        //    try
        //    {
        //        using (MySqlConnection cn = CreerConnection())
        //        {
        //            cn.Open();

        //            string requete = "SELECT CodeUTilisateur, NomUtilisateur, MotDePasse, Role FROM utilisateurs WHERE NomUtilisateur = @nom AND MotDePasse = @motDePasse";
        //            using (MySqlCommand cmd = new MySqlCommand(requete, cn))
        //            {
        //                cmd.Parameters.AddWithValue("@nom", nomUtilisateur);
        //                cmd.Parameters.AddWithValue("@motDePasse", motDePasse);

        //                using (var reader = cmd.ExecuteReader())
        //                {
        //                    // Vérifie si un utilisateur avec ce nom existe
        //                    if (reader.Read()) 
        //                    {
        //                        string motDePasseEnBase = reader["MotDePasse"] as string ?? string.Empty;
        //                        string nomUtilisateurEnBase = reader["NomUtilisateur"] as string ?? string.Empty;
        //                        role = reader["Role"] as string ?? string.Empty;

        //                        // Vérification du mot de passe
        //                        if (motDePasse == motDePasseEnBase && nomUtilisateur==nomUtilisateurEnBase)
        //                        {
        //                            return true; 
        //                        }

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

        //public static bool ConnexionUtilisateur(string nomUtilisateur, string motDePasse)
        //{
        //    MySqlConnection cn = CreerConnection();
        //    try

        //    {
        //        cn.Open();

        //        string requete = $"SELECT * FROM utilisateurs WHERE NomUtilisateur = nom AND MotDePasse = motDePasse";
        //        MySqlCommand cmd = new MySqlCommand(requete, cn);
        //        MySqlDataReader dr = cmd.ExecuteReader();
        //        while (dr.Read())
        //        {
        //            if (nomUtilisateur == dr.GetString(3) && motDePasse == dr.GetString(4))
        //            {
        //                Client client = new Client(dr.GetString(1), dr.GetString(2), dr.GetString(3));
        //                return true;
        //            }
        //        }

        //    }

        //    catch (Exception ex)
        //    {
        //        throw new Exception("Erreur lors de la connexion de l'utilisateur", ex);
        //    }
        //    finally
        //    {
        //        if (cn != null && cn.State == ConnectionState.Open)
        //        {
        //            cn.Close();
        //        }
        //    }
        //    return false;
        //}

        public static bool ConnexionUtilisateur(string nomUtilisateur, string motDePasse, out RoleUtilisateur role, out string messageErreur)
        {
            role = RoleUtilisateur.Client; // Valeur par défaut
            messageErreur = "Nom d'utilisateur ou mot de passe incorrect.";

            try
            {
                using (MySqlConnection cn = new MySqlConnection())
                {
                    cn.Open();

                    string requete = "SELECT Role FROM utilisateurs WHERE Nom = @nom AND MotDePasse = @motDePasse";
                    using (MySqlCommand cmd = new MySqlCommand(requete, cn))
                    {
                        cmd.Parameters.AddWithValue("@nom", nomUtilisateur);
                        cmd.Parameters.AddWithValue("@motDePasse", motDePasse);

                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            string roleString = result.ToString();

                            // Vérifier si la valeur du rôle existe dans l'énumération
                            if (Enum.TryParse(roleString, true, out RoleUtilisateur roleEnum))
                            {
                                role = roleEnum;
                                return true;
                            }
                            else
                            {
                                messageErreur = "Rôle inconnu.";
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


        /// <summary>
        /// Méthode de connexion d'un vendeur.
        /// </summary>
        /// <param name="nomUtilisateur"></param>
        /// <param name="motDePasse"></param>
        /// <param name="codeUtilisateur"></param>
        /// <param name="role"></param>
        /// <param name="messageErreur"></param>
        /// <returns></returns>
        public static bool ConnexionVendeur(string nomUtilisateur, string motDePasse, string codeUtilisateur, out string role, out string messageErreur)
        {
            role = string.Empty;
            messageErreur = "Nom d'utilisateur ou mot de passe incorrect.";

            try
            {
                using (MySqlConnection cn = CreerConnection())
                {
                    cn.Open();

                    string requete = "SELECT MotDePasse, Role FROM utilisateurs WHERE NomUtilisateur = @nom AND CodeUtilisateur = @codeUtilisateur";

                    using (MySqlCommand cmd = new MySqlCommand(requete, cn))
                    {
                        cmd.Parameters.AddWithValue("@nom", nomUtilisateur);
                        cmd.Parameters.AddWithValue("@codeUtilisateur", codeUtilisateur);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read()) // Si un utilisateur est trouvé
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
