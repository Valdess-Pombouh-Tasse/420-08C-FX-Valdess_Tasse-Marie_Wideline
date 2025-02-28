using ApplicationGestionProduitCosmetique.Classes;
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

        //private void btnConnecter_Click(object sender, RoutedEventArgs e)
        //{
        //    //try
        //    //{
        //    //    if (ValidationFormulaire())
        //    //    {

        //    //        string nomUtilisateur = txtNom.Text;
        //    //        string motDePasse = txtPassword.Password;
        //    //        Utilisateurs utilisateur = new Client(nomUtilisateur, motDePasse);

        //    //        if (utilisateur.SeConnecter(nomUtilisateur, motDePasse))
        //    //        {
        //    //            MessageBox.Show("Connexion réussie", "Connexion");
        //    //            FormApplication frmApplication = new FormApplication();
        //    //            frmApplication.Show();
        //    //            this.DialogResult = true;
        //    //        }
        //    //        else
        //    //        {
        //    //            MessageBox.Show("Nom ou mot de passe non valide", "Connexion");
        //    //            DialogResult = true;
        //    //        }
        //    //    }
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    MessageBox.Show("Erreur : " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
        //    //}

        //    try
        //    {
        //        if (ValidationFormulaire())
        //        {
        //            string nom = txtNom.Text.Trim();
        //            string motDePasse = txtPassword.Password.Trim();

        //            if (GestionCompteUtilisateur.ConnexionUtilisateur(nom, motDePasse, out string role, out string messageErreur))
        //            {
        //                // Connexion réussie → Ouvrir la FormApplication
        //                MessageBox.Show("Connexion réussie !", "Succès");
        //                FormApplication formApp = new FormApplication();
        //                formApp.Show();
        //                this.Close();
        //            }
        //            else
        //            {
        //                // Afficher l'erreur spécifique (nom incorrect ou mot de passe incorrect)
        //                MessageBox.Show(messageErreur, "Erreur de connexion");
        //            }
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
                // Validation des champs
                string nom = txtNom.Text.Trim();
                string motDePasse = txtPassword.Password.Trim();

                if (string.IsNullOrWhiteSpace(nom))
                    throw new Exception("Veuillez entrer un nom d'utilisateur.");

                if (string.IsNullOrWhiteSpace(motDePasse))
                    throw new Exception("Veuillez entrer un mot de passe.");

                if (!Client.EstMotDePasseValide(motDePasse))
                    throw new Exception("Le mot de passe doit contenir au moins une majuscule, un caractère spécial et 6 caractères.");

                // Vérification de la connexion
                if (GestionCompteUtilisateur.ConnexionUtilisateur(nom, motDePasse, out string role, out string messageErreur))
                {
                    MessageBox.Show("Connexion réussie !", "Succès");
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

        private void btnConnecterVendeur_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValidationFormulaire())
                {
                    string nom = txtNom.Text.Trim();
                    string motDePasse = txtPassword.Password.Trim();
                    string codeUtilisateur = txtCode.Text.Trim();

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
