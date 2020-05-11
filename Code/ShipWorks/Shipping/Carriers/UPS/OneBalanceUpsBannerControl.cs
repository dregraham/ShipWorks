using System;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.Collections;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    /// OneBalanceUpsBannerControl
    /// </summary>
    public partial class OneBalanceUpsBannerControl : UserControl
    {
        /// <summary>
        /// Raised when setup has completed
        /// </summary>
        public event EventHandler SetupComplete;

        /// <summary>
        /// Constructor
        /// </summary>
        public OneBalanceUpsBannerControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Resize the label when the control changes size
        /// </summary>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            detailLabel.Width = Width - 110;
        }

        /// <summary>
        /// Click Enable Ups
        /// </summary>
        private void OnEnableUps(object sender, EventArgs e)
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                var uspsAccountRepository = lifetimeScope.Resolve<ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>>();
                if (uspsAccountRepository.AccountsReadOnly.None())
                {
                    MessageHelper.ShowError(this.ParentForm, "Getting access to great rates with UPS from ShipWorks requires a Stamps.com account.\n\r\n\rPlease create a Stamps.com account in the USPS portion of Shipping Settings and try again.");
                    return;
                }
                lifetimeScope.ResolveKeyed<IOneBalanceSetupWizard>(ShipmentTypeCode.UpsOnLineTools).SetupOneBalanceAccount(this);
            }

            UpsAccountManager.CheckForChangesNeeded();
            RaiseSetupComplete();
        }

        /// <summary>
        /// Raise the setup complete event
        /// </summary>
        private void RaiseSetupComplete()
        {
            SetupComplete?.Invoke(this, EventArgs.Empty);
        }
    }
}
