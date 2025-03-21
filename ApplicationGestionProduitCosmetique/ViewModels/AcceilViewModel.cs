using FontAwesome.Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ApplicationGestionProduitCosmetique.ViewModels
{
    public class AcceilViewModel : ViewModelBase
    {
        private ViewModelBase _CorpsCourrantView;

        private string _titre;
        private IconChar _icon;

        public IconChar Icon
        {
            get { return _icon; }
            set
            {
                _icon = value;
                OnPropertyChanged(nameof(Icon));
            }
        }


        public string Titre

        {
            get { return _titre; }
            set
            {
                _titre = value;
                OnPropertyChanged(nameof(Titre));
            }
        }


        public ViewModelBase CorpsCourrantView
        {
            get { return _CorpsCourrantView; }
            set
            {
                _CorpsCourrantView = value;
                OnPropertyChanged(nameof(CorpsCourrantView));
            }
        }


        public ICommand ShowAccueilViewModel { get; set; }
        public ICommand ShowArticleChoisirViewModel { get; set; }
        public ICommand ShowListeVoeuxThemeViewModel { get; set; }
        public ICommand ShowFormCreerCompteViewModel { get; set; }
        public ICommand ShowConnexionViewModel { get; set; }

        public AcceilViewModel()
        {
            ShowAccueilViewModel = new ViewModeCommand(ExecuteShowAccueilViewCommand);

            ShowArticleChoisirViewModel = new ViewModeCommand(ExecuteShowArticleChoisirViewModel);
            ShowListeVoeuxThemeViewModel = new ViewModeCommand(ExecuteShowListeVoeuxThemeViewModel);
            ExecuteShowAccueilViewCommand(null);

        }


        private void ExecuteShowAccueilViewCommand(object parameter)
        {
            CorpsCourrantView = new AccueilViewModelMain();
            Titre = "Catalogue";
            Icon = IconChar.Home;
        }
        private void ExecuteShowArticleChoisirViewModel(object parameter)
        {
            CorpsCourrantView = new ArticleChoisirViewModels();
            Titre = "Articles Choisis";
            Icon = IconChar.ShoppingCart;
        }
        private void ExecuteShowListeVoeuxThemeViewModel(object parameter)
        {
            CorpsCourrantView = new ListeCommandThemeViewModel();
            Titre = "liste de Voeux";
            Icon = IconChar.ShoppingCart;
        }
    }
}
