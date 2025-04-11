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
using Orientation = System.Windows.Controls.Orientation;
using Button = System.Windows.Controls.Button;
using MessageBox = System.Windows.MessageBox;
using TextBox = System.Windows.Controls.TextBox;
using PdfSharp.Drawing;
using System.Media;

namespace ApplicationGestionProduitCosmetique.Views
{
    /// <summary>
    /// Logique d'interaction pour FormApplication.xaml
    /// </summary>
    public partial class FormApplication : Window
    {
        /// <summary>
        /// Liste de produits
        /// </summary>
        List<Produit> _produits;

        /// <summary>
        /// Liste de produits dans le panier
        /// </summary>
        public List<Produit> Produits
        {
            get { return _produits; }
            set { _produits = value; }
        }

        public FormApplication()
        {
            InitializeComponent();

            AfficherListeProduit();
            List<Categorie> listCategories = GestionCompteUtilisateur.ObtenirListeCategories();
            listCategories.Add(new Categorie(0, "Toutes"));
            cboCategories.ItemsSource = listCategories;
            cboCategories.SelectedIndex = listCategories.Count - 1;         
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AfficherListeProduit();
        }

        private void AfficherListeProduit(List<Produit> produits = null)
        {
            if (produits == null)           
                produits = GestionCompteUtilisateur.ObtenirListeProduits();
            
            wpProduits.Children.Clear();
  
            foreach (Produit produit in produits)
            {
               
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
                wpProduits.Children.Add(border);
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
        private Facture nouvelleFacture = null;
        private void ImageAjout_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            uint quantite = 1;
            Image imgProduit = (Image)sender;
            Produit produit = (Produit)imgProduit.Tag;

            if (nouvelleFacture == null)
            {
                nouvelleFacture = new Facture(1, DateTime.Now);

                wpProduitPanier.IsEnabled = true;
                btnPayer.IsEnabled = true;
 
            }

            ProduitFacture produitExist = null;
            foreach (var prodF in nouvelleFacture.ProduitsFacture)
            {
                if (prodF.Produit.Equals(produit))
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
                nouvelleFacture.AjouterProduit(produit, produit.Prix, quantite);
            }

            AfficherProduitDansFacture(nouvelleFacture);
        }


        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
        //    Factures facture = new Factures();
        //    facture.Show();

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

        public void AfficherProduitDansFacture(Facture facture)
        {

            wpProduitPanier.Children.Clear();

            foreach (ProduitFacture prodF in facture.ProduitsFacture)
            {
                TextBlock descriptionProduit = new TextBlock();
                descriptionProduit.Text = prodF.Produit.Nom;
                descriptionProduit.FontSize = 15;
                descriptionProduit.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                descriptionProduit.TextWrapping = TextWrapping.Wrap;
                descriptionProduit.Margin = new Thickness(5, 0, 5, 0);

                TextBlock codeProduit = new TextBlock();
                codeProduit.Text = prodF.Produit.Code;
                codeProduit.FontSize = 10;
                codeProduit.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                codeProduit.Margin = new Thickness(5, 0, 5, 0);

                StackPanel stackPanelCodeDescrip = new StackPanel();
                stackPanelCodeDescrip.Orientation = Orientation.Vertical;
                stackPanelCodeDescrip.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                stackPanelCodeDescrip.VerticalAlignment = VerticalAlignment.Center;
                stackPanelCodeDescrip.Width = 180;
                stackPanelCodeDescrip.Children.Add(descriptionProduit);
                stackPanelCodeDescrip.Children.Add(codeProduit);

                TextBox qteProduit = new TextBox();
                qteProduit.Text = prodF.Quantite.ToString();
                qteProduit.FontSize = 10;
                qteProduit.Width = 40;
                qteProduit.Height = 20;
                qteProduit.TextAlignment = System.Windows.TextAlignment.Center;
                qteProduit.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                qteProduit.VerticalAlignment = VerticalAlignment.Center;

                Button btnReduction = new Button();
                btnReduction.Width = 15;
                btnReduction.Height = 15;
                btnReduction.Margin = new Thickness(5);
                btnReduction.ToolTip = "Retirer 1";
                btnReduction.Background = new SolidColorBrush(Colors.Blue);
                btnReduction.BorderBrush = new SolidColorBrush(Colors.White);
                btnReduction.BorderThickness = new Thickness(1);
                btnReduction.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                btnReduction.VerticalAlignment = VerticalAlignment.Center;
                btnReduction.Click += new RoutedEventHandler(btnDimunuerQteProduit_Click);

                Image imgBtnReduire = new Image();
                imgBtnReduire.Source = new BitmapImage(new Uri("/Resources/minus.png", UriKind.Relative));
                imgBtnReduire.Width = 15;
                imgBtnReduire.Height = 15;
                imgBtnReduire.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                imgBtnReduire.VerticalAlignment = VerticalAlignment.Center;
                btnReduction.Content = imgBtnReduire;
                btnReduction.Tag = prodF.Produit;

                Button btnAjouter = new Button();
                btnAjouter.Width = 15;
                btnAjouter.Height = 15;
                btnAjouter.Margin = new Thickness(5);
                btnAjouter.ToolTip = "Ajouter 1";
                btnAjouter.Background = new SolidColorBrush(Colors.Blue);
                btnAjouter.BorderBrush = new SolidColorBrush(Colors.White);
                btnAjouter.BorderThickness = new Thickness(1);
                btnAjouter.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                btnAjouter.VerticalAlignment = VerticalAlignment.Center;
                btnAjouter.Click += new RoutedEventHandler(btnAugmenterQteProduit_Click);

                Image imgBtnAjout = new Image();
                imgBtnAjout.Source = new BitmapImage(new Uri("/Resources/plus.png", UriKind.Relative));
                imgBtnAjout.Width = 15;
                imgBtnAjout.Height = 15;
                imgBtnAjout.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                imgBtnAjout.VerticalAlignment = VerticalAlignment.Center;
                btnAjouter.Content = imgBtnAjout;
                btnAjouter.Tag = prodF.Produit;

                TextBlock prixProduit = new TextBlock();
                prixProduit.Text = prodF.Produit.Prix.ToString("c");
                prixProduit.FontSize = 13;
                prixProduit.FontWeight = FontWeights.Bold;
                prixProduit.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                prixProduit.Margin = new Thickness(4);

                StackPanel stackPanel = new StackPanel();
                stackPanel.Orientation = Orientation.Horizontal;
                stackPanel.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                stackPanel.VerticalAlignment = VerticalAlignment.Center;

                Border borderLigne = new Border();
                borderLigne.Width = 380;
                borderLigne.Margin = new Thickness(4);
                borderLigne.BorderBrush = new SolidColorBrush(Colors.Gray);
                borderLigne.BorderThickness = new Thickness(1);
                borderLigne.Background = new SolidColorBrush(Color.FromArgb(128, 0, 255, 0));
                borderLigne.CornerRadius = new CornerRadius(3);
                borderLigne.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                borderLigne.VerticalAlignment = VerticalAlignment.Center;

                stackPanel.Children.Add(stackPanelCodeDescrip);
                stackPanel.Children.Add(btnReduction);
                stackPanel.Children.Add(qteProduit);
                stackPanel.Children.Add(btnAjouter);
                stackPanel.Children.Add(prixProduit);

                borderLigne.Child = stackPanel;

                wpProduitPanier.Children.Add(borderLigne);
            }

            lblSousTotal.Text = facture.MontantSousTotal.ToString("c");
            lblTPS.Text = facture.MontantTPS.ToString("c");
            lblTVQ.Text = facture.MontantTVQ.ToString("c");
            lblTotal.Text = facture.MontantTotal.ToString("c");

            if (facture.ProduitsFacture.Count == 0)
            {
                lblSousTotal.Text = 0.ToString("c");
                lblTPS.Text = 0.ToString("c");
                lblTVQ.Text = 0.ToString("c");
                lblTotal.Text = 0.ToString("c");
            }
        }

        private void btnAugmenterQteProduit_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Produit produit = (Produit)button.Tag;

            ProduitFacture produitExist = null;
            foreach (var prodF in nouvelleFacture.ProduitsFacture)
            {
                if (prodF.Produit.Equals(produit))
                {
                    produitExist = prodF;
                }
            }


            if (produitExist != null)
            {
                produitExist.Quantite++;
            }
            AfficherProduitDansFacture(nouvelleFacture);
        }

