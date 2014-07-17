using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Interapptive.Shared.Net;
using log4net;
using log4net.Core;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using ShipWorks.Stores.Content;

namespace ShipWorks.Shipping.Insurance.InsureShip
{
    /// <summary>
    /// InsureShip request class for insuring a shipment
    /// </summary>
    public class InsureShipInsureShipmentRequest : InsureShipRequestBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InsureShipRequestBase"/> class.
        /// </summary>
        public InsureShipInsureShipmentRequest(ShipmentEntity shipment, InsureShipAffiliate affiliate) : 
            base(shipment, affiliate)
        {
            Log = LogManager.GetLogger(typeof(InsureShipInsureShipmentRequest));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InsureShipRequestBase"/> class.
        /// </summary>
        public InsureShipInsureShipmentRequest(IInsureShipResponseFactory responseFactory, ShipmentEntity shipment, InsureShipAffiliate affiliate, IInsureShipSettings insureShipSettings, ILog log) : 
            base(responseFactory, shipment, affiliate, insureShipSettings, log)
        {
        }

        /// <summary>
        /// Submits this request to InsureShip
        /// </summary>
        public override IInsureShipResponse Submit()
        {
            IInsureShipResponse insureShipResponse = ResponseFactory.CreateInsureShipmentResponse(this);
            Uri uri = new Uri(string.Format("{0}distributors/{1}/policies", Settings.Url.AbsoluteUri, Settings.DistributorID));

            insureShipResponse = SubmitPost(uri, CreatePostData());

            return insureShipResponse;
        }

        /// <summary>
        /// Builds a string of all the data that needs to be sent to InsureShip to insure a shipment.
        /// </summary>
        private string CreatePostData()
        {
            // TODO: What is the rate_id to use
            // TODO: Verify that iParcel always saves to InsuranceValue

            PopulateShipmentOrder();

            StringBuilder postData = new StringBuilder();
            postData.AppendFormat("distributor_id={0}&", Settings.DistributorID);
            postData.AppendFormat("store_id={0}&", Affiliate.InsureShipStoreID);
            postData.AppendFormat("store_name={0}&", Affiliate.InsureShipPolicyID);
            postData.AppendFormat("rate_id={0}&", 11);
            postData.AppendFormat("email={0}&", Shipment.OriginEmail);
            postData.AppendFormat("firstname={0}&", Shipment.OriginFirstName);
            postData.AppendFormat("lastname={0}&", Shipment.OriginLastName);
            postData.AppendFormat("shipping_city={0}&", Shipment.ShipCity);
            postData.AppendFormat("shipping_state={0}&", Shipment.ShipStateProvCode);
            postData.AppendFormat("shipping_zip={0}&", Shipment.ShipPostalCode);
            postData.AppendFormat("shipping_country ={0}&", Shipment.ShipCountryCode);
            postData.AppendFormat("shipment_value={0}&", GetShipmentValue());
            postData.AppendFormat("order_id={0}&", Shipment.Order.OrderNumber);
            postData.AppendFormat("shipment_id={0}&", GetUniqueShipmentId());
            postData.AppendFormat("tracking_id={0}&", Shipment.TrackingNumber);
            postData.AppendFormat("item_name={0}&", string.Join(",", Shipment.Order.OrderItems.Select(oi => oi.Name)));

            // If using the test server, append the test affiliate.
            if (Settings.UseTestServer)
            {
                postData.AppendFormat("affiliate_id={0}&", "A0000000003");
            }

            return postData.ToString();
        }

        /// <summary>
        /// Determines the insured value for the shipment.
        /// </summary>
        private decimal GetShipmentValue()
        {
            ShipmentTypeCode shipmentTypeCode = (ShipmentTypeCode) Shipment.ShipmentType;

            switch (shipmentTypeCode)
            {
                case ShipmentTypeCode.UpsOnLineTools:
                case ShipmentTypeCode.UpsWorldShip:
                    return Shipment.Ups.Packages.Sum(p => p.InsuranceValue);
                case ShipmentTypeCode.Endicia:
                case ShipmentTypeCode.Stamps:
                case ShipmentTypeCode.PostalWebTools:
                case ShipmentTypeCode.Express1Endicia:
                case ShipmentTypeCode.Express1Stamps:
                    return Shipment.Postal.InsuranceValue;
                case ShipmentTypeCode.FedEx:
                    return Shipment.FedEx.Packages.Sum(p => p.InsuranceValue);
                case ShipmentTypeCode.OnTrac:
                    return Shipment.OnTrac.InsuranceValue;
                case ShipmentTypeCode.iParcel:
                    return Shipment.IParcel.Packages.Sum(p => p.InsuranceValue);
                case ShipmentTypeCode.Other:
                    return Shipment.Other.InsuranceValue;
                case ShipmentTypeCode.EquaShip:
                    return Shipment.EquaShip.InsuranceValue;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
