using System.Threading.Tasks;

namespace ShipWorks.Stores.Content
{
    /// <summary>
    /// Interface for OrderUtility
    /// </summary>
    public interface IOrderUtility
    {
        /// <summary>
        /// Gets the next OrderNumber that an order should use.  This is useful for store types that don't supply their own
        /// order numbers for ShipWorks, such as Amazon and eBay.
        /// </summary>
        Task<long> GetNextOrderNumberAsync(long storeID);
    }
}
