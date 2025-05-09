using ApplicationGestionProduitCosmetique.Views;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
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

        private void videoPlayer2_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            MessageBox.Show("La vidéo n'a pas pu être chargée : " + e.ErrorException.Message);
        }

        private void btnConnexion_Click(object sender, RoutedEventArgs e)
        {
            monContentControl.Content = new FormConnexion();

        }

        private void btnCreation_Click(object sender, RoutedEventArgs e)
        {
            monContentControl.Content = new FormCreationcompte();
            txtCreerCompte.Text = "Vous n'avez pas de Compte";
            btnCreation.Visibility = Visibility.Visible;
            btnCreation.Width = 100;
            btnConnexion.Visibility = Visibility.Hidden;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            FormVideo formVideo = new FormVideo();
            formVideo.ShowDialog();


        }
    }
}
       
