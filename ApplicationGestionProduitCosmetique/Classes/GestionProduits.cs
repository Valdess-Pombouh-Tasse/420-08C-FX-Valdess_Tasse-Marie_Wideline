using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGestionProduitCosmetique.Classes
{
    public static class GestionProduits
    {
        private static List<Produit> produits;


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
        static GestionProduits()
        {
            _config = new ConfigurationBuilder().AddJsonFile(APPSETTINGS_FILE, false, true).Build();
        }

        /// <summary>
        /// Permet de charger la liste des produits
        /// </summary>
        public static void ChargerProduits()
        {
          

            using (MySqlConnection cn = CreerConnection())
            {

                cn.Open();

                try
                {
                   
                    string query = "SELECT ID, Code, Nom, Prix, ImagePath, Quantite, IdTeint, IdVendeur, Description FROM Produits";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        produits.Clear();

                        while (reader.Read())
                        {
                            produits.Add(new Produit(
                                id: reader.GetUInt32(0),
                                code: reader.GetString(1),
                                nom: reader.GetString(2),
                                prix: reader.GetDecimal(3),
                                image: reader.IsDBNull(4) ? "" : reader.GetString(4),
                                categorie: null, 
                                idTeint: reader.GetInt32(6),
                                idVendeur: reader.GetInt32(7),
                                description: reader.IsDBNull(8) ? "" : reader.GetString(8),
                                quantite: reader.GetInt32(5)
                            ));
                        }
                    }
                }
                catch (Exception ex)
                {
                    FenetreErreur.Afficher("Erreur lors du chargement des produits : " + ex.Message);
                }
            }
        }
    }

}
