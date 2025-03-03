using ApplicationGestionProduitCosmetique.enums;
using ApplicationGestionProduitCosmetique.Models;
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

namespace ApplicationGestionProduitCosmetique.Views
{
    /// <summary>
    /// Logique d'interaction pour FormAjoutModifSupp.xaml
    /// </summary>
    public partial class FormAjoutModifSupp : Window
    {
        private EtatFormulaire _etat;

        public EtatFormulaire Etat
        {
            get { return _etat; }
            set { _etat = value; }
        }

        private Vendeur _vendeur;

        public Vendeur Vendeur
        {
            get { return _vendeur; }
            set { _vendeur = value; }
        }

        private Client _client;

        public Client Client
        {
            get { return _client; }
            set { _client = value; }
        }




        public FormAjoutModifSupp()
        {
            InitializeComponent();
        }

        private void btnModifier_Click(object sender, RoutedEventArgs e)
        {
            if (Etat != EtatFormulaire.Supprimer)
            {

                    if (Etat == EtatFormulaire.Ajouter)
                    {
                        //Créer un nouveau Vendeur
                        Vendeur = new Vendeur(txtCode.Text, txtNom.Text, (RoleUtilisateur)Enum.Parse(typeof(RoleUtilisateur), cboRole.SelectedIndex.ToString()));
                    }
                    else
                    {
                        Vendeur.CodeUtilisateur = txtCode.Text;
                        Vendeur.NomUtilisateur = txtNom.Text;
                        Vendeur.MotDePasse = txtMotPasse.Text;
                        Vendeur.Role = (RoleUtilisateur)cboRole.SelectedIndex;

                    }
                    DialogResult = true;
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Desirez vous supprimez le coureur", "Suppression coureur", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

                if (result == MessageBoxResult.Yes)
                {
                    DialogResult = true;
                }
                else
                {
                    DialogResult = false;
                }
            }
        }

        /// <summary>
        /// Permet d'annuler l'action d'ajout, de modification ou de suppression d'un coureur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

       
    }
}
