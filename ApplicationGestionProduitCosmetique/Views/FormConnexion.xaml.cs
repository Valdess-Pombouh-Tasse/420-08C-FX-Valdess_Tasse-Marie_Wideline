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

        private bool ValidationFormulaire()
        {
            bool valide = true;

            Reinitialiser();

            if (string.IsNullOrWhiteSpace(txtNom.Text))
            {
                AfficherErreur(txtNom, lblErreurNom, "Le nom est requis.");
                valide = false;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                AfficherErreur(txtPassword, lblErreurPassword, "Le mot de passe est requis.");
                valide = false;
            }

            string roleSelectionne = (cmbTypeUtilisateur.SelectedItem as ComboBoxItem)?.Content.ToString();
            if ((roleSelectionne == "Vendeur" || roleSelectionne == "Manager") && string.IsNullOrWhiteSpace(txtCode.Text))
            {
                AfficherErreur(txtCode, lblErreurCode, "Le code est requis.");
                valide = false;
            }
            return valide;
        }

        private void cmbTypeUtilisateur_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbTypeUtilisateur.SelectedItem is ComboBoxItem selectedItem)
            {
                bool isClient = selectedItem.Content.ToString() == "Client";

                if (txtCode != null) txtCode.Visibility = isClient ? Visibility.Collapsed : Visibility.Visible;
                if (lblCode != null) lblCode.Visibility = isClient ? Visibility.Collapsed : Visibility.Visible;
                if (lblErreurCode != null) lblErreurCode.Visibility = Visibility.Collapsed; 
            }
        }
       

        private void btnConnecter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidationFormulaire()) return;

                string nom = txtNom.Text.Trim();
                string motDePasse = txtPassword.Password.Trim();
                string codeUtilisateur = txtCode.Text.Trim();
                string roleSelectionne = (cmbTypeUtilisateur.SelectedItem as ComboBoxItem)?.Content.ToString();

                if (GestionCompteUtilisateur.ConnexionUtilisateur(nom, motDePasse, codeUtilisateur, out RoleUtilisateur role, out string messageErreur))
                {
                    if (role.ToString() != roleSelectionne)
                    {
                        lblMessageErreur.Text = "Le type d'utilisateur sélectionné ne correspond pas.";
                        lblMessageErreur.Visibility = Visibility.Visible;
                        return;
                    }

                    int idVendeur = 1;
                    Window? form = role switch
                    {
                        RoleUtilisateur.Client => new FormApplication(),
                        RoleUtilisateur.Vendeur => new FormVendeur(idVendeur),
                        RoleUtilisateur.Manager => new FormUtilisateur(),
                        _ => null
                    };

                    form.WindowState = WindowState.Maximized;
                    form?.Show();
                    this.Close();
                }
                else
                {
                    lblMessageErreur.Text = "Nom ou mot de passe invalide.";
                    lblMessageErreur.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                lblMessageErreur.Text = "Une erreur inattendue est survenue : " + ex.Message;
                lblMessageErreur.Visibility = Visibility.Visible;
            }
        }

        private void AfficherErreur(Control control, TextBlock lblErreur, string message)
        {
            control.BorderBrush = Brushes.Red;
            control.BorderThickness = new Thickness(2);
            lblErreur.Text = message;
            lblErreur.Visibility = Visibility.Visible;
        }

        private void Reinitialiser()
        {
            ReinitialiserChamp(txtNom, lblErreurNom);
            ReinitialiserChamp(txtPassword, lblErreurPassword);
            ReinitialiserChamp(txtCode, lblErreurCode);
            lblMessageErreur.Visibility = Visibility.Collapsed;
        }

        private void ReinitialiserChamp(Control control, TextBlock lblErreur)
        {
            control.BorderBrush = Brushes.Gray;
            control.BorderThickness = new Thickness(1);
            lblErreur.Visibility = Visibility.Collapsed;
        }
    }
}
