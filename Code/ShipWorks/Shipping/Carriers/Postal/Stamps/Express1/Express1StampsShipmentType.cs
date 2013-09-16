using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Settings;
using System.Windows.Forms;
using ShipWorks.Shipping.Carriers.Postal.Express1;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps.Express1
{
    /// <summary>
    /// Shipment type for Express 1 for Stamps.com shipments.
    /// </summary>
    public class Express1StampsShipmentType : StampsShipmentType
    {
        /// <summary>
        /// Gets the shipment type code.
        /// </summary>
        public override ShipmentTypeCode ShipmentTypeCode
        {
            get
            {
                return ShipmentTypeCode.Express1Stamps;
            }
        }

        /// <summary>
        /// Creates the Express1/Stamps service control.
        /// </summary>
        public override ServiceControlBase CreateServiceControl()
        {
            return new Express1StampsServiceControl();
        }

        /// <summary>
        /// Creates the Express1/Stamps setup wizard.
        /// </summary>
        public override Form CreateSetupWizard()
        {
            var registration = new Express1Registration(ShipmentTypeCode,
                                                        new Express1RegistrationGateway(new Express1StampsConnectionDetails()),
                                                        null,
                                                        null);
            return new Express1SetupWizard(registration);
        }

        /// <summary>
        /// Creates the Express1/Stamps settings control.
        /// </summary>
        public override SettingsControlBase CreateSettingsControl()
        {
            return new StampsSettingsControl(true);
        }
    }
}
