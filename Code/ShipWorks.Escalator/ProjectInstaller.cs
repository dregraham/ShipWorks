using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace ShipWorks.Escalator
{
    /// <summary>
    /// Escalator Installer
    /// </summary>
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        private ServiceProcessInstaller serviceProcessInstaller;
        private ServiceInstaller serviceInstaller;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProjectInstaller()
        {
            serviceProcessInstaller = new ServiceProcessInstaller();
            serviceInstaller = new ServiceInstaller();
            // 
            // serviceProcessInstaller
            // 
            serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
            serviceProcessInstaller.Password = null;
            serviceProcessInstaller.Username = null;
            // 
            // serviceInstaller
            // 
            serviceInstaller.ServiceName = "ShipWorksEscalator";
            serviceInstaller.StartType = ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            Installers.AddRange(new Installer[] {
            serviceProcessInstaller,
            serviceInstaller});
            serviceInstaller.ServiceName = ServiceName.Resolve();
        }
    }
}
