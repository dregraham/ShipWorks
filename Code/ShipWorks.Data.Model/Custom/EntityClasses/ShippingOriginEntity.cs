using Interapptive.Shared.Business;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Extra data on the shipping origin entity
    /// </summary>
    public partial class ShippingOriginEntity
    {
        /// <summary>
        /// Gets the entity as a person adapter
        /// </summary>
        public PersonAdapter AsPersonAdapter() => new PersonAdapter(this, string.Empty);
    }
}
