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

        private Facture factureReçue;

        public Factures(Facture facture)
        {
            InitializeComponent();
            factureReçue = facture;
            lstFactures.ItemsSource = new List<Facture> { factureReçue }; // pour afficher une seule facture
        }
        private void BtnEnvoyerEmail_Click(object sender, RoutedEventArgs e)
        {
            string emailClient = Microsoft.VisualBasic.Interaction.InputBox(
                "Entrez votre adresse email :", "Envoi de facture", "client@email.com");

            if (string.IsNullOrWhiteSpace(emailClient) || !emailClient.Contains("@"))
            {
                MessageBox.Show("Adresse email invalide.", "Cosmetic Shop", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                string dossierFactures = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "FacturesCosmetiques");
                Directory.CreateDirectory(dossierFactures);
                string nomFichier = $"Facture_{factureReçue.Id}.pdf";
                string cheminComplet = System.IO.Path.Combine(dossierFactures, nomFichier);

                FacturePdf.GenererFacturePDF(factureReçue, cheminComplet);
                Mail.EnvoyerFacture(emailClient, cheminComplet);

                MessageBox.Show("Facture envoyée avec succès !", "Cosmetic Shop", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'envoi : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnFermer_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        ///// <summary>
        ///// Dossier contenant les factures
        ///// </summary>
        //string dossierFactures;
        //public Factures()
        //{
        //    InitializeComponent();
        //}

        ///// <summary>
        ///// Charge les factures à partir du dossier spécifié
        ///// </summary>
        //private void ChargerFactures()
        //{
        //    if (Directory.Exists(dossierFactures))
        //    {
        //        var fichiers = Directory.GetFiles(dossierFactures, "*.pdf");
        //        lstFactures.ItemsSource = fichiers.Select(Path.GetFileName).ToList();
        //    }
        //}

        ///// <summary>
        ///// Événement déclenché lorsque la fenêtre est chargée
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void Window_Loaded(object sender, RoutedEventArgs e)
        //{
        //    dossierFactures = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Factures");
        //    ChargerFactures();
        //}

        ///// <summary>
        ///// Événement déclenché lorsque le bouton "Ouvrir" est cliqué
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void BtnOuvrir_Click(object sender, RoutedEventArgs e)
        //{
        //    if (lstFactures.SelectedItem != null)
        //    {
        //        string fichier = Path.Combine(dossierFactures, lstFactures.SelectedItem.ToString());
        //        Process.Start("explorer", fichier); 
        //    }
        //}

    }
}
