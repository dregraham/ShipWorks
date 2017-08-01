using ShipWorks.Stores.Platforms.Jet.DTO;

namespace ShipWorks.Stores.Platforms.Jet
{
    /// <summary>
    /// Represents a Jet product repository
    /// </summary>
    public interface IProductRepo
    {
        /// <summary>
        /// Get the JetProduct matching he JetOrderItem
        /// </summary>
        JetProduct GetProduct(JetOrderItem item);
    }
}