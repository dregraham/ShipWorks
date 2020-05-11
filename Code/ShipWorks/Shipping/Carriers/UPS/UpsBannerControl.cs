using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.UPS
{
    public partial class UpsBannerControl : UserControl
    {
        /// <summary>
        /// Raised when setup has completed
        /// </summary>
        public event EventHandler SetupComplete;

        public UpsBannerControl()
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
                lifetimeScope.ResolveKeyed<IShipmentTypeSetupWizard>(ShipmentTypeCode.UpsOnLineTools).ShowDialog(this);
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
