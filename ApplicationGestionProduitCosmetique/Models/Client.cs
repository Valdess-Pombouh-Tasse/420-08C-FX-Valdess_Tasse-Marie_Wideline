using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGestionProduitCosmetique.Models
{
    public class Client
    {
        //Constantes
        public const int PASSWORD_MIN_LENGTH = 6;
       
        /// <summary>
        /// Nom de l'utilisateur
        /// </summary>
        private string? _nom;

        /// <summary>
        /// Courriel de l'utilisateur
        /// </summary>
        private string _courriel = "";

        /// <summary>
        /// Mot de passe de l'utilisateur
        /// </summary>
        private string _motDePasse = "";

        /// <summary>
        /// Nom de l'utilisateur
        /// </summary>
        public string? Nom
        {
            get { return _nom; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(nameof(Nom), "Le nom ne peut pas être vide");

                _nom = value.Trim();
            }
        }

        /// <summary>
        /// Courriel de l'utilisateur
        /// </summary>
        public string Courriel
        {
            get { return _courriel; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(nameof(Courriel), "Le courriel ne peut pas être vide");

                if (!EstCourrielValide(value))
                    throw new ArgumentException(nameof(Courriel), "Le courriel n'est pas valide");

                _courriel = value.Trim();
            }
        }

        /// <summary>
        /// Mot de passe de l'utilisateur
        /// </summary>
        public string MotDePasse
        {
            get { return _motDePasse; }
            set {
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

        /// <summary>
        /// Vérifie si le courriel est valide
        /// </summary>
        /// <param name="courriel"></param>
        /// <returns></returns>
        public static bool EstCourrielValide(string courriel)
        {
            try
            {
                var adresseMail = new MailAddress(courriel);
                return adresseMail.Address == courriel;
            }
            catch
            {
                return false;
            }
        }

        public Client(string? nom, string courriel, string motDePasse)
        {
            Nom = nom;
            Courriel = courriel;
            MotDePasse = motDePasse;
        }
    }
}
