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
        public Vendeur(string? nomUtilisateur, string motDePasse, string code) : base(nomUtilisateur, motDePasse)
        {
            Code = code;
        }
    }
}
