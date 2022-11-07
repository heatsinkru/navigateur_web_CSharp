using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Navigateur
{
    // Fonctions exposées par la vue à MainViewModel.
    public interface IMainView
    {
        void Navigue(string adresse);
    }

    public class MainViewModel : ViewModelBase
    {
        private readonly             IMainView _view;
        private readonly List<EntreeViewModel> _historique;
        private readonly         Stack<string> _precedents;
        private                         string _adresse;
        private                        string? _suivant;

        public MainViewModel(IMainView view)
        {
            _view       = view;
            _historique = new List<EntreeViewModel>();
            _precedents = new Stack<string>();
            _adresse    = "https://www.qwant.com/";
        }

        public string Adresse
        { 
            get { return _adresse; } 
            set
            {
                _adresse = value;
                OnPropertyChanged(nameof(Adresse));
            }
        }

        public ICommand NavigueCommand
        {
            get { return new RelayCommand(Navigue); }
        }

        public void Navigue()
        {
            _view.Navigue(_adresse);
        }

        public void Historise(string adresse)
        {
            if (adresse != _suivant) // (*) On s'évite d'empiler les adresses visitées via "Précédent".
            {
                _precedents.Push(_adresse);
            }

            Adresse = adresse;

            EntreeViewModel e = new EntreeViewModel(adresse);
            e.EstEpingleChanged += EstEpingleChanged;

            _historique.Add(e);
            OnPropertyChanged(nameof(Historique));
        }

        private void EstEpingleChanged()
        {
            OnPropertyChanged(nameof(Historique));
        }

        public EntreeViewModel[] Historique
        {
            get 
            {
                // Requête LINQ :
                return _historique
                    .OrderByDescending(e => e.EstEpingle)   // Tri par statut "épinglé" puis "non épinglé"
                    .ThenByDescending(e => e.Date)          // PUIS tri par dates décroissantes
                    .ToArray();                             // Compile le tout dans un tableau
            }
        }

        public ICommand PrecedentCommand
        {
            get { return new RelayCommand(Precedent); }
        }

        public void Precedent()
        {
            if (_precedents.Count > 0)
            {
                _suivant = _precedents.Pop(); // (*) Garde trace de l'adresse dépilée.
                _view.Navigue(_suivant);
            }
        }
    }
}