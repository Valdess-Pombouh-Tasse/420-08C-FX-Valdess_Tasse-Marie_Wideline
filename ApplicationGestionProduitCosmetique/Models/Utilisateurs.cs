using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGestionProduitCosmetique.Models
{
    public abstract class Utilisateurs
    {
        
        //Constantes
        public const int PASSWORD_MIN_LENGTH = 6;

        /// <summary>
        /// Nom de l'utilisateur
        /// </summary>
        private string? _nomUtilisateur;

        /// <summary>
        /// Mot de passe de l'utilisateur
        /// </summary>
        private string _motDePasse = "";

        /// <summary>
        /// Nom de l'utilisateur
        /// </summary>
        public string? NomUtilisateur
        {
            get { return _nomUtilisateur; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(nameof(NomUtilisateur), "Le nom ne peut pas être vide");

                _nomUtilisateur = value.Trim();
            }
        }


        /// <summary>
        /// Mot de passe de l'utilisateur
        /// </summary>
        public string MotDePasse
        {
            get { return _motDePasse; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(nameof(MotDePasse), "Le mot de passe ne peut pas être vide");

                if (!EstMotDePasseValide(value))
                    throw new ArgumentException(nameof(MotDePasse), "Le mot de passe doit contenir au moins 8 caractères");

                _motDePasse = value.Trim();
            }
        }

        /// <summary>
        /// Vérifie si le mot de passe est valide
        /// </summary>
        /// <param name="motDePasse"></param>
        /// <returns></returns>
        public static bool EstMotDePasseValide(string motDePasse)
        {
            if (motDePasse.Length < PASSWORD_MIN_LENGTH)
                return false;

            bool contientMajuscule = false;
            bool contientSpecial = false;

            foreach (char c in motDePasse)
            {
                if (char.IsUpper(c))
                    contientMajuscule = true;
                if (!char.IsLetterOrDigit(c)) // Vérifie si c'est un caractère spécial
                    contientSpecial = true;
            }

            return contientMajuscule && contientSpecial;
        }


        protected Utilisateurs(string? nomUtilisateur, string motDePasse)
        {
            NomUtilisateur = nomUtilisateur;
            MotDePasse = motDePasse;
        }

        /// <summary>
        /// permet à l'utilisateur de se connecter
        /// </summary>
        /// <param name="nomUtilisateur"></param>
        /// <param name="motDePasse"></param>
        public virtual bool SeConnecter(string nomUtilisateur, string motDePasse)
        {
            return nomUtilisateur == NomUtilisateur && motDePasse == MotDePasse;
  
        }

        public virtual void CreerConnecte(string nomUtilisateur, string motDePasse)
        {

        }
    }
}
