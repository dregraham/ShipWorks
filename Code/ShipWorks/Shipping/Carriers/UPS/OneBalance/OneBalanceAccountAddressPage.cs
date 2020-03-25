using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;
using Interapptive.Shared.Business;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Shipping.Carriers.UPS.OneBalance
{
    /// <summary>
    /// Wizard page for entering an address to use with One Balance
    /// </summary>
    [Component(RegistrationType.Self)]
    public partial class OneBalanceAccountAddressPage : WizardPage
    {
        private readonly IOneBalanceUpsAccountRegistrationActivity accountRegistrationActivity;
        private readonly IMessageHelper messageHelper;
        private readonly UpsAccountEntity upsAccount;
        DeviceIdentificationControl deviceIdentification;

        /// <summary>
        /// Constructor
        /// </summary>
        public OneBalanceAccountAddressPage(IOneBalanceUpsAccountRegistrationActivity accountRegistrationActivity, IMessageHelper messageHelper, UpsAccountEntity upsAccount)
        {
            this.accountRegistrationActivity = accountRegistrationActivity;
            this.messageHelper = messageHelper;
            this.upsAccount = upsAccount;
            deviceIdentification = new DeviceIdentificationControl() { Size = new Size(0, 0) };
            InitializeComponent();
            Controls.Add(deviceIdentification);

            StepNextAsync += OnStepNext;
        }

        /// <summary>
        /// When trying to proceed past this wizard page, check that all required fields are filled in and that all
        /// fields are less than their maximum length. If validation passes, register the UPS account with One Balance.
        /// </summary>
        private async Task OnStepNext(object sender, WizardStepEventArgs e)
        {
            if (upsAccount.ShipEngineCarrierId != null)
            {
                return;
            }

            Wizard.Cursor = Cursors.WaitCursor;

            // for some reason, simply disabling the button doesn't work
            Wizard.NextVisible = false;
            Wizard.BackEnabled = false;

            // create PersonAdapter from control
            PersonAdapter personAdapter = new PersonAdapter();
            personControl.SaveToEntity(personAdapter);

            // populate the account with the given address info
            PersonAdapter.Copy(personAdapter, new PersonAdapter(upsAccount, ""));

            Result result = await accountRegistrationActivity.Execute(upsAccount, deviceIdentification.Identity).ConfigureAwait(true);

            if (result.Failure)
            {
                messageHelper.ShowError(this, result.Message);
                e.NextPage = this;
            }
            else
            {
                Wizard.BackEnabled = false;
            }

            Wizard.Cursor = Cursors.Default;
            Wizard.NextVisible = true;
        }
    }
}
