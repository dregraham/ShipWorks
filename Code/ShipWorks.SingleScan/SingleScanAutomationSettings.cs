using ShipWorks.ApplicationCore;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore.Options;
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
        private readonly IMainForm mainForm;
        private readonly IUserSession userSession;
        private readonly IShippingSettings shippingSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleScanAutomationSettings"/> class.
        /// </summary>
        public SingleScanAutomationSettings(IMainForm mainForm, IUserSession userSession, IShippingSettings shippingSettings)
        {
            this.mainForm = mainForm;
            this.userSession = userSession;
            this.shippingSettings = shippingSettings;
        }

        /// <summary>
        /// Whether or not auto weigh is turned on
        /// </summary>
        public bool IsAutoWeighEnabled => userSession.Settings?.AutoWeigh ?? false;

        /// <summary>
        /// Should shipments be auto created
        /// </summary>
        public bool AutoCreateShipments => shippingSettings.AutoCreateShipments;

        /// <summary>
        /// Whether or not auto print is permitted in the current state
        /// </summary>
        public bool IsAutoPrintEnabled()
        {
            return userSession.Settings?.SingleScanSettings == (int) SingleScanSettings.AutoPrint &&
                   !mainForm.AdditionalFormsOpen();
        }
    }
}