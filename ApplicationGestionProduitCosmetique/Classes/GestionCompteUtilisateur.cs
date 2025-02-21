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
        /// Permet d'ajouter un vendeur
        /// </summary>
        /// <param name="vendeur"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AjouterVendeur(Vendeur vendeur)
        {
            MySqlConnection cn = CreerConnection();

            try
            {
                cn.Open();
                // Ajout des informations du vendeur
                string requeteVendeur = $"INSERT INTO vendeurs (Nom, Prenom,DateInscription) " +
                 $"VALUES(@nom,@prenom @date)";

                MySqlCommand cmdFacture = new MySqlCommand(requeteVendeur, cn);
                //cmdFacture.Parameters.AddWithValue("@id", vendeur.Id);
                cmdFacture.Parameters.AddWithValue("@nom", vendeur.Nom);
                cmdFacture.Parameters.AddWithValue("@prenom", vendeur.Prenom);
                cmdFacture.Parameters.AddWithValue("@date", vendeur.DateInscription);
               
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
        public static bool VendeurExiste(int idVendeur)
        {
            using (MySqlConnection cn = CreerConnection())
            {
                try
                {
                    cn.Open();
                    string requete = "SELECT COUNT(*) FROM vendeurs WHERE IdVendeur = @id";
                    using (MySqlCommand cmd = new MySqlCommand(requete, cn))
                    {
                        cmd.Parameters.AddWithValue("@id", idVendeur);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0; // Retourne vrai si le vendeur existe déjà
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Erreur lors de la vérification du vendeur : " + ex.Message, ex);
                }
            }
        }

    }
}
