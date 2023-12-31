﻿using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.OnTrac.Enums;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Stores.Platforms.Jet.DTO.Requests;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.Jet
{
    /// <summary>
    /// Factory for creating JetShipment requests
    /// </summary>
    [Component]
    public class JetShipmentRequestFactory : IJetShipmentRequestFactory
    {
        private readonly IShippingManager shippingManager;
        private readonly ISqlAdapterFactory sqlAdapterFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="JetShipmentRequestFactory"/> class.
        /// </summary>
        public JetShipmentRequestFactory(IShippingManager shippingManager, ISqlAdapterFactory sqlAdapterFactory)
        {
            this.shippingManager = shippingManager;
            this.sqlAdapterFactory = sqlAdapterFactory;
        }

        /// <summary>
        /// Create a jet shipment request from a shipment
        /// </summary>
        public JetShipmentRequest Create(ShipmentEntity shipment)
        {
            shippingManager.EnsureShipmentLoaded(shipment);

            JetShipmentRequest request = new JetShipmentRequest {Shipments = new List<JetShipment>()};

            DateTime responseShipmentDate = shipment.ProcessedDate > shipment.ShipDate
                ? shipment.ProcessedDate.Value
                : shipment.ShipDate;
            
            request.Shipments.Add(new JetShipment
            {
                ShipmentTrackingNumber = shipment.TrackingNumber,
                ResponseShipmentDate = responseShipmentDate,
                ResponseShipmentMethod = GetShipmentMethod(shipment),
                ShipFromZipCode = shipment.OriginPostalCode,
                Carrier = GetCarrier(shipment),
                ShipmentItems = GetShipmentItems(shipment)
            });

            return request;
        }

        /// <summary>
        /// Get a list of ShipmentItems for the shipment
        /// </summary>
        private static List<JetShipmentItem> GetShipmentItems(ShipmentEntity shipment)
        {
            return shipment.Order.OrderItems.OfType<IJetOrderItemEntity>()
                .Select(jetItem => new JetShipmentItem
                {
                    MerchantSku = jetItem.MerchantSku,
                    ResponseShipmentSkuQuantity = Convert.ToInt32(jetItem.Quantity)
                })
                .ToList();
        }

        /// <summary>
        /// Get the Jet specific carrier for the shipment
        /// </summary>
        private static string GetCarrier(ShipmentEntity shipment)
        {
            switch (shipment.ShipmentTypeCode)
            {
                case ShipmentTypeCode.UpsOnLineTools:
                case ShipmentTypeCode.UpsWorldShip:
                    return GetUpsCarrier(shipment.Ups);
                case ShipmentTypeCode.Endicia:
                case ShipmentTypeCode.PostalWebTools:
                case ShipmentTypeCode.Express1Endicia:
                case ShipmentTypeCode.Express1Usps:
                case ShipmentTypeCode.Usps:
                    return "USPS";
                case ShipmentTypeCode.FedEx:
                    return GetFedExCarrier(shipment.FedEx);
                case ShipmentTypeCode.OnTrac:
                    return "OnTrac";
                default:
                    return "Other";
            }
        }

        /// <summary>
        /// Get the Fedex specific carrier for Jet
        /// </summary>
        private static string GetFedExCarrier(FedExShipmentEntity shipmentFedEx)
        {
            switch ((FedExServiceType)shipmentFedEx.Service)
            {
                case FedExServiceType.SmartPost:
                    return "FedEx SmartPost";
                case FedExServiceType.FedEx1DayFreight:
                case FedExServiceType.FedEx2DayFreight:
                case FedExServiceType.FedEx3DayFreight:
                case FedExServiceType.FirstFreight:
                    return "FedEx Freight";
                default:
                    return "FedEx";
            }
        }

        /// <summary>
        /// Get the Ups specific carrier for Jet
        /// </summary>
        private static string GetUpsCarrier(UpsShipmentEntity shipmentUps)
        {
            UpsServiceType service = (UpsServiceType) shipmentUps.Service;

            if (UpsUtility.IsUpsSurePostService(service))
            {
                return "UPS SurePost";
            }

            if (UpsUtility.IsUpsMiService(service))
            {
                return "UPS Mail Innovations";
            }

            return "UPS";
        }

        /// <summary>
        /// Get the carrier specific Jet shipment method
        /// </summary>
        private static string GetShipmentMethod(ShipmentEntity shipment)
        {
            switch (shipment.ShipmentTypeCode)
            {
                case ShipmentTypeCode.UpsOnLineTools:
                case ShipmentTypeCode.UpsWorldShip:
                    return GetUpsShipmentMethod(shipment.Ups);
                case ShipmentTypeCode.Endicia:
                case ShipmentTypeCode.PostalWebTools:
                case ShipmentTypeCode.Express1Endicia:
                case ShipmentTypeCode.Express1Usps:
                case ShipmentTypeCode.Usps:
                    return GetUspsShipmentMethod(shipment.Postal);
                case ShipmentTypeCode.FedEx:
                    return GetFedExShipmentMethod(shipment.FedEx);
                case ShipmentTypeCode.OnTrac:
                    return GetOnTracShipmentMethod(shipment.OnTrac);
                default:
                    return "Other";
            }
        }

        /// <summary>
        /// Get the ontrac shipment method for Jet
        /// </summary>
        private static string GetOnTracShipmentMethod(OnTracShipmentEntity shipmentOnTrac)
        {
            switch ((OnTracServiceType) shipmentOnTrac.Service)
            {
                case OnTracServiceType.Sunrise:
                    return "OnTracSunrise";
                case OnTracServiceType.SunriseGold:
                    return "OnTracSunriseGold";
                case OnTracServiceType.PalletizedFreight:
                    return "OnTracPalletizedFreight";
                default:
                    return "OnTracGround";
            }
        }

        /// <summary>
        /// Get the Fedex Shipment Method for Jet
        /// </summary>
        /// <param name="shipmentFedEx"></param>
        /// <returns></returns>
        private static string GetFedExShipmentMethod(FedExShipmentEntity shipmentFedEx)
        {
            switch ((FedExServiceType) shipmentFedEx.Service)
            {
                case FedExServiceType.OneRatePriorityOvernight:
                case FedExServiceType.PriorityOvernight:
                    return "FedexPriorityOvernight";
                case FedExServiceType.OneRateStandardOvernight:
                case FedExServiceType.StandardOvernight:
                    return "FedexStandardOvernight";
                case FedExServiceType.OneRateFirstOvernight:
                case FedExServiceType.FirstOvernight:
                    return "FedexFirstOvernight";
                case FedExServiceType.OneRate2Day:
                case FedExServiceType.FedEx2Day:
                case FedExServiceType.OneRate2DayAM:
                case FedExServiceType.FedEx2DayAM:
                    return "Fedex2Day";
                case FedExServiceType.OneRateExpressSaver:
                case FedExServiceType.FedExExpressSaver:
                    return "FedexExpressSaver";
                case FedExServiceType.GroundHomeDelivery:
                    return "FedExHome";
                case FedExServiceType.SmartPost:
                    return "FedExSmartPost";
                case FedExServiceType.FedEx1DayFreight:
                case FedExServiceType.FedEx2DayFreight:
                case FedExServiceType.FedEx3DayFreight:
                case FedExServiceType.FirstFreight:
                    return "FedExFreight";
                default:
                    return "FedExGround";
            }
        }

        /// <summary>
        /// Get the usps shipment method for Jet
        /// </summary>
        /// <param name="shipmentPostal"></param>
        /// <returns></returns>
        private static string GetUspsShipmentMethod(PostalShipmentEntity shipmentPostal)
        {
            switch ((PostalServiceType) shipmentPostal.Service)
            {
                case PostalServiceType.PriorityMail:
                    return "USPSPriorityMail";
                case PostalServiceType.ExpressMail:
                case PostalServiceType.ExpressMailPremium:
                    return "USPSPriorityMailExpress";
                case PostalServiceType.MediaMail:
                case PostalServiceType.LibraryMail:
                case PostalServiceType.BoundPrintedMatter:
                    return "USPSMediaMail";
                case PostalServiceType.FirstClass:
                case PostalServiceType.ParcelSelect:
                case PostalServiceType.StandardPost:
                default:
                    return "USPSFirstClassMail";
            }
        }

        /// <summary>
        /// Get the ups Shipment method for Jet
        /// </summary>
        /// <param name="shipmentUps"></param>
        /// <returns></returns>
        private static string GetUpsShipmentMethod(UpsShipmentEntity shipmentUps)
        {
            switch ((UpsServiceType) shipmentUps.Service)
            {  
                case UpsServiceType.Ups3DaySelect:
                    return "UPS3DaySelect";
                case UpsServiceType.Ups2DayAir:
                    return "UPS2ndDayAir";
                case UpsServiceType.Ups2DayAirAM:
                    return "UPS2ndDayAirAM";
                case UpsServiceType.UpsNextDayAir:
                    return "UPSNextDayAir";
                case UpsServiceType.UpsNextDayAirSaver:
                    return "UPSNextDayAirSaver";
                case UpsServiceType.UpsNextDayAirAM:
                    return "UPSNextDayAirEarly";
                case UpsServiceType.UpsStandard:
                    return "UPSStandard";
                case UpsServiceType.UpsMailInnovationsFirstClass:
                case UpsServiceType.UpsMailInnovationsPriority:
                case UpsServiceType.UpsMailInnovationsExpedited:
                    return "UPSMailInnovations";
                case UpsServiceType.UpsSurePostLessThan1Lb:
                case UpsServiceType.UpsSurePost1LbOrGreater:
                case UpsServiceType.UpsSurePostBoundPrintedMatter:
                case UpsServiceType.UpsSurePostMedia:
                    return "UPSSurepost";
                case UpsServiceType.UpsExpress:
                case UpsServiceType.UpsExpressEarlyAm:
                case UpsServiceType.UpsExpressSaver:
                    return "UPSExpressCritical";
                default:
                    return "UPSGround";
            }
        }
    }
}