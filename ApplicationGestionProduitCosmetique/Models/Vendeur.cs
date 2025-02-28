using ApplicationGestionProduitCosmetique.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGestionProduitCosmetique.Models
{
    public class Vendeur : Utilisateur
    {
        public Vendeur(string? nomUtilisateur, string codeUtilisateur, RoleUtilisateur roleUtilisateur) : base(nomUtilisateur, codeUtilisateur, RoleUtilisateur.Vendeur)
        {
        }
    }
}
