using PDVnet.ControleCaixa.Business.Services;
using PDVnet.ControleCaixa.UI.ViewModels;
using System.Windows;

namespace PDVnet.ControleCaixa.UI.Views
{
    /// <summary>
    /// Lógica interna para MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
