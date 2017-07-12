using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Data.Model.ReadOnlyEntityClasses
{
    /// <summary>
    /// Custom customer code
    /// </summary>
    public partial class ReadOnlyCustomerEntity
    {
        private PersonAdapter billPerson;

        /// <summary>
        /// Gets the billing address as a person adapter
        /// </summary>
        public PersonAdapter BillPerson => billPerson;

        /// <summary>
        /// Copy extra data defined in the custom Shipment entity
        /// </summary>
        partial void CopyCustomCustomerData(ICustomerEntity source)
        {
            billPerson = source.BillPerson.CopyToNew();
        }
    }
}
