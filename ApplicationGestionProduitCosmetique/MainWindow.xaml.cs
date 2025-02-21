using ApplicationGestionProduitCosmetique.Models;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ApplicationGestionProduitCosmetique
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FormConnexion frmConnexion = new FormConnexion();
            bool? resultat = frmConnexion.ShowDialog();
   
            if (resultat == true)
            {
                MessageBox.Show("Contact ajouté avec succes!");
            }

        }
    }
}