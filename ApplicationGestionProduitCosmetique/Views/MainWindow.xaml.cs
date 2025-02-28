using ApplicationGestionProduitCosmetique.Classes;
using ApplicationGestionProduitCosmetique.Models;
using ApplicationGestionProduitCosmetique.Views;
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

        //private void btnCarrieres_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        // Affichage du formulaire pour ajouter un nouveau vendeur
        //        FormVendeur frmVendeur = new FormVendeur();
        //        bool? resultat = frmVendeur.ShowDialog();

        //        if (resultat == true)
        //        {
        //            // Récupération du nouveau vendeur à partir du formulaire
        //            Vendeur nouveauVendeur = frmVendeur.Vendeur;

        //            // Vérification de l'existence du vendeur dans la base de données
        //            if (!GestionCompteUtilisateur.VendeurExiste(nouveauVendeur.Id))
        //            {
        //                GestionCompteUtilisateur.AjouterVendeur(nouveauVendeur);
        //                MessageBox.Show("Le vendeur a été ajouté avec succès !", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
        //            }
        //            else
        //            {
        //                MessageBox.Show("Ce vendeur existe déjà dans la base de données.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
        //            }
        //        }
        //    }
        //    catch (InvalidOperationException ie)
        //    {
        //        MessageBox.Show(ie.Message, "Ajout d'un vendeur", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Une erreur est survenue : " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}

        private void btnConnexion_Click(object sender, RoutedEventArgs e)
        {
            FormConnexion frmConnexion = new FormConnexion();
            frmConnexion.ShowDialog();
        }

        private void btnCreation_Click(object sender, RoutedEventArgs e)
        {
            FormCreationcompte formCreationcompte = new FormCreationcompte();
            formCreationcompte.ShowDialog();

        }
    }
}
       
