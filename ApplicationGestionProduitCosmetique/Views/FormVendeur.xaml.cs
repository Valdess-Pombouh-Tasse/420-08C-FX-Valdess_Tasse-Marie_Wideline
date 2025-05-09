using ApplicationGestionProduitCosmetique.Classes;
using ApplicationGestionProduitCosmetique.enums;
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
    /// Logique d'interaction pour FormVendeur.xaml
    /// </summary>
    public partial class FormVendeur : Window
    {
        private List<Produit> _produits;
        public List<Produit> Produits
        {
            get { return _produits; }
            set { _produits = value; }
        }

        private int _idVendeur;
        public FormVendeur(int idVendeur)
        {
            InitializeComponent();
            _idVendeur = idVendeur;

        }


        private void AfficherProduitsVendeurDansListBox(int Idvendeur)
        {
            var produitsVendeur = GestionProduits.ObtenirProduitsVendeur(Idvendeur);

            lbProduits.Items.Clear();

            foreach (var produit in produitsVendeur)
            {
                lbProduits.Items.Add($"{produit.Nom} - {produit.Prix:C} - Qté: {produit.Quantite}");
            }
        }



        private void AfficherListeProduit(List<Produit> produits = null)
        {
            List<Produit> listeProduits = produits ?? GestionCompteUtilisateur.ObtenirListeProduits()
                                                      .GroupBy(p => p.Id)  
                                                      .Select(g => g.First())
                                                      .ToList();

            wpProduits.Children.Clear();

            foreach (Produit produit in listeProduits)
            {
                // Création du conteneur principal
                Border border = new Border
                {
                    BorderBrush = new SolidColorBrush(Colors.LightGray),
                    BorderThickness = new Thickness(2),
                    CornerRadius = new CornerRadius(10),
                    Margin = new Thickness(5),
                    Padding = new Thickness(5),
                    Width = 100,
                    Height = 280
                };

                // Image du produit
                BitmapImage biProduit = new BitmapImage(new Uri(produit.Image, UriKind.RelativeOrAbsolute));
                Image imgProduit = new Image
                {
                    Source = biProduit,
                    Margin = new Thickness(2),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Width = 90,
                    Height = 100,
                    Tag = produit
                };
                imgProduit.MouseLeftButtonDown += ImageProduit_MouseLeftButtonUp;

                // Description du produit
                TextBlock descriptionProduit = new TextBlock
                {
                    Text = produit.Nom,
                    FontSize = 12,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    TextWrapping = TextWrapping.Wrap,
                    MaxWidth = 85
                };

                // Prix du produit
                TextBlock prixProduit = new TextBlock
                {
                    Text = produit.Prix.ToString("C"),
                    FontSize = 13,
                    Foreground = new SolidColorBrush(Colors.Green),
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Margin = new Thickness(4)
                };

                // Conteneur pour la quantité (en bas)
                StackPanel quantitePanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Margin = new Thickness(0, 5, 0, 5)
                };

                TextBlock lblQuantite = new TextBlock
                {
                    Text = "Qté: ",
                    FontSize = 12,
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush(Colors.Black)
                };

                TextBlock quantiteBlocK = new TextBlock
                {
                    Text = produit.Quantite.ToString(),
                    FontSize = 12,
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush(Colors.DarkBlue)
                };

                quantitePanel.Children.Add(lblQuantite);
                quantitePanel.Children.Add(quantiteBlocK);

                // Bouton d'ajout (image)
                BitmapImage bibtn = new BitmapImage(new Uri(System.Configuration.ConfigurationManager.AppSettings["icons"] + "plus.png", UriKind.Relative));
                Image btnImageAjout = new Image
                {
                    Source = bibtn,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Width = 20,
                    Height = 20,
                    Tag = produit
                };
                btnImageAjout.MouseLeftButtonDown += ImageAjout_MouseLeftButtonDown;

                // Empiler les éléments dans un StackPanel
                StackPanel stackPanel = new StackPanel
                {
                    Orientation = Orientation.Vertical,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                stackPanel.Children.Add(imgProduit);
                stackPanel.Children.Add(descriptionProduit);
                stackPanel.Children.Add(prixProduit);
                stackPanel.Children.Add(quantitePanel);
                stackPanel.Children.Add(btnImageAjout);

                // Ajouter le stackPanel au Border
                border.Child = stackPanel;

                // Ajouter au conteneur principal
                wpProduits.Children.Add(border);
            }
        }

        private void ImageAjout_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void ImageProduit_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
                AfficherProduitsVendeurDansListBox(12);
            
                AfficherListeProduit();
        }

        private void btnAjouter_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnImage_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
