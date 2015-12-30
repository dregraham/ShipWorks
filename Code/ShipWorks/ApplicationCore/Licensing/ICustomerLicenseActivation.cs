using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI.Controls;
using ShipWorks.UI.Wizard;

namespace ShipWorks.ApplicationCore.Licensing
{
    public interface ICustomerLicenseActivation
    {
        /// <summary>
        /// Called to save the License Activation
        /// </summary>
        /// <returns></returns>
        UserEntity Save();

        /// <summary>
        /// Event handler when stepping next from the wizard page
        /// </summary>
        event EventHandler<WizardStepEventArgs> StepNext;

        /// <summary>
        /// Event handler for stepping into the wizard page
        /// </summary>
        event EventHandler<WizardSteppingIntoEventArgs> SteppingInto;

        /// <summary>
        /// The view model
        /// </summary>
        /// <remarks>
        /// Exposed because some setup wizards (SimpleDatabaseSetupWizard) needs access
        /// to the users credentials so that it can log the user into ShipWorks
        /// </remarks>
        ICustomerLicenseActivationViewModel ViewModel { get; }
    }
}