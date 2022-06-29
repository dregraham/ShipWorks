using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Amazon.SFP.Platform
{
    /// <summary>
    /// Amazon SFP Shipping web client using Platform
    /// </summary>
    public interface IAmazonSfpShippingPlatformWebClient
    {
        /// <summary>
        /// Gets rates for the given Shipment
        /// </summary>
        RateGroup GetRates(ShipmentEntity shipment);
    }
}
