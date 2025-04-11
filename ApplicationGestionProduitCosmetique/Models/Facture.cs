using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ApplicationGestionProduitCosmetique.Models
{
    public class Facture
    {
        public string NomClient { get; set; }

        /// <summary>
        /// Déclaration des constantes
        /// </summary>
        const float TAUX_TPS = 0.05f;
        const float TAUX_TVQ = 0.09975f;


        /// <summary>
        /// Identifiant unique de la facture
        /// </summary>
        private uint _id;

        /// <summary>
        /// Date de création de la facture
        /// </summary>
        private DateTime _dateCreation;

        /// <summary>
        /// Liste des produits dans la 
        /// </summary>
        private List<ProduitFacture> _produitsFacture;


        /// <summary>
        ///  Obtient ou définit l'identifiant unique de la facture
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Lancé lors que l'identifiant est 0 ou négatif.</exception>
        public uint Id
        {
            get { return _id; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(Id), "L'identifiant doit être supérieur à 0.");

                _id = value;
            }
        }


        /// <summary>
        /// Obtient ou définit la date de création de la facture
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Lancé lors que la date de création est supérieure à la date du jour.</exception>
        public DateTime DateCreation
        {
            get { return _dateCreation; }
            set
            {
                if (value > DateTime.Now)
                    throw new ArgumentOutOfRangeException($"La date de création ne peut pas être supérieure à la date du jour {DateTime.Now}", nameof(DateCreation));

                _dateCreation = value;
            }
        }


        /// <summary>
        /// Obtient ou définit la liste des produits faisants partie de la facture
        /// </summary>
        public List<ProduitFacture> ProduitsFacture
        {
            get { return _produitsFacture; }
            private set { _produitsFacture = value; }
        }


        /// <summary>
        /// Obtient le montant total de la facture hors taxe
        /// </summary>
        public decimal MontantSousTotal
        {
            get
            {
                decimal sousTotal = 0;
                foreach (ProduitFacture produit in ProduitsFacture)
                {
                    sousTotal += produit.SousTotal;
                }
                return sousTotal;
            }
        }

        /// <summary>
        /// Obtient le montant la TPS de la facture
        /// </summary>
        public decimal MontantTPS
        {
            get
            {
                return MontantSousTotal * (decimal)TAUX_TPS;
            }

        }


        /// <summary>
        /// Obtient le montant de la TVQ de la facture
        /// </summary>
        public decimal MontantTVQ
        {
            get
            {
                return MontantSousTotal * (decimal)TAUX_TVQ;
            }

        }


        /// <summary>
        /// Obtient le montant total de la facture inclunant le montant des taxes
        /// </summary>
        public decimal MontantTotal
        {
            get
            {
                return MontantSousTotal + MontantTPS + MontantTVQ;
            }
        }

        /// <summary>
        /// Constructeur sans paramètre
        /// </summary>
        /// <remarks>Initialise une liste de produits vide</remarks>
        public Facture()
        {
            ProduitsFacture = new List<ProduitFacture>();
        }

        /// <summary>
        /// Constructeur avec paramètere
        /// </summary>
        /// <param name="id">Identifiant de la facture</param>
        /// <param name="dateCreation">Date de créatoin de la facture</param>
        public Facture(uint id, DateTime dateCreation) : this()
        {
            Id = id;
            DateCreation = dateCreation;
        }

        /// <summary>
        /// Permet d'ajout un produit à une facture
        /// </summary>
        /// <param name="produit">Le produit à ajouter à la facture</param>
        /// <param name="prixUnitaire">Le prix unitaire du produit ajouter à la facture</param>
        /// <param name="quantite">La quantite du produit achetée</param>
        /// <remarks>Si le produit existe déjà dans la facture alors la quantité est ajouté au produit existant</remarks>
        /// <exception cref="ArgumentNullException">Lancée si le produit est null</exception>
        /// <exception cref="ArgumentOutOfRangeException">Lancée si la prix unitaire est inférieur à <see cref="Produit.PRIX_MIN_VAL"/></exception>
        /// <exception cref="ArgumentOutOfRangeException">Lancée si la quantité inférieur à <see cref="ProduitFacture.QUANTITE_MIN_VAL"/></exception>
        public void AjouterProduit(Produit produit, decimal prixUnitaire, uint quantite)
        {
            if (produit == null)
                throw new ArgumentNullException(nameof(produit), "Le produit ne peut pas être null");

            if (prixUnitaire < Produit.PRIX_MIN_VAL)
                throw new ArgumentOutOfRangeException(nameof(prixUnitaire), $"Le prix unitaire doit être supérieur ou égale à {Produit.PRIX_MIN_VAL}");

            if (quantite < ProduitFacture.QUANTITE_MIN_VAL)
                throw new ArgumentOutOfRangeException(nameof(quantite), $"La quantité doit être supérieure ou égale à {ProduitFacture.QUANTITE_MIN_VAL}");

            ProduitFacture produitTrouve = null;
            foreach (ProduitFacture produitF in ProduitsFacture)
            {
                if (produitF.Produit.Equals(produit))
                {
                    produitTrouve = produitF;
                }
            }

            if (ProduitsFacture.Contains(produitTrouve))
            {
                produitTrouve.Quantite++;
            }
            else
            {
                ProduitFacture newProduitFacture = new ProduitFacture(produit, prixUnitaire, quantite);
                ProduitsFacture.Add(newProduitFacture);
            }
        }

        /// <summary>
        /// Permet retirer un produit d'une facture.
        /// </summary>
        /// <param name="produit">Le produit à retirer de la facture</param>
        /// <exception cref="ArgumentNullException">Lancée lorsque le produit est nul</exception>
        ///  <exception cref="InvalidOperationException">Lancée lorsque le produit n'existe pas dans la facture</exception>
        public void RetirerProduit(Produit produit)
        {
            if (produit == null)
                throw new ArgumentNullException(nameof(produit), "Le produit ne peut pas être null");

            ProduitFacture produitTrouve = null;

            foreach (ProduitFacture produitF in ProduitsFacture)
            {
                if (produitF.Produit.Equals(produit))
                {
                    produitTrouve = produitF;
                }
            }

            if (!ProduitsFacture.Contains(produitTrouve))
                throw new InvalidOperationException("Ce produit n'existe pas dans cette facture.");

            if (produitTrouve.Quantite > 1)
            {
                produitTrouve.Quantite--;
            }
            else
            {
                ProduitsFacture.Remove(produitTrouve);
            }
        }
    }
}

