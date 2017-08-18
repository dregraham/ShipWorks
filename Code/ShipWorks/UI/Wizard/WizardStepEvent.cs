using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.ApplicationCore.Licensing;

namespace ShipWorks.UI.Wizard
{
    /// <summary>
    /// EventArgs for the WizardStep event
    /// </summary>
    public class WizardStepEventArgs : EventArgs
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public WizardStepEventArgs(WizardPage nextPage, bool skipping) :
            this(nextPage)
        {
            Skipping = skipping;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public WizardStepEventArgs(WizardPage nextPage)
        {
            NextPage = nextPage;
        }

        /// <summary>
        /// Get or set the next page that will be shown by the wizard.
        /// </summary>
        public WizardPage NextPage { get; set; }

        /// <summary>
        /// Indicates if the Next\Back button wasn't actually clicked, but rather is being
        /// simulated as apart of a Skip.
        /// </summary>
        public bool Skipping { get; set; }
    }
}
