using System;
using System.Linq;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net
{
    /// <summary>
    /// Results from creating an indicium
    /// </summary>
    public class CreateIndiciumResult
    {
        public string Result { get; internal set; }
        public string IntegratorTxID { get; set; }
        public string TrackingNumber { get; set; }
        public RateV20 Rate { get; set; }
        public Guid StampsTxID { get; set; }
        public string URL { get; set; }
        public PostageBalance PostageBalance { get; set; }
        public string Mac { get; set; }
        public string PostageHash { get; set; }
        public byte[][] ImageData { get; set; }

        /// <summary>
        /// Get the shipment cost
        /// </summary>
        public decimal ShipmentCost
        {
            get
            {
                return Rate.Amount +
                    (Rate.AddOns?.Where(a => a.AddOnType != AddOnTypeV7.SCAINS).Sum(a => a.Amount) ?? 0);
            }
        }
    }
}