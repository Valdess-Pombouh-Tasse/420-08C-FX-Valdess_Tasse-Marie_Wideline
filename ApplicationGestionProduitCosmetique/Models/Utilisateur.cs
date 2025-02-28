using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGestionProduitCosmetique.Models
{
    public abstract class Utilisateur
    {
        public const int NB_CARAC_MIN = 3;
        public const int NB_CARAC_MAX = 15;
        public const int PASSWORD_MIN_LENGTH = 6;

        private string? _nomUtilisateur;
        private string? _codeUtilisateur;
        private string? _motDePasse;
      

        public string? NomUtilisateur
        {
            get { return _nomUtilisateur; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(nameof(NomUtilisateur), "Le nom ne peut pas être vide");

                if (value.Length < NB_CARAC_MIN || value.Length > NB_CARAC_MAX)
                    throw new ArgumentOutOfRangeException(nameof(NomUtilisateur), $"Le nom doit contenir entre {NB_CARAC_MIN} et {NB_CARAC_MAX} caractères");

                _nomUtilisateur = value.Trim();
            }
        }
        public string? CodeUtilisateur
        {
            get { return _codeUtilisateur; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(nameof(CodeUtilisateur), "Le code utilisateur ne peut pas être vide");

                _codeUtilisateur = value.Trim();
            }
        }

        public string? MotDePasse
        {
            get { return _motDePasse; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(nameof(MotDePasse), "Le mot de passe ne peut pas être vide");

                
            }
        }

        protected Utilisateur(string? nomUtilisateur, string? motDePasse)
        {
            NomUtilisateur = nomUtilisateur;
           // CodeUtilisateur = codeUtilisateur;
            MotDePasse = motDePasse;
        }

       
        //public virtual bool EstMotDePasseValide(string motDePasse)
        //{
        //    if (motDePasse.Length >= PASSWORD_MIN_LENGTH)
        //        return false;

        //    bool contientMajuscule = motDePasse.Any(char.IsUpper);
        //    bool contientSpecial = motDePasse.Any(c => !char.IsLetterOrDigit(c));

        //    return contientMajuscule && contientSpecial;
        //}
    }
}

