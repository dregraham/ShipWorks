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
        // The next page the wizard is going to show
        WizardPage nextPage;

        // Indicates if the Next\Back button wasnt actually clicked, but rather is being
        // simulated as apart of a Skip.
        bool skipping;

        /// <summary>
        /// Constructor
        /// </summary>
        public WizardStepEventArgs(WizardPage nextPage, bool skipping) :
            this(nextPage)
        {
            this.skipping = skipping;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public WizardStepEventArgs(WizardPage nextPage)
        {
            this.nextPage = nextPage;
        }

        /// <summary>
        /// Get or set the next page that will be shown by the wizard.
        /// </summary>
        public WizardPage NextPage
        {
            get
            {
                return nextPage;
            }
            set
            {
                nextPage = value;
            }
        }

        /// <summary>
        /// Indicates if the Next\Back button wasnt actually clicked, but rather is being
        /// simulated as apart of a Skip.
        /// </summary>
        public bool Skipping
        {
            get { return skipping; }
            set { skipping = value; }
        }

        /// <summary>
        /// Task to await before moving to the next page
        /// </summary>
        public Task AwaitTask { get; set; }
    }
}
