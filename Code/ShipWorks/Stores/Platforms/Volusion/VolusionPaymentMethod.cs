using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.Volusion
{
    /// <summary>
    /// A single payment method in the Volusion system
    /// </summary>
    public class VolusionPaymentMethod
    {
        public int ID { get; set; }
        public string PaymentType { get; set; }
        public string Name { get; set; }
    }
}
