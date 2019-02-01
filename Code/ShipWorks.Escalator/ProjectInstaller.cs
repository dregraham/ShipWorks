using System.ComponentModel;
using System.Configuration.Install;

namespace ShipWorks.Escalator
{
    /// <summary>
    /// Escalator Installer
    /// </summary>
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ProjectInstaller()
        {
            InitializeComponent();
            serviceInstaller.ServiceName = ServiceName.Resolve();
        }
    }
}
