using ShipWorks.Stores.Content.CombinedOrderSearchProviders;

namespace ShipWorks.Stores.Platforms.SparkPay
{
    /// <summary>
    /// Combined order search provider for SparkPay
    /// </summary>
    public interface ISparkPayCombineOrderSearchProvider : ICombineOrderSearchProvider<long>
    {
    }
}