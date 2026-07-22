using PDVnet.ControleCaixa.UI.Commands;
using System.Windows.Input;

namespace PDVnet.ControleCaixa.UI.ViewModels;

public class MainViewModel : BaseViewModel
{
    private object _currentView;

    public object CurrentView
    {
        get => _currentView;
        set
        {
            _currentView = value;
            OnPropertyChanged();
        }
    }

    public ICommand ShowDashboardCommand { get; }
    public ICommand ShowTransacoesCommand { get; }
    public ICommand ShowFluxoDeCaixaCommand { get; }


    public MainViewModel()
    {
        ShowDashboardCommand = 
            new RelayCommand(_ => CurrentView = new DashBoardViewModel());

        ShowTransacoesCommand =
           new RelayCommand(_ => CurrentView = new TransacoesViewModel());

        ShowFluxoDeCaixaCommand =
            new RelayCommand(_ => CurrentView = new FluxoDeCaixaViewModel());

        CurrentView = new DashBoardViewModel();

    }
}
