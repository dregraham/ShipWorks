using System.Collections.Generic;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// IEqualityComparer for EbayOrderSearchEntity
    /// </summary>
    public class EbayOrderSearchEntityComparer : IEqualityComparer<EbayOrderSearchEntity>
    {
        /// <summary>
        /// Compare 2 EbayOrderSearchEntities
        /// </summary>
        public bool Equals(EbayOrderSearchEntity x, EbayOrderSearchEntity y)
        {
            return x.EbayOrderID.Equals(y.EbayOrderID) &&
                   x.EbayBuyerID.Equals(y.EbayBuyerID) &&
                   x.SellingManagerRecord.Equals(y.SellingManagerRecord);
        }

        /// <summary>
        /// Get hash code for an EbayOrderSearchEntity
        /// </summary>
        public int GetHashCode(EbayOrderSearchEntity ebayOrderSearchEntity)
        {
            return ebayOrderSearchEntity.EbayOrderID.GetHashCode() ^
                   ebayOrderSearchEntity.EbayBuyerID.GetHashCode() ^
                   ebayOrderSearchEntity.SellingManagerRecord.GetHashCode();
        }
    }
}
