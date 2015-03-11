using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Carriers.iParcel.Enums;
using ShipWorks.Data.Connection;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.FedEx;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.OnTrac.Enums;

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
            if (!shipment.Processed)
            {
                return "NONE";
            }

            // not going through ShippingManager.GetServiceDescription because we need to not include any prefixes like "USPS"
            ShipmentTypeCode type = (ShipmentTypeCode)shipment.ShipmentType;

            // If Other, just take the user-entered value
            if (type == ShipmentTypeCode.Other)
            {
                return shipment.Other.Service;
            }

            switch (type)
            {
                case ShipmentTypeCode.Other:
                    return shipment.Other.Service;
                case ShipmentTypeCode.iParcel:
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
                    }
                    return "None";
                case ShipmentTypeCode.FedEx:
                {
                    switch ((FedExServiceType) shipment.FedEx.Service)
                    {
                        case FedExServiceType.FedExGround:
                        case FedExServiceType.GroundHomeDelivery:
                            return "GROUND";

                        case FedExServiceType.FedEx2Day:
                        case FedExServiceType.OneRate2Day:
                            return "2Day";

                        case FedExServiceType.PriorityOvernight:
                        case FedExServiceType.OneRatePriorityOvernight:
                            return "PRIORITY";

                        case FedExServiceType.FirstOvernight:
                        case FedExServiceType.OneRateFirstOvernight:
                            return "1STOVERNIGHT";

                        case FedExServiceType.StandardOvernight:
                        case FedExServiceType.OneRateStandardOvernight:
                            return "OVERNIGHT";

                        case FedExServiceType.FedExExpressSaver:
                        case FedExServiceType.OneRateExpressSaver:
                            return "EXPSAVER";

                        case FedExServiceType.FirstFreight:
                        case FedExServiceType.FedEx1DayFreight:
                            return "OVERNIGHTFREIGHT";

                        case FedExServiceType.FedEx2DayFreight:
                            return "2DAYFREIGHT";

                        case FedExServiceType.InternationalPriority:
                            return "INTLPRIORITY";

                        case FedExServiceType.InternationalEconomy:
                            return "INTLECONOMY";

                        case FedExServiceType.InternationalFirst:
                            return "INTLFIRST";

                            /* undefined in CA, the user will have to add this to their store  */
                        case FedExServiceType.InternationalPriorityFreight:
                            return "Internaional Priority Freight";

                        case FedExServiceType.InternationalEconomyFreight:
                            return "International Economy Freight";

                        case FedExServiceType.SmartPost:
                            return "SmartPost";
                    }

                    break;
                }
                case ShipmentTypeCode.UpsOnLineTools:
                case ShipmentTypeCode.UpsWorldShip:
                {
                    switch ((UpsServiceType) shipment.Ups.Service)
                    {
                        case UpsServiceType.Ups2DayAir:
                            return "2DAY";
                        case UpsServiceType.Ups2DayAirAM:
                            return "2DAA";
                        case UpsServiceType.Ups3DaySelect:
                            return "3DS";
                        case UpsServiceType.UpsGround:
                            return "GROUND";
                        case UpsServiceType.UpsNextDayAir:
                            return "NEXTDAY";
                        case UpsServiceType.UpsNextDayAirAM:
                            return "NDAEA";
                        case UpsServiceType.UpsNextDayAirSaver:
                            return "NDAS";
                        case UpsServiceType.UpsStandard:
                            return "STD";
                        case UpsServiceType.WorldwideExpedited:
                            return "WWEX";
                        case UpsServiceType.WorldwideExpress:
                            return "WWE";
                        case UpsServiceType.WorldwideExpressPlus:
                            return "WWEP";
                        case UpsServiceType.UpsSurePostLessThan1Lb:
                            return "SurePost Less than 1 lb";
                        case UpsServiceType.UpsSurePost1LbOrGreater:
                            return "SurePost 1 lb or Greater";
                        case UpsServiceType.UpsSurePostBoundPrintedMatter:
                            return "SurePost Bound Printed Matter";
                        case UpsServiceType.UpsSurePostMedia:
                            return "SurePost Media";
                        case UpsServiceType.UpsMailInnovationsExpedited:
                        case UpsServiceType.UpsMailInnovationsFirstClass:
                        case UpsServiceType.UpsMailInnovationsPriority:
                        case UpsServiceType.UpsMailInnovationsIntEconomy:
                        case UpsServiceType.UpsMailInnovationsIntPriority:
                            return "MI";
                        case UpsServiceType.WorldwideSaver:
                            return "UPSWorldwideSaver";
                        case UpsServiceType.UpsExpress:
                            return "UPSExpress";
                        case UpsServiceType.UpsExpressEarlyAm:
                            return "UPSExpressEarlyAm";
                        case UpsServiceType.UpsExpressSaver:
                            return "UPSExpressSaver";
                        case UpsServiceType.UpsExpedited:
                            return "UPSExpedited";
                        case UpsServiceType.Ups3DaySelectFromCanada:
                            return "UPS3DaySelectFromCanada";
                        case UpsServiceType.UpsCaWorldWideExpressSaver:
                            return "UPSCaWorldWideExpressSaver";
                        case UpsServiceType.UpsCaWorldWideExpressPlus:
                            return "UPSCaWorldWideExpressPlus";
                        case UpsServiceType.UpsCaWorldWideExpress:
                            return "UPSCaWorldWideExpress";
                        case UpsServiceType.Ups2nDayAirIntra:
                            return "UPS2nDayAirIntra";
                    }

                    break;
                }

                case ShipmentTypeCode.Usps:
                case ShipmentTypeCode.Endicia:
                case ShipmentTypeCode.Express1Endicia:
                case ShipmentTypeCode.Express1Usps:
                case ShipmentTypeCode.PostalWebTools:
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

                    if (ShipmentTypeManager.IsEndiciaConsolidator(postalServiceType))
                    {
                        return "CONSOLIDATOR";
                    }

                    if (ShipmentTypeManager.IsDhl(postalServiceType))
                    {
                        return "Global Mail";
                    }

                    break;
                }
                case ShipmentTypeCode.OnTrac:
                    return EnumHelper.GetDescription((OnTracServiceType) shipment.OnTrac.Service);
            }

            return "NONE";
        }

        /// <summary>
        /// Gets the CA shipment Carrier code.  The values are user-customizable in the CA admin site.
        /// </summary>
        public static string GetCarrierCode(ShipmentEntity shipment)
        {
            if (!shipment.Processed)
            {
                return "None";
            }

            switch ((ShipmentTypeCode)shipment.ShipmentType)
            {
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
                    else if (ShipmentTypeManager.IsEndiciaConsolidator(service))
                    {
                        return "Consolidator";
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
    }
}
