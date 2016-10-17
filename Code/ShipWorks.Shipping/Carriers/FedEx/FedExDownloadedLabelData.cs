using System;
using System.Collections.Generic;
using log4net;
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
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExDownloadedLabelData(IEnumerable<ICarrierResponse> carrierResponses, Func<Type, ILog> getLogger)
        {
            this.carrierResponses = carrierResponses;
            log = getLogger(typeof(FedExDownloadedLabelData));
        }

        /// <summary>
        /// Save label data to the database and/or disk
        /// </summary>
        public void Save()
        {
            try
            {
                foreach (ICarrierResponse carrierResponse in carrierResponses)
                {
                    carrierResponse.Process();
                }
            }
            catch (CarrierException ex)
            {
                // There was an exception communicating with the API - the request went through and a response
                // was received, but the response most likely had an error result.
                log.Error(ex.Message);

                string errorMessage = $"An error occurred while communicating with FedEx. {ex.Message}";

                if (!errorMessage.EndsWith("."))
                {
                    errorMessage = $"{errorMessage}.";
                }

                throw new ShippingException(errorMessage, ex);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new ShippingException(ex.Message, ex);
            }
        }
    }
}

