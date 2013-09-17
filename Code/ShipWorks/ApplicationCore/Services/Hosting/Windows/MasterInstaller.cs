using System.ComponentModel;
using System.Configuration.Install;

namespace ShipWorks.ApplicationCore.Services.Hosting.Windows
{
    /// <summary>
    /// The Installer required in order to make InstallUtil.exe work.  The real meat is in the designer.
    /// </summary>
    [RunInstaller(true)]
    public partial class MasterInstaller : Installer
    {
        /// <summary>
        /// Cosntructor
        /// </summary>
        public MasterInstaller()
        {
            InitializeComponent();
            ShipWorksSession.Initialize(ShipWorksCommandLine.Empty);
        }
    }
}
