using System.ComponentModel;
using System.Configuration.Install;
using ShipWorks.ApplicationCore.ExecutionMode;


namespace ShipWorks.ApplicationCore.WindowsServices.Installers
{
    [RunInstaller(true)]
    public partial class MasterInstaller : Installer
    {
        public MasterInstaller()
        {
            InitializeComponent();
            ShipWorksSession.Initialize(new CommandLineExecutionMode(ShipWorksCommandLine.Empty));
        }
    }
}
