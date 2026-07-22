using System.Windows;

namespace PDVnet.ControleCaixa.UI.Views
{
    /// <summary>
    /// Lógica interna para MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();

            DataContext = new ViewModels.MainViewModel();
        }
    }
}
