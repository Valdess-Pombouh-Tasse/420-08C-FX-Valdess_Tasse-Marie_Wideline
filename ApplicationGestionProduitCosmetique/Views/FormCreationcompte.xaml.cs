using ApplicationGestionProduitCosmetique.Classes;
using ApplicationGestionProduitCosmetique.enums;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Enum = System.Enum;

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
            MainWindow? mainWindow = Window.GetWindow(this) as MainWindow;

            if (mainWindow != null)
            {
                mainWindow.monContentControl.Content = new FormConnexion();
                mainWindow.txtCreerCompte.Text = "Vous avez déjà un compte ?";
                mainWindow.btnCreation.Visibility = Visibility.Hidden;
                mainWindow.btnCreation.Width = 0;
                mainWindow.btnConnexion.Visibility = Visibility.Visible;
            }

            try
            {
                if (!ValiderFormulaire()) return;

                string nom = txtNom.Text.Trim();
                string motDePasse = txtPassword.Password.Trim();
                string code = txtCode.Text.Trim();
                string? typeUtilisateur = (cmbTypeUtilisateur.SelectedItem as ComboBoxItem)?.Content.ToString();

                RoleUtilisateur role = (RoleUtilisateur)Enum.Parse(typeof(RoleUtilisateur), typeUtilisateur);

                if (GestionCompteUtilisateur.CreerCompte(nom, motDePasse, role, code, out string messageErreur))
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

        private bool ValiderFormulaire()
        {
            bool valide = true;
            string message = "";
            ReinitialiserErreurs();

            if (string.IsNullOrWhiteSpace(txtNom.Text))
                message = "- Nom d'utilisateur vide\n";
                valide = false;

            if (string.IsNullOrWhiteSpace(txtPassword.Password))
                message = "- Mot de passe vide\n";
                valide = false ;

            if(cmbTypeUtilisateur.SelectedItem == null)
                message = "- Type d'utilisateur non sélectionné\n";
            string? roleSelectionne = (cmbTypeUtilisateur.SelectedItem as ComboBoxItem)?.Content.ToString();
            if ((roleSelectionne == "Vendeur" || roleSelectionne == "Manager") && string.IsNullOrWhiteSpace(txtCode.Text))
            {
                AfficherErreur(txtCode, lblErreurCode, "Le code est requis.");
                valide = false;
            }

            if (message != "")
            {
                MessageBox.Show("Des informations sont manquantes: \n" + message);
                return false;
            }
            return true;
        }

        private void AfficherErreur(Control control, TextBlock lblErreur, string message)
        {
            control.BorderBrush = Brushes.Red;
            control.BorderThickness = new Thickness(2);
            lblErreur.Text = message;
            lblErreur.Visibility = Visibility.Visible;
        }
        private bool ValidationFormulaire()
        {
            bool valide = true;
            Reinitialiser();

            if (string.IsNullOrWhiteSpace(txtNom.Text))
            {
                AfficherErreur(lblErreurNom, "Nom d'utilisateur requis.");
                valide = false;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                AfficherErreur(lblErreurPassword, "Le mot de passe est requis.");
                valide = false;
            }        

            string? roleSelectionne = (cmbTypeUtilisateur.SelectedItem as ComboBoxItem)?.Content.ToString();
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

        private void cmbTypeUtilisateur_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbTypeUtilisateur.SelectedItem is ComboBoxItem selectedItem)
            {
                bool isClient = selectedItem.Content.ToString() == "Client";

                if (txtCode != null) txtCode.Visibility = isClient ? Visibility.Collapsed : Visibility.Visible;
                if (txtCode != null) txtCode.Visibility = isClient ? Visibility.Collapsed : Visibility.Visible;
                if (lblErreurCode != null) lblErreurCode.Visibility = Visibility.Collapsed;
            }
        }
    }
}
