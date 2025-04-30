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

namespace ApplicationGestionProduitCosmetique.Views
{
    /// <summary>
    /// Logique d'interaction pour Base.xaml
    /// </summary>
    public partial class Base : Window
    {

        List<Produit> produits;

        public List<Produit> Produits
        {
            get { return produits; }
            set { produits = value; }
        }
        public Base()
        {
            InitializeComponent();
            AfficherUserControl(new FormApplication());
        }

        public void AfficherUserControl(UserControl control)
        {
            MainContentGrid.Children.Clear();
            MainContentGrid.Children.Add(control);
        }
        
        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            AfficherUserControl(new FormApplication());
        }

        private void Factures_Checked(object sender, RoutedEventArgs e)
        {
            AfficherUserControl(new FormPanier(Produits));
        }

        private void ArticlesChoisis_Checked(object sender, RoutedEventArgs e)
        {
            AfficherUserControl(new ArticleCommandes());
        }

        private void ArticlesCommandes_Checked(object sender, RoutedEventArgs e)
        {
            AfficherUserControl(new ArticleCommandes());
        }

        private void Deconnexion_Click(object sender, RoutedEventArgs e)
        {
            AfficherUserControl(new FormConnexion());
        }
    }
}
