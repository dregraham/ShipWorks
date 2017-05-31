using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Utility;

namespace ShipWorks.Shipping.Settings
{
    /// <summary>
    /// Mark a shipment as configured after successfully setting it up
    /// </summary>
    [Component(RegistrationType.Self)]
    public class ConfigurationShipmentTypeSetupWizard : IShipmentTypeSetupWizard
    {
        private readonly IShipmentTypeSetupWizard inner;
        private readonly ShipmentTypeCode shipmentTypeCode;
        private readonly IMessageHelper messageHelper;

        /// <summary>
        /// Constructor
        /// </summary>
        public ConfigurationShipmentTypeSetupWizard(IShipmentTypeSetupWizard inner, ShipmentTypeCode shipmentTypeCode, IMessageHelper messageHelper)
        {
            this.messageHelper = messageHelper;
            this.shipmentTypeCode = shipmentTypeCode;
            this.inner = inner;
        }

        /// <summary>
        /// Show the dialog
        /// </summary>
        public DialogResult ShowDialog(IWin32Window control)
        {
            ShipmentType shipmentType = ShipmentTypeManager.GetType(shipmentTypeCode);

            try
            {
                using (SqlAppResourceLock setupLock = new SqlAppResourceLock("Setup - " + shipmentType.ShipmentTypeName))
                {
                    DialogResult result = inner.ShowDialog(control);

                    if (result == DialogResult.OK)
                    {
                        ShippingSettings.MarkAsConfigured(shipmentType.ShipmentTypeCode);
                    }

                    return result;
                }
            }
            catch (SqlAppResourceLockException)
            {
                messageHelper.ShowInformation("The shipping provider is currently being setup on another computer.");
            }

            return DialogResult.Abort;
        }

        /// <summary>
        /// Gets the wizard without any wrapping wizards
        /// </summary>
        public IShipmentTypeSetupWizard GetUnwrappedWizard() => inner;

        /// <summary>
        /// Dispose the inner control
        /// </summary>
        public void Dispose() => inner.Dispose();
    }
}
