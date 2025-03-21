using ApplicationGestionProduitCosmetique.Classes;
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
        //public List<Produit> ListeProduits { get; set; }
        //public FormVendeur()
        //{
        //    InitializeComponent();
        //    lbProduits.ItemsSource = ListeProduits;
        //}
        public FormVendeur(int idUtilisateur)
        {
            IdUtilisateur = idUtilisateur;
        }

        public int IdUtilisateur { get; }
    }
}
