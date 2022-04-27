using System.Collections.Generic;
using ShipWorks.Shipping.ShipEngine.DTOs;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Amazon.SWA
{
    public interface IAmazonSWARateGroupFactory
    {
        RateGroup Create(RateResponse rateResponse, ShipmentTypeCode shipmentType, IEnumerable<string> availableServiceTypeApiCodes);
    }
}