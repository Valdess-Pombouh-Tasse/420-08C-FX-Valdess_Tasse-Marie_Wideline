using ApplicationGestionProduitCosmetique.Classes;
using ApplicationGestionProduitCosmetique.Models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ApplicationGestionProduitCosmetique.Views
{
    /// <summary>
    /// Logique d'interaction pour Factures.xaml
    /// </summary>
    public partial class Factures : UserControl
    {
        private Facture factureActuelle;

        public Factures(Facture nouvelleFacture = null)
        {
            InitializeComponent();
            factureActuelle = nouvelleFacture;
            ChargerFacturesDepuisBD();
            if (factureActuelle != null)
            {
                lstFactures.SelectedItem = factureActuelle;
            }
        }

        private void ChargerFacturesDepuisBD()
        {
            try
            {
                List<Facture> factures = GestionCompteUtilisateur.ObtenirToutesLesFactures();
                lstFactures.ItemsSource = factures;

                // Si aucune facture n’a été passée, on peut sélectionner la première disponible
                if (factureActuelle == null && factures.Count > 0)
                {
                    factureActuelle = factures[0];
                    lstFactures.SelectedItem = factureActuelle;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement des factures : " + ex.Message, "Cosmetic Shop", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //public Factures(Facture facture)
        //{
        //    InitializeComponent();
        //    factureActuelle = facture;
        //    AfficherFacture();
        //}

        //private void AfficherFacture()
        //{
        //    List<Facture> liste = new List<Facture>();
        //    liste.Add(factureActuelle);
        //    lstFactures.ItemsSource = liste;
        //}

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

            string cheminPDF = Path.Combine(
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


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (factureActuelle == null)
            {
                MessageBox.Show("Aucune facture sélectionnée.", "Cosmetic Shop", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                string numeroUnique = $"CS-{DateTime.Now:yyyyMMdd}-{factureActuelle.Id:D4}";

                string cheminSource = Path.Combine(
                string cheminPDF = System.IO.Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    "FacturesCosmetiques",
                    $"Facture_{factureActuelle.Id}.pdf");

                if (!File.Exists(cheminSource))
                {
                    MessageBox.Show("Le fichier PDF n'existe pas.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Fichier PDF (*.pdf)|*.pdf";
                saveFileDialog.FileName = $"Facture_{numeroUnique}.pdf";
                saveFileDialog.Title = "Enregistrer la facture";

                if (saveFileDialog.ShowDialog() == true)
                {
                    File.Copy(cheminSource, saveFileDialog.FileName, true);
                    MessageBox.Show("Facture enregistrée avec succès !", "Cosmetic Shop", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de l'enregistrement du PDF : " + ex.Message, "Cosmetic Shop", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}