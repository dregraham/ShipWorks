using System.Windows.Forms;
using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Shipping.Carriers.DhlEcommerce;

namespace ShipWorks.Shipping.UI.Carriers.DhlEcommerce
{
    /// <summary>
    /// UserControl for editing options specific to the DHL eCommerce integration
    /// </summary>
    public partial class DhlEcommerceOptionsControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DhlEcommerceOptionsControl()
        {
            InitializeComponent();

            requestedLabelFormat.ExcludeFormats(new[] { ThermalLanguage.ZPL, ThermalLanguage.EPL });
        }

        /// <summary>
        /// Load the configured settings into the control
        /// </summary>
        public void LoadSettings()
        {
            using (var scope = IoC.BeginLifetimeScope())
            {
                var shipmentType = scope.Resolve<DhlEcommerceShipmentType>();
                requestedLabelFormat.LoadDefaultProfile(shipmentType);
            }
        }

        /// <summary>
        /// Save the settings to the database
        /// </summary>
        public void SaveSettings()
        {
            requestedLabelFormat.SaveDefaultProfile();
        }
    }
}