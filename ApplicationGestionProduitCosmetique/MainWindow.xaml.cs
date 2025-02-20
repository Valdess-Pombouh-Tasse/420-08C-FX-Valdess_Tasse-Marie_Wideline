using ApplicationGestionProduitCosmetique.Classes;
using MySqlConnector;
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


            // Création d'un objet Vendeur
            Vendeur nouveauVendeur = new Vendeur(12,"Dupont","Arnélia",DateTime.Now);
       

            // Appel de la méthode AjouterVendeur
            try
            {
                GestionCompteUtilisateur.AjouterVendeur(nouveauVendeur);
                MessageBox.Show("Vendeur ajouté avec succès !");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur est survenue : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}