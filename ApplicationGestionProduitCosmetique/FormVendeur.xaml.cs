using ApplicationGestionProduitCosmetique.Models;
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
using static ApplicationGestionProduitCosmetique.Models.Enum;

namespace ApplicationGestionProduitCosmetique
{
    /// <summary>
    /// Logique d'interaction pour FormVendeur.xaml
    /// </summary>
    public partial class FormVendeur : Window
    {
        private Vendeur _vendeur;

        public Vendeur Vendeur
        {
            get { return _vendeur; }
            set { _vendeur = value; }
        }
        /// <summary>
        /// L'état du formulaire
        /// </summary>
        private EtatFormulaire _etat;

        /// <summary>
        /// Obtient ou modifie l'état du formulaire
        /// </summary>
        public EtatFormulaire Etat
        {
            get { return _etat; }
            set { _etat = value; }
        }

        public FormVendeur(EtatFormulaire etat = EtatFormulaire.Ajouter)
        {
            InitializeComponent();
            Etat = etat;
        }

        public void AfficherVendeur()
        {
            if (Vendeur != null)
            {
                txtNom.Text = Vendeur.Nom;
                txtPrenom.Text = Vendeur.Prenom;
                dpDate.Text = Vendeur.DateInscription.ToString();
                ;
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AfficherVendeur();
        }

        private void btnAjouter_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                // Vérification des champs
                if (string.IsNullOrWhiteSpace(txtNom.Text) ||
                    string.IsNullOrWhiteSpace(txtPrenom.Text) ||
                    dpDate.SelectedDate == null)
                {
                    MessageBox.Show("Veuillez remplir tous les champs.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Création du vendeur en utilisant le constructeur
                _vendeur = new Vendeur(
                    txtNom.Text,
                    txtPrenom.Text,
                    dpDate.SelectedDate.Value
                );

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur est survenue : " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnModifier_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSupprimer_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
