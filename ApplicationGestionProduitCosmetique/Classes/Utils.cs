using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGestionProduitCosmetique.Classes
{
    public static class Utils
    {

        public enum ValidationNom
        {
            Conforme,
            ErreurVide
        }

        public enum ValidationPassword
        {
            Faible,
            Moyen,
            Fort
        }

        public enum validationCourriel
        {
            Conforme,
            ErreurVide,
            ErreurFormat
        }
    }
}
