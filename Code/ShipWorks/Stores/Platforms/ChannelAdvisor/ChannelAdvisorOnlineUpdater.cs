using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.iParcel.Enums;
using ShipWorks.Shipping.Carriers.OnTrac.Enums;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;
using System;
using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor
{
    /// <summary>
    /// Posts shipping information
    /// </summary>
    public class ChannelAdvisorOnlineUpdater
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(ChannelAdvisorOnlineUpdater));
						  
        // the store instance 
        ChannelAdvisorStoreEntity store;

        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelAdvisorOnlineUpdater(ChannelAdvisorStoreEntity store)
        {
            this.store = store;
        }

        /// <summary>
        /// Upload the tracking number of the shipment
        /// </summary>
        public void UploadTrackingNumber(long shipmentID)
        {
            ShipmentEntity shipment = ShippingManager.GetShipment(shipmentID);
            if (shipment == null)
            {
                log.InfoFormat("Not uploading tracking number for shipment {0}, order was deleted.", shipmentID);
            }
            else
            {
                UploadTrackingNumber(shipment);
            }
        }

        /// <summary>
        /// Upload the tracking number of the shipment
        /// </summary>
        public void UploadTrackingNumber(ShipmentEntity shipment)
        {
            if (!shipment.Processed || shipment.Voided)
            {
                log.InfoFormat("Not uploading tracking number since shipment ID {0} is not processed.", shipment.ShipmentID);
                return;
            }

            OrderEntity order = shipment.Order;

            if (!order.IsManual)
            {
                string carrierCode = "";
                string serviceClass = "";
                string trackingNumber = "";

                try
                {
                    ShippingManager.EnsureShipmentLoaded(shipment);
                }
                catch (ObjectDeletedException)
                {
                    log.InfoFormat("Not uploading tracking number since shipment {0} or related information has been deleted.", shipment.ShipmentID);
                    return;
                }
                catch (SqlForeignKeyException)
                {
                    log.InfoFormat("Not uploading tracking number since shipment {0} or related information has been deleted.", shipment.ShipmentID);
                    return;
                }

                GetShipmentUploadValues(shipment, out carrierCode, out serviceClass, out trackingNumber);

                // Upload tracking number
                try
                {
                    ChannelAdvisorClient client = new ChannelAdvisorClient(store);

                    client.UploadShipmentDetails((int)order.OrderNumber, shipment.ProcessedDate.Value, carrierCode, serviceClass, trackingNumber);
                }
                catch (ChannelAdvisorException ex)
                {
                    if (ex.MessageCode == 1)
                    {
                        string newMessage = String.Format("The shipment details upload failed for Carrier '{0}' and Class '{1}'.\r\n\r\n" +
                                                          "Update your ChannelAdvisor store's Account Shipping Carriers to include these values as supported carriers.   The supported carriers are located under the Sales menu in your online store.",
                                                          carrierCode,
                                                          serviceClass);

                        throw new ChannelAdvisorException(newMessage, ex);
                    }
                    else
                    {
                        // rethrow the existing exception 
                        throw;
                    }

                }
            }
            else
            {
                log.InfoFormat("Not uploading tracking number since order {0} is manual.", order.OrderID);
            }
        }

        /// <summary>
        /// Gets the codes required for uploading shipment details to ChannelAdvisor
        /// </summary>
        private static void GetShipmentUploadValues(ShipmentEntity shipment, out string carrierCode, out string serviceClass, out string trackingNumber)
        {
            string tempTracking = shipment.TrackingNumber;
            string tempCarrierCode = GetCarrierCode(shipment);
            string tempServiceClass = GetShipmentClassCode(shipment);

            WorldShipUtility.DetermineAlternateTracking(shipment, (track, service) =>
                {
                    // From CA support:
                    // Thank you for contacting ChannelAdvisor Support. For UPS Mail Innovations, the default carrier code is UPS 
                    // and the class code is MI. 
                    if (!string.IsNullOrWhiteSpace(track))
                    {
                        tempTracking =  track;
                    }
                    tempCarrierCode = "UPS";
                    tempServiceClass = "MI";
                });

            trackingNumber = tempTracking;
            carrierCode = tempCarrierCode;
            serviceClass = tempServiceClass;
        }

                /// <summary>
        /// Gets the CA shipment Class code
        /// http://ssc.channeladvisor.com/howto/account-shipping-carrier-options
        /// </summary>
        public static string GetShipmentClassCode(ShipmentEntity shipment)
        {
            ChannelAdvisorStoreEntity store = StoreManager.GetStore(shipment.Order.StoreID) as ChannelAdvisorStoreEntity;
            return GetShipmentClassCode(shipment, store);
        }

        /// <summary>
        /// Gets the CA shipment Class code
        /// http://ssc.channeladvisor.com/howto/account-shipping-carrier-options
        /// </summary>
        public static string GetShipmentClassCode(ShipmentEntity shipment, ChannelAdvisorStoreEntity store)
        {
            if (!shipment.Processed)
            {
                return "NONE";
            }

            // not going through ShippingManager.GetServiceDescription because we need to not include any prefixes like "USPS"
            ShipmentTypeCode type = (ShipmentTypeCode)shipment.ShipmentType;
            
            if (type == ShipmentTypeCode.Amazon)
            {
                return GetAmazonShipmentClassCode(shipment);
            }

            // If Other, just take the user-entered value
            if (type == ShipmentTypeCode.Other)
            {
                return shipment.Other.Service;
            }

            switch (type)
            {
                case ShipmentTypeCode.Other:
                    return GetOtherShipmentClassCode(shipment);

                case ShipmentTypeCode.iParcel:
                    return GetIParcelShipmentClassCode(shipment);

                case ShipmentTypeCode.FedEx:
                    return GetFedExShipmentClassCode(shipment);

                case ShipmentTypeCode.UpsOnLineTools:
                case ShipmentTypeCode.UpsWorldShip:
                    return GetUpsShipmentClassCode(shipment);

                case ShipmentTypeCode.Usps:
                case ShipmentTypeCode.Endicia:
                case ShipmentTypeCode.Express1Endicia:
                case ShipmentTypeCode.Express1Usps:
                case ShipmentTypeCode.PostalWebTools:
                    return GetPostalShipmentClassCode(shipment, store); 

                case ShipmentTypeCode.OnTrac:
                    return GetOntracShipmentClassCode(shipment);
            }

            return "NONE";
        }

        /// <summary>
        /// Gets the other shipment class code.
        /// </summary>
        private static string GetOtherShipmentClassCode(ShipmentEntity shipment) =>
            shipment.Other.Service;

        /// <summary>
        /// Gets the i parcel shipment class code.
        /// </summary>
        private static string GetIParcelShipmentClassCode(ShipmentEntity shipment)
        {
            switch ((iParcelServiceType)shipment.IParcel.Service)
            {
                case iParcelServiceType.Immediate:
                    return "Immediate";
                case iParcelServiceType.Preferred:
                    return "Preferred";
                case iParcelServiceType.Saver:
                    return "Saver";
                case iParcelServiceType.SaverDeferred:
                    return "Saver Deferred";
                default:
                    return "None";
            }
        }

        /// <summary>
        /// Gets the fed ex shipment class code.
        /// </summary>
        private static string GetFedExShipmentClassCode(ShipmentEntity shipment)
        {
            Dictionary<FedExServiceType, string> fedExServiceTypeClassCode = new Dictionary<FedExServiceType, string>();


            fedExServiceTypeClassCode.Add(FedExServiceType.FedExGround, "GROUND");
            fedExServiceTypeClassCode.Add(FedExServiceType.GroundHomeDelivery, "GROUND");
            fedExServiceTypeClassCode.Add(FedExServiceType.FedExInternationalGround, "GROUND");

            fedExServiceTypeClassCode.Add(FedExServiceType.FedEx2Day, "2Day");
            fedExServiceTypeClassCode.Add(FedExServiceType.OneRate2Day, "2Day");

            fedExServiceTypeClassCode.Add(FedExServiceType.PriorityOvernight, "PRIORITY");
            fedExServiceTypeClassCode.Add(FedExServiceType.OneRatePriorityOvernight, "PRIORITY");

            fedExServiceTypeClassCode.Add(FedExServiceType.FirstOvernight, "1STOVERNIGHT");
            fedExServiceTypeClassCode.Add(FedExServiceType.OneRateFirstOvernight, "1STOVERNIGHT");

            fedExServiceTypeClassCode.Add(FedExServiceType.StandardOvernight, "OVERNIGHT");
            fedExServiceTypeClassCode.Add(FedExServiceType.OneRateStandardOvernight, "OVERNIGHT");
            
            fedExServiceTypeClassCode.Add(FedExServiceType.FedExExpressSaver, "EXPSAVER");
            fedExServiceTypeClassCode.Add(FedExServiceType.OneRateExpressSaver, "EXPSAVER");
            
            fedExServiceTypeClassCode.Add(FedExServiceType.FirstFreight, "OVERNIGHTFREIGHT");
            fedExServiceTypeClassCode.Add(FedExServiceType.FedEx1DayFreight, "OVERNIGHTFREIGHT");
            
            fedExServiceTypeClassCode.Add(FedExServiceType.FedEx2DayFreight, "2DAYFREIGHT");

            fedExServiceTypeClassCode.Add(FedExServiceType.InternationalPriority, "INTLPRIORITY");

            fedExServiceTypeClassCode.Add(FedExServiceType.InternationalEconomy, "INTLECONOMY");

            fedExServiceTypeClassCode.Add(FedExServiceType.InternationalFirst, "INTLFIRST");

            /* undefined in CA, the user will have to add this to their store  */
            fedExServiceTypeClassCode.Add(FedExServiceType.InternationalPriorityFreight, "Internaional Priority Freight");

            fedExServiceTypeClassCode.Add(FedExServiceType.InternationalEconomyFreight, "International Economy Freight");

            fedExServiceTypeClassCode.Add(FedExServiceType.SmartPost, "SmartPost");

            FedExServiceType fedExServiceType = (FedExServiceType)shipment.FedEx.Service;

            if (fedExServiceTypeClassCode.ContainsKey((FedExServiceType)shipment.FedEx.Service))
            {
                return fedExServiceTypeClassCode[fedExServiceType];
            }

            return "None";
        }

        /// <summary>
        /// Gets the ups shipment class code.
        /// </summary>
        private static string GetUpsShipmentClassCode(ShipmentEntity shipment)
        {
            Dictionary<UpsServiceType, string> upsServiceTypeClassCodes = GetUpsServiceTypeClassCodesMap();

            UpsServiceType upsServiceType = (UpsServiceType) shipment.Ups.Service;
            return upsServiceTypeClassCodes.ContainsKey(upsServiceType)
                ? upsServiceTypeClassCodes[upsServiceType]
                : "NONE";
        }

        /// <summary>
        /// Gets the ups service type class codes map.
        /// </summary>
        private static Dictionary<UpsServiceType, string> GetUpsServiceTypeClassCodesMap()
        {
            Dictionary<UpsServiceType, string> upsServiceTypeClassCodes = new Dictionary<UpsServiceType, string>();
            upsServiceTypeClassCodes.Add(UpsServiceType.Ups2DayAir, "2DAY");
            upsServiceTypeClassCodes.Add(UpsServiceType.Ups2DayAirAM, "2DAA");
            upsServiceTypeClassCodes.Add(UpsServiceType.Ups3DaySelect, "3DS");
            upsServiceTypeClassCodes.Add(UpsServiceType.UpsGround, "GROUND");
            upsServiceTypeClassCodes.Add(UpsServiceType.UpsNextDayAir, "NEXTDAY");
            upsServiceTypeClassCodes.Add(UpsServiceType.UpsNextDayAirAM, "NDAEA");
            upsServiceTypeClassCodes.Add(UpsServiceType.UpsNextDayAirSaver, "NDAS");
            upsServiceTypeClassCodes.Add(UpsServiceType.UpsStandard, "STD");
            upsServiceTypeClassCodes.Add(UpsServiceType.WorldwideExpedited, "WWEX");
            upsServiceTypeClassCodes.Add(UpsServiceType.WorldwideExpress, "WWE");
            upsServiceTypeClassCodes.Add(UpsServiceType.WorldwideExpressPlus, "WWEP");
            upsServiceTypeClassCodes.Add(UpsServiceType.UpsSurePostLessThan1Lb, "SurePost Less than 1 lb");
            upsServiceTypeClassCodes.Add(UpsServiceType.UpsSurePost1LbOrGreater, "SurePost 1 lb or Greater");
            upsServiceTypeClassCodes.Add(UpsServiceType.UpsSurePostBoundPrintedMatter, "SurePost Bound Printed Matter");
            upsServiceTypeClassCodes.Add(UpsServiceType.UpsSurePostMedia, "SurePost Media");
            upsServiceTypeClassCodes.Add(UpsServiceType.UpsMailInnovationsExpedited, "MI");
            upsServiceTypeClassCodes.Add(UpsServiceType.UpsMailInnovationsFirstClass, "MI");
            upsServiceTypeClassCodes.Add(UpsServiceType.UpsMailInnovationsPriority, "MI");
            upsServiceTypeClassCodes.Add(UpsServiceType.UpsMailInnovationsIntEconomy, "MI");
            upsServiceTypeClassCodes.Add(UpsServiceType.UpsMailInnovationsIntPriority, "MI");
            upsServiceTypeClassCodes.Add(UpsServiceType.WorldwideSaver, "UPSWorldwideSaver");
            upsServiceTypeClassCodes.Add(UpsServiceType.UpsExpress, "UPSExpress");
            upsServiceTypeClassCodes.Add(UpsServiceType.UpsExpressEarlyAm, "UPSExpressEarlyAm");
            upsServiceTypeClassCodes.Add(UpsServiceType.UpsExpressSaver, "UPSExpressSaver");
            upsServiceTypeClassCodes.Add(UpsServiceType.UpsExpedited, "UPSExpedited");
            upsServiceTypeClassCodes.Add(UpsServiceType.Ups3DaySelectFromCanada, "UPS3DaySelectFromCanada");
            upsServiceTypeClassCodes.Add(UpsServiceType.UpsCaWorldWideExpressSaver, "UPSCaWorldWideExpressSaver");
            upsServiceTypeClassCodes.Add(UpsServiceType.UpsCaWorldWideExpressPlus, "UPSCaWorldWideExpressPlus");
            upsServiceTypeClassCodes.Add(UpsServiceType.UpsCaWorldWideExpress, "UPSCaWorldWideExpress");
            upsServiceTypeClassCodes.Add(UpsServiceType.Ups2ndDayAirIntra, "UPS2nDayAirIntra");
            return upsServiceTypeClassCodes;
        }

        /// <summary>
        /// Gets the postal shipment class code.
        /// </summary>
        private static string GetPostalShipmentClassCode(ShipmentEntity shipment, ChannelAdvisorStoreEntity store)
        {
            PostalServiceType postalServiceType = (PostalServiceType)shipment.Postal.Service;
            switch (postalServiceType)
            {
                case PostalServiceType.ExpressMail:
                    return "EXPRESS";
                case PostalServiceType.FirstClass:
                    return "FIRSTCLASS";
                case PostalServiceType.GlobalPostPriority:
                case PostalServiceType.InternationalExpress:
                    return "GEM";
                case PostalServiceType.GlobalPostEconomy:
                case PostalServiceType.InternationalFirst:
                    return "IFIRSTCLASS";
                case PostalServiceType.InternationalPriority:
                    return "IPRIORITY";
                case PostalServiceType.LibraryMail:
                    return "LIBRARYMAIL";
                case PostalServiceType.MediaMail:
                    return "MEDIA";
                case PostalServiceType.StandardPost:
                    return "PARCELPOST";
                case PostalServiceType.ParcelSelect:
                    return "PARCELSELECT";
                case PostalServiceType.PriorityMail:
                    return "PRIORITY";

                // CA doesnt have a default code for Critical right now (10-21-2011) so fallback
                case PostalServiceType.CriticalMail:
                    return "PRIORITY";
            }

            if (ShipmentTypeManager.IsConsolidator(postalServiceType))
            {
                return store.ConsolidatorAsUsps ? "IECONOMY" : "CONSOLIDATOR";
            }

            if (ShipmentTypeManager.IsDhl(postalServiceType))
            {
                return "Global Mail";
            }

            return "NONE";
        }

        /// <summary>
        /// Gets the ontrac shipment class code.
        /// </summary>
        private static string GetOntracShipmentClassCode(ShipmentEntity shipment) =>
            EnumHelper.GetDescription((OnTracServiceType)shipment.OnTrac.Service);

        /// <summary>
        /// Gets the CA shipment Carrier code.  The values are user-customizable in the CA admin site.
        /// </summary>
        public static string GetCarrierCode(ShipmentEntity shipment)
        {
            ChannelAdvisorStoreEntity store = StoreManager.GetStore(shipment.Order.StoreID) as ChannelAdvisorStoreEntity;
            return GetCarrierCode(shipment, store);
        }

        /// <summary>
        /// Gets the CA shipment Carrier code.  The values are user-customizable in the CA admin site.
        /// </summary>
        public static string GetCarrierCode(ShipmentEntity shipment, ChannelAdvisorStoreEntity store)
        {
            if (!shipment.Processed)
            {
                return "None";
            }

            switch ((ShipmentTypeCode)shipment.ShipmentType)
            {
                case ShipmentTypeCode.Amazon:
                    return GetAmazonCarrierName(shipment);

                case ShipmentTypeCode.FedEx:
                    return "FEDEX";
                
                case ShipmentTypeCode.UpsOnLineTools:
                case ShipmentTypeCode.UpsWorldShip:
                    return "UPS";

                case ShipmentTypeCode.Express1Endicia:
                case ShipmentTypeCode.Express1Usps:
                case ShipmentTypeCode.PostalWebTools:
                    return "USPS";

                case ShipmentTypeCode.Usps:
                case ShipmentTypeCode.Endicia:
                    PostalServiceType service = (PostalServiceType) shipment.Postal.Service;

                    if (ShipmentTypeManager.IsDhl(service))
                    {
                        return "DHL";
                    }
                    else if (ShipmentTypeManager.IsConsolidator(service))
                    {
                        return store.ConsolidatorAsUsps ? "USPS" : "Consolidator";
                    }
                    else
                    {
                        return "USPS"; 
                    }
                
                case ShipmentTypeCode.OnTrac:
                    return "OnTrac";
					
				case ShipmentTypeCode.iParcel:
                    return "i-Parcel";

                case ShipmentTypeCode.Other:
                        return shipment.Other.Carrier;

                default:
                    return "None";
            }
        }

        /// <summary>
        /// Gets the actual carrier for an Amazon shipment
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns></returns>
        private static string GetAmazonCarrierName(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment);
            MethodConditions.EnsureArgumentIsNotNull(shipment.Amazon);

            string carrierName = shipment.Amazon.CarrierName.ToUpperInvariant();

            switch (carrierName)
            {
                case "FEDEX":
                    return "FEDEX";
                case "UPS":
                    return "UPS";
                case "USPS":
                case "STAMPS_DOT_COM":
                    return "USPS";
                default:
                    return "None";
            }
        }

        /// <summary>
        /// Gets the actual service for a Amazon shipment
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns></returns>
        private static string GetAmazonShipmentClassCode(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment);
            MethodConditions.EnsureArgumentIsNotNull(shipment.Amazon);

            // Check to see if it's USPS
            string shippingServiceName = GetAmazonShipmentClassCodeUsps(shipment.Amazon.ShippingServiceName);

            // If it wasn't, check UPS
            if (string.IsNullOrWhiteSpace(shippingServiceName))
            {
                shippingServiceName = GetAmazonShipmentClassCodeUps(shipment.Amazon.ShippingServiceName);
            }

            // If it wasn't, check FedEx
            if (string.IsNullOrWhiteSpace(shippingServiceName))
            {
                shippingServiceName = GetAmazonShipmentClassCodeFedEx(shipment.Amazon.ShippingServiceName);
            }

            // If it wasn't, default to NONE
            if (string.IsNullOrWhiteSpace(shippingServiceName))
            {
                shippingServiceName = "NONE";
            }

            return shippingServiceName;
        }

        /// <summary>
        /// Gets the actual service for a Amazon shipment for UPS
        /// </summary>
        /// <returns></returns>
        private static string GetAmazonShipmentClassCodeUps(string amazonShippingServiceName)
        {
            switch (amazonShippingServiceName)
            {
                case "UPS Ground":
                    return "GROUND";
                case "UPS Next Day Air":
                    return "NEXTDAY";
                case "UPS Next Day Air Saver":
                    return "NDAS";
                case "UPS 2nd Day Air":
                    return "2DAY";
                case "UPS 3 Day Select":
                    return "3DS";
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the actual service for a Amazon shipment for USPS
        /// </summary>
        /// <returns></returns>
        private static string GetAmazonShipmentClassCodeUsps(string amazonShippingServiceName)
        {
            switch (amazonShippingServiceName)
            {
                case "USPS First Class":
                    return "FIRSTCLASS";
                case "USPS Priority Mail":
                case "USPS Priority Mail Flat Rate Box":
                case "USPS Priority Mail Small Flat Rate Box":
                case "USPS Priority Mail Large Flat Rate Box":
                case "USPS Priority Mail Flat Rate Envelope":
                case "USPS Priority Mail Legal Flat Rate Envelope":
                case "USPS Priority Mail Padded Flat Rate Envelope":
                case "USPS Priority Mail Regional Rate Box A":
                case "USPS Priority Mail Regional Rate Box B":
                case "USPS Priority Mail Regional Rate Box C":
                    return "PRIORITY";
                case "USPS Priority Mail Express":
                case "USPS Priority Mail Express Flat Rate Envelope":
                case "USPS Express Mail":
                case "USPS Express Mail Flat Rate Envelope":
                case "USPS Express Mail Legal Flat Rate Envelope":
                    return "EXPRESS";
                case "USPS Parcel Select":
                    return "PARCELSELECT";
                case "USPS Bound Printed Matter":
                    return "BOUNDPRINTEDMATTER";
                case "USPS Media Mail":
                    return "MEDIA";
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the actual service for a Amazon shipment for FedEx
        /// </summary>
        /// <returns></returns>
        private static string GetAmazonShipmentClassCodeFedEx(string amazonShippingServiceName)
        {
            switch (amazonShippingServiceName)
            {
                case "FedEx Priority Overnight®":
                    return "PRIORITY";
                case "FedEx Standard Overnight®":
                    return "OVERNIGHT";
                case "FedEx 2Day®A.M.":
                case "FedEx 2Day®":
                    return "2DAY";
                case "FedEx Express Saver®":
                    return "EXPSAVER";
                case "FedEx Home Delivery®":
                // The spreadsheet from Amazon has the double space between FedEx and Home, so adding it here just in case.
                case "FedEx  Home Delivery®":
                case "FedEx Ground®":
                    return "GROUND";
            }

            return string.Empty;
        }
    }
}
