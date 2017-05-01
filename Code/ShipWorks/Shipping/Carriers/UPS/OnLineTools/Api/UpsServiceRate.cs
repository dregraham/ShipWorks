using ShipWorks.Shipping.Carriers.UPS.Enums;

namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api
{
    /// <summary>
    /// Data structure for storing rating information for a UPS service
    /// </summary>
    public class UpsServiceRate
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UpsServiceRate(UpsServiceType service, decimal amount, bool negotiated, int? guaranteedDaysToDelivery)
        {
            Service = service;
            Amount = amount;
            Negotiated = negotiated;
            GuaranteedDaysToDelivery = guaranteedDaysToDelivery;
        }

        /// <summary>
        /// The guaranteed days automatic delivery if provided by Ups
        /// </summary>
        public int? GuaranteedDaysToDelivery { get; private set; }

        /// <summary>
        /// The service the rate is for
        /// </summary>
        public UpsServiceType Service { get; private set; }

        /// <summary>
        /// The cost to ship with the service
        /// </summary>
        public decimal Amount { get; protected set; }

        /// <summary>
        /// Indicates if the rate is a negotiated "Account Based Rate" (ABR)
        /// </summary>
        public bool Negotiated { get; private set; }
    }
}
