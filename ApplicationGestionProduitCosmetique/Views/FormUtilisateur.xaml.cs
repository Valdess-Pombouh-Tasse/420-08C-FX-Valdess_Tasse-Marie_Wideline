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

        private EtatFormulaire _etat;

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
            Utilisateurs = GestionCompteUtilisateur.ChargerUtilisateurs();
            dgUtilisateurs.ItemsSource = null;
            dgUtilisateurs.ItemsSource = Utilisateurs;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Utilisateurs = GestionCompteUtilisateur.ChargerUtilisateurs();
            AfficherUtilisateur();
            Utilisateurs = GestionCompteUtilisateur.ChargerUtilisateurs();
            AfficherUtilisateur();

            var factures = GestionCompteUtilisateur.GetFactures();
            dgFactures.ItemsSource = factures;
        }

        private void btnAjouter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FormAjoutModifSupp formAjout = new FormAjoutModifSupp
                {
                    Etat = EtatFormulaire.Ajouter,
                    //TypeUtilisateur = RoleUtilisateur.Vendeur
                };

                bool? resultat = formAjout.ShowDialog();

                if (resultat == true)
                {
                    Utilisateur nouvelUtilisateur = formAjout.Utilisateur;

                    if (Utilisateurs.Any(u => u.CodeUtilisateur == nouvelUtilisateur.CodeUtilisateur))
                    {
                        MessageBox.Show("Un utilisateur avec ce code existe déjà.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    GestionCompteUtilisateur.AjouterUtilisateur(nouvelUtilisateur);
                    AfficherUtilisateur();

                    MessageBox.Show("L'utilisateur a été ajouté avec succès !", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (InvalidOperationException ie)
            {
                MessageBox.Show(ie.Message, "Ajout d'un utilisateur", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur est survenue : " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnModifier_Click(object sender, RoutedEventArgs e)
        {

            if (dgUtilisateurs.SelectedItem is Utilisateur utilisateurSelect)
            {
                FormAjoutModifSupp formModif = new FormAjoutModifSupp();
                formModif.tbTitre.Text = $"Modifier un utilisateur";
                formModif.Utilisateur = utilisateurSelect;
                formModif.txtNom.Text = utilisateurSelect.NomUtilisateur;
                formModif.txtCode.Text = utilisateurSelect.CodeUtilisateur;
                formModif.txtPassword.Password = utilisateurSelect.MotDePasse;
                formModif.cboRole.SelectedItem = utilisateurSelect.Role;

                bool? resultat = formModif.ShowDialog();

                if (resultat == true)
                {
                    Utilisateur utilisateurModifie = formModif.Utilisateur;

                    try
                    {
                        GestionCompteUtilisateur.ModifierUtilisateur(utilisateurModifie);
                        AfficherUtilisateur();

                        MessageBox.Show("L'utilisateur a été modifié avec succès !", "Modification réussie", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erreur lors de la modification : " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un utilisateur à modifier.", "Aucune sélection", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }
        private void btnSupprimer_Click(object sender, RoutedEventArgs e)
        {
            if (dgUtilisateurs.SelectedItem is Utilisateur utilisateurSelectionne)
            {
                MessageBoxResult result = MessageBox.Show(
                    $"Êtes-vous sûr de vouloir supprimer l'utilisateur : {utilisateurSelectionne.NomUtilisateur} ?",
                    "Confirmation de suppression",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        GestionCompteUtilisateur.SupprimerUtilisateur(utilisateurSelectionne.CodeUtilisateur);
                        AfficherUtilisateur();

                        MessageBox.Show("L'utilisateur a été supprimé avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erreur lors de la suppression : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un utilisateur à supprimer.", "Information", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void dgFactures_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgFactures.SelectedItem != null)
            {
                var facture = (Facture)dgFactures.SelectedItem;

                // Affichez les détails de la facture
                string details = $"Facture {facture.Id}\nMontant Total: {facture.MontantTotal}";

                foreach (var prodFacture in facture.ProduitsFacture)
                {
                    details += $"\nProduit: {prodFacture.Produit.Nom}, Quantité: {prodFacture.Quantite}, Prix Unitaire: {prodFacture.PrixUnitaire}";
                }

                MessageBox.Show(details, "Détails de la Facture", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
