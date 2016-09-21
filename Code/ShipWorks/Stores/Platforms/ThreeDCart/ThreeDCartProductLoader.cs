using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.ThreeDCart
{
    /// <summary>
    /// Loads 3dcart product details
    /// </summary>
    public class ThreeDCartProductLoader
    {
        private readonly ThreeDCartStoreEntity store;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreeDCartProductLoader"/> class.
        /// </summary>
        /// <param name="store">The store.</param>
        public ThreeDCartProductLoader(ThreeDCartStoreEntity store)
        {
            this.store = store;
        }

        /// <summary>
        /// Loads the product.
        /// </summary>
        public ThreeDCartProductDTO LoadProduct(string threeDCartOrderItemProductId, string threeDCartOrderItemName,
            string cacheOrderItemName, XmlNode getProductResponseXml)
        {
            // Get the product from the result
            XElement apiProduct =
                getProductResponseXml.ToXElement().XPathSelectElement("//GetProductDetailsResponse/Product");

            // Create our product dto to return, setting the properties we know
            ThreeDCartProductDTO productDto = new ThreeDCartProductDTO
            {
                ItemName = threeDCartOrderItemProductId,
                WarehouseBin = GetWarehouseBin(apiProduct)
            };

            LoadProductImages(productDto, apiProduct);

            // get a list of all options for the product
            IEnumerable<XElement> optionTypes = apiProduct.XPathSelectElements("Options/Option/OptionType");

            // If no item options were found or the item name that was passed in is blank we can skip checking options
            // A product could have no options when the order was placed, but it is possible that the product could now have options.
            if (optionTypes.Any() && !string.IsNullOrWhiteSpace(threeDCartOrderItemName))
            {
                List<ThreeDCartOptionNode> optionNodes = GetOptionNodes(apiProduct, cacheOrderItemName);
                if (optionNodes.Any())
                {
                    LoadProductOptions(productDto, optionNodes);
                    return productDto;
                }
            }
            else
            {
                return productDto;
            }

            return null;
        }

        /// <summary>
        /// Gets the warehouse bin.
        /// </summary>
        private string GetWarehouseBin(XElement apiProduct)
        {
            // Get the WarehouseBin, if any
            string warehouseBin = string.Empty;
            XElement warehouseBinXElement = apiProduct.XPathSelectElement("WarehouseBin");
            if (warehouseBinXElement != null)
            {
                warehouseBin = warehouseBinXElement.Value;
            }
            return warehouseBin;
        }

        /// <summary>
        /// Loads the product options.
        /// </summary>
        private void LoadProductOptions(ThreeDCartProductDTO productDto, List<ThreeDCartOptionNode> optionTypeValues)
        {
            ThreeDCartOptionNode optionTypeValue = optionTypeValues.First();

            // The item price could be different from the product xml option price since the item is a snapshot in time
            // But, if we can't get the price from the item, we'll go ahead and set the price to the product's current price
            // then below we'll try to get the item price and use it if we find it
            decimal optionPrice = 0m;
            XElement optionPriceXElement = optionTypeValue.OptionValueNode.XPathSelectElement("OptionPrice");
            if (!string.IsNullOrWhiteSpace(optionPriceXElement?.Value))
            {
                if (!decimal.TryParse(optionPriceXElement.Value, out optionPrice))
                {
                    optionPrice = 0.0m;
                }
            }

            // Add option details to Dto
            productDto.OptionDescription = optionTypeValue.OptionValueNode.XPathSelectElement("Name").Value;
            productDto.OptionName = optionTypeValue.OptionTypeNode.Value;
            productDto.OptionPrice = optionPrice;
        }

        /// <summary>
        /// Load the product images for the item
        /// </summary>
        private void LoadProductImages(ThreeDCartProductDTO productDto, XElement product)
        {
            // We may not have been able to find the product for the online catalog (old product maybe??)
            if (productDto == null)
            {
                return;
            }

            string imageUrl = string.Empty;

            //Iterate through each image, finding a non blank url
            foreach (
                XElement image in
                    product.XPathSelectElements("Images/Image/Url").Where(img => !string.IsNullOrWhiteSpace(img.Value)))
            {
                imageUrl = $"http://{store.StoreDomain}/{image.Value.Trim()}";
                break;
            }

            string thumbnailUrl = string.Empty;
            XElement thumbNail = product.XPathSelectElement("Images/Thumbnail");
            if (!string.IsNullOrWhiteSpace(thumbNail?.Value))
            {
                thumbnailUrl = $"http://{store.StoreDomain}/{thumbNail.Value.Trim()}";
            }

            productDto.ImageUrl = imageUrl;

            if (string.IsNullOrWhiteSpace(thumbnailUrl))
            {
                thumbnailUrl = imageUrl;
            }

            productDto.ImageThumbnail = thumbnailUrl;
        }

        /// <summary>
        /// Gets the option type values.
        /// </summary>
        private List<ThreeDCartOptionNode> GetOptionNodes(XElement apiProduct, string cacheOrderItemName)
        {
            // 3dcart joins the attribute and value by a : in the item product name, so use that format to
            // create a string to check based on the store's products, removing spaces to get an accurate search.
            // The attribute/value combination could be anywhere in the product name, so just see if it's in there anywhere
            // If so, set the product, option, and value.
            List<ThreeDCartOptionNode> optionTypeValues =
                (from optionTypeElement in apiProduct.Descendants("OptionType")
                    from optionValueNameElement in optionTypeElement.Parent?.Descendants("Name")
                    where
                        cacheOrderItemName == $"{optionTypeElement.Value.Trim().Replace(" ", "").ToUpperInvariant()}:" +
                        $"{optionValueNameElement.Value.Trim().Replace(" ", "").ToUpperInvariant()}"
                    select
                        new ThreeDCartOptionNode
                        {
                            OptionTypeNode = optionTypeElement,
                            OptionValueNode = optionValueNameElement.Parent
                        }).ToList();

            return optionTypeValues;
        }
    }
}