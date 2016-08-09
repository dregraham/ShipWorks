using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Data.Model.ReadOnlyEntityClasses
{
    /// <summary>
    /// Custom data for the origin entity
    /// </summary>
    public partial class ReadOnlyShippingOriginEntity
    {
        private PersonAdapter personAdapter;

        /// <summary>
        /// Get the entity as a person adapter
        /// </summary>
        public PersonAdapter AsPersonAdapter() => personAdapter;

        /// <summary>
        /// Copy extra data
        /// </summary>
        partial void CopyCustomShippingOriginData(IShippingOriginEntity source)
        {
            personAdapter = source.AsPersonAdapter();
        }
    }
}
