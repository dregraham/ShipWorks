using Interapptive.Shared.Business;

namespace ShipWorks.Data.Model.EntityInterfaces
{
    /// <summary>
    /// Custom data for the origin entity
    /// </summary>
    public partial interface IShippingOriginEntity
    {
        /// <summary>
        /// Get the entity as a person adapter
        /// </summary>
        PersonAdapter AsPersonAdapter();
    }
}
