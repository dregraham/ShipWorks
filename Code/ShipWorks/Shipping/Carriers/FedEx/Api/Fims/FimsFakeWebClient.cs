using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Fims
{
    /// <summary>
    /// Fake FIMS web client for testing success/failure
    /// </summary>
    public class FimsFakeWebClient : IFimsWebClient
    {
        /// <summary>
        /// Ships a FIMS shipment.  
        /// To simulate a successfully request, use "success" as the Username.
        /// For a failed request, send "failure" as the Username.
        /// </summary>
        public IFimsShipResponse Ship(IFimsShipRequest fimsShipRequest)
        {
            if (fimsShipRequest == null)
            {
                throw new ArgumentNullException("fimsShipRequest");
            }

            FimsShipResponse fimsShipResponse;

            // If the username is failure, return a failed responce
            if (fimsShipRequest.Username.Equals("failure", StringComparison.OrdinalIgnoreCase))
            {
                fimsShipResponse = new FimsShipResponse(Guid.NewGuid().ToString(), "0");
                fimsShipResponse.Errors.Add("Fake failure");
            }
            else
            {
                fimsShipResponse = new FimsShipResponse(Guid.NewGuid().ToString(), "1");
                fimsShipResponse.LabelPdfData = Properties.Resources.FakeFIMSLabel;
            }
            
            return fimsShipResponse;
        }
    }
}
