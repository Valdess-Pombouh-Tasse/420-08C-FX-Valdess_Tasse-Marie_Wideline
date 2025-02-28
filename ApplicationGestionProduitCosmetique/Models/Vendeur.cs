using ApplicationGestionProduitCosmetique.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ApplicationGestionProduitCosmetique.Models
{
    public class Vendeur : Utilisateur
    {
      
        public Vendeur(string nomUtilisateur, string nomDePasse, string codeUtilisateur) : base(nomUtilisateur, nomDePasse, codeUtilisateur)
        {
        }

    }
}
