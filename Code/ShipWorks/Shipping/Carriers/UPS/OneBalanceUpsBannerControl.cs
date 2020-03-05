using System;
using System.Windows.Forms;
using Autofac;
using ShipWorks.ApplicationCore;

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
                lifetimeScope.Resolve<UpsSetupWizard>().SetupOneBalanceAccount(this);
            }
            
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
