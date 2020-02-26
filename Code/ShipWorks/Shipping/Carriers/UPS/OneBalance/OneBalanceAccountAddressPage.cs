using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Business;
using Interapptive.Shared.Collections;
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
    [Component]
    public partial class OneBalanceAccountAddressPage : WizardPage
    {
        private readonly IOneBalanceUpsAccountRegistrationActivity accountRegistrationActivity;
        private readonly IMessageHelper messageHelper;
        private readonly UpsAccountEntity upsAccount;

        /// <summary>
        /// Constructor
        /// </summary>
        public OneBalanceAccountAddressPage(IOneBalanceUpsAccountRegistrationActivity accountRegistrationActivity, IMessageHelper messageHelper, UpsAccountEntity upsAccount)
        {
            this.accountRegistrationActivity = accountRegistrationActivity;
            this.messageHelper = messageHelper;
            this.upsAccount = upsAccount;
            InitializeComponent();

            StepNextAsync += OnStepNext;
        }

        /// <summary>
        /// When trying to proceed past this wizard page, check that all required fields are filled in and that all
        /// fields are less than their maximum length. If validation passes, register the UPS account with One Balance.
        /// </summary>
        private async Task OnStepNext(object sender, WizardStepEventArgs e)
        {
            // create PersonAdapter from control
            PersonAdapter personAdapter = new PersonAdapter();
            personControl.SaveToEntity(personAdapter);

            // populate the account with the given address info
            PersonAdapter.Copy(personAdapter, new PersonAdapter(upsAccount, ""));

            // execute account registration activity
            Result result = await accountRegistrationActivity.Execute(upsAccount).ConfigureAwait(true);
            if (result.Failure)
            {
                messageHelper.ShowError(this, result.Message);
                e.NextPage = this;
            }
        }
    }
}
