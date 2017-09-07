using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Import;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.ThreeDCart.RestApi;

namespace ShipWorks.Stores.Platforms.ThreeDCart
{
    /// <summary>
    /// Loader for 3dCart Item attributes
    /// </summary>
    /// <seealso cref="ShipWorks.Stores.Platforms.ThreeDCart.RestApi.IThreeDCartRestItemAttributeLoader" />
    [Component]
    public class ThreeDCartRestItemAttributeLoader : IThreeDCartRestItemAttributeLoader
    {
        private readonly IOrderElementFactory orderElementFactory;

        public ThreeDCartRestItemAttributeLoader(IOrderElementFactory orderElementFactory)
        {
            this.orderElementFactory = orderElementFactory;
        }

        /// <summary>
        /// Loads the item attributes from the item description
        /// </summary>
        public void LoadItemNameAndAttributes(ThreeDCartOrderItemEntity item, string itemDescription)
        {
            // Unfortunately there is no clean way of getting item attributes from 3dcart, so we have to extract them from the item description field.
            // These appear differently based on the type of option it is, (single selection, multi selection, file, etc.). Here is a sample response.
            // "Cool hat<br><b>
            // Size:</b>&nbsp;Large - $1.00<br><b>
            // Cool Image:</b>&nbsp;Eagle - $8.00<br><b>
            // Pins:</b>&nbsp;Cool Pin $2.00<br>Sweet Pin $3.00<br>Dude Pin $5.00<br><b>
            // Custom Image:</b>&nbsp;<a href=\"/assets/order_images/159(2).jpg\" target=_new>File</a> - $2.00<br><b>
            // Color:</b>&nbsp;Blue"
            //
            // Option types from description above, in order:
            // Item Name, always first line
            // Single value
            // Single value
            // Multiple values, have <br> between each value, no - between value and price
            // File
            // Single value, no price

            // Each option line (name, selected values and their prices) is separated by <br><b>
            string[] descriptionLines = itemDescription.Split(new[] { "<br><b>" }, StringSplitOptions.RemoveEmptyEntries);

            item.Name = descriptionLines.FirstOrDefault();

            // Skip first line since it is item name
            foreach (string descriptionLine in descriptionLines.Skip(1))
            {
                // The option name is always followed by </b>&nbsp; tags
                string[] optionLine = descriptionLine.Split(new[] { "</b>&nbsp;" }, StringSplitOptions.RemoveEmptyEntries);

                // Remove the : because we already add one in the grid
                string optionName = optionLine[0].Trim().TrimEnd(':');

                // Skip one because first line is the option name
                foreach (string optionValues in optionLine.Skip(1))
                {
                    // If an item has multiple values for a single option, they are split by <br>
                    string[] optionValueLines = optionValues.Split(new[] { "<br>" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string optionValueLine in optionValueLines)
                    {
                        LoadAttribute(item, optionName, optionValueLine);
                    }
                }
            }
        }

        /// <summary>
        /// Parses the option value and price out of the option value string and loads the item attribute
        /// </summary>
        private void LoadAttribute(ThreeDCartOrderItemEntity item, string optionName, string optionValueAndPrice)
        {
            // Get unit price
            Regex pricePattern = new Regex(@"\$\d+(?:\.\d+)?");
            Match match = pricePattern.Match(optionValueAndPrice);
            decimal optionPrice = 0;
            if (match.Groups.Count == 1)
            {
                string amount = match.Groups[0].Value;
                decimal.TryParse(amount, NumberStyles.Currency, null, out optionPrice);
            }

            // Get description
            Regex removePricePattern = new Regex(@" \$\d+(?:\.\d+)?");
            string optionValue = removePricePattern.Replace(optionValueAndPrice, string.Empty).Trim('-', ' ');

            orderElementFactory.CreateItemAttribute(item, optionName, optionValue, optionPrice, false);
        }
    }
}