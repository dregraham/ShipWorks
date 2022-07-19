using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.Shipping.Carriers.Amazon.SFP.DTO
{
    public class AmazonTermsVersion
    {
        public string Version { get; set; }
        public string Url { get; set; }
        public string AvailableDate { get; set; }
        public string DeadlineDate { get; set; }
    }
}