        private void btnDimunuerQteProduit_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Produit produit = (Produit)button.Tag;

            bool produitExist = false;
            foreach (var prodF in nouvelleFacture.ProduitsFacture)
            {
                if (prodF.Produit.Equals(produit))
                {
                    produitExist = true;
                }
            }

            if (produitExist)
                nouvelleFacture.RetirerProduit(produit);

            AfficherProduitDansFacture(nouvelleFacture);
        }

        
        private void btnPayer_Click(object sender, RoutedEventArgs e)
        {
            if (nouvelleFacture == null || nouvelleFacture.ProduitsFacture.Count == 0)
            {
                MessageBox.Show("Aucune facture à envoyer. Veuillez ajouter des articles au panier.",
                                 "Cosmetic Shop", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Enregistrer la facture dans la base de données
                GestionCompteUtilisateur.EnregistrerFacture(nouvelleFacture);

                // Générer le fichier PDF de la facture
                string dossierFactures = System.IO.Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    "FacturesCosmetiques");
                Directory.CreateDirectory(dossierFactures);

                string nomFichier = $"Facture_{nouvelleFacture.Id}.pdf";
                string cheminComplet = System.IO.Path.Combine(dossierFactures, nomFichier);

                using (PdfSharp.Pdf.PdfDocument document = new PdfSharp.Pdf.PdfDocument())
                {
                    document.Info.Title = $"Facture {nouvelleFacture.Id}";
                    PdfSharp.Pdf.PdfPage page = document.AddPage();
                    XGraphics gfx = XGraphics.FromPdfPage(page);

                    var fontTitre = new XFont("Verdana", 16);
                    var fontCorps = new XFont("Verdana", 12);

                    int y = 40;
                    gfx.DrawString($"Facture No : {nouvelleFacture.Id}", fontTitre, XBrushes.Black,
                        new XRect(0, y, page.Width, 0), XStringFormats.TopCenter);
                    y += 30;

                    gfx.DrawString($"Date : {nouvelleFacture.DateCreation}", fontCorps, XBrushes.Black, 40, y);
                    y += 30;

                    gfx.DrawString("Produits achetés :", fontCorps, XBrushes.Black, 40, y);
                    y += 25;

                    for (int i = 0; i < nouvelleFacture.ProduitsFacture.Count; i++)
                    {
                        ProduitFacture item = nouvelleFacture.ProduitsFacture[i];
                        string ligne = $"{item.Produit.Nom} - Qté: {item.Quantite} - Prix: {item.Produit.Prix * item.Quantite}$";
                        gfx.DrawString(ligne, fontCorps, XBrushes.Black, 40, y);
                        y += 20;
                    }

                    y += 30;
                    gfx.DrawString($"Total: {nouvelleFacture.MontantTotal} $", fontTitre, XBrushes.DarkBlue, 40, y);

                    document.Save(cheminComplet);
                }

                // ✅ Paiement confirmé
                FenetreErreur.Afficher("Paiement effectué avec succès !");

                // 🔊 Joue le son de succès
                try
                {
                    SoundPlayer player = new SoundPlayer("Resources/Sons/success.wav");
                    player.Load();
                    player.Play();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur de lecture du son : {ex.Message}");
                }

                 Factures fenetreFactures = new Factures(nouvelleFacture);
                fenetreFactures.ShowDialog();

                // ✅ Désactive les contrôles après paiement
                wpProduitPanier.IsEnabled = false;
                btnPayer.IsEnabled = false;
            }
            catch (Exception ex)
            {
                FenetreErreur.Afficher($"Erreur lors du paiement.\nDétails : {ex.Message}");
            }
        }




        private void btnNouvelleFacture_Click_1(object sender, RoutedEventArgs e)
        {
            wpProduitPanier.Children.Clear();
            lblSousTotal.Text = 0.ToString("c");
            lblTPS.Text = 0.ToString("c");
            lblTVQ.Text = 0.ToString("c");
            lblTotal.Text = 0.ToString("c");
            nouvelleFacture = null;
            wpProduitPanier.IsEnabled = true;
            btnPayer.IsEnabled = true;
        }

  
    }
}
