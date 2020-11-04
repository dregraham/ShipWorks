using System;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Windows;
using GalaSoft.MvvmLight;
using log4net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShipWorks.Installer.Api;
using ShipWorks.Installer.Enums;
using ShipWorks.Installer.Environments;
using ShipWorks.Installer.Logging;
using ShipWorks.Installer.Services;
using ShipWorks.Installer.Sql;
using ShipWorks.Installer.ViewModels;
using ShipWorks.Installer.Views;

namespace ShipWorks.Installer
{
    public partial class App : Application
    {
        private readonly IHost host;

        public static IServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            host = Host.CreateDefaultBuilder()  // Use default settings
                                                //new HostBuilder()          // Initialize an empty HostBuilder
                    .ConfigureAppConfiguration((context, builder) =>
                    {
                        // Add other configuration files...
                        DbProviderFactories.RegisterFactory("System.Data.SqlClient", System.Data.SqlClient.SqlClientFactory.Instance);
                    }).ConfigureServices((context, services) =>
                    {
                        ConfigureServices(context.Configuration, services);
                    })
                    .ConfigureLogging(logging =>
                    {
                        // Add other loggers...
                    })
                    .Build();

            ServiceProvider = host.Services;
        }

        private void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            services.AddScoped<ISqlSession, SqlSession>();
            services.AddScoped<ISqlUtility, SqlUtility>();
            services.AddScoped<ISqlServerLookupService, SqlServerLookupService>();
            services.AddScoped<IDriveInfo, DriveInfoWrapper>();
            services.AddScoped<ISystemInfoService, SystemInfoWrapperService>();
            services.AddScoped<ISystemCheckService, SystemCheckService>();
            services.AddScoped<IRegistryService, RegistryService>();
            services.AddScoped<IInnoSetupService, InnoSetupService>();
            services.AddScoped<INavigationService<NavigationPageType>, NavigationService<NavigationPageType>>();
            services.AddScoped<IHubApiClient, HubApiClient>();
            services.AddScoped<IHubService, HubService>();
            services.AddScoped<IWebClientEnvironmentFactory, WebClientEnvironmentFactory>();

            services.AddSingleton((provider) =>
            {
                return new Func<Type, ILog>(
                    (type) => LogManager.GetLogger(type)
                );
            });

            // Register all ViewModels.
            var viewModelsToAdd = Assembly
                .GetEntryAssembly()?
                .GetTypes()
                .Where(t => (t.BaseType == typeof(InstallerViewModelBase) ||
                             t.BaseType == typeof(ViewModelBase)) &&
                            t != typeof(InstallerViewModelBase));
            foreach (var vm in viewModelsToAdd)
            {
                services.AddSingleton(vm);
            }

            // Register all the Windows of the applications.
            services.AddSingleton<MainWindow>();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            Logger.Setup();
            await host.StartAsync();

            var window = ServiceProvider.GetRequiredService<MainWindow>();
            window.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            using (host)
            {
                await host.StopAsync(TimeSpan.FromSeconds(5));
            }

            base.OnExit(e);
        }
    }
}
