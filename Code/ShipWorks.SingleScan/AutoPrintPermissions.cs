using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.ApplicationCore.Options;
using ShipWorks.Users;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Permissions for AutoPrint
    /// </summary>
    [Component]
    public class AutoPrintPermissions : IAutoPrintPermissions
    {
        private readonly IMainForm mainForm;
        private readonly IUserSession userSession;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoPrintPermissions"/> class.
        /// </summary>
        public AutoPrintPermissions(IMainForm mainForm, IUserSession userSession)
        {
            this.mainForm = mainForm;
            this.userSession = userSession;
        }

        /// <summary>
        /// Whether or not auto print is permitted in the current state
        /// </summary>
        public bool AutoPrintPermitted()
        {
            return userSession.Settings?.SingleScanSettings == (int) SingleScanSettings.AutoPrint &&
                   !mainForm.AdditionalFormsOpen();
        }

        /// <summary>
        /// Whether or not auto weigh is turned on
        /// </summary>
        public bool AutoWeighOn() => userSession.Settings?.AutoWeigh ?? false;
    }
}