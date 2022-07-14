using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Amazon.DTO
{
    public class CouponCode
    {
        [JsonProperty("orderItemId")]
        public string OrderItemId { get; set; }

        [JsonProperty("codes")]
        public string[] Codes { get; set; }
    }
}
