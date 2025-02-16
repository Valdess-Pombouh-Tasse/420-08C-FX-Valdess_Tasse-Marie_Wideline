using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGestionProduitCosmetique.Classes
{
    public class Vendeur
    {
		private string _nom;
		private string _prenom;
		private int _id;

		public string Nom
		{
			get { return _nom; }
			set { _nom = value; }
		}

		public string Prenom
		{
			get { return _prenom; }
			set { _prenom = value; }
		}

		public int ID
		{
			get { return _id; }
			set { _id = value; }
		}

		private DateTime _dateInscription;

		public DateTime DateInscription
		{
			get { return _dateInscription; }
			set { _dateInscription = value; }
		}

        public Vendeur(string nom, string prenom, int iD, DateTime dateInscription)
        {
            Nom = nom;
            Prenom = prenom;
            ID = iD;
            DateInscription = dateInscription;
        }

        public override string ToString()
        {
			return $"{ID}{Nom}{Prenom}{DateInscription}";
        }
    }
}
