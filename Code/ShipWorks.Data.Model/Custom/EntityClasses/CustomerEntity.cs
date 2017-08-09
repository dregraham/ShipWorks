using Interapptive.Shared.Business;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Custom customer code
    /// </summary>
    public partial class CustomerEntity
    {
        /// <summary>
        /// Gets the billing address as a person adapter
        /// </summary>
        public PersonAdapter BillPerson => new PersonAdapter(this, "Bill");

        /// <summary>
        /// Gets the shipping address as a person adapter
        /// </summary>
        public PersonAdapter ShipPerson => new PersonAdapter(this, "Ship");
    }
}
