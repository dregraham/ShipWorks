using System;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.UI.Controls;

namespace ShipWorks.OrderLookup.ScanPack
{
    /// <summary>
    /// WinForms element host for ScanPackControl
    /// </summary>
    [Component(RegisterAs = RegistrationType.SpecificService, Service = typeof(IScanPack))]
    public partial class ScanPackControlHost : UserControl, IScanPack
    {
        private readonly IScanPackViewModel scanPackViewModel;
        private ScanPackControl scanPackControl;

        /// <summary>
        /// Constructor
        /// </summary>
        public ScanPackControlHost(IScanPackViewModel scanPackViewModel)
        {
            InitializeComponent();
            this.scanPackViewModel = scanPackViewModel;
            scanPackViewModel.CanAcceptFocus = () => Visible && CanFocus;
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

        /// <summary>
        /// Unload the order
        /// </summary>
        public void Unload()
        {
            scanPackViewModel.Reset();
        }
    }
}
