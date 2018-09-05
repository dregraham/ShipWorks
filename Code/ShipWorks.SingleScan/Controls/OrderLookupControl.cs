using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.UI.Controls;

namespace ShipWorks.SingleScan.Controls
{
    /// <summary>
    /// Control to look up orders for single scan mode
    /// </summary>
    [Component]
    public partial class OrderLookupControl : UserControl, IOrderLookup
    {
        
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Expose the Control
        /// </summary>
        public UserControl Control => this;
    }
}
