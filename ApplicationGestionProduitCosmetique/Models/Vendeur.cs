using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGestionProduitCosmetique.Models
{
    public class Vendeur
    {

        public const int NB_CARAC_MIN = 3;
        public const int NB_CARAC_MAX = 15;

        private string _nom;
        private string _prenom;
        private int _id;
        private DateTime _dateInscription;

        public string Nom
        {
            get { return _nom; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(nameof(Nom), $"Le nom ne peut pas etre nul ou vide");
                if (!(value.Length >= NB_CARAC_MIN) || !(value.Length <= NB_CARAC_MAX))
                    throw new ArgumentOutOfRangeException(nameof(Nom), $"Le nom doit contenir entre {NB_CARAC_MIN} et {NB_CARAC_MAX} caractères");
                _nom = value.Trim();
            }
        }

        public string Prenom
        {
            get { return _prenom; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(nameof(Nom), $"Le prenom ne peut pas etre nul ou vide");
                if (!(value.Length >= NB_CARAC_MIN) || !(value.Length <= NB_CARAC_MAX))
                    throw new ArgumentOutOfRangeException(nameof(Nom), $"Le prenom doit contenir entre {NB_CARAC_MIN} et {NB_CARAC_MAX} caractères");
                _prenom = value.Trim();
            }
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public DateTime DateInscription
        {
            get { return _dateInscription; }
            set { _dateInscription = value; }
        }

        public Vendeur(int iD, string nom, string prenom, DateTime dateInscription)
        {
            Id = iD;
            Nom = nom;
            Prenom = prenom;
            DateInscription = dateInscription;
        }

        public override string ToString()
        {
            return $"{Id}{Nom}{Prenom}{DateInscription}";
        }
    }
}
