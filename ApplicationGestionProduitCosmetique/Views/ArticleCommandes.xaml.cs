using ApplicationGestionProduitCosmetique.Classes;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ApplicationGestionProduitCosmetique.Views
{
    /// <summary>
    /// Logique d'interaction pour ArticleCommandes.xaml
    /// </summary>
    public partial class ArticleCommandes : UserControl
    {
        public ArticleCommandes(Facture nouvelleFacture)
        {
            InitializeComponent();
            this.DataContext = this;
            ChargerFacturesDepuisBD();
        }



        private void ChargerFacturesDepuisBD()
        {
            try
            {
                List<Facture> factures = GestionCompteUtilisateur.ObtenirToutesLesFactures(); 
                lstFactures.ItemsSource = factures;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement des factures : " + ex.Message);
            }
        }
    }
}
