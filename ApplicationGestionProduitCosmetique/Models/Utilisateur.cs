using ApplicationGestionProduitCosmetique.enums;
using System;


namespace ApplicationGestionProduitCosmetique.Models
{
    public abstract class Utilisateur
    {
        public RoleUtilisateur Role { get; set; }
        
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
        /// Code de l'utilisateur
        /// </summary>
        private string _codeUtilisateur;


        /// <summary>
        /// Nom de l'utilisateur
        /// </summary>
        public string NomUtilisateur
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
        /// Code de l'utilisateur
        /// </summary>
        public string CodeUtilisateur
        {
            get { return _codeUtilisateur; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(nameof(CodeUtilisateur), "Le code utilisateur ne peut pas être vide");

                _codeUtilisateur = value.Trim();
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
                    _motDePasse = value.Trim();
            }
        }

        protected Utilisateur(string codeUtilisateur,string nomUtilisateur,string motDePasse, RoleUtilisateur role)
        {
            CodeUtilisateur = codeUtilisateur;
            NomUtilisateur = nomUtilisateur;
            MotDePasse = motDePasse;
            Role = role;
        }


        public override string ToString()
        {
            return $"{NomUtilisateur.PadLeft(20)}{CodeUtilisateur.PadRight(10)}{Role,-10}";
        }
    }
}
