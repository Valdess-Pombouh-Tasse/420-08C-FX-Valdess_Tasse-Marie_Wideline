using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGestionProduitCosmetique.Classes
{
    public class ProduitFacture
    {
        /// <summary>
        /// Déclaration des constantes
        /// </summary>
        public const int QUANTITE_MIN_VAL = 1;
        public const int PRIX_MIN_VAL = 0;


        /// <summary>
        /// Produit 
        /// </summary>
        private Produit _produit;

        /// <summary>
        /// Prix unitaire du produit
        /// </summary>
        private decimal _prixUnitaire;


        /// <summary>
        /// Quantité du produit acheté
        /// </summary>
        private uint _quantite;



        /// <summary>
        /// Obtient ou défini le produit à ajouter à la facture
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Lancée lorsque le produit est nul.</exception>
        public Produit Produit
        {
            get { return _produit; }
            private set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value), "Le produit ne peut pas être nul.");
                _produit = value;
            }
        }

        /// <summary>
        /// Obtient ou définit le prix unitaire du produit
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">Lancées lorsque le prix est inférieur ou égale à <see cref="PRIX_MIN_VAL"/></exception>
        public decimal PrixUnitaire
        {
            get { return _prixUnitaire; }
            set
            {
                if (value <= PRIX_MIN_VAL)
                    throw new ArgumentOutOfRangeException(nameof(PrixUnitaire), "Le prix doit etre inférieur ou égale a {PRIX_MIN_VAL}.");
                _prixUnitaire = value;
            }
        }

        /// <summary>
        /// Obtient ou définit la quantité du produit achetée
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">Lancée lorsque la quantité est inférieur à ou égale à <see cref="QUANTITE_MIN_VAL"/> .</exception>
        public uint Quantite
        {

            get { return _quantite; }
            set
            {
                if (value < QUANTITE_MIN_VAL)
                    throw new ArgumentOutOfRangeException(nameof(Quantite), "La quantité doit etre inférieur ou égale a {PRIX_MIN_VAL}.");
                _quantite = value;
            }
        }

        /// <summary>
        /// Obtient le sous-total pour le produit selon le prix unitaire et la quantité achetée.
        /// </summary>
        public decimal SousTotal
        {
            get
            {
                return _prixUnitaire * _quantite;
            }

        }


        /// <summary>
        /// Constructeur avec paramètres
        /// </summary>
        /// <param name="produit">Produit à ajouter à la facture</param>
        /// <param name="prixUnitaire">Prix unitaire du produit</param>
        /// <param name="quantite">Quantité du produit</param>
        public ProduitFacture(Produit produit, decimal prixUnitaire, uint quantite)
        {
            Produit = produit;
            PrixUnitaire = prixUnitaire;
            Quantite = quantite;
        }
    }
}
