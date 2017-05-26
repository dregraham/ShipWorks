using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.Metrics;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Core.Shipping;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Configuration
{
    /// <summary>
    /// Telemetry wrapper for the shipment setup wizard
    /// </summary>
    [Component(RegistrationType.Self)]
    public class TelemetricShipmentTypeSetupWizardForm : IShipmentTypeSetupWizard
    {
        private readonly IShipmentTypeSetupWizard inner;
        private readonly OpenedFromSource openedFrom;
        private readonly ShipmentTypeCode shipmentTypeCode;
        private readonly Func<ICarrierSettingsTrackedDurationEvent> createTelemetry;

        /// <summary>
        /// Constructor
        /// </summary>
        public TelemetricShipmentTypeSetupWizardForm(IShipmentTypeSetupWizard inner, ShipmentTypeCode shipmentTypeCode, OpenedFromSource openedFrom,
            Func<ICarrierSettingsTrackedDurationEvent> createTelemetry)
        {
            this.shipmentTypeCode = shipmentTypeCode;
            this.createTelemetry = createTelemetry;
            this.openedFrom = openedFrom;
            this.inner = inner;
        }

        /// <summary>
        /// Show the dialog
        /// </summary>
        public DialogResult ShowDialog(IWin32Window control)
        {
            using (var telemetryEvent = createTelemetry())
            {
                var result = inner.ShowDialog(control);

                telemetryEvent.RecordConfiguration(shipmentTypeCode);
                telemetryEvent.AddProperty("Abandoned", result == DialogResult.OK ? "No" : "Yes");
                telemetryEvent.AddProperty("OpenedFrom", openedFrom.ToString());

                return result;
            }
        }

        /// <summary>
        /// Dispose the control
        /// </summary>
        public void Dispose()
        {
            inner.Dispose();
        }
    }
}
