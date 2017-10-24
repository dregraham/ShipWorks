using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.Stores.Platforms.Magento
{
    /// <summary>
    /// DTO to hold the data needed for a Magneto Upload Action
    /// </summary>
    public struct MagentoUploadAction
    {
        public long OrderNumber { get; set; }
        public string Action { get; set; }
        public string Comments { get; set; }
        public string Carrier { get; set; }
        public string TrackingNumber { get; set; }
        public bool SendEmail { get; set; }
    }
}
