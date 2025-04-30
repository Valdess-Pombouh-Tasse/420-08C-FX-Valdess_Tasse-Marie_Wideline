using ApplicationGestionProduitCosmetique.Classes;
using ApplicationGestionProduitCosmetique.enums;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ApplicationGestionProduitCosmetique.Views
{
    public partial class FormCreationcompte : UserControl
    {
        public FormCreationcompte()
        {
            InitializeComponent();
        }

        private void btnCreerCompte_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidationFormulaire()) return;

                string nom = txtNom.Text.Trim();
                string motDePasse = txtPassword.Password.Trim();
                string code = txtCode.Text.Trim();
                string typeUtilisateur = (cmbTypeUtilisateur.SelectedItem as ComboBoxItem)?.Content.ToString();

                RoleUtilisateur role = (RoleUtilisateur)Enum.Parse(typeof(RoleUtilisateur), typeUtilisateur);

                if (GestionCompteUtilisateur.CreerCompte(nom, motDePasse, code, role, out string messageErreur))
                {
                    lblMessage.Text = "Compte créé avec succès !";
                    lblMessage.Foreground = Brushes.Green;
                    lblMessage.Visibility = Visibility.Visible;
                    Reinitialiser();
                }
                else
                {
                    lblMessage.Text = messageErreur;
                    lblMessage.Foreground = Brushes.Red;
                    lblMessage.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Erreur inattendue : " + ex.Message;
                lblMessage.Foreground = Brushes.Red;
                lblMessage.Visibility = Visibility.Visible;
            }
        }

        private bool ValidationFormulaire()
        {
            bool valide = true;
            ReinitialiserErreurs();

            if (string.IsNullOrWhiteSpace(txtNom.Text))
            {
                AfficherErreur(lblErreurNom, "Le nom est requis.");
                valide = false;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                AfficherErreur(lblErreurPassword, "Le mot de passe est requis.");
                valide = false;
            }

            if (txtPassword.Password != txtConfirmPassword.Password)
            {
                AfficherErreur(lblErreurConfirmPassword, "Les mots de passe ne correspondent pas.");
                valide = false;
            }

            string roleSelectionne = (cmbTypeUtilisateur.SelectedItem as ComboBoxItem)?.Content.ToString();
            if ((roleSelectionne == "Vendeur" || roleSelectionne == "Manager") && string.IsNullOrWhiteSpace(txtCode.Text))
            {
                AfficherErreur(lblErreurCode, "Le code est requis pour ce type d'utilisateur.");
                valide = false;
            }

            return valide;
        }

        private void AfficherErreur(TextBlock lblErreur, string message)
        {
            lblErreur.Text = message;
            lblErreur.Visibility = Visibility.Visible;
        }

        private void ReinitialiserErreurs()
        {
            lblErreurNom.Visibility = Visibility.Collapsed;
            lblErreurPassword.Visibility = Visibility.Collapsed;
            lblErreurConfirmPassword.Visibility = Visibility.Collapsed;
            lblErreurCode.Visibility = Visibility.Collapsed;
            lblMessage.Visibility = Visibility.Collapsed;
        }

        private void Reinitialiser()
        {
            txtNom.Text = "";
            txtPassword.Password = "";
            txtConfirmPassword.Password = "";
            txtCode.Text = "";
            cmbTypeUtilisateur.SelectedIndex = 0;
        }
    }
}
