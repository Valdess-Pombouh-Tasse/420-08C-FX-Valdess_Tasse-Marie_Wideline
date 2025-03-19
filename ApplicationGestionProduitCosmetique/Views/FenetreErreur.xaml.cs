using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ApplicationGestionProduitCosmetique.Classes
{
    /// <summary>
    /// Logique d'interaction pour FenetreErreur.xaml
    /// </summary>
    public partial class FenetreErreur : Window
    {
        public FenetreErreur(string message)
        {
            InitializeComponent();
            txtErreur.Text = message;
        }
        private void Fermer_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Méthode statique pour afficher la fenêtre d'erreur.
        /// </summary>
        public static void Afficher(string message)
        {
            FenetreErreur fenetre = new FenetreErreur(message);
            fenetre.ShowDialog();
        }
    }
}

