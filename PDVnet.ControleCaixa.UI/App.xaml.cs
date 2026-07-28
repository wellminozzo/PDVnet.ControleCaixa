using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PDVnet.ControleCaixa.Business.Services;
using PDVnet.ControleCaixa.Data;
using PDVnet.ControleCaixa.Data.Repositories;
using PDVnet.ControleCaixa.UI.ViewModels;
using PDVnet.ControleCaixa.UI.Views;
using System.Globalization;
using System.Windows;

namespace PDVnet.ControleCaixa.UI;

public partial class App : Application
{
    public static IServiceProvider ServiceProvider { get; private set; }

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        var culture = new CultureInfo("pt-BR");
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;

        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddSingleton(new SqlConnectionFactory(connectionString));
        ConfigureServices(services);

        ServiceProvider = services.BuildServiceProvider();

        var mainViewModel = new MainViewModel();
        var mainView = new MainView(mainViewModel);
        mainView.Show();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        // Repositories
        services.AddSingleton<MovimentacaoRepository>();
        services.AddSingleton<ConfiguracaoCaixaRepository>();

        // Services
        services.AddSingleton<MovimentacaoService>();
        services.AddSingleton<ConfiguracaoCaixaService>();

        // ViewModels
        services.AddTransient<TransacoesViewModel>();
        services.AddTransient<DashBoardViewModel>();
        services.AddTransient<FluxoDeCaixaViewModel>();

        // Views
        services.AddSingleton<MainView>();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        if (ServiceProvider is IDisposable disposable)
        {
            disposable.Dispose();
        }
        base.OnExit(e);
    }
}
