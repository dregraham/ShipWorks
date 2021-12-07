using System.Collections.Generic;
using ShipEngine.CarrierApi.Client.Model;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Amazon.SWA
{
    public interface IAmazonSWARateGroupFactory
    {
        RateGroup Create(RateResponse rateResponse, ShipmentTypeCode shipmentType, IEnumerable<string> availableServiceTypeApiCodes);
    }
}