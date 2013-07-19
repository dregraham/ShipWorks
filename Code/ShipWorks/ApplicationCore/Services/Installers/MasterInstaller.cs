using System.ComponentModel;
using System.Configuration.Install;
using ShipWorks.ApplicationCore.ExecutionMode;


namespace ShipWorks.ApplicationCore.Services.Installers
{
    [RunInstaller(true)]
    public partial class MasterInstaller : Installer
    {
        public MasterInstaller()
        {
            InitializeComponent();
            ShipWorksSession.Initialize(ShipWorksCommandLine.Empty);
        }
    }
}
