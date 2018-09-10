using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.UI.Controls;

namespace ShipWorks.OrderLookup.Controls
{
    /// <summary>
    /// Control to look up orders for single scan mode
    /// </summary>
    [Component]
    public partial class OrderLookupControlHost : UserControl, IOrderLookup
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupControlHost()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Expose the Control
        /// </summary>
        public Control Control => this;
    }
}
