using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.OrderLookup.ScanPack.ScanPack;
using ShipWorks.SingleScan.ScanPack;
using ShipWorks.UI.Controls;

namespace ShipWorks.OrderLookup.ScanPack
{
    [Component(RegisterAs = RegistrationType.SpecificService, Service = typeof(IScanPack))]
    public partial class ScanPackControlHost : UserControl, IScanPack
    {
        private readonly IScanPackViewModel scanPackViewModel;
        private ScanPackControl scanPackControl;

        public ScanPackControlHost(IScanPackViewModel scanPackViewModel)
        {
            InitializeComponent();
            this.scanPackViewModel = scanPackViewModel;
        }

        /// <summary>
        /// Expose the Control
        /// </summary>
        public UserControl Control => this;

        /// <summary>
        /// Set the element host on load
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            Dock = DockStyle.Fill;

            base.OnLoad(e);

            scanPackControl = new ScanPackControl
            {
                DataContext = scanPackViewModel
            };

            ElementHost host = new ElementHost
            {
                Dock = DockStyle.Fill,
                Child = scanPackControl
            };

            Controls.Add(host);
        }
    }
}
