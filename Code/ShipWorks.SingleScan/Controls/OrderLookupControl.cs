using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.UI.Controls;

namespace ShipWorks.SingleScan.Controls
{
    /// <summary>
    /// Control to look up orders for single scan mode
    /// </summary>
    [Component]
    public partial class OrderLookupControl : UserControl, IOrderLookupControl
    {
        
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupControl()
        {
            InitializeComponent();
        }
    }
}
