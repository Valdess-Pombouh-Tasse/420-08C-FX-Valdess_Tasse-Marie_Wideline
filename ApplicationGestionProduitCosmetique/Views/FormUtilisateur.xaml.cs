using ApplicationGestionProduitCosmetique.Classes;
using ApplicationGestionProduitCosmetique.enums;
using ApplicationGestionProduitCosmetique.Models;
using ApplicationGestionProduitCosmetique.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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

        public RoleUtilisateur TypeUtilisateur { get; set; }

        public FormUtilisateur(EtatFormulaire etat = EtatFormulaire.Ajouter)
        {
            InitializeComponent();
            Etat = etat;
        }

        private void AfficherFactures()
        {
            try
            {
                List<Facture> factures = GestionCompteUtilisateur.ObtenirToutesLesFactures();
                dgFactures.ItemsSource = null;
                dgFactures.ItemsSource = factures;
            }
            catch (Exception ex)
            {
                FenetreErreur.Afficher("Erreur lors du chargement des factures : " + ex.Message);
            }
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
            AfficherFactures();
        }

        private void btnAjouter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FormAjoutModifSupp formAjout = new FormAjoutModifSupp
                {
                    Etat = EtatFormulaire.Ajouter,
                };
                formAjout.tbTitre.Text = $"{Etat} un {TypeUtilisateur}";

                bool? resultat = formAjout.ShowDialog();

                if (resultat == true)
                {
                    Utilisateur nouvelUtilisateur = formAjout.Utilisateur;

                    if (Utilisateurs.Any(u => u.CodeUtilisateur == nouvelUtilisateur.CodeUtilisateur))
                    {
                        BoxCosmetic.Afficher("Un utilisateur avec ce code existe déjà.", "Erreur");
                        return;
                    }

                    GestionCompteUtilisateur.AjouterUtilisateur(nouvelUtilisateur);
                    AfficherUtilisateur();

                    BoxCosmetic.Afficher("L'utilisateur a été ajouté avec succès !", "Confirmation");
                }
            }
            catch (InvalidOperationException ie)
            {
                BoxCosmetic.Afficher(ie.Message, "Ajout d'un utilisateur");
            }
            catch (Exception ex)
            {
                BoxCosmetic.Afficher("Une erreur est survenue : " + ex.Message, "Erreur");
            }
        }

        private void btnModifier_Click(object sender, RoutedEventArgs e)
        {
            if (dgUtilisateurs.SelectedItem is Utilisateur utilisateurSelect)
            {
                FormAjoutModifSupp formModif = new FormAjoutModifSupp
                {
                    tbTitre = { Text = "Modifier un client" },
                    Utilisateur = utilisateurSelect,
                    txtNom = { Text = utilisateurSelect.NomUtilisateur },
                    txtCode = { Text = utilisateurSelect.CodeUtilisateur },
                    txtPassword = { Password = utilisateurSelect.MotDePasse },
                    cboRole = { SelectedItem = utilisateurSelect.Role }
                };

                bool? resultat = formModif.ShowDialog();

                if (resultat == true)
                {
                    try
                    {
                        GestionCompteUtilisateur.ModifierUtilisateur(formModif.Utilisateur);
                        AfficherUtilisateur();

                        BoxCosmetic.Afficher("L'utilisateur a été modifié avec succès !", "Modification réussie");
                    }
                    catch (Exception ex)
                    {
                        BoxCosmetic.Afficher("Erreur lors de la modification : " + ex.Message, "Erreur");
                    }
                }
            }
            else
            {
                BoxCosmetic.Afficher("Veuillez sélectionner un utilisateur à modifier.", "Aucune sélection");
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

                        BoxCosmetic.Afficher("L'utilisateur a été supprimé avec succès.", "Succès");
                    }
                    catch (Exception ex)
                    {
                        BoxCosmetic.Afficher($"Erreur lors de la suppression : {ex.Message}", "Erreur");
                    }
                }
            }
            else
            {
                BoxCosmetic.Afficher("Veuillez sélectionner un utilisateur à supprimer.", "Information");
            }
        }
    }
}