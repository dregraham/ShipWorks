using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using Forms = System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore.Settings;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.ApplicationCore.Settings.Warehouse;

namespace ShipWorks.UI.Controls.Settings
{
    /// <summary>
    /// Interaction logic for WarehouseSettings.xaml
    /// </summary>
    [Component(RegistrationType.SpecificService, Service = typeof(IWarehouseSettings))]
    public partial class WarehouseSettings : UserControl, IWarehouseSettings
    {
        private Forms.Control container;

        /// <summary>
        /// Constructor
        /// </summary>
        public WarehouseSettings()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the control associated with the warehouse settings
        /// </summary>
        public Forms.Control Control => container ?? (container = BuildContainer(this));

        /// <summary>
        /// View model associated with the control
        /// </summary>
        public IWarehouseSettingsViewModel ViewModel
        {
            get => DataContext as IWarehouseSettingsViewModel;
            set => DataContext = value;
        }

        /// <summary>
        /// Build the container
        /// </summary>
        private static Forms.Control BuildContainer(WarehouseSettings warehouseSettings) =>
            new ElementHost
            {
                Dock = Forms.DockStyle.Fill,
                Child = warehouseSettings,
            };
    }
}
