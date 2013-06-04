using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.Volusion.WizardPages
{
    /// <summary>
    /// Setup wizard state information for configuring Volusion
    /// </summary>
    public class VolusionSetupState
    {
        // type of configuration selected by the user
        public VolusionSetupConfiguration Configuration { get; set; }

        // whether or not shipping methods were automatically detected/downloaded
        public bool AutoDownloadedShippingMethods { get; set; }

        // whether or not payment methods were automaticallyi detected/downloaded
        public bool AutoDownloadedPaymentMethods { get; set; }
    }
}
