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
            TestConnexion();
        }

        private void TestConnexion()
        {
            MySqlConnection connexion = null;
            try
            {
                connexion = DAL.CreerConnection();
                connexion.Open();
                MessageBox.Show("Connexion réussie à la base de données !");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur : {ex.Message}");
            }
            finally
            {
                if (connexion != null && connexion.State == System.Data.ConnectionState.Open)
                {
                    connexion.Close();
                }
            }
        }
    }
}