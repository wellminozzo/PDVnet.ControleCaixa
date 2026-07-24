using Microsoft.Extensions.DependencyInjection;
using PDVnet.ControleCaixa.Business.Services;
using PDVnet.ControleCaixa.UI.Commands;
using System.Windows.Input;

namespace PDVnet.ControleCaixa.UI.ViewModels;

public class MainViewModel : BaseViewModel
{
    private readonly IServiceProvider _serviceProvider;
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
            new RelayCommand(_ => CurrentView = App.ServiceProvider.GetRequiredService<DashBoardViewModel>());

        ShowTransacoesCommand =
           new RelayCommand(_ => CurrentView = App.ServiceProvider.GetRequiredService<TransacoesViewModel>());

        ShowFluxoDeCaixaCommand =
            new RelayCommand(_ => CurrentView = App.ServiceProvider.GetRequiredService<FluxoDeCaixaViewModel>());

        CurrentView = App.ServiceProvider.GetRequiredService<DashBoardViewModel>();

    }
}
