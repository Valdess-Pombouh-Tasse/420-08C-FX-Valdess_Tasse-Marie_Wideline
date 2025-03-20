using ApplicationGestionProduitCosmetique.Classes;
using ApplicationGestionProduitCosmetique.enums;
using ApplicationGestionProduitCosmetique.Models;
using ApplicationGestionProduitCosmetique.Views;
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
        public FormVendeur(int idVendeur)
        {
            InitializeComponent();
            ChargerProduitsVendeur(idVendeur);
        }

        private void ChargerProduitsVendeur(int idVendeur)
        {
            try
            {
                List<Produit> produitsVendeur = GestionProduits.ObtenirProduitsParVendeur(idVendeur);
            }
            catch (Exception ex)
            {
                FenetreErreur.Afficher("Erreur lors du chargement des produits : " + ex.Message);
            }
        }

    }
}
