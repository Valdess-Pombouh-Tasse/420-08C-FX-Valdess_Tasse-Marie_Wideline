using ApplicationGestionProduitCosmetique.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGestionProduitCosmetique.Models
{
    public class Manager : Utilisateur
    {
        public Manager(string? nomUtilisateur, string codeUtilisateur, RoleUtilisateur roleUtilisateur) : base(nomUtilisateur, codeUtilisateur, RoleUtilisateur.Manager)
        {
        }
    }
}
