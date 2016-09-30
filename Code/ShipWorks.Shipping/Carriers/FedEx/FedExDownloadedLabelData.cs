using System.Collections.Generic;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Shipping.Carriers.Api;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Label data that has been downloaded from a carrier
    /// </summary>
    [Component(RegistrationType.Self)]
    public class FedExDownloadedLabelData : IDownloadedLabelData
    {
        private readonly IEnumerable<ICarrierResponse> carrierResponses;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExDownloadedLabelData(IEnumerable<ICarrierResponse> carrierResponses)
        {
            this.carrierResponses = carrierResponses;
        }

        /// <summary>
        /// Save label data to the database and/or disk
        /// </summary>
        public void Save()
        {
            foreach (ICarrierResponse carrierResponse in carrierResponses)
            {
                carrierResponse.Process();
            }
        }
    }
}
