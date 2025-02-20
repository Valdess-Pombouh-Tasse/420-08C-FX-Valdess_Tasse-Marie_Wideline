using ApplicationGestionProduitCosmetique.Classes;
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

        private bool ValidationFormulaire()
        {
            StringBuilder sb = new StringBuilder();

            if (string.IsNullOrWhiteSpace(txtNom.Text))
                sb.AppendLine("Le nom ne peut pas être vide");
            if (string.IsNullOrWhiteSpace(txtCourriel.Text) || txtCourriel.Text != Client.VALID_COURRIEL)
                sb.AppendLine("Le courriel ne peut pas être vide");
            if (string.IsNullOrWhiteSpace(txtPassword.Password) || txtPassword.Password.Length < Client.PASSWORD_MIN_LENGTH || txtPassword.Password != Client.VALID_PASSWORD)
                sb.AppendLine("Le mot de passe ne peut pas être vide");

            if (sb.Length > 0)
            {
                MessageBox.Show(sb.ToString(), "Validation du client");
                return false;
            }
            return true;
        }
        private void btnConnecter_Click(object sender, RoutedEventArgs e)
        {

        }


    }
}