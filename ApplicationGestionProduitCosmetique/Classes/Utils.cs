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

        public enum ValidationCourriel
        {
            Conforme,
            ErreurVide,
            ErreurFormat
        }

        private const int NBR_CARACTERE_MAX = 2;
       
  

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pNom">Initiales saisies</param>
        /// <returns>Enum d'état de validation</returns>
        public static ValidationNom ValiderInitiales(string pNom)
        {
            if (string.IsNullOrWhiteSpace(pNom))
                return ValidationNom.ErreurVide;

            return ValidationNom.Conforme;
        }

        /// <summary>
        /// Valide les initiales du joueurs, il faut que ce soit 2 lettres majuscules
        /// </summary>
        /// <param name="pInitiales">Initiales saisies</param>
        /// <returns>Enum d'état de validation</returns>
        public static ValidationCourriel ValiderCourriel(string pCourriel)
        {
            if (string.IsNullOrWhiteSpace(pCourriel))
                return ValidationCourriel.ErreurVide;

            // Vérifier si la longueur est bien de 2 caractères et que les initiales sont en majuscules
            if (pCourriel.Trim().Length != NBR_CARACTERE_MAX)
                return ValidationCourriel.ErreurFormat;

            if (pCourriel != pCourriel.ToUpper())
                return ValidationCourriel.ErreurFormat;

            return ValidationCourriel.Conforme;
        }
    }
}
