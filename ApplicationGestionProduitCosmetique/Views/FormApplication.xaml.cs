using ApplicationGestionProduitCosmetique.Classes;
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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Orientation = System.Windows.Controls.Orientation;

namespace ApplicationGestionProduitCosmetique.Views
{
    /// <summary>
    /// Logique d'interaction pour FormApplication.xaml
    /// </summary>
    public partial class FormApplication : Window
    {
        List<Produit> _produits;

        public List<Produit> Produits
        {
            get { return _produits; }
            set { _produits = value; }
        }

        public FormApplication()
        {
            InitializeComponent();

            List<Categorie> listCategories = GestionCompteUtilisateur.ObtenirListeCategories();
            listCategories.Add(new Categorie(0, "Toutes"));
            cboCategories.ItemsSource = listCategories;
            cboCategories.SelectedIndex = listCategories.Count - 1;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AfficherListeProduit();
        }

        /// <summary>
        /// Fonction qui permet d'afficher la liste des produits
        /// </summary>
        private void AfficherListeProduit(List<Produit> produits = null)
        {
            List<Produit> listeProduits = new List<Produit>();

            if (produits == null)
            {
                listeProduits = GestionCompteUtilisateur.ObtenirListeProduits();
            }
            else
            {
                listeProduits = produits;
            }

            wpProduit.Children.Clear();
            lblTitre.Content = $"Liste des ({listeProduits.Count}) produits ";

            foreach (Produit produit in listeProduits)
            {
                Border border = new Border();
                border.BorderBrush = new SolidColorBrush(Colors.LightGray);
                border.BorderThickness = new Thickness(2);
                border.CornerRadius = new CornerRadius(10);
                border.Margin = new Thickness(5);
                border.Padding = new Thickness(1);
                border.Width = 100;
                border.Height = 250;

                BitmapImage biProduit = new BitmapImage();
                biProduit.BeginInit();
                biProduit.UriSource = new Uri(produit.Image);
                biProduit.CacheOption = BitmapCacheOption.OnLoad;
                biProduit.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                biProduit.EndInit();

                Image imgProduit = new Image();
                imgProduit.Source = biProduit;
                imgProduit.Margin = new Thickness(2);
                imgProduit.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                imgProduit.VerticalAlignment = VerticalAlignment.Top;
                imgProduit.Width = 90;
                imgProduit.Height = 100;
                imgProduit.Tag = produit;
                //imgProduit.MouseLeftButtonDown += new MouseButtonEventHandler(ImageProduit_MouseLeftButtonUp);

                TextBlock descriptionProduit = new TextBlock();
                imgProduit.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                descriptionProduit.Text = produit.Nom;
                descriptionProduit.FontSize = 12;
                descriptionProduit.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                descriptionProduit.TextWrapping = TextWrapping.Wrap;
                descriptionProduit.MaxWidth = 85;

                TextBlock prixProduit = new TextBlock();
                prixProduit.Text = produit.Prix.ToString("c");
                prixProduit.FontSize = 13;
                prixProduit.Foreground = new SolidColorBrush(Colors.Green);
                prixProduit.FontWeight = FontWeights.Bold;
                prixProduit.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                prixProduit.VerticalAlignment = VerticalAlignment.Bottom;
                prixProduit.MaxWidth = 80;
                prixProduit.Margin = new Thickness(4);


                StackPanel buttonPanel = new StackPanel();
                buttonPanel.Orientation = Orientation.Horizontal;
                buttonPanel.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                buttonPanel.Margin = new Thickness(0, 10, 0, 0);

                StackPanel stackPanel = new StackPanel();
                stackPanel.Orientation = Orientation.Vertical;
                stackPanel.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                stackPanel.VerticalAlignment = VerticalAlignment.Top;

                stackPanel.Children.Add(imgProduit);
                stackPanel.Children.Add(descriptionProduit);
                stackPanel.Children.Add(prixProduit);
                stackPanel.Children.Add(buttonPanel);

                border.Child = stackPanel;
                wpProduit.Children.Add(border);
            }
        }
        /// <summary>
        /// Permet de diminuer la taille de la fenetre de l'application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMinFenetrer_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Permet d'augmenter la taille de la fenetre de l'application au max
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMaxFenetrer_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
                btnMaxFenetrer.Content = "2";
            }
            else
            {
                this.WindowState = WindowState.Normal;
                btnMaxFenetrer.Content = "1";
            }
        }

        /// <summary>
        /// Permet de fermer l'application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFermer_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

       
    }
}
