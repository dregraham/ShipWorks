using Microsoft.Extensions.DependencyInjection;

namespace ShipWorks.Installer.ViewModels
{
    public class ViewModelLocator
    {
        public MainViewModel MainViewModel => App.ServiceProvider.GetRequiredService<MainViewModel>();
        public EulaViewModel EulaViewModel => App.ServiceProvider.GetRequiredService<EulaViewModel>();
        public InstallPathViewModel InstallPathViewModel => App.ServiceProvider.GetRequiredService<InstallPathViewModel>();
        public SystemCheckViewModel SystemCheckViewModel => App.ServiceProvider.GetRequiredService<SystemCheckViewModel>();
        public LoginViewModel LoginViewModel => App.ServiceProvider.GetRequiredService<LoginViewModel>();
        public LocationConfigViewModel LocationConfigViewModel => App.ServiceProvider.GetRequiredService<LocationConfigViewModel>();
        public InstallShipworksViewModel InstallShipworksViewModel => App.ServiceProvider.GetRequiredService<InstallShipworksViewModel>();
        public InstallDatabaseViewModel InstallDatabaseViewModel => App.ServiceProvider.GetRequiredService<InstallDatabaseViewModel>();
        public UpgradeShipWorksViewModel UpgradeShipWorksViewModel => App.ServiceProvider.GetRequiredService<UpgradeShipWorksViewModel>();
        public WarningViewModel WarningViewModel => App.ServiceProvider.GetRequiredService<WarningViewModel>();
        public UseShipWorksViewModel UseShipWorksViewModel => App.ServiceProvider.GetRequiredService<UseShipWorksViewModel>();
    }
}
