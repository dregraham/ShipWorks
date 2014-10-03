using Interapptive.Shared.Business;
using ShipWorks.Shipping.Carriers.Postal.Stamps;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    public class UspsSetupWizard : StampsSetupWizard
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UspsSetupWizard"/> class.
        /// </summary>
        /// <param name="promotion">The promotion.</param>
        /// <param name="allowRegisteringExistingAccount">if set to <c>true</c> [allow registering existing account].</param>
        public UspsSetupWizard(IRegistrationPromotion promotion, bool allowRegisteringExistingAccount)
            : base(promotion, allowRegisteringExistingAccount, ShipmentTypeCode.Usps)
        { }

        /// <summary>
        /// Gets or sets the initial account address.
        /// </summary>
        /// <value>
        /// The initial account address.
        /// </value>
        public PersonAdapter InitialAccountAddress { get; set; }
        
        /// <summary>
        /// Initialization
        /// </summary>
        protected override void OnLoad(object sender, System.EventArgs e)
        {
            base.OnLoad(sender, e);

            if (InitialAccountAddress != null)
            {
                // Pre-load the person control with our initial account address (in the event an account is being
                // created via the Activate Postage Discount dialog
                PersonControl.LoadEntity(InitialAccountAddress);
            }

        }
        /// <summary>
        /// Prepares the stamps account for save. Just sets the reseller type to expedited.
        /// </summary>
        protected override void PrepareStampsAccountForSave()
        {
            base.PrepareStampsAccountForSave();
            StampsAccount.StampsReseller = (int) StampsResellerType.StampsExpedited;
        }
    }
}
