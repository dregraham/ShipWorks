using System.Reflection;

namespace ShipWorks.Stores.Platforms.Amazon.Warehouse
{
    /// <summary>
    /// Amazon warehouse item
    /// </summary>
    [Obfuscation]
    public class AmazonWarehouseItem
    {
        /// <summary>
        /// The items Amazon order item code
        /// </summary>
        public string AmazonOrderItemCode { get; set; }

        /// <summary>
        /// The items ASIN
        /// </summary>
        public string ASIN { get; set; }

        /// <summary>
        /// The items Condition Note
        /// </summary>
        public string ConditionNote { get; set; }
    }
}
