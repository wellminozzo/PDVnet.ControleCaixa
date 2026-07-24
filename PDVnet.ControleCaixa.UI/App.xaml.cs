using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PDVnet.ControleCaixa.Business.Services;
using PDVnet.ControleCaixa.Data.Contexts;
using PDVnet.ControleCaixa.Data.Repositories;
using PDVnet.ControleCaixa.UI.ViewModels;
using PDVnet.ControleCaixa.UI.Views;
using System.Windows;

namespace PDVnet.ControleCaixa.UI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    
        public static IServiceProvider ServiceProvider { get; private set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            ServiceProvider = services.BuildServiceProvider();

            var mainViewModel = new MainViewModel();
            var mainView = new MainView(mainViewModel);
            mainView.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<AppDbContext>();
            // Repositories
            services.AddSingleton<MovimentacaoRepository>();

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
