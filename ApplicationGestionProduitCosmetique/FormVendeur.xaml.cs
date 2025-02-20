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

        public FormVendeur(Vendeur vendeur)
        {
            InitializeComponent();
        }

        public void AfficherVendeur()
        {
            if (Vendeur != null)
            {
                txtId.Text = Vendeur.Id.ToString();
                txtNom.Text = Vendeur.Nom;
                txtPrenom.Text = Vendeur.Prenom;
                dpDate.Text = Vendeur.DateInscription.ToString();
;            }

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AfficherVendeur();
        }
    }
}
