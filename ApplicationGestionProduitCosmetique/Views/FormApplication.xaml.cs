using ApplicationGestionProduitCosmetique.Classes;
using ApplicationGestionProduitCosmetique.Models;
using System.IO;
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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Extensions.Configuration;
using System.Xml;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Orientation = System.Windows.Controls.Orientation;
using static ApplicationGestionProduitCosmetique.Views.ProduitChoisir;


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
        public List<Produit> panierProduits = new List<Produit>();
        public Facture facture = new Facture();


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
            RetournerListePanier();
            AfficherListeProduit();

            List<Categorie> listCategories = GestionCompteUtilisateur.ObtenirListeCategories();
            listCategories.Add(new Categorie(0, "Toutes"));
            cboCategories.ItemsSource = listCategories;
            cboCategories.SelectedIndex = listCategories.Count - 1;

            if (accueil.utilisateur == null)
            {
                lblErreurUtilisateur.Visibility = Visibility.Visible;
                lblErreurUtilisateur.Content = "Aucun utilisateur connecté.";

                return;
            }

            Utilisateur utilisateur = accueil.utilisateur;
            txtUtilisateurClient.Text = utilisateur.NomUtilisateur;
            this.Utilisateur = utilisateur;
            accueil.utilisateur = utilisateur;
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AfficherListeProduit();
        }

        public Utilisateur ObtenirUtilisateur()
        {
            return Utilisateur;
        }

        public static class SharedResources
        {
            public static ListePanier MaListe { get; set; }
        }

        public ListePanier RetournerListePanier()
        {
            return listePanier;
        }

        private void AfficherListeProduit(List<Produit> produits = null)
        {
            HashSet<int> idsProduitsAjoutes = new HashSet<int>();
            List<Produit> listeProduits = produits ?? GestionCompteUtilisateur.ObtenirListeProduits();

            wpProduit.Children.Clear();
            lblTitre.Content = $"Liste des ({listeProduits.Count}) produits ";

            foreach (Produit produit in listeProduits)
            {
                // Vérifier si l'ID du produit a déjà été ajouté
                if (idsProduitsAjoutes.Contains((int)produit.Id))
                {
                    continue;
                }

                // Ajouter l'ID du produit à la liste des produits affichés
                idsProduitsAjoutes.Add((int)produit.Id);

                Border border = new Border
                {
                    BorderBrush = new SolidColorBrush(Colors.LightGray),
                    BorderThickness = new Thickness(2),
                    CornerRadius = new CornerRadius(10),
                    Margin = new Thickness(5),
                    Padding = new Thickness(1),
                    Width = 200,
                    Height = 280,
                    Background = new SolidColorBrush(Colors.AliceBlue)
                };

                BitmapImage biProduit = new BitmapImage();
                biProduit.BeginInit();
                biProduit.UriSource = new Uri(produit.Image, UriKind.Relative);
                biProduit.CacheOption = BitmapCacheOption.OnLoad;
                biProduit.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                biProduit.EndInit();

                Image imgProduit = new Image
                {
                    Source = biProduit,
                    Margin = new Thickness(2),
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Width = 125,
                    Height = 125,
                    Tag = produit
                };
                imgProduit.MouseLeftButtonDown += ImageProduit_MouseLeftButtonUp;
                Separator separator1 = new Separator();
                string nom = produit.Nom;

                TextBlock descriptionProduit = new TextBlock
                {
                    Text = produit.Description,
                    FontSize = 12,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                    TextWrapping = TextWrapping.Wrap,
                    MaxWidth = 85
                };

                TextBlock prixProduit = new TextBlock
                {
                    Text = produit.Prix.ToString("c"),
                    FontSize = 13,
                    Foreground = new SolidColorBrush(Colors.Green),
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    MaxWidth = 80,
                    Margin = new Thickness(4)
                };
                Separator separator2 = new Separator();

                BitmapImage bibtn = new BitmapImage();
                bibtn.BeginInit();
                bibtn.UriSource = new Uri(produit.Image, UriKind.Relative);
                bibtn.CacheOption = BitmapCacheOption.OnLoad;
                bibtn.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                bibtn.EndInit();

                Image btnImageAjout = new Image
                {
                    Source = new BitmapImage(new Uri("/Resources/Images/ImagesIcons/plus.png", UriKind.Relative)),
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Width = 20,
                    Height = 20,
                    Tag = produit
                };
                btnImageAjout.MouseLeftButtonDown += ImageAjout_MouseLeftButtonDown;


                StackPanel stackPanel = new StackPanel();

                stackPanel.Children.Add(imgProduit);
                stackPanel.Children.Add(separator1);
                stackPanel.Children.Add(descriptionProduit);
                stackPanel.Children.Add(prixProduit);
                stackPanel.Children.Add(separator2);
                stackPanel.Children.Add(btnImageAjout);

                border.Child = stackPanel;
                wpProduit.Children.Add(border);
            }
        }


      
        private void ImageProduit_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            List<Produit> prod = new List<Produit>();
            uint quantite = 1;
            Image imageProduit = sender as Image;
        
            if (imageProduit != null)
            {
                Produit produit = imageProduit.Tag as Produit;
                if (produit != null)
                {
                    
                    panierProduits.Add(produit);
                    FenetreErreur.Afficher("Produit ajouté au panier");
                   
                }
            }
            prod = panierProduits;

        }

        /// <summary>
        /// permet d'ajouter un article a la liste d'article de voeux d'un article
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private Facture nouvelleFacture = null;
        private void ImageAjout_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            uint quantite = 1;
            Image imgProduit = (Image)sender;
            List<Produit> prod = new List<Produit>();

            if (nouvelleFacture == null)
            {
                
                ProduitChoisir frm = new ProduitChoisir(panierProduits);
                nouvelleFacture = new Facture(1, DateTime.Now);

                frm.wpProduitPanier.IsEnabled = true;  
                frm.btnPayer.IsEnabled = true;
                frm.txtNoFacture.Text = "";
                frm.lblDateFacture.Text = $"Date : {nouvelleFacture.DateCreation.ToString()}";
            }

            ProduitFacture produitExist = null;
            foreach (var prodF in nouvelleFacture.ProduitsFacture)
            {
                if (prodF.Produit.Equals(prod))
                {
                    produitExist = prodF;
                }
            }


            if (produitExist != null)
            {
                produitExist.Quantite++;
                quantite = produitExist.Quantite;
            }
            else 
            {
                nouvelleFacture.AjouterProduit((Produit)imgProduit.Tag, ((Produit)imgProduit.Tag).Prix, quantite);
                FenetreErreur.Afficher("Produit ajouté au panier");
            }
            prod = panierProduits;
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

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            ProduitChoisir produitChoisir = new ProduitChoisir(panierProduits);
            produitChoisir.Show();
           
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            Factures facture = new Factures();
            facture.Show();
            
        }

        private void btnRechercherProduit_Click(object sender, RoutedEventArgs e)
        {
            List<Produit> produits = new List<Produit>();
            string recherche = txtRechercher.Text;
            Categorie categorie = (Categorie)cboCategories.SelectedItem;

            if (!string.IsNullOrWhiteSpace(recherche) && (categorie == null || categorie.Nom == "Toutes"))
            {
                produits = GestionCompteUtilisateur.ObtenirListeProduits(recherche);

            }
            else if ((categorie != null && categorie.Nom != "Toutes") && string.IsNullOrWhiteSpace(recherche))
            {
                produits = GestionCompteUtilisateur.ObtenirListeProduits("", categorie);
            }
            else if (!string.IsNullOrWhiteSpace(recherche) && categorie != null && categorie.Nom != "Toutes")
            {
                produits = GestionCompteUtilisateur.ObtenirListeProduits(recherche, categorie);
            }
            else
            {
                produits = GestionCompteUtilisateur.ObtenirListeProduits();
            }
            AfficherListeProduit(produits);
        }

        private void txtRechercher_TextChanged(object sender, TextChangedEventArgs e)
        {
            string motCle = txtRechercher.Text;
            List<Produit> produits = GestionCompteUtilisateur.ObtenirListeProduits(motCle);

            if(produits.Count != 0)
            {
                AfficherListeProduit(produits);
            }
            else
            {
                AfficherListeProduit();
            }
        }
    }
}
