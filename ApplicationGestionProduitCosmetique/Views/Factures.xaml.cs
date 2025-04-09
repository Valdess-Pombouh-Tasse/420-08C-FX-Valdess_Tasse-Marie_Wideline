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

namespace ApplicationGestionProduitCosmetique.Views
{
    /// <summary>
    /// Logique d'interaction pour Factures.xaml
    /// </summary>
    public partial class Factures : Window
    {
        /// <summary>
        /// Dossier contenant les factures
        /// </summary>
        string dossierFactures;
        public Factures()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Charge les factures à partir du dossier spécifié
        /// </summary>
        private void ChargerFactures()
        {
            if (Directory.Exists(dossierFactures))
            {
                var fichiers = Directory.GetFiles(dossierFactures, "*.pdf");
                lstFactures.ItemsSource = fichiers.Select(Path.GetFileName).ToList();
            }
        }

        /// <summary>
        /// Événement déclenché lorsque la fenêtre est chargée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dossierFactures = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Factures");
            ChargerFactures();
        }

        /// <summary>
        /// Événement déclenché lorsque le bouton "Ouvrir" est cliqué
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOuvrir_Click(object sender, RoutedEventArgs e)
        {
            if (lstFactures.SelectedItem != null)
            {
                string fichier = Path.Combine(dossierFactures, lstFactures.SelectedItem.ToString());
                Process.Start("explorer", fichier); 
            }
        }

    }
}
