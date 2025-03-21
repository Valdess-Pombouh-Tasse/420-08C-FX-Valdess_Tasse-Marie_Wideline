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
using static ApplicationGestionProduitCosmetique.Views.FormApplication;

namespace ApplicationGestionProduitCosmetique.Views
{
    /// <summary>
    /// Logique d'interaction pour ProduitChoisir.xaml
    /// </summary>
    public partial class ProduitChoisir : Window
    {
        List<Produit> produits = new List<Produit>();
        ListePanier listePanier = SharedResources.MaListe;

        public List<Produit> Produits
        {
            get { return produits; }
            set { produits = value; }
        }
        public ProduitChoisir()
        {
            InitializeComponent();
        }

        /// <summary>
        /// permet de diminuer le nombre d'article de la liste de voeux
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imgItemDiminuer_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //Items elmItem = listePanier.Items[lvListeVoeux.SelectedIndex];
            ////Items elmItem = listePanier.Items[lvListeVoeux.SelectedIndex];
            //elmItem.Quantite--;
            //if (elmItem.Quantite == 0)
            //{
            //    listePanier.RetirerArticle(elmItem);
            //}

            //InitialiserlstArticleChoisir();
        }

        /// <summary>
        /// permet d'initialiser la liste des articles choisir
        /// </summary>
        public void InitialiserlstArticleChoisir()
        {

            //if (listePanier != null)
            //{
            //    lblPrixSansTaxe.Text = $"le prix total de vos Articles: {listePanier.PrixSousTotal.ToString()}$";
            //    lvListeVoeux.Items.Refresh();
            //}
        }

        /// <summary>
        /// permet d'augmenter le nombre d'article de la liste de voeux
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imgItemAugmenter_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //Items elmItem = listeVoeux.Items[lvListeVoeux.SelectedIndex];
            //elmItem.NbItem++;

            //InitialiserlstArticleChoisir();
        }
        /// <summary>
        /// permet de supprimer l'article de la liste de voeux
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imgItemSupprimer_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //Items elmItem = listePanier.Items[lvListeVoeux.SelectedIndex];
            //MessageBoxResult result = MessageBox.Show("Voulez-vous vraiment supprimer cet article?", "Supprimer l'article", MessageBoxButton.YesNo, MessageBoxImage.Question,
            //        MessageBoxResult.No);
            //if (result == MessageBoxResult.Yes)
            //{

            //    listePanier.RetirerArticle(elmItem);
            //    InitialiserlstArticleChoisir();
            //    MessageBox.Show("L'article  été Supprimé", "supprimer film");
            //}
            //else
            //{
            //    MessageBox.Show("la suppression a été annulé");
            //}

        }

        /// <summary>
        private void btnEnreigistrerLst_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //lvListePanier.ItemsSource = panier.Items;
        }
    }
}
