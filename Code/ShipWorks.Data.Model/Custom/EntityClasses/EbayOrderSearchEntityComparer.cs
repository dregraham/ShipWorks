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
        public bool Equals(EbayOrderSearchEntity x, EbayOrderSearchEntity y) =>
            x.EbayOrderID.Equals(y.EbayOrderID) &&
            x.EbayBuyerID.Equals(y.EbayBuyerID) &&
            x.SellingManagerRecord.Equals(y.SellingManagerRecord) &&
            x.ExtendedOrderID.Equals(y.ExtendedOrderID);

        /// <summary>
        /// Get hash code for an EbayOrderSearchEntity
        /// </summary>
        public int GetHashCode(EbayOrderSearchEntity ebayOrderSearchEntity) =>
            ebayOrderSearchEntity.EbayOrderID.GetHashCode() ^
            ebayOrderSearchEntity.EbayBuyerID.GetHashCode() ^
            ebayOrderSearchEntity.SellingManagerRecord.GetHashCode() ^
            ebayOrderSearchEntity.ExtendedOrderID.GetHashCode();
    }
}
