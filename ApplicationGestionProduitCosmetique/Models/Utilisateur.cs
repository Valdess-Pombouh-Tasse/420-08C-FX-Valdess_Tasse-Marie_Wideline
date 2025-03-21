using ApplicationGestionProduitCosmetique.enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

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

        //public int Id { get; set; }
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
                //if (string.IsNullOrWhiteSpace(value))
                //    throw new ArgumentException(nameof(MotDePasse), "Le mot de passe ne peut pas être vide");
                //if (!EstMotDePasseValide(value))
                    //    throw new ArgumentException(nameof(MotDePasse), $"Le mot de passe doit contenir au moins {PASSWORD_MIN_LENGTH} caractères, une majuscule et un caractère spécial");
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



        ///// <summary>
        ///// Vérifie si le mot de passe est valide
        ///// </summary>
        ///// <param name="motDePasse"></param>
        ///// <returns></returns>
        //public static bool EstMotDePasseValide(string motDePasse)
        //{
        //    if (motDePasse.Length < PASSWORD_MIN_LENGTH)
        //        return false;

        //    bool contientMajuscule = false;
        //    bool contientSpecial = false;

        //    foreach (char c in motDePasse)
        //    {
        //        if (char.IsUpper(c))
        //            contientMajuscule = true;
        //        if (!char.IsLetterOrDigit(c))
        //            contientSpecial = true;
        //    }

        //    return contientMajuscule && contientSpecial;
        //}

        public override string ToString()
        {
            return $"{NomUtilisateur.PadLeft(20)}{CodeUtilisateur.PadRight(10)}{Role,-10}";
        }
    }
}
