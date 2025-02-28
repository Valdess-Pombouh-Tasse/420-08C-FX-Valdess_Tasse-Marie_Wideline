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
        public Client(string? nomUtilisateur, string codeUtilisateur, RoleUtilisateur roleUtilisateur) : base(nomUtilisateur, codeUtilisateur, RoleUtilisateur.Client)
        {
        }
    }
}
