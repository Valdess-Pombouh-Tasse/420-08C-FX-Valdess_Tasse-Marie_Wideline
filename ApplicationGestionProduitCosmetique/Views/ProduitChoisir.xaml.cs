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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static ApplicationGestionProduitCosmetique.Views.FormApplication;
using Button = System.Windows.Controls.Button;
using Orientation = System.Windows.Controls.Orientation;

namespace ApplicationGestionProduitCosmetique.Views
{
    /// <summary>
    /// Logique d'interaction pour ProduitChoisir.xaml
    /// </summary>
    public partial class ProduitChoisir : Window
    {
        List<Produit> produits;
        ListePanier listePanier = SharedResources.MaListe;
        Facture facture = new Facture();


        public List<Produit> Produits
        {
            get { return produits; }
            set { produits = value; }
        }
        public ProduitChoisir(List<Produit> produits)
        {
            InitializeComponent();
            Produits = produits;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AfficherProduitDansPanier(Produits);
        }

        /// <summary>
        /// permet d'initialiser la liste des articles choisir
        /// </summary>
        public void InitialiserlstArticleChoisir()
        {

            if (listePanier != null)
            {
                lblTPS.Text = $"le prix total de vos Articles: {listePanier.PrixSousTotal.ToString()}$";
                wpProduitPanier.Children.Clear();
            }
        }
        /// <summary>
        /// Permet d'afficher liste des produits choisir dans le panier
        /// </summary>
        private void AfficherProduitDansPanier(List<Produit> produits)
        {
            wpProduitPanier.Children.Clear();

            foreach (Produit prodF in produits)
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
                biProduit.UriSource = new Uri(prodF.Image);
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
                imgProduit.Tag = prodF;

                TextBlock descriptionProduit = new TextBlock();
                descriptionProduit.Text = prodF.Nom;
                descriptionProduit.FontSize = 15;
                descriptionProduit.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                descriptionProduit.TextWrapping = TextWrapping.Wrap;
                descriptionProduit.Margin = new Thickness(5, 0, 5, 0);

                TextBlock codeProduit = new TextBlock();
                codeProduit.Text = prodF.Code;
                codeProduit.FontSize = 10;
                codeProduit.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                codeProduit.Margin = new Thickness(5, 0, 5, 0);

                StackPanel stackPanelCodeDescrip = new StackPanel();
                stackPanelCodeDescrip.Orientation = Orientation.Vertical;
                stackPanelCodeDescrip.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                stackPanelCodeDescrip.VerticalAlignment = VerticalAlignment.Center;
                stackPanelCodeDescrip.Children.Add(descriptionProduit);
                stackPanelCodeDescrip.Children.Add(codeProduit);

                System.Windows.Controls.TextBox qteProduit = new System.Windows.Controls.TextBox();
                qteProduit.Text = "1";
                //qteProduit.Text = prodF.Quantite.ToString();
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
                btnReduction.Click += new RoutedEventHandler(btnDimunuerProduit_Click);

                Image imgBtnReduire = new Image();
                imgBtnReduire.Source = new BitmapImage(new Uri("/Resources/Images/ImagesIcons/minus.png", UriKind.Relative));
                imgBtnReduire.Width = 15;
                imgBtnReduire.Height = 15;
                imgBtnReduire.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                imgBtnReduire.VerticalAlignment = VerticalAlignment.Center;
                btnReduction.Content = imgBtnReduire;
                btnReduction.Tag = prodF;

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
                btnAjouter.Click += new RoutedEventHandler(btnAugmenterProduit_Click);

                Image imgBtnAjout = new Image();
                imgBtnAjout.Source = new BitmapImage(new Uri("/Resources/Images/ImagesIcons/plus.png", UriKind.Relative));
                imgBtnAjout.Width = 15;
                imgBtnAjout.Height = 15;
                imgBtnAjout.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                imgBtnAjout.VerticalAlignment = VerticalAlignment.Center;
                btnAjouter.Content = imgBtnAjout;
                btnAjouter.Tag = prodF;

                TextBlock prixProduit = new TextBlock();
                prixProduit.Text = prodF.Prix.ToString("c");
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
                borderLigne.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                borderLigne.VerticalAlignment = VerticalAlignment.Center;

                stackPanel.Children.Add(imgProduit);
                stackPanel.Children.Add(stackPanelCodeDescrip);
                stackPanel.Children.Add(btnReduction);
                stackPanel.Children.Add(qteProduit);
                stackPanel.Children.Add(btnAjouter);
                stackPanel.Children.Add(prixProduit);

                borderLigne.Child = stackPanel;

                wpProduitPanier.Children.Add(borderLigne);
            }

           
        }


        Facture nouvelleFacture = null;
        
        private void btnAugmenterProduit_Click(object sender, RoutedEventArgs e)
        {
            ////Button button = (Button)sender;
            ////Produit produit = (Produit)button.Tag;

            //ProduitFacture produitExist = null;
            //foreach (Produit prodF in produits)
            //{
            //    if (prodF.Equals(produit))
            //    {
            //        //produitExist = prodF;
            //    }
            //}


            //if (produitExist != null)
            //{
            //    produitExist.Quantite++;
            //}
            //AfficherProduitDansPanier(Produits);
        }

        private void btnDimunuerProduit_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Produit produit = (Produit)button.Tag;

            bool produitExist = false;
            foreach (Produit prodF in produits)
            {
                if (prodF.Equals(produits))
                {
                    produitExist = true;
                }
            }

            if (produitExist)
                //nouveauPanier.RetirerArticle(produit);

            AfficherProduitDansPanier(Produits);
            
        }




        /// <summary>
        /// permet d'initialiser la liste des articles choisir
        /// </summary>
        //public void InitialiserlstArticleChoisir()
        //{

        //    //if (listePanier != null)
        //    //{
        //    //    lblPrixSansTaxe.Text = $"le prix total de vos Articles: {listePanier.PrixSousTotal.ToString()}$";
        //    //    lvListeVoeux.Items.Refresh();
        //    //}
        //}


        /// <summary>
        private void btnEnreigistrerLst_Click(object sender, RoutedEventArgs e)
        {

        }

        
    }
}
