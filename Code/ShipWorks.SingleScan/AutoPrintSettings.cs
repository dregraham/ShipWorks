﻿using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.ApplicationCore.Options;
using ShipWorks.Users;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Permissions for AutoPrint
    /// </summary>
    [Component]
    public class AutoPrintSettings : IAutoPrintSettings
    {
        private readonly IMainForm mainForm;
        private readonly IUserSession userSession;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoPrintSettings"/> class.
        /// </summary>
        public AutoPrintSettings(IMainForm mainForm, IUserSession userSession)
        {
            this.mainForm = mainForm;
            this.userSession = userSession;
        }

        /// <summary>
        /// Whether or not auto print is permitted in the current state
        /// </summary>
        public bool IsAutoPrintEnabled()
        {
            return userSession.Settings?.SingleScanSettings == (int) SingleScanSettings.AutoPrint &&
                   !mainForm.AdditionalFormsOpen();
        }

        /// <summary>
        /// Whether or not auto weigh is turned on
        /// </summary>
        public bool IsAutoWeighEnabled() => userSession.Settings?.AutoWeigh ?? false;
    }
}