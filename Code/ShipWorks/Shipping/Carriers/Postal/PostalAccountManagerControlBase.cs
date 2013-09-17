using System.Windows.Forms;

namespace ShipWorks.Shipping.Carriers.Postal
{
    /// <summary>
    /// Defines the interface for a postal account manager control
    /// </summary>
    public abstract class PostalAccountManagerControlBase : UserControl
    {
        /// <summary>
        /// Initializes the control
        /// </summary>
        public abstract void Initialize();
    }
}