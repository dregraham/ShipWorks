using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.LemonStand.DTO
{
    public class LemonStandOrderStatuses
    {
        [JsonProperty("data")]
        public List<LemonStandOrderStatus> OrderStatus { get; set; } 
    }
}
