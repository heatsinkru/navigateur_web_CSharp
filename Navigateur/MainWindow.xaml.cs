using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Navigateur
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainView
    {
        public MainWindow()
        {
            InitializeComponent();

            // Sur ma machine, je dois forcer l'initialisation du composant WebView2 (_wv.CoreWebView2 reste null sinon) :
            _wv.EnsureCoreWebView2Async(null);
        }

        // IMainView
        public void Navigue(string adresse)
        {
            if (_wv.CoreWebView2 != null)
            {
                _wv.CoreWebView2.Navigate(adresse);
            }
        }

        private void NavigationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
        {
            if (DataContext is MainViewModel vm)
            {
                vm.Historise(_wv.Source.AbsoluteUri);
            }
        }
    }
}
