using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Magento
{
    /// <summary>
    /// IEqualityComparer for MagentoOrderSearchEntity
    /// </summary>
    public class MagentoCombineOrderSearchProviderComparer : IEqualityComparer<MagentoOrderSearchEntity>
    {
        /// <summary>
        /// Compare 2 MagentoOrderSearchEntities
        /// </summary>
        public bool Equals(MagentoOrderSearchEntity x, MagentoOrderSearchEntity y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return x.MagentoOrderID.Equals(y.MagentoOrderID);
        }

        /// <summary>
        /// Get hash code for an MagentoOrderSearchEntity
        /// </summary>
        public int GetHashCode(MagentoOrderSearchEntity MagentoOrderSearchEntity)
        {
            return MagentoOrderSearchEntity.MagentoOrderID.GetHashCode();
        }
    }
}
