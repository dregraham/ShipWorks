using System;
using System.Xml.XPath;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor
{
    /// <summary>
    /// Represents an item in the MarketplaceAdvisor inventory system for the user.
    /// </summary>
    /// <remarks>
    /// THIS STORE IS DEAD
    /// This store is scheduled for removal as it no longer exists. Do not update this store when making
    /// all-platform changes.
    /// </remarks>
    [Serializable]
    public class MarketplaceAdvisorInventoryItem
    {
        string imageUrl = "";
        double weightLbs = 0;
        string isbn = "";
        string upc = "";
        string sku = "";
        decimal cost = 0.0m;
        string description = "";

        /// <summary>
        /// Constructor for case when inventory lookup failed
        /// </summary>
        public MarketplaceAdvisorInventoryItem()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MarketplaceAdvisorInventoryItem(XPathNavigator item)
        {
            imageUrl = XPathUtility.Evaluate(item, "ImageUrl1", "");

            cost = XPathUtility.Evaluate(item, "CostEach", (decimal) 0.0);

            isbn = XPathUtility.Evaluate(item, "ISBN", "");
            upc = XPathUtility.Evaluate(item, "UPC", "");
            sku = XPathUtility.Evaluate(item, "SKU", "");
            description = XPathUtility.Evaluate(item, "Description", "");

            double weight = XPathUtility.Evaluate(item, "Weight", 0.0);
            int weightType = XPathUtility.Evaluate(item, "WeightType", 0);

            switch (weightType)
            {
                // Oz
                case 12000: weightLbs = weight / 16.0; break;

                // Lb
                case 12001: weightLbs = weight; break;

                // KG
                case 12002: weightLbs = weight * 2.20462262; break;

                // Grams
                case 12003: weightLbs = weight * 2.20462262 / 1000.0; break;
            }
        }

        /// <summary>
        /// The URL to the image for the item.
        /// </summary>
        public string ImageUrl
        {
            get { return imageUrl; }
        }

        /// <summary>
        /// Return the weight, in pounds, of the item.
        /// </summary>
        public double WeightLbs
        {
            get { return weightLbs; }
        }

        public decimal Cost
        {
            get { return cost; }
        }

        public string Description
        {
            get { return description; }
        }

        public string ISBN
        {
            get { return isbn; }
        }

        public string UPC
        {
            get { return upc; }
        }

        public string SKU
        {
            get { return sku; }
        }
    }
}
