using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Utility;
using System.Xml.XPath;

namespace ShipWorks.Stores.Platforms.Infopia
{
    /// <summary>
	/// Represents information downloaded from Infopia about a product SKU
	/// </summary>
	public class InfopiaProductInfo
	{
        public string ImageUrl { get; set; }
        public double WeightLbs { get; set; }
        public string Isbn { get; set; }
        public string Upc { get; set; }
        public string Manufacturer { get; set; }
        public string ManufacturerID { get; set; }
        public string Location { get; set; }
        public string Supplier { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public InfopiaProductInfo()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
		public InfopiaProductInfo(XPathNavigator xpathSku)
		{
            ImageUrl = XPathUtility.Evaluate(xpathSku, "Cell[@Name='*IMAGE 1 URL*']", "");
            Isbn = XPathUtility.Evaluate(xpathSku, "Cell[@Name='*ISBN*']", "");
            Upc = XPathUtility.Evaluate(xpathSku, "Cell[@Name='*UPC*']", "");
            Manufacturer = XPathUtility.Evaluate(xpathSku, "Cell[@Name='*MANUFACTURER*']", "");
            ManufacturerID = XPathUtility.Evaluate(xpathSku, "Cell[@Name='*MANUFACTURER ID*']", "");
            Location = XPathUtility.Evaluate(xpathSku, "Cell[@Name='*BIN LOCATION*']", "");
            Supplier = XPathUtility.Evaluate(xpathSku, "Cell[@Name='*SUPPLIER*']", "");

            double lbs = XPathUtility.Evaluate(xpathSku, "Cell[@Name='*WEIGHT LBS*']", 0.0);
            double oz = XPathUtility.Evaluate(xpathSku, "Cell[@Name='*WEIGHT OZ*']", 0.0);

            WeightLbs = lbs + (oz / 16.0);
        }
    }
}
