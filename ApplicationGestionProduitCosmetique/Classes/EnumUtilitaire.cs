using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGestionProduitCosmetique.Classes
{
    public static class EnumUtilitaire
    {
        /// <summary>
        /// Extension permettant d'obtenir la description pour une constante d'une énumération, si disponible.
        /// S'il n'y a pas de description associée à la constante de l'énumération, permet d'obtenir la valeur
        /// de celle-ci.
        /// Source : https://msmvps.com/blogs/deborahk/archive/2009/07/10/enum-binding-to-the-description-attribute.aspx
        /// </summary>
        /// <param name="currentEnum">Énumération pour laquelle on désire obtenir une description.</param>
        /// <returns>
        /// Description associée à la constante de l'énumération ou bien la valeur s'il n'y a pas de description.
        /// </returns>
        public static string GetDescription(this Enum currentEnum)
        {
            string description;
            DescriptionAttribute da;

            FieldInfo? fi = currentEnum.GetType().GetField(currentEnum.ToString());
            da = (DescriptionAttribute)Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));
            if (da != null)
                description = da.Description;
            else
                description = currentEnum.ToString();

            return description;
        }

        /// <summary>
        /// Permet d'obtenir toutes les descriptions associées aux constantes d'une énumération, si disponible.
        /// S'il n'y a pas de description associée à une constante de l'énumération, permet d'obtenir
        /// la valeur de celle-ci.
        /// </summary>
        /// <returns>
        /// Les descriptions associées aux constantes de l'énumération ou bien les valeurs
        /// s'il n'y a pas de description.
        /// </returns>
        public static string[] GetAllDescriptions<T>()
        {
            Type enumType = typeof(T);
            List<String> lesDescriptions = new List<String>();
            foreach (Enum valeur in Enum.GetValues(enumType))
            {
                lesDescriptions.Add(valeur.GetDescription());
            }
            return lesDescriptions.ToArray();
        }
    }
}
