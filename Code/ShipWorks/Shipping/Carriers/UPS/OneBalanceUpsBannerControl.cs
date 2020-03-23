using System;
using System.Windows.Forms;
using Autofac;
using ShipWorks.ApplicationCore;
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
