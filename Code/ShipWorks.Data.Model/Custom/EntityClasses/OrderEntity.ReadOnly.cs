using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;

namespace ShipWorks.Data.Model.ReadOnlyEntityClasses
{
    /// <summary>
    /// Partial class extension of the LLBLGen ReadOnlyOrderEntity
    /// </summary>
    public partial class ReadOnlyOrderEntity
    {
        private PersonAdapter billPerson;
        private PersonAdapter shipPerson;

        /// <summary>
        /// Gets the bill as a person adapter
        /// </summary>
        /// <remarks>Because we don't have a readonly person adapter, we'll copy it each time it's requested.
        /// This is a relatively light operation so it shouldn't be a big deal.</remarks>
        public PersonAdapter BillPerson => billPerson.CopyToNew();

        /// <summary>
        /// Gets the shipping address as a person adapter
        /// </summary>
        /// <remarks>Because we don't have a readonly person adapter, we'll copy it each time it's requested.
        /// This is a relatively light operation so it shouldn't be a big deal.</remarks>
        public PersonAdapter ShipPerson => shipPerson.CopyToNew();

        /// <summary>
        /// Copy extra data defined in the custom Order entity
        /// </summary>
        partial void CopyCustomOrderData(IOrderEntity source)
        {
            billPerson = source.BillPerson.CopyToNew();
            shipPerson = source.ShipPerson.CopyToNew();
        }
    }
}
