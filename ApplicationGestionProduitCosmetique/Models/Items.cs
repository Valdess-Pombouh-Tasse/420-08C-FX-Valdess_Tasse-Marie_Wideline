using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGestionProduitCosmetique.Models
{
    public class Items
    {
		private int _nbItem;
		private string _nomItem;
		private Produit _lstProduit;
		private int _idItems;
		private string _nomUtilisateurs;

		public int NbItem
		{
			get { return _nbItem; }
			set { _nbItem = value; }
		}

		public string NomItem
		{
			get { return _nomItem; }
			set { _nomItem = value; }
		}

		public Produit LstProduit
		{
			get { return _lstProduit; }
			set { _lstProduit = value; }
		}

		public int IdItems
		{
			get { return _idItems; }
			set { _idItems = value; }
		}

		public string NomUtilisateurs
		{
			get { return _nomUtilisateurs; }
			set { _nomUtilisateurs = value; }
		}

        public string FullPath
        {
            get
            {
                string absolutePath = Path.GetFullPath(_nomItem);
                return absolutePath;
            }
        }
        public Items() { }

        public Items(int nbItem, string nomItem, Produit lstProduit, string nomUtilisateurs)
        {
            NbItem = nbItem;
            NomItem = nomItem;
            LstProduit = lstProduit;
            NomUtilisateurs = nomUtilisateurs;
        }
    }
}
