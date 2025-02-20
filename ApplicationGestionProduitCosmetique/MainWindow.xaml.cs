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
        }



        private bool ValidationFormulaire()
        {
            StringBuilder sb = new StringBuilder();

            if (string.IsNullOrWhiteSpace(txtNom.Text))
                sb.AppendLine("Le nom ne peut pas être vide");
            if (string.IsNullOrWhiteSpace(txtCourriel.Text) || !Client.EstCourrielValide(txtCourriel.Text))
                sb.AppendLine("Le courriel ne peut pas être vide et doit respecter la forme initiale@gmail.com.");
            if (string.IsNullOrWhiteSpace(txtPassword.Password) || !Client.EstMotDePasseValide(txtPassword.Password))
                sb.AppendLine($"Le mot de passe ne peut pas être vide et doit avoir au moins {Client.PASSWORD_MIN_LENGTH}");

            if (sb.Length > 0)
            {
                MessageBox.Show(sb.ToString(), "Validation du client");
                return false;
            }
            return true;
        }
        private void btnConnecter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValidationFormulaire())
                {

                    string nom = txtNom.Text;
                    string Courriel = txtCourriel.Text;
                    string MotDePasse = txtPassword.Password;

                    MessageBox.Show("Compte créé avec succès", "Création du compte client");
                    DialogResult = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur de création du client");
            }
        }


    }
}