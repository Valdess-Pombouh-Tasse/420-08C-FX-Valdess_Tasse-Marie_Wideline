using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGestionProduitCosmetique.Models
{
    public class ListePanier
    {
        #region CONSTANTES
        public const decimal PRIX_MIN = 10;
        public const float POURCENTAGE_TPS = 0.03f;
        public const float POURCENTAGE_TVQ = 0.06f;
        #endregion

        #region ATTRIBUTS
        /// <summary>
        /// 
        /// </summary>
        private string _id;
        /// <summary>
        /// 
        /// </summary>
        private decimal _prixTotal;
        /// <summary>
        /// 
        /// </summary>
        private List<Items> _items;

        #endregion

        #region PROPRIÉTÉS
        /// <summary>
        /// 
        /// </summary>
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal PrixTotal
        {
            get { return _prixTotal; }
            set { _prixTotal = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public List<Items> Items
        {
            get { return _items; }
            set { _items = value; }
        }
        /// <summary>
        /// permet d'obtenir le prix du sous total de la liste de voeux
        /// </summary>
        public decimal PrixSousTotal
        {
            get
            {
                decimal total = 0;
                for (int i = 0; i < Items.Count; i++)
                {

                    total += _items[i].LstProduit.Prix * _items[i].NbItem;
                }
                return total;

            }

        }
        /// <summary>
        /// permet d'obtenir le prix de la TPS de la liste de voeux
        /// </summary>
        public decimal PrixTPS
        {
            get
            {
                decimal totalTps = PrixSousTotal * Convert.ToDecimal(POURCENTAGE_TPS);
                return totalTps;

            }

        }
        /// <summary>
        /// permet d'obtenir le prix de la TVQ de la liste de voeux
        /// </summary>
        public decimal PrixTVQ
        {
            get
            {
                decimal totalTvq = PrixSousTotal * Convert.ToDecimal(POURCENTAGE_TVQ);
                return totalTvq;
            }

        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public ListePanier()
        {
            Items = new List<Items>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="items"></param>
        public ListePanier(string id, List<Items> items)
        {
            Id = id;
            Items = items;
        }

        public void AjouterArticle(Items pItems)
        {
            if (pItems is null)
                throw new ArgumentNullException("L'item est null");
           
            Items.Add(pItems);
        }

        public void RetirerArticle(Items pItems)
        {
            if (pItems is null)
                throw new("L'article est null");

            if (!Items.Contains(pItems))
            {
                throw new InvalidOperationException("L'article n'existe pas dans la liste voeux");
            }

            Items.Remove(pItems);
        }
    }
}
