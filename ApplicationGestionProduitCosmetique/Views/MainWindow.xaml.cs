using ApplicationGestionProduitCosmetique.Classes;
using ApplicationGestionProduitCosmetique.Models;
using ApplicationGestionProduitCosmetique.Views;
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

        
        private void btnConnexion_Click(object sender, RoutedEventArgs e)
        {
            FormConnexion frmConnexion = new FormConnexion();
            frmConnexion.ShowDialog();
        }

        private void btnCreation_Click(object sender, RoutedEventArgs e)
        {
            FormCreationcompte formCreationcompte = new FormCreationcompte();
            formCreationcompte.ShowDialog();
            Close();
            txtCreerCompte.Text = "Vous n'avez pas de Compte";
            btnCreation.Visibility = Visibility.Visible;
            btnCreation.Width = 100;
            btnConnexion.Visibility = Visibility.Hidden;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
       
