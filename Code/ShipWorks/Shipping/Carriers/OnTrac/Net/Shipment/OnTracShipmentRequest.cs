using System;
using System.Text;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Carriers.OnTrac.Net.Shipment
{
    /// <summary>
    /// Request for creating a shipment
    /// </summary>
    public class OnTracShipmentRequest : OnTracRequest
    {
        private readonly IHttpRequestSubmitterFactory httpRequestSubmitterFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public OnTracShipmentRequest(IOnTracAccountEntity account) :
            base(account, "OnTracShipmentRequest")
        {
            httpRequestSubmitterFactory = new HttpRequestSubmitterFactory();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public OnTracShipmentRequest(int accountNumber, string password, IHttpRequestSubmitterFactory submitterFactory, ILogEntryFactory logEntryFactory)
            : base(accountNumber, password, logEntryFactory, ApiLogSource.OnTrac, "OnTracShipmentRequest", LogActionType.Other)
        {
            httpRequestSubmitterFactory = submitterFactory;
        }

        /// <summary>
        /// Call OnTrac webservice: OnTrac will create a new shipment and return OnTrac specific information
        /// </summary>
        /// <param name="shipmentRequest">Shipment request DTO based on OnTrac XSD</param>
        /// <returns> Shipment response DTO based on OnTrac XSD </returns>
        /// <exception cref="OnTracException">Thrown if error from OnTrac</exception>
        public Schemas.ShipmentResponse.Shipment ProcessShipment(Schemas.ShipmentRequest.OnTracShipmentRequest shipmentRequest)
        {
            //serialize object for http transmission
            string shipmentRequestListString = SerializationUtility.SerializeToXml(shipmentRequest);
            byte[] shipmentRequestListBytes = Encoding.UTF8.GetBytes(shipmentRequestListString);

            //Create HttpRequest
            IHttpRequestSubmitter shipmentRequestSubmitter = httpRequestSubmitterFactory.GetHttpBinaryPostRequestSubmitter(shipmentRequestListBytes);

            //base string
            string url = $"{BaseUrlUsedToCallOnTrac}{AccountNumber}/shipments?pw={OnTracPassword}";

            shipmentRequestSubmitter.Uri = new Uri(url);

            Schemas.ShipmentResponse.OnTracShipmentResponse shipmentResponseList = ExecuteLoggedRequest<Schemas.ShipmentResponse.OnTracShipmentResponse>(shipmentRequestSubmitter);

            // OnTrac may not return any shipments
            if (shipmentResponseList.Shipments == null || shipmentResponseList.Shipments.Length == 0)
            {
                throw new OnTracException("OnTrac was unable to process the selected shipment.");
            }

            // We only ever request one shipment
            return shipmentResponseList.Shipments[0];
        }
    }
}