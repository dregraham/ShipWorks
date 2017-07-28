using Interapptive.Shared.Collections;

namespace ShipWorks.Stores.Platforms.Magento
{
    /// <summary>
    /// Utility class that contains the logic for parsing the Magento Two Rest IncrementId
    /// into a ShipWorks order number and postfix
    /// </summary>
    public static class MagentoTwoRestOrderNumberUtility
    {
        /// <summary>
        /// Get the Order Number from the increment id
        /// </summary>
        public static long GetOrderNumber(string incrementId)
        {
            long orderNumber;

            // If orderNumberComplete is a number then use it
            if (long.TryParse(incrementId, out orderNumber))
            {
                return orderNumber;
            }

            string[] orderNumberParts = incrementId.Split('-');
            
            // If orderNumberComplete has dashes use the first part as the order number
            if (orderNumberParts.IsCountEqualTo(2) && long.TryParse(orderNumberParts[0], out orderNumber))
            {
                return orderNumber;
            }
            
            // we dont know what format the number is in 
            throw new MagentoException($"Order number {incrementId} is in an unknown format.");

        }

        /// <summary>
        /// Get the Order Number Postfix from the increment id
        /// </summary>
        public static string GetOrderNumberPostfix(string incrementId)
        {
            string[] orderNumberParts = incrementId.Split('-');

            // If there are no dashes we assume that there is no postfix
            if (orderNumberParts.IsCountEqualTo(1))
            {
                return string.Empty;
            }

            // If there is a dash we assume the second part is the postix 
            if (orderNumberParts.IsCountEqualTo(2))
            {
                return $"-{orderNumberParts[1]}";
            }
            
            // we dont know what format the number is in 
            throw new MagentoException($"Order number {incrementId} is in an unknown format.");
        }
    }
}