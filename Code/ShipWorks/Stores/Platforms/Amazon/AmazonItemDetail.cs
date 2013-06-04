using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.Amazon
{
    /// <summary>
    /// Cached amazon item information
    /// </summary>
    public class AmazonItemDetail
    {
        public string Asin { get; set; }
        public double Weight { get; set; }
        public string ItemUrl { get; set; }
    }
}
