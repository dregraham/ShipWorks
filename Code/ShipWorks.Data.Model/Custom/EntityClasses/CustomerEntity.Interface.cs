using Interapptive.Shared.Business;

namespace ShipWorks.Data.Model.EntityInterfaces
{
    /// <summary>
    /// Custom customer code
    /// </summary>
    public partial interface ICustomerEntity
    {
        /// <summary>
        /// Gets the billing address as a person adapter
        /// </summary>
        PersonAdapter BillPerson { get; }

        /// <summary>
        /// Gets the shipping address as a person adapter
        /// </summary>
        PersonAdapter ShipPerson { get; }
    }
}
