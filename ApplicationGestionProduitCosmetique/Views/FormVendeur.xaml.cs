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
        private Produit produitSelectionne = null;
        public FormVendeur(int idVendeur)
        {
            InitializeComponent();
            _idVendeur = idVendeur;
        }



        private void AfficherMesProduits(List<Produit> produits)
        {
            wpMesProduits.Children.Clear();

            foreach (Produit produit in produits)
            {
                // Conteneur principal
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

                Image btnImageAjout = new Image
                {
                    Source = new BitmapImage(new Uri("/Resources/Images/ImagesIcons/delete.png", UriKind.Relative)),
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Width = 20,
                    Height = 20,
                    Tag = produit
                };
                btnImageAjout.MouseLeftButtonDown += ImageMesProduits_MouseLeftButtonDown;

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


                // Conteneur pour la quantité
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

                btnImageAjout.MouseLeftButtonDown += ImageAjout_MouseLeftButtonDown;


                // Empiler les éléments
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
                border.Child = stackPanel;
                wpMesProduits.Children.Add(border);
            }
        }


        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Border carte = (Border)sender;
            produitSelectionne = (Produit)carte.Tag;
        }

        private void AfficherListeProduit(List<Produit> produits = null)
        {
            List<Produit> listeProduits = produits ?? GestionCompteUtilisateur.ObtenirListeProduits();

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
                    Height = 200
                };
                border.MouseLeftButtonDown += Border_MouseLeftButtonDown;
                border.Tag = produit; // Associer le produit à la carte


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
            Produits = GestionProduits.ObtenirProduitsVendeur(_idVendeur);
            AfficherMesProduits(Produits);

            AfficherListeProduit();
        }

        private void btnAjouter_Click(object sender, RoutedEventArgs e)
        {
            FormProduit frmAjout = new FormProduit(null, EtatFormulaire.Ajouter);
            bool? resultat = frmAjout.ShowDialog();

            if (resultat == true)
            {
                try
                {
                    Produit nouveauProduit = frmAjout.Produit;
                    GestionProduits.AjouterProduit(nouveauProduit);
                    BoxCosmetic.Afficher("Produit ajouté avec succès.", "Ajout");

                    AfficherListeProduit();
                }
                catch (Exception ex)
                {
                    throw;
                    BoxCosmetic.Afficher($"Erreur lors de l'ajout : {ex.Message}", "Erreur");
                }
            }
        }

        private void ImageMesProduits_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image imageProduit = sender as Image;

            if (imageProduit == null)
            {
                MessageBox.Show("Erreur d'interaction avec l'image.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Produit produitSelectionne = imageProduit.Tag as Produit;

            if (produitSelectionne == null)
            {
                MessageBox.Show("Veuillez sélectionner un produit à supprimer.", "Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBoxResult result = MessageBox.Show(
                $"Êtes-vous sûr de vouloir supprimer le produit : {produitSelectionne.Nom} ?",
                "Confirmation de suppression",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    bool estSupprime = GestionProduits.SupprimerProduit(produitSelectionne);
                    if (estSupprime)
                    {
                        AfficherListeProduit();
                        MessageBox.Show("Produit supprimé avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Le produit n'a pas pu être supprimé.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (ArgumentNullException ane)
                {
                    MessageBox.Show($"Produit introuvable : {ane.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (Exception ex)
                {
                    throw;
                    MessageBox.Show($"Une erreur est survenue lors de la suppression : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
