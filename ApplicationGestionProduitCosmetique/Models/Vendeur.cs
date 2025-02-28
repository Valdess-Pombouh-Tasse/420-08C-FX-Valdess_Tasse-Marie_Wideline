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
        public string Code { get; set; }

        public Vendeur(string? nomUtilisateur, string? motDePasse, string code) : base(nomUtilisateur, motDePasse)
        {
            Code = code;
        }
        

    }
}
