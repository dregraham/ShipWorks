using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Ebay.Tokens;
using ShipWorks.Stores.Platforms.Ebay.WebServices;

namespace ShipWorks.Stores.Platforms.Ebay.Requests
{
    /// <summary>
    /// Implementation of the eBay GetItem API
    /// </summary>
    public class EbayGetItemRequest : EbayRequest<ItemType, GetItemRequestType, GetItemResponseType>
    {
        GetItemRequestType request;

        /// <summary>
        /// Initializes a new instance of the <see cref="EbayItemRequest"/> class.
        /// </summary>
        public EbayGetItemRequest(EbayToken token, string itemID)
            : base(token, "GetItem")
        {
            request = new GetItemRequestType()
                {
                    ItemID = itemID,
                    OutputSelector = new string[]
                        {
                            "Item.PictureDetails.PictureURL",
                            "Item.ProductListingDetails.IncludeStockPhotoURL",
                            "Item.ProductListingDetails.StockPhotoURL",
                            "Item.ProductListingDetails.UPC",
                            "Item.ProductListingDetails.ISBN",
                            "Item.ShippingPackageDetails.WeightMajor",
                            "Item.ShippingPackageDetails.WeightMinor"
                        },
                    DetailLevel = new DetailLevelCodeType[0]
                };
        }

        /// <summary>
        /// Gets the item details
        /// </summary>
        public override ItemType Execute()
        {
            return SubmitRequest().Item;
        }

        /// <summary>
        /// Create the request to be used for submission
        /// </summary>
        protected override AbstractRequestType CreateRequest()
        {
            return request;
        }
    }
}
