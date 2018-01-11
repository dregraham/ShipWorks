﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.iParcel.Enums;
using ShipWorks.Shipping.Carriers.OnTrac.Enums;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;
using ShipWorks.Stores.Platforms.ChannelAdvisor.DTO;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.OnlineUpdating
{
    /// <summary>
    /// Posts shipping information
    /// </summary>
    [Component]
    public class ChannelAdvisorOnlineUpdater : IChannelAdvisorOnlineUpdater
    {
        private readonly ILog log;
        private readonly IChannelAdvisorUpdateClient updateClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelAdvisorOnlineUpdater(IChannelAdvisorUpdateClient updateClient,
            Func<Type, ILog> createLogger)
        {
            this.updateClient = updateClient;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Upload the tracking number of the shipment
        /// </summary>
        public async Task UploadTrackingNumber(IChannelAdvisorStoreEntity store, long shipmentID)
        {
            ShipmentEntity shipment = ShippingManager.GetShipment(shipmentID);
            if (shipment == null)
            {
                log.InfoFormat("Not uploading tracking number for shipment {0}, order was deleted.", shipmentID);
            }
            else
            {
                await UploadTrackingNumber(store, shipment).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Upload the tracking number of the shipment
        /// </summary>
        public async Task UploadTrackingNumber(IChannelAdvisorStoreEntity store, ShipmentEntity shipment)
        {
            if (!shipment.Processed || shipment.Voided)
            {
                log.InfoFormat("Not uploading tracking number since shipment ID {0} is not processed.", shipment.ShipmentID);
                return;
            }

            OrderEntity order = shipment.Order;

            // If the order is manual but also has combined orders, let it through so the updater
            // can check the combined orders manual status.
            if (!order.IsManual || order.CombineSplitStatus == CombineSplitStatusType.Combined)
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
                    ChannelAdvisorShipment uploadShipment = new ChannelAdvisorShipment()
                    {
                        ShippedDateUtc = shipment.ProcessedDate.Value,
                        ShippingCarrier = carrierCode,
                        ShippingClass = serviceClass,
                        TrackingNumber = trackingNumber
                    };

                    await updateClient.UploadShipmentDetails(store, uploadShipment, order).ConfigureAwait(false);
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
                        tempTracking = track;
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
            ShipmentTypeCode type = (ShipmentTypeCode) shipment.ShipmentType;

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
            switch ((iParcelServiceType) shipment.IParcel.Service)
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

            fedExServiceTypeClassCode.Add(FedExServiceType.FedExFreightEconomy, "Freight Economy");
            fedExServiceTypeClassCode.Add(FedExServiceType.FedExFreightPriority, "Freight Priority");

            fedExServiceTypeClassCode.Add(FedExServiceType.InternationalPriority, "INTLPRIORITY");
            fedExServiceTypeClassCode.Add(FedExServiceType.InternationalPriorityExpress, "INTLPRIORITY");

            fedExServiceTypeClassCode.Add(FedExServiceType.InternationalEconomy, "INTLECONOMY");

            fedExServiceTypeClassCode.Add(FedExServiceType.InternationalFirst, "INTLFIRST");

            /* undefined in CA, the user will have to add this to their store  */
            fedExServiceTypeClassCode.Add(FedExServiceType.InternationalPriorityFreight, "Internaional Priority Freight");

            fedExServiceTypeClassCode.Add(FedExServiceType.InternationalEconomyFreight, "International Economy Freight");

            fedExServiceTypeClassCode.Add(FedExServiceType.SmartPost, "SmartPost");

            FedExServiceType fedExServiceType = (FedExServiceType) shipment.FedEx.Service;

            if (fedExServiceTypeClassCode.ContainsKey((FedExServiceType) shipment.FedEx.Service))
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
            UpsServiceType upsServiceType = (UpsServiceType) shipment.Ups.Service;

            // Sears has a special value we have to send for SurePost
            if (UpsUtility.IsUpsSurePostService(upsServiceType) && IsSearsMarketplaceOrder(shipment))
            {
                return "SurePost";
            }

            Dictionary<UpsServiceType, string> upsServiceTypeClassCodes = GetUpsServiceTypeClassCodesMap();

            return upsServiceTypeClassCodes.ContainsKey(upsServiceType)
                ? upsServiceTypeClassCodes[upsServiceType]
                : "NONE";
        }

        /// <summary>
        /// If the shipment belongs to and order which originated on the sears marketplace
        /// </summary>
        private static bool IsSearsMarketplaceOrder(ShipmentEntity shipment)
        {
            ChannelAdvisorOrderEntity caOrder = shipment.Order as ChannelAdvisorOrderEntity;
            return caOrder?.MarketplaceNames.Contains("sears", StringComparison.InvariantCultureIgnoreCase) ?? false;
        }

        /// <summary>
        /// Gets the ups service type class codes map.
        /// </summary>
        private static Dictionary<UpsServiceType, string> GetUpsServiceTypeClassCodesMap()
        {
            return new Dictionary<UpsServiceType, string>
            {
                {UpsServiceType.Ups2DayAir, "2DAY"},
                {UpsServiceType.Ups2DayAirAM, "2DAA"},
                {UpsServiceType.Ups3DaySelect, "3DS"},
                {UpsServiceType.UpsGround, "GROUND"},
                {UpsServiceType.UpsNextDayAir, "NEXTDAY"},
                {UpsServiceType.UpsNextDayAirAM, "NDAEA"},
                {UpsServiceType.UpsNextDayAirSaver, "NDAS"},
                {UpsServiceType.UpsStandard, "STD"},
                {UpsServiceType.WorldwideExpedited, "WWEX"},
                {UpsServiceType.WorldwideExpress, "WWE"},
                {UpsServiceType.WorldwideExpressPlus, "WWEP"},
                {UpsServiceType.UpsSurePostLessThan1Lb, "SurePost Less than 1 lb"},
                {UpsServiceType.UpsSurePost1LbOrGreater, "SurePost 1 lb or Greater"},
                {UpsServiceType.UpsSurePostBoundPrintedMatter, "SurePost Bound Printed Matter"},
                {UpsServiceType.UpsSurePostMedia, "SurePost Media"},
                {UpsServiceType.UpsMailInnovationsExpedited, "MI"},
                {UpsServiceType.UpsMailInnovationsFirstClass, "MI"},
                {UpsServiceType.UpsMailInnovationsPriority, "MI"},
                {UpsServiceType.UpsMailInnovationsIntEconomy, "MI"},
                {UpsServiceType.UpsMailInnovationsIntPriority, "MI"},
                {UpsServiceType.WorldwideSaver, "UPSWorldwideSaver"},
                {UpsServiceType.UpsExpress, "UPSExpress"},
                {UpsServiceType.UpsExpressEarlyAm, "UPSExpressEarlyAm"},
                {UpsServiceType.UpsExpressSaver, "UPSExpressSaver"},
                {UpsServiceType.UpsExpedited, "UPSExpedited"},
                {UpsServiceType.Ups3DaySelectFromCanada, "UPS3DaySelectFromCanada"},
                {UpsServiceType.UpsCaWorldWideExpressSaver, "UPSCaWorldWideExpressSaver"},
                {UpsServiceType.UpsCaWorldWideExpressPlus, "UPSCaWorldWideExpressPlus"},
                {UpsServiceType.UpsCaWorldWideExpress, "UPSCaWorldWideExpress"},
                {UpsServiceType.Ups2ndDayAirIntra, "UPS2nDayAirIntra"}
            };
        }

        /// <summary>
        /// Gets the postal shipment class code.
        /// </summary>
        [SuppressMessage("SonarLint", "S1871: Two branches in the same conditional structure should not have exactly the same implementation",
            Justification = "It's worth keeping the comment about CriticalMail")]
        private static string GetPostalShipmentClassCode(ShipmentEntity shipment, ChannelAdvisorStoreEntity store)
        {
            PostalServiceType postalServiceType = (PostalServiceType) shipment.Postal.Service;
            switch (postalServiceType)
            {
                case PostalServiceType.ExpressMail:
                    return "EXPRESS";

                case PostalServiceType.FirstClass:
                    return "FIRSTCLASS";

                case PostalServiceType.InternationalExpress:
                    return "GEM";

                case PostalServiceType.GlobalPostSmartSaverEconomyIntl:
                case PostalServiceType.GlobalPostEconomyIntl:
                case PostalServiceType.InternationalFirst:
                    return "IFIRSTCLASS";

                case PostalServiceType.GlobalPostSmartSaverStandardIntl:
                case PostalServiceType.GlobalPostStandardIntl:
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

                // CA doesn't have a default code for Critical right now (10-21-2011) so fall back
                case PostalServiceType.CriticalMail:
                    return "PRIORITY";
            }

            if (ShipmentTypeManager.IsConsolidator(postalServiceType))
            {
                return store.ConsolidatorAsUsps ? "IECONOMY" : "CONSOLIDATOR";
            }

            if (ShipmentTypeManager.IsDhl(postalServiceType))
            {
                return GetDhlClassCode(shipment);
            }

            return "NONE";
        }

        /// <summary>
        /// Get the DHL class code for the shipment
        /// </summary>
        private static string GetDhlClassCode(ShipmentEntity shipment)
        {
            ChannelAdvisorOrderEntity caOrder = shipment.Order as ChannelAdvisorOrderEntity;
            if (caOrder == null)
            {
                throw new ChannelAdvisorException(0, "Non ChannelAdvisor order passed to ChannelAdvisor Uploader.");
            }

            // ebay orders need to upload as Global Mail
            if (caOrder.MarketplaceNames.IndexOf("ebay", StringComparison.OrdinalIgnoreCase) != -1)
            {
                return "Global Mail";
            }

            return "DHL";
        }

        /// <summary>
        /// Gets the OnTrac shipment class code.
        /// </summary>
        private static string GetOntracShipmentClassCode(ShipmentEntity shipment) =>
            EnumHelper.GetDescription((OnTracServiceType) shipment.OnTrac.Service);

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

            switch ((ShipmentTypeCode) shipment.ShipmentType)
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
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));
            MethodConditions.EnsureArgumentIsNotNull(shipment.Amazon, nameof(shipment.Amazon));

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
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));
            MethodConditions.EnsureArgumentIsNotNull(shipment.Amazon, nameof(shipment.Amazon));

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
            // There are a lot of combinations of USPS Express/Priority/FirstClass shipments for Amazon shipping
            // assume that if the service contains USPS and Express that its an express shipment
            // if the shipment contains USPS and Priority its a priority shipment and so on
            if (amazonShippingServiceName.Contains("usps", StringComparison.InvariantCultureIgnoreCase))
            {
                if (amazonShippingServiceName.Contains("express", StringComparison.InvariantCultureIgnoreCase))
                {
                    return "EXPRESS";
                }

                if (amazonShippingServiceName.Contains("priority", StringComparison.InvariantCultureIgnoreCase))
                {
                    return "PRIORITY";
                }

                if (amazonShippingServiceName.Contains("first class", StringComparison.InvariantCultureIgnoreCase))
                {
                    return "FIRSTCLASS";
                }
            }

            switch (amazonShippingServiceName)
            {
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
