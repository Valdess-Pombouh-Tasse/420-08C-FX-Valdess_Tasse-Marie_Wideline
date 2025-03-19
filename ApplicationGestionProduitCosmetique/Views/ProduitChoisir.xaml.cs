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
    /// Logique d'interaction pour ProduitChoisir.xaml
    /// </summary>
    public partial class ProduitChoisir : Window
    {
        public ProduitChoisir()
        {
            InitializeComponent();
        }

        private void btnEnreigistrerLst_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //lvListePanier.ItemsSource = panier.Items;
        }
    }
}
