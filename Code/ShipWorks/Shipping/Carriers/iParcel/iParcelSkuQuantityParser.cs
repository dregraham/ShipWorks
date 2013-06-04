using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.iParcel
{
    /// <summary>
    /// A class for parsing a pipe delimited list into a Dictionary of SKUs and quantities where
    /// the list in the format of [SKU],[Quantity | [SKU],[Quantity] |...
    /// </summary>
    public class iParcelSkuQuantityParser
    {
        private readonly ITokenProcessor tokenProcessor;
        private readonly ShipmentEntity shipment;

        /// <summary>
        /// Initializes a new instance of the <see cref="iParcelSkuQuantityParser" /> class.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        public iParcelSkuQuantityParser(ShipmentEntity shipment)
            : this(shipment, new iParcelTokenProcessor())
        { }

        /// <summary>
        /// Prevents a default instance of the <see cref="iParcelSkuQuantityParser" /> class from being created.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="tokenProcessor">The token processor.</param>
        public iParcelSkuQuantityParser(ShipmentEntity shipment, ITokenProcessor tokenProcessor)
        {
            this.shipment = shipment;
            this.tokenProcessor = tokenProcessor;
        }

        /// <summary>
        /// Builds a dictionary of SKU and quantities based on the delimited string provided. The string 
        /// should be in the format of [SKU],[Quantity | [SKU],[Quantity] |...
        /// </summary>
        /// <param name="delimitedItemList">The delimited item list.</param>
        /// <returns>A Dictionary of SKU and quanitity values.</returns>
        /// <exception cref="iParcelException">SKU/Quantity string is in an invalid format. The expected format is [SKU],[Quantity|[SKU],[Quantity]</exception>
        public Dictionary<string, int> Parse(string delimitedItemList)
        {
            Dictionary<string, int> skuQuantities = new Dictionary<string, int>();

            // The string should be in the format of [SKU],[Quantity | [SKU],[Quantity] |...
            // Trim the result as any trailing whitespace after a pipe would cause a parsing error
            string processedItemList = tokenProcessor.Process(delimitedItemList, shipment).Trim();
            if (processedItemList.EndsWith("|", StringComparison.OrdinalIgnoreCase))
            {
                // Make sure the delimited string does not end with | otherwise an error would
                // be generated when parsing out the groupings due to an empty/dangling item group
                processedItemList = processedItemList.Remove(processedItemList.Length - 1);
            }

            string[] itemGroupings = processedItemList.Split(new char[] { '|' });
            foreach (string grouping in itemGroupings)
            {
                // Now split the string again into a SKU and Quantity
                string[] skuQuantity = grouping.Trim().Split(new char[] { ',' });
                if (skuQuantity.Length != 2)
                {
                    throw new iParcelException("\"SKU and Quantity\" string is in an invalid format. The expected format is [SKU],[Quantity] | [SKU],[Quantity]");
                }

                string sku = skuQuantity[0].Trim();
                if (string.IsNullOrWhiteSpace(sku))
                {
                    throw new iParcelException("The \"SKU and Quantity\" field is in an invalid format. A SKU value is missing.");
                }


                int quantity = 0;
                if (!int.TryParse(skuQuantity[1].Trim(), out quantity))
                {
                    string message = string.Format("{0} is an invalid quantity value referenced by the \"SKU and Quantity\" field. The quantity value must be an integer.", skuQuantity[1]);
                    throw new iParcelException(message);
                }

                if (skuQuantities.ContainsKey(sku))
                {
                    skuQuantities[sku] += quantity;
                }
                else
                {
                    skuQuantities.Add(sku, quantity);
                }
            }

            return skuQuantities;
        }
    }
}
