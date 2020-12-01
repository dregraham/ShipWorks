using System;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
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

        /// <summary>
        /// Constructor
        /// </summary>
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

        /// <summary>
        /// Configure all of our services
        /// </summary>
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
            services.AddScoped<IShipWorksCommandLineService, ShipWorksCommandLineService>();

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

        /// <summary>
        /// Override for the base App.OnStartup event handler
        /// </summary>
        protected override async void OnStartup(StartupEventArgs e)
        {
            Logger.Setup();

            Telemetry.Telemetry.TrackStartShipWorksInstaller();

            await host.StartAsync();

            var window = ServiceProvider.GetRequiredService<MainWindow>();
            window.Show();
            SetupUnhandledExceptionHandling();
            base.OnStartup(e);
        }

        /// <summary>
        /// Override for the base App.OnExit event handler
        /// </summary>
        protected override async void OnExit(ExitEventArgs e)
        {
            var mainViewModel = ServiceProvider.GetRequiredService<MainViewModel>();
            var navService = ServiceProvider.GetRequiredService<INavigationService<NavigationPageType>>();

            Telemetry.Telemetry.TrackFinish(mainViewModel.InstallSettings, navService.CurrentPageKey);

            using (host)
            {
                await host.StopAsync(TimeSpan.FromSeconds(5));
            }

            base.OnExit(e);
        }

        /// <summary>
        /// Setup handlers for any exceptions that are not caught
        /// </summary>
        private void SetupUnhandledExceptionHandling()
        {
            // Catch exceptions from all threads in the AppDomain.
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
                ShowUnhandledException(args.ExceptionObject as Exception);


            // Catch exceptions from each AppDomain that uses a task scheduler for async operations.
            TaskScheduler.UnobservedTaskException += (sender, args) =>
            {
                // If we are debugging, let Visual Studio handle the exception and take us to the code that threw it.
                if (!Debugger.IsAttached)
                {
                    args.SetObserved();
                    ShowUnhandledException(args.Exception);
                }
            };


            // Catch exceptions from a single specific UI dispatcher thread.
            Dispatcher.UnhandledException += (sender, args) =>
            {
                // If we are debugging, let Visual Studio handle the exception and take us to the code that threw it.
                if (!Debugger.IsAttached)
                {
                    args.Handled = true;
                    ShowUnhandledException(args.Exception);
                }
            };
        }

        /// <summary>
        /// In the event of an unhandled exception we take users to the warnings page
        /// and log out the exception for support
        /// </summary>
        void ShowUnhandledException(Exception e)
        {
            var mainViewModel = ServiceProvider.GetRequiredService<MainViewModel>();
            mainViewModel.InstallSettings.Error = InstallError.Unknown;
            var navService = ServiceProvider.GetRequiredService<INavigationService<NavigationPageType>>();
            var logFactory = ServiceProvider.GetRequiredService<Func<Type, ILog>>();

            logFactory(typeof(App)).Error("An uncaught exception occured:", e);
            Telemetry.Telemetry.TrackException(e);
            navService.NavigateTo(NavigationPageType.Warning);
        }
    }
}
