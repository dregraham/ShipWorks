using System;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Settings;
using ShipWorks.Settings;
using ShipWorks.Shipping.Settings;
using ShipWorks.Users;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Permissions for AutoPrint
    /// </summary>
    [Component]
    public class SingleScanAutomationSettings : ISingleScanAutomationSettings
    {
        private readonly Func<IMainForm> createMainForm;
        private readonly IUserSession userSession;
        private readonly IShippingSettings shippingSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleScanAutomationSettings"/> class.
        /// </summary>
        public SingleScanAutomationSettings(Func<IMainForm> createMainForm, IUserSession userSession, IShippingSettings shippingSettings)
        {
            this.createMainForm = createMainForm;
            this.userSession = userSession;
            this.shippingSettings = shippingSettings;
        }

        /// <summary>
        /// Whether or not auto weigh is turned on
        /// </summary>
        public bool IsAutoWeighEnabled => userSession.Settings?.AutoWeigh ?? false;

        /// <summary>
        /// Whether or not to require orders to be scan pack validated
        /// </summary>
        public bool RequireVerificationToShip => userSession.Settings?.RequireVerificationToShip ?? false;

        /// <summary>
        /// Should shipments be auto created
        /// </summary>
        public bool AutoCreateShipments => shippingSettings.AutoCreateShipments;

        /// <summary>
        /// Behavior when an scanned order has multiple shipments
        /// </summary>
        public SingleScanConfirmationMode ConfirmationMode => userSession.Settings.SingleScanConfirmationMode;

        /// <summary>
        /// Whether or not auto print is permitted in the current state
        /// </summary>
        public bool IsAutoPrintEnabled()
        {
            return userSession.Settings?.SingleScanSettings == (int) SingleScanSettings.AutoPrint &&
                   !createMainForm().AdditionalFormsOpen();
        }
    }
}
