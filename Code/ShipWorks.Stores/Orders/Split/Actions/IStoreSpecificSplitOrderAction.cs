using System.Threading.Tasks;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Orders.Split.Actions
{
    /// <summary>
    /// Split action that is specific to a platform
    /// </summary>
    public interface IStoreSpecificSplitOrderAction
    {
        /// <summary>
        /// Perform the platform specific action
        /// </summary>
        void Perform(long originalOrderID, OrderEntity splitOrder);
    }
}