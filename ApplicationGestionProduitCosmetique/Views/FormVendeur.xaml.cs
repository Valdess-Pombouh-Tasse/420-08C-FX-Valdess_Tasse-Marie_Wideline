using ApplicationGestionProduitCosmetique.Classes;
using ApplicationGestionProduitCosmetique.enums;
using ApplicationGestionProduitCosmetique.Models;
using ApplicationGestionProduitCosmetique.Views;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.IO;
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
        private int idVendeur;

        public FormVendeur(int idVendeur)
        {
            InitializeComponent();
            this.idVendeur = idVendeur;

            // 🔹 Vérification de l'ID du vendeur
            if (this.idVendeur <= 0)
            {
                MessageBox.Show("ID du vendeur invalide !", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ChargerProduitsVendeur();
        }

        private void ChargerProduitsVendeur()
        {
            try
            {
                // 🔹 Récupération des produits du vendeur
                List<Produit> produitsVendeur = GestionProduits.ObtenirProduitsVendeur(idVendeur);

                // 🔹 Vérification si la liste est vide
                if (produitsVendeur == null || produitsVendeur.Count == 0)
                {
                    MessageBox.Show("Aucun produit trouvé pour ce vendeur.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                // 🔹 Mise à jour de la ListBox
                lbProduits.ItemsSource = produitsVendeur;

                // 🔹 Mise à jour de l'affichage en cartes
                wpProduits.Children.Clear();
                foreach (var produit in produitsVendeur)
                {
                    try
                    {
                        Border border = new Border
                        {
                            Background = Brushes.White,
                            BorderBrush = Brushes.Gray,
                            BorderThickness = new Thickness(1),
                            CornerRadius = new CornerRadius(10),
                            Padding = new Thickness(10),
                            Margin = new Thickness(5)
                        };

                        StackPanel stackPanel = new StackPanel { HorizontalAlignment = HorizontalAlignment.Center };

                        // 🔹 Vérification et chargement de l'image
                        Image img = new Image { Width = 100, Height = 100, Margin = new Thickness(0, 0, 0, 5) };
                        if (!string.IsNullOrEmpty(produit.Image))
                        {
                            try
                            {
                                Uri imageUri;
                                if (Uri.TryCreate(produit.Image, UriKind.Absolute, out imageUri))
                                {
                                    img.Source = new BitmapImage(imageUri);
                                }
                                else if (File.Exists(produit.Image))
                                {
                                    img.Source = new BitmapImage(new Uri(produit.Image, UriKind.Absolute));
                                }
                                else
                                {
                                    throw new FileNotFoundException("Image introuvable : " + produit.Image);
                                }
                            }
                            catch (Exception)
                            {
                                img.Source = new BitmapImage(new Uri("pack://application:,,,/Images/placeholder.png"));
                            }
                        }
                        else
                        {
                            img.Source = new BitmapImage(new Uri("pack://application:,,,/Images/placeholder.png"));
                        }

                        stackPanel.Children.Add(img);

                        // 🔹 Nom du produit
                        TextBlock txtNom = new TextBlock
                        {
                            Text = produit.Nom,
                            FontWeight = FontWeights.Bold,
                            Foreground = Brushes.Black,
                            FontSize = 14,
                            TextAlignment = TextAlignment.Center
                        };
                        stackPanel.Children.Add(txtNom);

                        // 🔹 Prix du produit
                        TextBlock txtPrix = new TextBlock
                        {
                            Text = $"Prix: {produit.Prix} €",
                            Foreground = Brushes.DarkGreen,
                            FontSize = 12,
                            TextAlignment = TextAlignment.Center
                        };
                        stackPanel.Children.Add(txtPrix);

                        border.Child = stackPanel;
                        wpProduits.Children.Add(border);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erreur lors de l'affichage du produit \"{produit.Nom}\" : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement des produits : " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

