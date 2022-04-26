using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Shipping.ShipEngine.DTOs;

namespace ShipWorks.Shipping.ShipEngine.API
{
    public class LabelsApi : ILabelsApi
    {
        private readonly string endpoint = string.Empty;

        public LabelsApi()
        {

        }

        public LabelsApi(string endpoint)
        {
            this.endpoint = endpoint;
        }

        public Task<Label> LabelsPurchaseLabelAsync(PurchaseLabelRequest request, string apiKey)
        {
            throw new NotImplementedException();
        }

        public Task<Label> LabelsPurchaseLabelAsync(PurchaseLabelRequest request, string apiKey, string onBehalfOf = null)
        {
            throw new NotImplementedException();
        }


        public Task<Label> LabelsPurchaseLabelWithRateAsync(string rateId, PurchaseLabelWithoutShipmentRequest request, string apiKey)
        {
            throw new NotImplementedException();
        }

        public Task<VoidLabelResponse> LabelsVoidLabelAsync(string labelId, string apiKey)
        {
            throw new NotImplementedException();
        }

        public Task<TrackingInformation> LabelsTrackAsync(string labelId, string apiKey)
        {
            throw new NotImplementedException();
        }

        public Task<TrackingInformation> TrackingTrackAsync(string apiKey, string carrierCode = null, string trackingNumber = null)
        {
            throw new NotImplementedException();
        }
    }
}
