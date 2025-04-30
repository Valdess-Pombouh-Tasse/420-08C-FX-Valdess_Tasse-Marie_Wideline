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
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.RegularExpressions;
using Orientation = System.Windows.Controls.Orientation;
using static ApplicationGestionProduitCosmetique.Views.FormPanier;
using System.Windows.Interop;
using Syncfusion.Pdf.Graphics;
using System.Diagnostics;
using Syncfusion.Pdf;
using System;
using UserControl = System.Windows.Controls.UserControl;

namespace ApplicationGestionProduitCosmetique.Views
{
    /// <summary>
    /// Logique d'interaction pour FormApplication.xaml
    /// </summary>
    public partial class FormApplication : UserControl
    {
        public Utilisateur Utilisateur { get; private set; }
        List<Produit> _produits;
        public ListePanier listePanier = new ListePanier();
        public List<Produit> panierProduits = new List<Produit>();

        public List<Produit> Produits
        {
            get { return _produits; }
            set { _produits = value; }
        }

        public static class accueill
        {
            public static Utilisateur utilisateur { get; set; }
        }
       
        public FormApplication(Utilisateur utilisateur)
        {
            
            RetournerListePanier();
            AfficherListeProduit();

            List<Categorie> listCategories = GestionCompteUtilisateur.ObtenirListeCategories();
            listCategories.Add(new Categorie(0, "Toutes"));
            cboCategories.ItemsSource = listCategories;
            cboCategories.SelectedIndex = listCategories.Count - 1;

            txtUtilisateurClient.Text = utilisateur.NomUtilisateur;
            this.Utilisateur = utilisateur;
            accueill.utilisateur = utilisateur;
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;

        }

        public FormApplication()
        {
            InitializeComponent();
            AfficherListeProduit();
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
            HashSet<int> idsProduitsAjoutes = new HashSet<int>(); // Stocker les IDs déjà affichés
            List<Produit> listeProduits = produits ?? GestionCompteUtilisateur.ObtenirListeProduits();
            
            wpProduit.Children.Clear();

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
                    VerticalAlignment = VerticalAlignment.Top,
                    Width = 125,
                    Height = 125,
                    Tag = produit
                };
                imgProduit.MouseLeftButtonDown += ImageProduit_MouseLeftButtonUp;

                TextBlock descriptionProduit = new TextBlock
                {
                    Text = produit.Nom,
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



                StackPanel stackPanel = new StackPanel
                {
                    Orientation = Orientation.Vertical,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Top
                };

                stackPanel.Children.Add(imgProduit);
                stackPanel.Children.Add(descriptionProduit);
                stackPanel.Children.Add(prixProduit);

                BitmapImage bibtn = new BitmapImage();
                bibtn.BeginInit();
                bibtn.UriSource = new Uri("/Resources/Images/ImagesIcons/plus.png", UriKind.Relative);
                bibtn.CacheOption = BitmapCacheOption.OnLoad;
                bibtn.CreateOptions = BitmapCreateOptions.IgnoreImageCache;


                Image btnImageAjout = new Image
                {
                    Source = bibtn,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Width = 20,
                    Height = 20,
                    Tag = produit
                };
                btnImageAjout.MouseLeftButtonDown += ImageAjout_MouseLeftButtonDown;

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
                    //Items items = new Items();
                    //items.LstProduit = produit;
                    //items.NbItem = 1;
                    //items.NomItem = System.Configuration.ConfigurationManager.AppSettings["images"] + produit.Nom;
                    //bool articleExiste = false;
                    //for (int i = 0; i < listePanier.Items.Count; i++)
                    //{
                    //    if (listePanier.Items[i].LstProduit == items.LstProduit)
                    //    {
                    //        listePanier.Items[i].NbItem++;
                    //        articleExiste = true;
                    //    }
                    //}
                    //if (!articleExiste)
                    //{
                    //    listePanier.AjouterArticle(items);
                    //    SharedResources.MaListe = listePanier;
                    //}
                    panierProduits.Add(produit);
                    // List<Produit> p =  panierProduits.Add(produit);
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

                            articleExiste = true;
                            break; // Sortie de la boucle dès qu'on trouve l'article
                        }
                    }

                    if (!articleExiste)
                    {
                        listePanier.AjouterArticle(items);
                        SharedResources.MaListe = listePanier;

                    }
                }
            }
        }


        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            FormPanier formPanier = new FormPanier(panierProduits);
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            // Créer un document PDF
            using (Syncfusion.Pdf.PdfDocument document = new Syncfusion.Pdf.PdfDocument())
            {
                // Ajouter une page au document
                PdfPage page = document.Pages.Add();

                // Créer des graphiques pour la page
                PdfGraphics graphics = page.Graphics;

                // Définir une police pour le titre
                PdfFont titleFont = new PdfStandardFont(PdfFontFamily.Helvetica, 24, PdfFontStyle.Bold);

                // Définir le titre de l'application
                string titreApplication = "Cometic Shop";

                // Mesurer la largeur du texte pour le centrer
                System.Drawing.SizeF textSize = titleFont.MeasureString(titreApplication);
                float xPosition = (page.GetClientSize().Width - textSize.Width) / 2;

                // Dessiner le titre centré en haut de la page
                graphics.DrawString(titreApplication, titleFont, PdfBrushes.Black, new System.Drawing.PointF(xPosition, 20));

                // Enregistrer le document
                string filePath = "MesFactures.pdf";
                document.Save(filePath);

                // Ouvrir le PDF généré
                Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
            }
        }

    }
}
