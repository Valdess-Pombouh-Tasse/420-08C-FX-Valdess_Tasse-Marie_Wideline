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
        public Utilisateur Utilisateur { get; private set; }
        List<Produit> _produits;
        public ListePanier listePanier = new ListePanier();


        public List<Produit> Produits
        {
            get { return _produits; }
            set { _produits = value; }
        }

        public static class accueil
        {
            public static Utilisateur utilisateur { get; set; }
        }
        public FormApplication()
        {
            InitializeComponent();

            List<Categorie> listCategories = GestionCompteUtilisateur.ObtenirListeCategories();
            listCategories.Add(new Categorie(0, "Toutes"));
            cboCategories.ItemsSource = listCategories;
            cboCategories.SelectedIndex = listCategories.Count - 1;

            Utilisateur utilisateur = accueil.utilisateur;
            txtUtilisateurClient.Text = utilisateur.NomUtilisateur;
            this.Utilisateur = utilisateur;
            accueil.utilisateur = utilisateur;
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AfficherListeProduit();
            RetournerListeVoeux();
        }

        public Utilisateur ObtenirUtilisateur()
        {
            return Utilisateur;
        }

        public static class SharedResources
        {
            public static ListePanier MaListe { get; set; }
        }

        public ListePanier RetournerListeVoeux()
        {
            return listePanier;
        }

        /// <summary>
        /// Fonction qui permet d'afficher la liste des produits
        /// </summary>
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
                border.Height = 280; // Augmenté pour inclure l'ID du vendeur

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
                imgProduit.MouseLeftButtonDown += new MouseButtonEventHandler(ImageProduit_MouseLeftButtonUp);

                TextBlock descriptionProduit = new TextBlock();
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

                // Ajout de l'ID du vendeur
                TextBlock vendeurProduit = new TextBlock();
                vendeurProduit.Text = $"Vendeur: {produit.IdVendeur}";
                vendeurProduit.FontSize = 11;
                vendeurProduit.Foreground = new SolidColorBrush(Colors.Blue);
                vendeurProduit.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                vendeurProduit.Margin = new Thickness(0, 2, 0, 0);

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
                stackPanel.Children.Add(vendeurProduit); // Affichage du vendeur
                stackPanel.Children.Add(prixProduit);
                stackPanel.Children.Add(buttonPanel);

                Separator separator1 = new Separator();
                string nom = produit.Nom;
                TextBlock nomblock = new TextBlock();
                nomblock.Text = nom;
                nomblock.FontSize = 12;
                nomblock.FontWeight = FontWeights.Bold;
                nomblock.TextAlignment = TextAlignment.Center;
                string quantite = produit.Quantite.ToString();
                TextBlock quantiteBlocK = new TextBlock();
                quantiteBlocK.Text = quantite;
                Separator separator2 = new Separator();
                BitmapImage bibtn = new BitmapImage();
                bibtn.BeginInit();
                string cheminIcon = System.Configuration.ConfigurationManager.AppSettings["icons"];

                bibtn.UriSource = new Uri(cheminIcon + "plus.png", UriKind.Relative);
                bibtn.CacheOption = BitmapCacheOption.OnLoad;
                bibtn.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                bibtn.EndInit();
                Image btnImageAjout = new Image();
                btnImageAjout.Source = bibtn;
                btnImageAjout.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                btnImageAjout.VerticalAlignment = VerticalAlignment.Center;
                btnImageAjout.Width = 20;
                btnImageAjout.Height = btnImageAjout.Width;


                btnImageAjout.MouseLeftButtonDown += new MouseButtonEventHandler(ImageAjout_MouseLeftButtonDown);
                btnImageAjout.Tag = produit;

                border.Child = stackPanel;
                wpProduit.Children.Add(border);
            }
        }


        private void ImageProduit_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        /// <summary>
        /// permet d'ajouter un article a la liste d'article de voeux d'un article
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImageAjout_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image btnImageAjout = sender as Image;
            if (btnImageAjout != null)
            {
                Produit pArticle = btnImageAjout.Tag as Produit;
                if (pArticle != null)
                {
                    // Vérifier si listePanier est bien initialisée
                    if (listePanier == null)
                    {
                        listePanier = new ListePanier();
                    }

                    Items items = new Items();
                    items.LstProduit = pArticle;
                    items.NbItem = 1;
                    items.NomItem = System.Configuration.ConfigurationManager.AppSettings["images"] + pArticle.Nom;
                    bool articleExiste = false;

                    for (int i = 0; i < listePanier.Items.Count; i++)
                    {
                        if (listePanier.Items[i].LstProduit == items.LstProduit)
                        {
                            listePanier.Items[i].NbItem++;
                            // InitialiserlstArticleChoisir();
                            // afficherToast("L'article a été rajouté avec succès", "succes");
                            articleExiste = true;
                            break; // Sortie de la boucle dès qu'on trouve l'article
                        }
                    }

                    if (!articleExiste)
                    {
                        listePanier.AjouterArticle(items);
                        SharedResources.MaListe = listePanier;
                        // afficherToast("L'article a été ajouté avec succès", "succes");
                    }
                }
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
