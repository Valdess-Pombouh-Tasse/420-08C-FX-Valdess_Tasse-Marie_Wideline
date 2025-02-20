using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGestionProduitCosmetique.Models
{
    public class Client
    {
        //Constantes
        public const int PASSWORD_MIN_LENGTH = 8;
        public const string VALID_COURRIEL = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        public const string VALID_PASSWORD = @"^(?=.*[A-Z])(?=.*[\W_]).{8,}$";

        /// <summary>
        /// Nom de l'utilisateur
        /// </summary>
        private string _nom;

        /// <summary>
        /// Courriel de l'utilisateur
        /// </summary>
        private string _courriel;

        /// <summary>
        /// Mot de passe de l'utilisateur
        /// </summary>
        private string _motDePasse;

        /// <summary>
        /// Nom de l'utilisateur
        /// </summary>
        public string Nom
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
            set {
                    if (string.IsNullOrWhiteSpace(value))
                        throw new ArgumentException(nameof(Courriel), "Le courriel ne peut pas être vide");

                    if(!System.Text.RegularExpressions.Regex.IsMatch(value, VALID_COURRIEL))
                    throw new ArgumentException(nameof(Courriel), "Le courriel n'est pas valide");
                _courriel = value.Trim(); }
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

                if (value.Length < PASSWORD_MIN_LENGTH)
                    throw new ArgumentException(nameof(MotDePasse), "Le mot de passe doit contenir au moins 8 caractères");

                if (!System.Text.RegularExpressions.Regex.IsMatch(value, VALID_PASSWORD))
                         throw new ArgumentException(nameof(MotDePasse), "Le mot de passe doit contenir au moins une lettre majuscule et un caractère spécial");
                _motDePasse = value.Trim(); }
        }

        public Client(string nom, string courriel, string motDePasse)
        {
            Nom = nom;
            Courriel = courriel;
            MotDePasse = motDePasse;
        }
    }
}
