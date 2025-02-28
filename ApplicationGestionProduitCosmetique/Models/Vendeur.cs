using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGestionProduitCosmetique.Models
{
    public class Vendeur : Utilisateurs
    {
        public string Code { get; set; }
        public Vendeur(string? nom, string motDePasse, string code) : base(nom, motDePasse)
        {
            Code = code;
        }
    }
}
