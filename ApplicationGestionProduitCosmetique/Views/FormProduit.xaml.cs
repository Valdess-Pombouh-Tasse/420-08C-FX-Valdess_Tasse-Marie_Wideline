using ApplicationGestionProduitCosmetique.enums;
using ApplicationGestionProduitCosmetique.Models;
using Microsoft.Win32;
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
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using ApplicationGestionProduitCosmetique.Classes;
 
namespace ApplicationGestionProduitCosmetique.Views
{
    /// <summary>
    /// Logique d'interaction pour FormProduit.xaml
    /// </summary>
    public partial class FormProduit : Window
    {
        private string imageProduitPath = "";
        private Produit _produit;
        private EtatFormulaire _etat;

        public Produit Produit
        {
            get { return _produit; }
            set { _produit = value; }
        }

        public EtatFormulaire Etat
        {
            get { return _etat; }
            set { _etat = value; }
        }


        public FormProduit(Produit produit = null, EtatFormulaire etat = EtatFormulaire.Ajouter)
        {
            Produit = produit;
            Etat = etat;
            InitializeComponent();
            cboCategories.ItemsSource = GestionProduits.ObtenirListeCategories();
            if (produit != null)
                Produit = produit;

            InitialiserFormulaire();
        }

        private void InitialiserFormulaire()
        {
            switch (Etat)
            {
                case EtatFormulaire.Ajouter:
                    this.Title = "Ajouter un produit";
                    break;

                case EtatFormulaire.Modifier:
                case EtatFormulaire.Supprimer:
                    if (Produit != null)
                    {
                        txtNom.Text = Produit.Nom;
                        txtCode.Text = Produit.Code;
                        txtPrix.Text = Produit.Prix.ToString("F2");

                        if (Produit.Categorie != null)
                            cboCategories.SelectedItem = Produit.Categorie;

                        if (!string.IsNullOrEmpty(Produit.Image))
                        {
                            try
                            {
                                imgProduit.Source = new BitmapImage(new Uri(Produit.Image));
                            }
                            catch
                            {
                                // Image non trouvée ou invalide
                                imgProduit.Source = null;
                            }
                        }
                    }

                    this.Title = (Etat == EtatFormulaire.Modifier) ? "Modifier un produit" : "Supprimer un produit";
                    txtNom.IsEnabled = Etat == EtatFormulaire.Modifier;
                    txtCode.IsEnabled = Etat == EtatFormulaire.Modifier;
                    txtPrix.IsEnabled = Etat == EtatFormulaire.Modifier;
                    cboCategories.IsEnabled = Etat == EtatFormulaire.Modifier;
                    btnAjouterImage.IsEnabled = Etat == EtatFormulaire.Modifier;
                    break;
            }
        }

        private void btnAjouterImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Image PNG (*.png)|*.png|Images JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    string sourcePath = openFileDialog.FileName;
                    string dossierImages = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");

                    if (!Directory.Exists(dossierImages))
                        Directory.CreateDirectory(dossierImages);

                    string nomFichier = System.IO.Path.GetFileName(sourcePath);
                    string cheminDestination = System.IO.Path.Combine(dossierImages, nomFichier);

                    File.Copy(sourcePath, cheminDestination, true);

                    imgProduit.Source = new BitmapImage(new Uri(cheminDestination));
                    imageProduitPath = cheminDestination;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur s'est produite :\n" + ex.Message, "Ajout d'une image", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void btnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
        private void btnAjouterModifier_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (Etat)
                {
                    case EtatFormulaire.Ajouter:
                    case EtatFormulaire.Modifier:

                        if (!ValiderFormulaire())
                            return;

                        uint id = 0;
                        string code = txtCode.Text.Trim();
                        string nom = txtNom.Text.Trim();
                        decimal prix = decimal.Parse(txtPrix.Text);
                        Categorie categorie = (Categorie)cboCategories.SelectedItem;
                        string description = txtDescription.Text.Trim();
                        int quantite = int.Parse(txtQuantite.Text);

                        string imageSource = "";
                        BitmapImage bi = imgProduit.Source as BitmapImage;
                        if (bi != null && bi.UriSource != null)
                            imageSource = bi.UriSource.LocalPath;

                        if (Etat == EtatFormulaire.Ajouter)
                        {
                            Produit = new Produit(id, code, nom, prix, imageSource, categorie, description, quantite);
                        }
                        else
                        {
                            Produit.Code = code;
                            Produit.Nom = nom;
                            Produit.Prix = prix;
                            Produit.Categorie = categorie;
                            Produit.Image = imageSource;
                            Produit.Description = description;
                            Produit.Quantite = quantite;
                        }

                        DialogResult = true;
                        break;

                    case EtatFormulaire.Supprimer:
                        DialogResult = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message, "Cosmetic Shop", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            btnAjouterModifierSupprimer.Content = Etat;
            lblTitre.Text = $"{Etat} un produit";

            switch (Etat)
            {
                case EtatFormulaire.Ajouter:
                    break;
                case EtatFormulaire.Modifier:
                case EtatFormulaire.Supprimer:
                    txtNom.Text = Produit.Nom;
                    txtCode.Text = Produit.Code;
                    txtPrix.Text = Produit.Prix.ToString();
                    cboCategories.SelectedItem = Produit.Categorie;
                    imgProduit.Source = new BitmapImage(new Uri(Produit.Image));
                    break;
            }
        }

        private bool ValiderFormulaire()
        {
            if (string.IsNullOrWhiteSpace(txtNom.Text))
            {
                MessageBox.Show("Le nom du produit est requis.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtCode.Text))
            {
                MessageBox.Show("Le code du produit est requis.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!decimal.TryParse(txtPrix.Text, out _))
            {
                MessageBox.Show("Le prix doit être un nombre valide.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!int.TryParse(txtQuantite.Text, out _))
            {
                MessageBox.Show("La quantité doit être un entier valide.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (cboCategories.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner une catégorie.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

    }
}
