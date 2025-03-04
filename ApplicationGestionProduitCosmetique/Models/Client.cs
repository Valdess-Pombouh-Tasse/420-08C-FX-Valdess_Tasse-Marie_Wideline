using ApplicationGestionProduitCosmetique.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGestionProduitCosmetique.Models
{
    public class Client : Utilisateur
    {
      
        public Client(string codeUtilisateur,string nomUtilisateur,string motDePasse, RoleUtilisateur role) : base(codeUtilisateur,nomUtilisateur, motDePasse, role)
        {
        }

        //public static bool EstMotDePasseValide(string motDePasse)
        //{
        //    if (motDePasse.Length < PASSWORD_MIN_LENGTH)
        //        return false;

        //    bool contientMajuscule = motDePasse.Any(char.IsUpper);
        //    bool contientSpecial = motDePasse.Any(c => !char.IsLetterOrDigit(c));

        //    return contientMajuscule && contientSpecial;
        //}


    }
}
