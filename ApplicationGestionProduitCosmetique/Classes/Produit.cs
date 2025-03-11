using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGestionProduitCosmetique.Classes
{
    public class Produit
    {
        ///Les constantes
        public const int CODE_NB_CARAC_MIN = 5;
        public const int CODE_NB_CARAC_MAX = 10;
        public const int NOM_NB_CARAC_MIN = 3;
        public const int NOM_NB_CARAC_MAX = 30;
        public const int PRIX_MIN_VAL = 0;


        /// <summary>
        /// Identifiant unique du produit
        /// </summary>
        private uint _id;

        /// <summary>
        /// Code du produit
        /// </summary>
        private string _code;

        /// <summary>
        ///Nom du produit
        /// </summary>
        private string _nom;

        /// <summary>
        /// Catégorie du produit
        /// </summary>
        private Categorie _categorie;


        /// <summary>
        /// Prix de vente du produit
        /// </summary>
        private decimal _prix;

        /// <summary>
        /// Nom de l'image du produit
        /// </summary>
        private string _image;

        /// <summary>
        /// Description du produit
        /// </summary>
        private string _description;

        /// <summary>
        /// Quantité de produit
        /// </summary>
        private int _quantite;


        /// <summary>
        /// Obtient ou définit l'indentifiant unique du produit.
        /// </summary>
        public uint Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Obtient ou définit le code du produit.
        /// </summary>
        /// <remarks>Le code doit toujours être en majuscule et sans espaces inutiles</remarks>
        /// <exception cref="System.ArgumentException">Lancée lorsque le code est nul ou vide</exception>
        /// <exception cref="System.ArgumentOutOfRangeException"> Lancée lorsque le code ne contient pas entre <see cref="CODE_NB_CARC_MIN"/> et <see cref="CODE_NB_CARC_MAX"/> caractères.</exception>
        public string Code
        {
            get { return _code; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(nameof(Code), $"Le code ne peut pas être null ou vide");

                if (value.Trim().Length < CODE_NB_CARAC_MIN || value.Trim().Length > CODE_NB_CARAC_MAX)
                    throw new ArgumentOutOfRangeException(nameof(Code), $"Le code doit être entre {CODE_NB_CARAC_MIN} et {CODE_NB_CARAC_MAX} caractères.");

                _code = value.Trim().ToUpper();

            }

        }


        /// <summary>
        /// Obtient ou définit le nom du produit
        /// </summary>
        /// <remarks>Le nom ne doit pas contenir d'espaces inutiles</remarks>
        /// <exception cref="System.ArgumentException">Lancée lorsque le nom est nul ou vide</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Lancée lorsque le nom ne contient pas entre <see cref="NOM_NB_CARC_MIN"/> et <see cref="NOM_NB_CARC_MAX"/> </exception>
        /// @author Valdess
        public string Nom
        {
            get { return _nom; }
            set
            {

                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(nameof(Nom), "Le nom du produit ne doit pas être null ou contenir des espaces");

                if (value.Trim().Length < NOM_NB_CARAC_MIN || value.Trim().Length > NOM_NB_CARAC_MAX)
                    throw new ArgumentOutOfRangeException(nameof(Nom), $"Le nom doit être entre {NOM_NB_CARAC_MIN} et {NOM_NB_CARAC_MAX} caractères");

                _nom = value.Trim();

            }
        }

        /// <summary>
        /// Obtient ou définit la catérorie à laquelle appartient le produit
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Lancée lorsque la catégorie est nulle</exception>
        /// @author Valdess
        public Categorie Categorie
        {
            get { return _categorie; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(Categorie), "La catégorie ne peut pas être null.");

                _categorie = value;
            }
        }


        /// <summary>
        /// Obtient ou définit le prix de vente du produit
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">Lancées lorsque le prix est inférieur ou égale à <see cref="PRIX_MIN_VAL"/></exception>
        /// @author Valdess
        public decimal Prix
        {
            get { return _prix; }
            set
            {
                if (value <= PRIX_MIN_VAL)
                    throw new ArgumentOutOfRangeException(nameof(Prix), $"Le prix doit être superieur à {PRIX_MIN_VAL}");

                _prix = value;
            }
        }

        /// <summary>
        /// Obtient ou définit le nom du fichier image du produit.
        /// </summary>
        /// <exception cref="System.ArgumentException">Lancée lorsque l'image est nulle, vide ou ne contenir que des espaces</exception>
        /// @author Valdess
        public String Image
        {
            get { return _image; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(nameof(Image), "L'image ne doit pas être null, vide ou contenir seulement des espaces.");

                _image = value.Trim();
            }
        }

        /// <summary>
        /// Obtient ou définit la description du produit
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

       
        /// <summary>
        /// Obtient ou définit la quantité de produit
        /// </summary>
        public int Quantite
        {
            get { return _quantite; }
            set { _quantite = value; }
        }

        private int _idTeint;

        public int IdTeint
        {
            get { return _idTeint; }
            set { _idTeint = value; }
        }

        private int _idVendeur;

        public int IdVendeur
        {
            get { return _idVendeur; }
            set { _idVendeur = value; }
        }



        /// <summary>
        /// Permet de construire un nouveau produit
        /// </summary>
        /// <param name="code">identifiant unique du produit</param>
        /// <param name="nom">Nom du produit</param>
        /// <param name="categorie">Catégorie du produit</param>
        /// <param name="prix">Prix de vente du produit</param>
        /// <param name="image">Nom du fichier image du produit</param>
        /// @author Valdess
        public Produit(uint id, string code, string nom, decimal prix, string image, Categorie categorie, int idTeint, int idVendeur, string description, int quantite)
        {
            Id = id;
            Code = code;
            Nom = nom;
            Categorie = categorie;
            Prix = prix;
            Image = image;
            IdTeint = idTeint;
            IdVendeur = idVendeur;
            Description = description;
            Quantite = quantite;
        }


        /// <summary>
        /// Permet d'obtenir une représentation du produit.
        /// </summary>
        /// <returns>Un représentation de ***** sous forme de chaîne de caractères.</returns>
        /// @author Valdess
        public override string ToString()
        {

            return $"{Code} {Nom} {Prix}";
        }

        /// <summary>
        /// Permet de vérifier si deux objets de type Produit sont égaux.
        /// </summary>
        /// <param name="obj">Objet de type Produit à comparer avec l'objet courant</param>
        /// <returns>true si les deux objets sont égaux i.e que leur code sont identique; false autrement.</returns>
        /// @author Valdess
        public override bool Equals(Object? obj)
        {
            if (obj is null || obj is not Produit)
                return false;

            Produit produit = obj as Produit;

            if (string.Compare(this.Code, produit.Code, CultureInfo.InvariantCulture, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) == 0)
                return true;

            return false;
        }

        /// <summary>
        /// Définition de l'opérateur d'égalité entre 2 produits
        /// </summary>
        /// <param name="a">Produit à gauche de l'opérateur</param>
        /// <param name="b">Produit à droite de l'opérateur</param>
        /// <returns>True si égals, false sinon</returns>
        /// @author Valdess
        public static bool operator ==(Produit a, Produit b)
        {
            if (Object.ReferenceEquals(a, b))
                return true;

            if ((Object)a == null || (Object)b == null)
                return false;

            if (string.Compare(a.Code, b.Code, CultureInfo.InvariantCulture, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) == 0)
                return true;

            return false;
        }

        /// <summary>
        /// Définition de l'opérateur non égale entre 2 produits
        /// </summary>
        /// <param name="a">Produit à gauche de l'opérateur</param>
        /// <param name="b">Produit à droite de l'opérateur</param>
        /// <returns>True si différents, false sinon</returns>
        /// @author Valdess
        public static bool operator !=(Produit a, Produit b)
        {
            return !(a == b);
        }
    }
}
