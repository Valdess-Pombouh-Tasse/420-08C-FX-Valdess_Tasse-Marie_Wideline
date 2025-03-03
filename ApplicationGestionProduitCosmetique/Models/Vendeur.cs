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
        /// <summary>
        /// Constructeur de la classe Vendeur
        /// </summary>
        /// <param name="nomUtilisateur"></param>
        /// <param name="nomDePasse"></param>
        /// <param name="codeUtilisateur"></param>
        public Vendeur(string nomUtilisateur, string nomDePasse, string codeUtilisateur) : base(nomUtilisateur, nomDePasse, codeUtilisateur)
        {
        }

    }
}
