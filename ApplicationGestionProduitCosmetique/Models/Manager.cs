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
        public Manager(string codeUtilisateur, string nomUtilisateur, string motDePasse, RoleUtilisateur role) : base(codeUtilisateur, nomUtilisateur, motDePasse, role)
        {
        }
    }
}
