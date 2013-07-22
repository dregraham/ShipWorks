using System.ComponentModel;
using System.Configuration.Install;


namespace ShipWorks.ApplicationCore.Services.Hosting.Windows
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
