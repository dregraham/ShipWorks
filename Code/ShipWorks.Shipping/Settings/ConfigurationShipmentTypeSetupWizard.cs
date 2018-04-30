using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Utility;
using ShipWorks.Shipping.Profiles;

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
        private readonly IShippingProfileManager shippingProfileManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public ConfigurationShipmentTypeSetupWizard(IShipmentTypeSetupWizard inner, ShipmentTypeCode shipmentTypeCode, IMessageHelper messageHelper, IShippingProfileManager shippingProfileManager)
        {
            this.messageHelper = messageHelper;
            this.shippingProfileManager = shippingProfileManager;
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
                        ShippingProfileEntity profile = shippingProfileManager.GetOrCreatePrimaryProfile(shipmentType);
                        if (profile.IsNew)
                        {
                            shippingProfileManager.SaveProfile(profile);
                        }

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
