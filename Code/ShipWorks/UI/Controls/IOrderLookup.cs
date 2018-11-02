using System;
using System.Windows.Forms;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// Interface for a OrderLookup
    /// </summary>
    public interface IOrderLookup
    {
        /// <summary>
        /// The Order Lookup Control
        /// </summary>
        UserControl Control { get; }

        /// <summary>
        /// Unload the order
        /// </summary>
        void Unload();

        /// <summary>
        /// Create the label for a shipment
        /// </summary>
        void CreateLabel();

        /// <summary>
        /// Allow the creation of a label
        /// </summary>
        bool CreateLabelAllowed();

        /// <summary>
        /// Register the profile handler
        /// </summary>
        void RegisterProfileHandler(Func<Func<ShipmentTypeCode?>, Action<IShippingProfile>, IDisposable> profileRegistration);
    }
}