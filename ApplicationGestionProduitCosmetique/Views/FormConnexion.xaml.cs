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
            if (string.IsNullOrWhiteSpace(txtPassword.Password) )
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
            string nom = txtNom.Text.Trim();
            string motDePasse = txtPassword.Password.Trim();

            try
            {
                if (GestionCompteUtilisateur.ConnexionUtilisateur(nom, motDePasse, out RoleUtilisateur role, out string messageErreur))
                {
                    MessageBox.Show($"Connexion réussie ! Rôle : {role}", "Succès");

                    FormApplication formApp = new FormApplication();
                    formApp.Show();
                    this.Close();
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

        /// <summary>
        /// Méthode appelée lors du clic sur le bouton "Se connecter en tant que vendeur"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// * @author : Valdess
        private void btnConnecterVendeur_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValidationFormulaire())
                {
                    string nom = txtNom.Text.Trim();
                    string motDePasse = txtPassword.Password.Trim();
                    string codeUtilisateur = txtCode.Text.Trim(); // Manquait dans l'appel

                    if (GestionCompteUtilisateur.ConnexionVendeur(nom, motDePasse, codeUtilisateur, out string role, out string messageErreur))
                    {
                        // Connexion réussie → Ouvrir la FormApplication
                        MessageBox.Show("Connexion réussie !", "Succès");
                        FormApplication formApp = new FormApplication();
                        formApp.Show();
                        this.Close();
                    }
                    else
                    {
                        // Afficher l'erreur spécifique (nom incorrect ou mot de passe incorrect)
                        MessageBox.Show(messageErreur, "Erreur de connexion");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur est survenue : " + ex.Message, "Erreur");
            }
        }

    }
}
