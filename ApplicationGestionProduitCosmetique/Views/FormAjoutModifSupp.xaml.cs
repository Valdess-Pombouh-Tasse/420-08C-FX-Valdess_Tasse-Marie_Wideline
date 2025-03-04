using ApplicationGestionProduitCosmetique.Classes;
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
        public EtatFormulaire Etat { get; set; }

        public RoleUtilisateur TypeUtilisateur { get; set; }

        public Utilisateur Utilisateur { get; set; }

        public FormAjoutModifSupp()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Permet de valider les saisies dans le formulaire utilisateur
        /// </summary>
        /// <returns>Vrai si le formulaire est valide</returns>
        private bool ValiderFormulaireUtilisateur()
        {
            string messageErreur = "";

            // Validation du code utilisateur
            if (string.IsNullOrWhiteSpace(txtCode.Text) || txtCode.Text.Trim().Length < 3)
            {
                messageErreur += "- Le code utilisateur est obligatoire et doit contenir au moins 3 caractères.\n";
            }

            // Validation du nom
            if (string.IsNullOrWhiteSpace(txtNom.Text) || txtNom.Text.Trim().Length < 3)
            {
                messageErreur += "- Le nom est obligatoire et doit contenir au moins 3 caractères.\n";
            }

            // Validation du mot de passe
            if (string.IsNullOrWhiteSpace(txtPassword.Password) || txtPassword.Password.Trim().Length < Utilisateur.PASSWORD_MIN_LENGTH)
            {
                messageErreur += "- Le mot de passe est obligatoire et doit contenir au moins 6 caractères et un caractère spécial.\n";
            }

            // Validation de la sélection d'un rôle
            if (cboRole.SelectedIndex == -1)
            {
                messageErreur += "- Vous devez sélectionner un rôle.\n";
            }

            if (messageErreur != "")
            {
                MessageBox.Show(messageErreur, "Enregistrement des modifications", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            return messageErreur == "";
        }
        private void btnModifier_Click(object sender, RoutedEventArgs e)
        {

            if (ValiderFormulaireUtilisateur())
            {
                if (Etat == EtatFormulaire.Ajouter)
                {
                    if (TypeUtilisateur == RoleUtilisateur.Client)
                    {
                        Utilisateur = new Client(
                            txtCode.Text,
                            txtNom.Text,
                           txtPassword.Password,
                            (RoleUtilisateur)cboRole.SelectedIndex
                        );
                    }
                    else
                    {
                        Utilisateur = new Vendeur(
                            txtCode.Text,
                            txtNom.Text,
                            txtPassword.Password,
                            (RoleUtilisateur)cboRole.SelectedIndex
                        );
                    }
                }

                else
                {
                    if (Utilisateur is Client client)
                    {
                        client.CodeUtilisateur = txtCode.Text;
                        client.NomUtilisateur = txtNom.Text;
                        client.MotDePasse = txtPassword.Password;
                        client.Role = (RoleUtilisateur)cboRole.SelectedIndex;
                    }
                    else if (Utilisateur is Vendeur vendeur)
                    {
                        vendeur.CodeUtilisateur = txtCode.Text;
                        vendeur.NomUtilisateur = txtNom.Text;
                        vendeur.MotDePasse = txtPassword.Password;
                        vendeur.Role = (RoleUtilisateur)cboRole.SelectedIndex;
                    }
                }
                DialogResult = true;
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

        /// <summary>
        /// Permet d'afficher les types d'utilisateur
        /// </summary>
        private void InitialiserRoleUtilisateur()
        {
            string[] vectRoleUtilisateur = EnumUtilitaire.GetAllDescriptions<RoleUtilisateur>();

            for (int i = 0; i < vectRoleUtilisateur.Length; i++)
            {
                cboRole.Items.Add(vectRoleUtilisateur[i]);
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitialiserRoleUtilisateur();
            btnModifier.Content = Etat;
            tbTitre.Text = $"{Etat} un {TypeUtilisateur}";

            if (Etat != EtatFormulaire.Ajouter && Utilisateur != null)
            {
                txtCode.Text = Utilisateur.CodeUtilisateur;
                txtNom.Text = Utilisateur.NomUtilisateur;
                txtPassword.Password = Utilisateur.MotDePasse;
                cboRole.SelectedItem = EnumUtilitaire.GetDescription(Utilisateur.Role);

                if (Etat == EtatFormulaire.Supprimer)
                {
                    txtCode.IsEnabled = false;
                    txtNom.IsEnabled = false;
                    txtPassword.IsEnabled = false;
                    cboRole.IsEnabled = false;
                }
            }
        }
    }
}
