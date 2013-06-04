using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Enums;

namespace ShipWorks.Tests.Integration.Fitnesse
{
    public class FedExSmartPostFixture : FedExPrototypeFixture
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FedExSmartPostFixture" /> class.
        /// </summary>
        public FedExSmartPostFixture()
            : base()
        {
            
        }

        public string ServiceCode { get; set; }
        public string SmartPostIndicia { get; set; }
        public string SmartPostAncillaryEndorsement { get; set; }
        public string SmartPostHubId { get; set; }
        public string SmartPostCustomerManifestId { get; set; }

        /// <summary>
        /// Creates the shipment.
        /// </summary>
        /// <returns></returns>
        public override ShipmentEntity CreateShipment()
        {
            CustomerReferenceType = "customer_reference";

            ShipmentEntity shipment = base.CreateShipment();

            shipment.FedEx.ReferenceCustomer = string.Empty;
            shipment.FedEx.ReferenceInvoice = string.Empty;

            // FedEx doesn't allow the ref field to be over 30 characters...truncate
            string referencePO = shipment.FedEx.ReferencePO;
            if (!string.IsNullOrWhiteSpace(referencePO) && referencePO.Length > 30)
            {
                shipment.FedEx.ReferencePO = shipment.FedEx.ReferencePO.Replace(" ", "").Substring(0, 30);
            }

            shipment.FedEx.Signature = (int)FedExSignatureType.ServiceDefault;

            shipment.FedEx.SmartPostCustomerManifest = SmartPostCustomerManifestId;
            shipment.FedEx.SmartPostHubID = SmartPostHubId;

            switch (SmartPostIndicia)
            {
                case "MEDIA_MAIL":
                    shipment.FedEx.SmartPostIndicia = (int)FedExSmartPostIndicia.MediaMail;
                    break;
                case "PARCEL_SELECT":
                    shipment.FedEx.SmartPostIndicia = (int)FedExSmartPostIndicia.ParcelSelect;
                    break;
                case "PRESORTED_STANDARD":
                    shipment.FedEx.SmartPostIndicia = (int)FedExSmartPostIndicia.PresortedStandard;
                    break;
                case "PRESORTED_BOUND_PRINTED_MATTER":
                    shipment.FedEx.SmartPostIndicia = (int)FedExSmartPostIndicia.BoundPrintedMatter;
                    break;
                case "PARCEL_RETURN":
                    shipment.FedEx.SmartPostIndicia = (int)FedExSmartPostIndicia.ParcelReturn;
                    break;
            }
            
            switch (SmartPostAncillaryEndorsement)
            {
                case "CARRIER_LEAVE_IF_NO_RESPONSE":
                    shipment.FedEx.SmartPostEndorsement = (int)FedExSmartPostEndorsement.LeaveIfNoResponse;
                    break;
                case "RETURN_SERVICE":
                    shipment.FedEx.SmartPostEndorsement = (int)FedExSmartPostEndorsement.ReturnService;
                    break;
            }

            return shipment;
        }
    }
}
