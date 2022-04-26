using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Shipping.ShipEngine.DTOs;

namespace ShipWorks.Shipping.ShipEngine
{
    public interface ILabelsApi
    {
        Task<Label> LabelsPurchaseLabelAsync(PurchaseLabelRequest request, string apiKey);

        Task<Label> LabelsPurchaseLabelAsync(PurchaseLabelRequest request, string apiKey, string onBehalfOf = null);

        Task<Label> LabelsPurchaseLabelWithRateAsync(string rateId, PurchaseLabelWithoutShipmentRequest request, string apiKey);

        Task<VoidLabelResponse> LabelsVoidLabelAsync(string labelId, string apiKey);

        Task<TrackingInformation> LabelsTrackAsync(string labelId, string apiKey);

        Task<TrackingInformation> TrackingTrackAsync(string apiKey, string carrierCode = null, string trackingNumber = null);
    }
}
