using ApplicationGestionProduitCosmetique.Classes;
using ApplicationGestionProduitCosmetique.enums;
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
    /// Logique d'interaction pour FormVendeur.xaml
    /// </summary>
    public partial class FormUtilisateur : Window
    {
        private List<Utilisateur> _utilisateurs = new List<Utilisateur>();

        public List<Utilisateur> Utilisateurs
        {
            get { return _utilisateurs; }
            set { _utilisateurs = value; }
        }

        private EtatFormulaire  _etat;

        public EtatFormulaire Etat
        {
            get { return _etat; }
            set { _etat = value; }
        }


        public FormUtilisateur(EtatFormulaire etat = EtatFormulaire.Ajouter)
        {
            InitializeComponent();
            Etat = etat;
        }

        public void AfficherUtilisateur()
        {
            lstUtilisateurs.Items.Clear();

            if (_utilisateurs is not null)
            {
                foreach (Utilisateur utilisateur in Utilisateurs)
                {
                    lstUtilisateurs.Items.Add(utilisateur);
                }
            }
        }


        
      

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Utilisateurs = GestionCompteUtilisateur.ChargerUtilisateurs();
            AfficherUtilisateur();
        }

        private void btnAjouter_Click(object sender, RoutedEventArgs e)
        {
            FormAjoutModifSupp formAjout = new FormAjoutModifSupp();
            formAjout.ShowDialog(); 
        }



        private void btnModifier_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnSupprimer_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
