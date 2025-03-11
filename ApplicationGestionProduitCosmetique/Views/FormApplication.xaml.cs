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

            //foreach (Produit produit in listeProduits)
            //{
            //    Border border = new Border();
            //    border.BorderBrush = new SolidColorBrush(Colors.LightGray);
            //    border.BorderThickness = new Thickness(2);
            //    border.CornerRadius = new CornerRadius(10);
            //    border.Margin = new Thickness(5);
            //    border.Padding = new Thickness(1);
            //    border.Width = 100;
            //    border.Height = 250;

            //    BitmapImage biProduit = new BitmapImage();
            //    biProduit.BeginInit();
            //    biProduit.UriSource = new Uri(produit.Image);
            //    biProduit.CacheOption = BitmapCacheOption.OnLoad;
            //    biProduit.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            //    biProduit.EndInit();

            //    Image imgProduit = new Image();
            //    imgProduit.Source = biProduit;
            //    imgProduit.Margin = new Thickness(2);
            //    imgProduit.HorizontalAlignment = HorizontalAlignment.Center;
            //    imgProduit.VerticalAlignment = VerticalAlignment.Top;
            //    imgProduit.Width = 90;
            //    imgProduit.Height = 100;
            //    imgProduit.Tag = produit;
            //    imgProduit.MouseLeftButtonDown += new MouseButtonEventHandler(ImageProduit_MouseLeftButtonUp);

            //    TextBlock descriptionProduit = new TextBlock();
            //    descriptionProduit.Text = produit.Nom;
            //    descriptionProduit.FontSize = 12;
            //    descriptionProduit.HorizontalAlignment = HorizontalAlignment.Center;
            //    descriptionProduit.TextWrapping = TextWrapping.Wrap;
            //    descriptionProduit.MaxWidth = 85;

            //    TextBlock prixProduit = new TextBlock();
            //    prixProduit.Text = produit.Prix.ToString("c");
            //    prixProduit.FontSize = 13;
            //    prixProduit.Foreground = new SolidColorBrush(Colors.Green);
            //    prixProduit.FontWeight = FontWeights.Bold;
            //    prixProduit.HorizontalAlignment = HorizontalAlignment.Right;
            //    prixProduit.VerticalAlignment = VerticalAlignment.Bottom;
            //    prixProduit.MaxWidth = 80;
            //    prixProduit.Margin = new Thickness(4);


            //    StackPanel buttonPanel = new StackPanel();
            //    buttonPanel.Orientation = Orientation.Horizontal;
            //    buttonPanel.HorizontalAlignment = HorizontalAlignment.Right;
            //    buttonPanel.Margin = new Thickness(0, 10, 0, 0);

            //    Button btnModifier = new Button();
            //    btnModifier.Width = 30;
            //    btnModifier.Height = 30;
            //    btnModifier.Margin = new Thickness(5);
            //    btnModifier.ToolTip = "Modifier";
            //    btnModifier.Background = new SolidColorBrush(Colors.White);

            //    Image imgModifier = new Image();
            //    imgModifier.Source = new BitmapImage(new Uri("/Resources/edit.png", UriKind.Relative));
            //    imgModifier.Width = 16;
            //    imgModifier.Height = 16;
            //    btnModifier.Content = imgModifier;
            //    btnModifier.Click += new RoutedEventHandler(btnModifierProduit_Click);
            //    btnModifier.Tag = produit;


            //    Button btnSupprimer = new Button();
            //    btnSupprimer.Width = 30;
            //    btnSupprimer.Height = 30;
            //    btnSupprimer.Margin = new Thickness(5);
            //    btnSupprimer.ToolTip = "Supprimer";
            //    btnSupprimer.Background = new SolidColorBrush(Colors.White);
            //    Image imgSupprimer = new Image();
            //    imgSupprimer.Source = new BitmapImage(new Uri("/Resources/delete.png", UriKind.Relative));
            //    imgSupprimer.Width = 16;
            //    imgSupprimer.Height = 16;

            //    btnSupprimer.Content = imgSupprimer;
            //    btnSupprimer.Click += new RoutedEventHandler(btnSupprimerProduit_Click);
            //    btnSupprimer.Tag = produit;


            //    buttonPanel.Children.Add(btnModifier);
            //    buttonPanel.Children.Add(btnSupprimer);

            //    StackPanel stackPanel = new StackPanel();
            //    stackPanel.Orientation = Orientation.Vertical;
            //    stackPanel.HorizontalAlignment = HorizontalAlignment.Center;
            //    stackPanel.VerticalAlignment = VerticalAlignment.Top;

            //    stackPanel.Children.Add(imgProduit);
            //    stackPanel.Children.Add(descriptionProduit);
            //    stackPanel.Children.Add(prixProduit);
            //    stackPanel.Children.Add(buttonPanel);

            //    border.Child = stackPanel;

            //    wpProduit.Children.Add(border);
            //}
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
