using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
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

        OrderEntity Order { get; }

        /// <summary>
        /// Unload the order
        /// </summary>
        void Unload();

        /// <summary>
        /// Create the label for a shipment
        /// </summary>
        Task CreateLabel();

        /// <summary>
        /// Allow the creation of a label
        /// </summary>
        bool CreateLabelAllowed();

        /// <summary>
        /// Allow reshipping an order
        /// </summary>
        bool ShipAgainAllowed();

        /// <summary>
        /// Ship the shipment again
        /// </summary>
        void ShipAgain();

        /// <summary>
        /// Register the profile handler
        /// </summary>
        void RegisterProfileHandler(Func<Func<ShipmentEntity>, Action<IShippingProfile>, IDisposable> profileRegistration);

        /// <summary>
        /// Save the order
        /// </summary>
        void Save();
    }
}
