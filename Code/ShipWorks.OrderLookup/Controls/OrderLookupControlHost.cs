using System;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.OrderLookup.Controls.OrderLookupControl;
using ShipWorks.UI.Controls;

namespace ShipWorks.OrderLookup.Controls
{
    /// <summary>
    /// Control to look up orders for single scan mode
    /// </summary>
    [Component]
    public partial class OrderLookupControlHost : UserControl, IOrderLookup
    {
        private readonly OrderLookupViewModel orderLookupViewModel;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupControlHost(OrderLookupViewModel orderLookupViewModel)
        {
            InitializeComponent();
            this.orderLookupViewModel = orderLookupViewModel;
        }

        /// <summary>
        /// Set the element host on load
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            Dock = DockStyle.Fill;

            base.OnLoad(e);
            
            ElementHost host = new ElementHost
            {
                Dock = DockStyle.Fill,
                Child = new OrderLookupControl.OrderLookupControl()
                {
                    DataContext = orderLookupViewModel
                }
            };

            Controls.Add(host);
        }

        /// <summary>
        /// Expose the Control
        /// </summary>
        public UserControl Control => this;
    }
}
