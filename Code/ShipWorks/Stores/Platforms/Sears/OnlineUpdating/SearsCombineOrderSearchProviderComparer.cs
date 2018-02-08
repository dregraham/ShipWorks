using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.Sears.OnlineUpdating
{
    /// <summary>
    /// IEqualityComparer for SearsOrderDetail
    /// </summary>
    public class SearsCombineOrderSearchProviderComparer : IEqualityComparer<SearsOrderDetail>
    {
        /// <summary>
        /// Compare 2 SearsOrderDetails
        /// </summary>
        public bool Equals(SearsOrderDetail x, SearsOrderDetail y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return x.PoNumber.Equals(y.PoNumber) &&
                   x.OrderDate.Equals(y.OrderDate);
        }

        /// <summary>
        /// Get hash code for an SearsOrderDetail
        /// </summary>
        public int GetHashCode(SearsOrderDetail orderDetail)
        {
            return orderDetail.PoNumber.GetHashCode() ^
                   orderDetail.OrderDate.GetHashCode();
        }
    }
}
