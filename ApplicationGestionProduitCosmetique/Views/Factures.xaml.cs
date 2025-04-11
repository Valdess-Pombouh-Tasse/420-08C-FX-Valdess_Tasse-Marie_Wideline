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
using System.Diagnostics;
using System.IO;
using Path = System.IO.Path;
using ApplicationGestionProduitCosmetique.Models;

namespace ApplicationGestionProduitCosmetique.Views
{
    /// <summary>
    /// Logique d'interaction pour Factures.xaml
    /// </summary>
    public partial class Factures : Window
    {

        private Facture factureActuelle;

        public Factures(Facture facture)
        {
            InitializeComponent();
            factureActuelle = facture;
            MessageBox.Show($"Produits dans la facture : {factureActuelle.ProduitsFacture.Count}");
            AfficherFacture();
        }

        private void AfficherFacture()
        {
            List<Facture> liste = new List<Facture>();
            liste.Add(factureActuelle);
            lstFactures.ItemsSource = liste;
        }

        private void BtnEnvoyerEmail_Click(object sender, RoutedEventArgs e)
        {
            if (factureActuelle == null)
            {
                MessageBox.Show("Aucune facture sélectionnée.", "Cosmetic Shop", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string email = Microsoft.VisualBasic.Interaction.InputBox(
                "Entrez votre adresse email pour recevoir la facture :", "Envoi de la facture");

            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Adresse email invalide.", "Cosmetic Shop", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string cheminPDF = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "FacturesCosmetiques",
                $"Facture_{factureActuelle.Id}.pdf");

            if (File.Exists(cheminPDF))
            {
                Mail.EnvoyerFacture(email, cheminPDF);
                MessageBox.Show("Facture envoyée par email avec succès !", "Cosmetic Shop", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("La facture n'a pas été trouvée.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void BtnFermer_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string cheminPDF = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    "FacturesCosmetiques",
                    $"Facture_{factureActuelle.Id}.pdf");

                if (File.Exists(cheminPDF))
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(cheminPDF) { UseShellExecute = true });
                else
                    MessageBox.Show("Le fichier PDF n'existe pas.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de l'ouverture du PDF : " + ex.Message);
            }
        }
    }
}
