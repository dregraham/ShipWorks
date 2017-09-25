using Interapptive.Shared.Business;
using ShipWorks.Shipping;

namespace ShipWorks.Data.Model.EntityInterfaces
{
    /// <summary>
    /// Entity interface which represents the entity 'Order'
    /// </summary>
    public partial interface IOrderEntity
    {
        /// <summary>
        /// Gets the billing address as a person adapter
        /// </summary>
        PersonAdapter BillPerson { get; }

        /// <summary>
        /// Shipping address as a person adapter
        /// </summary>
        PersonAdapter ShipPerson { get; }
    }
}
