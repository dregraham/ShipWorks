using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.Postal;
using Interapptive.Shared.Win32;
using ShipWorks.Data.Connection;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor
{
    /// <summary>
    /// Utility functions for working with MarketplaceAdvisor data
    /// </summary>
    public static class MarketplaceAdvisorUtility
    {
        /// <summary>
        /// Controls if orders are marked as processed on MarketplaceAdvisor after being downloaded
        /// </summary>
        public static bool MarkProcessedAfterDownload
        {
            get
            {
                return InterapptiveOnly.Registry.GetValue("MarketplaceAdvisorProcess", true);
            }
            set
            {
                InterapptiveOnly.Registry.SetValue("MarketplaceAdvisorProcess", value);
            }
        }

        /// <summary>
        /// Return the payment method name of the given code.
        /// </summary>
        public static string GetMethodName(string code)
        {
            switch (code)
            {
                case "51103": return "American Express";
                case "51108": return "BidPay";
                case "51107": return "Billpoint";
                case "51114": return "Cash";
                case "51100": return "Certified Check";
                case "51104": return "Discover";
                case "51111": return "ExchangePath";
                case "51101": return "Money Order";
                case "51110": return "MoneyZap";
                case "51112": return "Other";
                case "51105": return "Other Credit Card";
                case "51109": return "Other Online Service";
                case "51106": return "PayPal";
                case "51113": return "Personal Check";
                case "51102": return "Visa/Mastercard";
                default:
                    return "Unknown";
            }
        }

        /// <summary>
        /// Indicates if the method code represents a CC payment
        /// </summary>
        public static bool IsMethodCC(string code)
        {
            if (code == "51102" ||
                code == "51103" ||
                code == "51104" ||
                code == "51105")
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Indicates if the method code represents a paypal payment
        /// </summary>
        public static bool IsMethodPayPal(string code)
        {
            return (code == "51106");
        }

        /// <summary>
        /// Get the MarketplaceAdvisor method id for the given shipment.
        /// </summary>
        [NDependIgnoreLongMethod]
        [NDependIgnoreComplexMethodAttribute]
        public static int GetShippingMethodID(ShipmentEntity shipment)
        {
            if (!shipment.Processed)
            {
                return -1;
            }

            try
            {
                ShippingManager.EnsureShipmentLoaded(shipment);
            }
            catch (ObjectDeletedException)
            {
                // Shipment was deleted
                return -1;
            }
            catch (SqlForeignKeyException)
            {
                // Shipment was deleted
                return -1;
            }

            switch ((ShipmentTypeCode) shipment.ShipmentType)
            {
                // UPS
                case ShipmentTypeCode.UpsOnLineTools:
                case ShipmentTypeCode.UpsWorldShip:
                    {
                        switch ((UpsServiceType) shipment.Ups.Service)
                        {
                            case UpsServiceType.UpsGround: return 65515;
                            case UpsServiceType.Ups3DaySelect: return 65521;
                            case UpsServiceType.Ups2DayAir: return 65519;
                            case UpsServiceType.Ups2DayAirAM: return 65520;
                            case UpsServiceType.UpsNextDayAir: return 65516;
                            case UpsServiceType.UpsNextDayAirSaver: return 65517;
                            case UpsServiceType.UpsNextDayAirAM: return 65518;

                            case UpsServiceType.WorldwideExpress: return 65524;
                            case UpsServiceType.WorldwideExpedited: return 65525;
                            case UpsServiceType.WorldwideExpressPlus: return 65526;
                            case UpsServiceType.UpsStandard: return 65522;
                            case UpsServiceType.WorldwideSaver: return 65523;
                        }

                        break;
                    }

                // FedEx
                case ShipmentTypeCode.FedEx:
                    {
                        switch ((FedExServiceType) shipment.FedEx.Service)
                        {
                            case FedExServiceType.PriorityOvernight:
                            case FedExServiceType.OneRatePriorityOvernight: 
                                return 65545;

                            case FedExServiceType.StandardOvernight:
                            case FedExServiceType.OneRateStandardOvernight: 
                                return 65546;

                            case FedExServiceType.FirstOvernight:
                            case FedExServiceType.OneRateFirstOvernight: 
                                return 65529;

                            case FedExServiceType.FedEx2Day:
                            case FedExServiceType.OneRate2Day: 
                                return 65532;

                            case FedExServiceType.FedExExpressSaver:
                            case FedExServiceType.OneRateExpressSaver: 
                                return 65547;

                            case FedExServiceType.InternationalPriority: return 65544;
                            case FedExServiceType.InternationalEconomy: return 65543;

                            case FedExServiceType.InternationalFirst: 
                            case FedExServiceType.FedExGround: 
                            case FedExServiceType.GroundHomeDelivery: 
                            case FedExServiceType.FedExInternationalGround:
                                return 65527;
                        }

                        break;
                    }

                // Postal
                case ShipmentTypeCode.PostalWebTools:
                case ShipmentTypeCode.Endicia:
                case ShipmentTypeCode.Express1Endicia:
                case ShipmentTypeCode.Express1Usps:
                case ShipmentTypeCode.Usps:
                    {
                        switch ((PostalServiceType) shipment.Postal.Service)
                        {
                            case PostalServiceType.PriorityMail: return 65502;
                            case PostalServiceType.FirstClass: return 65500;
                            case PostalServiceType.StandardPost: return 65501;
                            case PostalServiceType.MediaMail: return 65503;
                            case PostalServiceType.ExpressMail: return 65504;
                            case PostalServiceType.InternationalExpress: return 65507;
                            case PostalServiceType.InternationalPriority: return 65510;

                            // Going to treat Critical as Priority - not sure where to get updated code
                            case PostalServiceType.CriticalMail: return 65502;
                        }

                        break;
                    }
            }

            return -1;
        }
    }
}
