using System;
using System.Collections.Generic;
using System.Text;

namespace ShipWorks.UI.Wizard
{
    /// <summary>
    /// Represents the reason why we are changing to a given page in the wizard
    /// </summary>
    public enum WizardStepReason
    {
        /// <summary>
        /// The user clicked the back button
        /// </summary>
        StepBack,

        /// <summary>
        /// The user clicked the next button
        /// </summary>
        StepForward
    }
}
