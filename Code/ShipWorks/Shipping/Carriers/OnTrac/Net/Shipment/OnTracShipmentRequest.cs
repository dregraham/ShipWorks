using System;
using System.Text;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.OnTrac.Schemas.Shipment;

namespace ShipWorks.Shipping.Carriers.OnTrac.Net.Shipment
{
    /// <summary>
    /// Request for creating a shipment
    /// </summary>
    public class OnTracShipmentRequest : OnTracRequest
    {
        readonly IHttpRequestSubmitterFactory httpRequestSubmitterFactory;

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
        /// <param name="shipmentRequestList">Shipment request DTO based on OnTrac XSD</param>
        /// <returns> Shipment response DTO based on OnTrac XSD </returns>
        /// <exception cref="OnTracException">Thrown if error from OnTrac</exception>
        public ShipmentResponse ProcessShipment(ShipmentRequestList shipmentRequestList)
        {
            //serialize object for http transmission
            string shipmentRequestListString = SerializationUtility.SerializeToXml(shipmentRequestList);
            byte[] shipmentRequestListBytes = Encoding.UTF8.GetBytes(shipmentRequestListString);

            //Create HttpRequest
            HttpRequestSubmitter shipmentRequestSubmitter = httpRequestSubmitterFactory.GetHttpBinaryPostRequestSubmitter(shipmentRequestListBytes);

            //base string
            string url = string.Format(
                "{0}{1}/shipments?pw={2}",
                BaseUrlUsedToCallOnTrac,
                AccountNumber,
                OnTracPassword);

            shipmentRequestSubmitter.Uri = new Uri(url);

            ShipmentResponseList shipmentResponseList = ExecuteLoggedRequest<ShipmentResponseList>(shipmentRequestSubmitter);

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