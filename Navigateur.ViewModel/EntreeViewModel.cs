using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Navigateur
{
    public class EntreeViewModel : ViewModelBase
    {
        private readonly   string _adresse;
        private readonly DateTime _date;

        private bool _estEpingle;

        public EntreeViewModel(string adresse)
        {
            _adresse    = adresse;
            _date       = DateTime.Now;
            _estEpingle = false;
        }

        public override string ToString()
        {
            return _adresse;
        }

        public string Adresse
        {
            get { return _adresse; }
        }

        public DateTime Date
        {
            get { return _date; }
        }

        public bool EstEpingle
        {
            get { return _estEpingle; }
        }

        // Déclenché quand EstEpingle est modifié : l'historique doit être retrié.
        public event Action? EstEpingleChanged; 

        public ICommand EpingleCommand
        {
            get { return new RelayCommand(Epingle); }
        }

        public void Epingle()
        {
            _estEpingle = !_estEpingle;

            if (EstEpingleChanged != null)
            {
                EstEpingleChanged();
            }
        }
    }
}
