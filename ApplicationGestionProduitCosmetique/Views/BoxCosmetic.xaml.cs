using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ApplicationGestionProduitCosmetique.Classes
{
    public partial class BoxCosmetic : Window
    {
        public BoxCosmetic(string message, string type = "erreur")
        {
            InitializeComponent();
            txtErreur.Text = message;
            AfficherIcone(type);
        }

        private void Fermer_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AfficherIcone(string type)
        {
            string chemin = "pack://application:,,,/Images/Icons/";

            switch (type.ToLower())
            {
                case "info":
                    chemin += "info.png";
                    break;
                case "warning":
                    chemin += "warning.png";
                    break;
                case "erreur":
                default:
                    chemin += "erreur.png";
                    break;
            }

            
        }

        // Méthodes statiques pour afficher la fenêtre
        public static void Afficher(string message)
        {
            new BoxCosmetic(message).ShowDialog();
        }

        public static void Afficher(string message, string type)
        {
            new BoxCosmetic(message, type).ShowDialog();
        }

        public static void Afficher(string message, string type, string titre)
        {
            BoxCosmetic fenetre = new BoxCosmetic(message, type);
            fenetre.Title = "Cosmetic Shop - " + titre;
            fenetre.ShowDialog();
        }
    }
}