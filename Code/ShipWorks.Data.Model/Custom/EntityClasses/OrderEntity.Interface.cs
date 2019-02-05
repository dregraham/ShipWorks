using Interapptive.Shared.Business;

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

        /// <summary>
        /// Total cost of the items of the order
        /// </summary>
        decimal SubTotal { get; }
    }
}
