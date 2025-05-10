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
using System.IO;

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

                MySqlCommand cmdInsertFacture = new MySqlCommand(requeteAddFacture, cn); ;
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
        /// <summary>
        /// Autohor Valdess
        /// Permet d'obtenir liste des catégories provenant de la base de données triée par nom croissant
        /// </summary>
        /// <returns>Liste de Categorie</returns>
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

        public static void AjouterProduit(Produit produit)
        {
            if (produit == null)
                throw new ArgumentNullException(nameof(produit), "Le produit ne doit pas être null.");

            MySqlConnection cn = CreerConnection();

            try
            {
                cn.Open();

                // Requête d'insertion dans la table produits
                string requeteProduit = @"INSERT INTO produit (IdProduit, Code, Nom, Prix, Image, IdCategorie, Description, Quantite)
                              VALUES (NULL, @code, @nom, @prix, @image, @categorie, @description, @quantite)";

                // Extension de l'image et génération d'un nom unique
                string extension = System.IO.Path.GetExtension(produit.Image);
                string nomImage = Guid.NewGuid().ToString() + extension;

                // Commande MySQL
                MySqlCommand cmdProduit = new MySqlCommand(requeteProduit, cn);
                cmdProduit.Parameters.AddWithValue("@code", produit.Code);
                cmdProduit.Parameters.AddWithValue("@nom", produit.Nom);
                cmdProduit.Parameters.AddWithValue("@prix", produit.Prix);
                cmdProduit.Parameters.AddWithValue("@image", nomImage);
                cmdProduit.Parameters.AddWithValue("@categorie", produit.Categorie.Id);
                cmdProduit.Parameters.AddWithValue("@description", produit.Description);
                cmdProduit.Parameters.AddWithValue("@quantite", produit.Quantite);

                // Exécution de la commande
                if (cmdProduit.ExecuteNonQuery() > 0)
                {
                    // Copier l'image dans le dossier de stockage
                    File.Copy(produit.Image, _config[IMAGE_PATH] + nomImage);
                }

            }
            catch (Exception ex)
            {
                // Gérer les exceptions si nécessaire
                throw new Exception("Erreur lors de l'ajout du produit.", ex);
            }
            finally
            {
                // Fermer la connexion à la base de données
                FermerConnexion(cn);
            }
        }


        public static void ModifierProduit(Produit produit)
        {
            if (produit == null)
                throw new ArgumentNullException(nameof(produit), "Le produit ne peut pas être null.");

            MySqlConnection cn = CreerConnection();

            try
            {
                cn.Open();

                // Recherche du produit actuel par son Id
                string requeteRech = "SELECT Id, Image FROM produits WHERE Id = @id";

                MySqlCommand cmdRech = new MySqlCommand(requeteRech, cn);
                cmdRech.Parameters.AddWithValue("@id", produit.Id);

                MySqlDataReader dr = cmdRech.ExecuteReader();

                if (!dr.Read())
                {
                    throw new InvalidOperationException("Ce produit n'existe pas dans la base de données.");
                }
                else
                {
                    string imageActuelle = dr.GetString(1);
                    dr.Close();

                    // Requête de mise à jour
                    string requeteUpdate = @"UPDATE produits SET 
                                 Code = @code,
                                 Nom = @nom,
                                 Prix = @prix,
                                 IdCategorie = @categorieId,
                                 Description = @description,
                                 Quantite = @quantite";

                    MySqlCommand cmdUpdate = new MySqlCommand(requeteUpdate, cn);
                    cmdUpdate.Parameters.AddWithValue("@code", produit.Code);
                    cmdUpdate.Parameters.AddWithValue("@nom", produit.Nom);
                    cmdUpdate.Parameters.AddWithValue("@prix", produit.Prix);
                    cmdUpdate.Parameters.AddWithValue("@categorieId", produit.Categorie.Id);
                    cmdUpdate.Parameters.AddWithValue("@description", produit.Description);
                    cmdUpdate.Parameters.AddWithValue("@quantite", produit.Quantite);
                    cmdUpdate.Parameters.AddWithValue("@id", produit.Id);

                    // Si l'image a changé
                    if (produit.Image != (_config[IMAGE_PATH] + imageActuelle))
                    {
                        requeteUpdate += ", Image = @image WHERE Id = @id";

                        string extension = System.IO.Path.GetExtension(produit.Image);
                        string nomImage = Guid.NewGuid().ToString() + extension;

                        cmdUpdate.Parameters.AddWithValue("@image", nomImage);
                        cmdUpdate.CommandText = requeteUpdate;

                        // Exécution de la mise à jour
                        if (cmdUpdate.ExecuteNonQuery() > 0)
                        {
                            // Copier la nouvelle image
                            File.Copy(produit.Image, _config[IMAGE_PATH] + nomImage);
                            // Supprimer l'ancienne image
                            File.Delete(_config[IMAGE_PATH] + imageActuelle);
                        }
                    }
                    else
                    {
                        cmdUpdate.CommandText = requeteUpdate + " WHERE Id = @id";
                        cmdUpdate.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la modification du produit.", ex);
            }
            finally
            {
                FermerConnexion(cn);
            }
        }

        public static bool SupprimerProduit(Produit produit)
        {
            if (produit == null)
                throw new ArgumentNullException(nameof(produit), "Le produit ne doit pas être null.");

            MySqlConnection cn = CreerConnection();

            try
            {
                cn.Open();

                // Vérification si le produit est lié à une facture
                string reqVerifProd = @"SELECT COUNT(pf.IdProduit) FROM produitsfactures pf
                             INNER JOIN produit p ON p.IdProduit = pf.IdProduit
                             WHERE p.IdProduit = @id";

                MySqlCommand cmdVerifProd = new MySqlCommand(reqVerifProd, cn);
                cmdVerifProd.Parameters.AddWithValue("@id", produit.Id);

                if ((Int64)cmdVerifProd.ExecuteScalar() > 0)
                    throw new InvalidOperationException("Le produit existe dans au moins une facture.");

                // Requête de suppression du produit
                string requeteSupression = @"DELETE FROM produit WHERE IdProduit = @id";
                MySqlCommand cmdSupprProduit = new MySqlCommand(requeteSupression, cn);
                cmdSupprProduit.Parameters.AddWithValue("@id", produit.Id);

                // Exécution de la suppression
                if (cmdSupprProduit.ExecuteNonQuery() > 0)
                {
                    // Supprimer l'image du produit
                    File.Delete(produit.Image);
                    return true;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la suppression du produit.", ex);
            }
            finally
            {
                FermerConnexion(cn);
            }

            return false;
        }

    }
}
