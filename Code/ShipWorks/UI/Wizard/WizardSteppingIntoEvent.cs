using System;
using System.Collections.Generic;
using System.Text;

namespace ShipWorks.UI.Wizard
{
    /// <summary>
    /// EventArgs for the SteppingInto event of the wizard
    /// </summary>
    public class WizardSteppingIntoEventArgs : EventArgs
    {
        WizardPage steppingIntoPage;
        WizardStepReason reason;
        bool firstTime;

        bool skip;
        bool raiseStepEventWhenSkipping;
        WizardPage skipToPage;


        /// <summary>
        /// Constructor
        /// </summary>
        public WizardSteppingIntoEventArgs(WizardPage steppingIntoPage, WizardStepReason reason, bool firstTime, WizardPage skipToPage)
        {
            this.steppingIntoPage = steppingIntoPage;
            this.reason = reason;
            this.firstTime = firstTime;
            this.skipToPage = skipToPage;

            this.skip = false;
            this.raiseStepEventWhenSkipping = false;
        }

        /// <summary>
        /// The page that is being stepped into, if not skipped.
        /// </summary>
        public WizardPage SteppingIntoPage
        {
            get { return steppingIntoPage; }
        }

        /// <summary>
        /// The reason that the wizard is stepping into the page.
        /// </summary>
        public WizardStepReason StepReason
        {
            get
            {
                return reason;
            }
        }

        /// <summary>
        /// Indicates if this is the first time the page is being stepped into
        /// </summary>
        public bool FirstTime
        {
            get { return firstTime; }
        }

        /// <summary>
        /// Indicates if this step of the wizard should be skipped.  The page to skip
        /// to is indicated to SkipToPage.
        /// </summary>
        public bool Skip
        {
            get { return skip; }
            set { skip = value; }
        }

        /// <summary>
        /// Indicates if the StepNext or StepBack event will will be raised even when skipping.
        /// </summary>
        public bool RaiseStepEventWhenSkipping
        {
            get { return raiseStepEventWhenSkipping; }
            set { raiseStepEventWhenSkipping = value; }
        }

        /// <summary>
        /// The page to skip to if Skip is true.  Defaults to the next page in the wizard sequence.
        /// </summary>
        public WizardPage SkipToPage
        {
            get
            {
                return skipToPage;
            }
            set
            {
                skipToPage = value;
            }
        }
    }
}
