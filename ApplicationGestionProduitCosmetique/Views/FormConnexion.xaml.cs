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

namespace ApplicationGestionProduitCosmetique
{
    /// <summary>
    /// Logique d'interaction pour FormConnexion.xaml
    /// </summary>
    public partial class FormConnexion : Window
    {
        public FormConnexion()
        {
            InitializeComponent();
        }

        private bool ValidationFormulaire()
        {
            StringBuilder sb = new StringBuilder();

            if (string.IsNullOrWhiteSpace(txtNom.Text))
                sb.AppendLine("Le nom ne peut pas être vide");
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

                    string nomUtilisateur = txtNom.Text;
                    string motDePasse = txtPassword.Password;
                    Utilisateurs utilisateur = new Client(nomUtilisateur, motDePasse);

                    if (utilisateur.SeConnecter(nomUtilisateur, motDePasse))
                    {
                        MessageBox.Show("Connexion réussie", "Connexion");
                        FormApplication frmApplication = new FormApplication();
                        frmApplication.Show();
                        this.DialogResult = true;
                    }
                    else
                    {
                        MessageBox.Show("Nom ou mot de passe non valide", "Connexion");
                        DialogResult = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
