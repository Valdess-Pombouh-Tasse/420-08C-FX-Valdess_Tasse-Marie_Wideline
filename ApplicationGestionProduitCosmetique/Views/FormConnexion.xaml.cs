using ApplicationGestionProduitCosmetique.Classes;
using ApplicationGestionProduitCosmetique.enums;
using ApplicationGestionProduitCosmetique.Models;
using ApplicationGestionProduitCosmetique.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ApplicationGestionProduitCosmetique
{
    /// <summary>
    /// Logique d'interaction pour FormConnexion.xaml
    /// </summary>
    public partial class FormConnexion : Window
    {
        public FormConnexion()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Méthode de validation du formulaire
        /// </summary>
        /// <returns></returns>
        /// * @author Valdess
        private bool ValidationFormulaire()
        {
            StringBuilder sb = new StringBuilder();

            if (string.IsNullOrWhiteSpace(txtNom.Text))
                sb.AppendLine("Le nom ne peut pas être vide");
            if (string.IsNullOrWhiteSpace(txtPassword.Password))
                sb.AppendLine($"Le mot de passe ne peut pas être vide et doit avoir au moins {Client.PASSWORD_MIN_LENGTH}");

            if (sb.Length > 0)
            {
                MessageBox.Show(sb.ToString(), "Validation du client");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Méthode appelée lors du clic sur le bouton "Se connecter"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///  @author : Valdess
        //private void btnConnecter_Click(object sender, RoutedEventArgs e)
        //{
        //    // Validation des champs
        //    string nom = txtNom.Text.Trim();
        //    string motDePasse = txtPassword.Password.Trim();
        //    try
        //    {
        //        // Vérification de la connexion
        //        if (GestionCompteUtilisateur.ConnexionUtilisateur(nom, motDePasse))
        //        {
        //            MessageBox.Show("Connexion réussie !", "Succès");
        //            FormApplication formApp = new FormApplication();
        //            formApp.Show();
        //            this.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Une erreur est survenue : " + ex.Message, "Erreur");
        //    }
        //}
        private void btnConnecter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string nom = txtNom.Text.Trim();
                string motDePasse = txtPassword.Password.Trim();
                string codeUtilisateur = txtCode.Text.Trim();

                if (GestionCompteUtilisateur.ConnexionUtilisateur(nom, motDePasse, codeUtilisateur, out RoleUtilisateur role, out string messageErreur))
                {
                    MessageBox.Show($"Connexion réussie ! Rôle : {role}", "Succès");

                    if (role == RoleUtilisateur.Vendeur)
                    {
                        FormApplication formApp = new FormApplication();
                        formApp.Show();
                    }
                    else if (role == RoleUtilisateur.Manager)
                    {
                        FormUtilisateur formUtilisateur = new FormUtilisateur();
                        formUtilisateur.Show();
                    }

                    this.Close(); // Ferme la fenêtre de connexion
                }
                else
                {
                    MessageBox.Show(messageErreur, "Erreur de connexion");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur est survenue : " + ex.Message, "Erreur");
            }
        }
    }

    
}
