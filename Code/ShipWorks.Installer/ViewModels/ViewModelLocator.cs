using Microsoft.Extensions.DependencyInjection;

namespace ShipWorks.Installer.ViewModels
{
    /// <summary>
    /// View model locator
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Main view model
        /// </summary>
        public MainViewModel MainViewModel => App.ServiceProvider.GetRequiredService<MainViewModel>();

        /// <summary>
        /// Eula View Model
        /// </summary>
        public EulaViewModel EulaViewModel => App.ServiceProvider.GetRequiredService<EulaViewModel>();

        /// <summary>
        /// Install Path View Model
        /// </summary>
        public InstallPathViewModel InstallPathViewModel => App.ServiceProvider.GetRequiredService<InstallPathViewModel>();

        /// <summary>
        /// System Check View Model
        /// </summary>
        public SystemCheckViewModel SystemCheckViewModel => App.ServiceProvider.GetRequiredService<SystemCheckViewModel>();

        /// <summary>
        /// Login View Model
        /// </summary>
        public LoginViewModel LoginViewModel => App.ServiceProvider.GetRequiredService<LoginViewModel>();

        /// <summary>
        /// Location Config View Model
        /// </summary>
        public LocationConfigViewModel LocationConfigViewModel => App.ServiceProvider.GetRequiredService<LocationConfigViewModel>();

        /// <summary>
        /// Install ShipWorks View Model
        /// </summary>
        public InstallShipWorksViewModel InstallShipworksViewModel => App.ServiceProvider.GetRequiredService<InstallShipWorksViewModel>();

        /// <summary>
        /// Install Database View Model
        /// </summary>
        public InstallDatabaseViewModel InstallDatabaseViewModel => App.ServiceProvider.GetRequiredService<InstallDatabaseViewModel>();

        /// <summary>
        /// Upgrade ShipWorks View Model
        /// </summary>
        public UpgradeShipWorksViewModel UpgradeShipWorksViewModel => App.ServiceProvider.GetRequiredService<UpgradeShipWorksViewModel>();

        /// <summary>
        /// Warning View Model
        /// </summary>
        public WarningViewModel WarningViewModel => App.ServiceProvider.GetRequiredService<WarningViewModel>();

        /// <summary>
        /// Use ShipWorks View Model
        /// </summary>
        public UseShipWorksViewModel UseShipWorksViewModel => App.ServiceProvider.GetRequiredService<UseShipWorksViewModel>();

        /// <summary>
        /// Database Config View Model
        /// </summary>
        public DatabaseConfigViewModel DatabaseConfigViewModel => App.ServiceProvider.GetRequiredService<DatabaseConfigViewModel>();
    }
}
