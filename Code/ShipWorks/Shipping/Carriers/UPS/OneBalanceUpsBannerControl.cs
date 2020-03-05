using System;
using System.Windows.Forms;
using Autofac;

namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    /// OneBalanceUpsBannerControl
    /// </summary>
    public partial class OneBalanceUpsBannerControl : UserControl
    {
        private readonly ILifetimeScope scope;

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
        /// Constructor
        /// </summary>
        public OneBalanceUpsBannerControl(ILifetimeScope scope)
        {
            InitializeComponent();
            this.scope = scope;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            detailLabel.Width = Width - 100;

        }

        /// <summary>
        /// Click Enable Ups
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEnableUps(object sender, EventArgs e)
        {
            scope.Resolve<UpsSetupWizard>().SetupOneBalanceAccount(this);
            
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
