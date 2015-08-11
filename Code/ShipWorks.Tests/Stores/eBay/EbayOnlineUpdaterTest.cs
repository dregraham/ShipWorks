using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Utility;
using Xunit;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;
using ShipWorks.Stores.Platforms.Ebay;
using ShipWorks.Stores.Platforms.Ebay.WebServices;

namespace ShipWorks.Tests.Stores.eBay
{
    /// <summary>
    /// Summary description for EbayOnlineUpdater
    /// </summary>
    public class EbayOnlineUpdaterTest
    {
        private EbayOrderItemEntity itemEntity;
        private EbayOrderEntity orderEntity;
        private FedExShipmentEntity fedExShipmentEntity;
        private UpsShipmentEntity upsShipmentEntity;
        private ShipmentEntity shipmentEntity;
        private PostalShipmentEntity postalShipmentEntity;
        private EndiciaShipmentEntity endiciaShipmentEntity;
        private UspsShipmentEntity uspsShipmentEntity;
        private OtherShipmentEntity otherShipmentEntity;

        [TestInitialize]
        public void Initialize()
        {
            EbayOrderItemEntity.SetEffectiveCheckoutStatusAlgorithm(e => 0);
            EbayOrderItemEntity.SetEffectivePaymentMethodAlgorithm(e => 0);

            orderEntity = new EbayOrderEntity { OrderNumber = 123456, IsManual = false, };
            itemEntity = new EbayOrderItemEntity { Order = orderEntity, IsManual = false, EbayItemID = 3};
            fedExShipmentEntity = new FedExShipmentEntity { Service = (int)FedExServiceType.FedExGround };
            upsShipmentEntity = new UpsShipmentEntity { Service = (int)UpsServiceType.UpsGround, UspsTrackingNumber = "mi tracking #" };
            shipmentEntity = new ShipmentEntity { Order = orderEntity, TrackingNumber = "ABCD1234", ShipDate = DateTime.UtcNow, ShipmentType = (int)ShipmentTypeCode.FedEx, FedEx = fedExShipmentEntity };
            uspsShipmentEntity = new UspsShipmentEntity();
            postalShipmentEntity = new PostalShipmentEntity { Service = (int)PostalServiceType.FirstClass };
            otherShipmentEntity = new OtherShipmentEntity { Carrier = "Some other carrier", Service = "Fast Ground" };
            endiciaShipmentEntity = new EndiciaShipmentEntity();

            orderEntity.OrderItems.AddRange(new List<OrderItemEntity>() {itemEntity});
        }

        [Fact]
        public void GetCarrierCode_ReturnsFedEx_Test()
        {
            shipmentEntity.ShipmentType = (int) ShipmentTypeCode.FedEx;
            shipmentEntity.FedEx = fedExShipmentEntity;

            CheckCarrierCodeAndTrackingNumber();
        }

        [Fact]
        public void GetCarrierCode_ReturnsFedEx_WhenOtherCarrierIsFedEx_Test()
        {
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Other;
            otherShipmentEntity.Carrier = "FedEx";
            shipmentEntity.Other = otherShipmentEntity;

            CheckCarrierCodeAndTrackingNumber();
        }

        [Fact]
        public void GetCarrierCode_ReturnsUps_WhenOtherCarrierIsUps_Test()
        {
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Other;
            otherShipmentEntity.Carrier = "Ups";
            shipmentEntity.Other = otherShipmentEntity;

            CheckCarrierCodeAndTrackingNumber();
        }

        [Fact]
        public void GetCarrierCode_ReturnsUsps_WhenOtherCarrierIsUsps_Test()
        {
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Other;
            otherShipmentEntity.Carrier = "Usps";
            shipmentEntity.Other = otherShipmentEntity;

            CheckCarrierCodeAndTrackingNumber();
        }

        [Fact]
        public void GetCarrierCode_ReturnsDhl_WhenOtherCarrierIsDhl_Test()
        {
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Other;
            otherShipmentEntity.Carrier = "Dhl";
            shipmentEntity.Other = otherShipmentEntity;

            CheckCarrierCodeAndTrackingNumber();
        }

        [Fact]
        public void GetCarrierCode_ReturnsOther_WhenOtherCarrierIsOther_Test()
        {
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Other;
            otherShipmentEntity.Carrier = "Other";
            shipmentEntity.Other = otherShipmentEntity;

            CheckCarrierCodeAndTrackingNumber();
        }

        [Fact]
        public void GetCarrierCode_ReturnsUps_WhenUpsAndServiceIsGround_Test()
        {
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.UpsOnLineTools;
            upsShipmentEntity.Service = (int) UpsServiceType.UpsGround;
            shipmentEntity.Ups = upsShipmentEntity;

            CheckCarrierCodeAndTrackingNumber();

            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.UpsWorldShip;

            CheckCarrierCodeAndTrackingNumber();
        }

        [Fact]
        public void GetCarrierCode_ReturnsOther_WhenUpsAndMiServiceAndNoUspsTrackingNumber_Test()
        {
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.UpsOnLineTools;
            upsShipmentEntity.Service = (int)UpsServiceType.UpsMailInnovationsExpedited;
            upsShipmentEntity.UspsTrackingNumber = "";
            shipmentEntity.Ups = upsShipmentEntity;

            CheckCarrierCodeAndTrackingNumber();

            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.UpsWorldShip;

            CheckCarrierCodeAndTrackingNumber();
        }

        [Fact]
        public void GetCarrierCode_ReturnsUsps_WhenUspsAndFirstClassService_Test()
        {
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Usps;
            postalShipmentEntity.Service = (int)PostalServiceType.FirstClass;
            shipmentEntity.Postal = postalShipmentEntity;

            CheckCarrierCodeAndTrackingNumber();
        }

        [Fact]
        public void GetCarrierCode_ReturnsDhl_WhenUspsAndDhlService_Test()
        {
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Usps;
            postalShipmentEntity.Service = (int)PostalServiceType.DhlParcelExpedited;
            shipmentEntity.Postal = postalShipmentEntity;

            CheckCarrierCodeAndTrackingNumber();
        }

        [Fact]
        public void GetCarrierCode_ReturnsOther_WhenUspsAndConsolidatorService_Test()
        {
            shipmentEntity.ShipmentType = (int)ShipmentTypeCode.Usps;
            postalShipmentEntity.Service = (int)PostalServiceType.ConsolidatorDomestic;
            shipmentEntity.Postal = postalShipmentEntity;

            CheckCarrierCodeAndTrackingNumber();
        }




        /// <summary>
        /// Common method used to compare the old ebay online updater with new version.
        /// </summary>
        private void CheckCarrierCodeAndTrackingNumber()
        {
            string oldTrackingNumber = string.Empty;
            string oldCarrierCode = OldEbayOnlineUpdater.UpdateOnlineStatus(orderEntity, shipmentEntity, false, true, out oldTrackingNumber);

            string newCarrierCode = string.Empty;
            string trackingNumber = string.Empty;

            EbayOnlineUpdater.GetShippingCarrierAndTracking(shipmentEntity, out newCarrierCode, out trackingNumber);

            Assert.AreEqual(oldCarrierCode, newCarrierCode);
            Assert.AreEqual(oldTrackingNumber, trackingNumber);
        }
    }


    /// <summary>
    /// This is how the old version of the EbayOnlineUpdater worked, sans the db access and non-carrier/tracking number code.
    /// 
    /// The tests will use this as the expected output and compare it to the new EbayOnlineUpdater method results.
    /// 
    /// At some point, this old updater may no longer be relevant.  At that point we'll want to switch to just using hard coded expected values.
    /// But for now, since there may be additional tests that need to be written against this old version, I'll leave it like it is for now.
    /// </summary>
    public class OldEbayOnlineUpdater
    {
        /// <summary>
        /// Push the online status for an order.
        /// </summary>
        public static string UpdateOnlineStatus(EbayOrderEntity order, ShipmentEntity shipment, bool? paid, bool? shipped, out string trackingNumber)
        {
            string shippingCarrierUsed = string.Empty;
            string tmpTrackingNumber = string.Empty;
            trackingNumber = string.Empty;

            // IsManual should actually never be true - instead order will be null, b\c manual orders (at this time) don't have a derived table presence.
            if (order == null || order.IsManual)
            {
                return string.Empty;
            }

            bool useUpsMailInnovationsCarrierType = false;

            // update each item
            foreach (EbayOrderItemEntity ebayItem in order.OrderItems)
            {
                // This was a manually added item, we can't update ebay with it so continue on to the next one
                if (ebayItem.IsManual || ebayItem.EbayItemID == 0)
                {
                    //log.WarnFormat("Not updating online status for order item {0} becaue it is manual or was deleted.", ebayItem.EbayItemID);
                    continue;
                }

                ShippingCarrierCodeType carrierType = ShippingCarrierCodeType.CustomCode;

                if (shipped.HasValue && shipped.Value)
                {
                    if (shipment != null)
                    {
                        carrierType = GetShippingCarrier(shipment);

                        // tracking number to upload
                        tmpTrackingNumber = shipment.TrackingNumber;

                        FakeDetermineAlternateTracking(shipment, (track, service) =>
                        {
                            // Try to use the alternate tracking number for MI if it's set
                            if (!string.IsNullOrEmpty(track))
                            {
                                tmpTrackingNumber = track;
                            }

                            // International MI seems to use the normal UPS tracking number, so we'll just use that
                            if (!string.IsNullOrEmpty(tmpTrackingNumber))
                            {
                                // From eBay web service info:
                                // For those using UPS Mail Innovations, supply the value UPS-MI for UPS Mail Innnovations. 
                                // Buyers will subsequently be sent to the UPS Mail Innovations website for tracking.
                                useUpsMailInnovationsCarrierType = true;
                            }
                            else
                            {
                                // Mail Innovations but without a USPS tracking number will just get uploaded
                                // as an Other shipment.  Tracking will be whatever the user entered in the Reference 1 field
                                // in the SW WorldShip window.
                                carrierType = ShippingCarrierCodeType.Other;
                            }
                        });

                        // can only upload tracking details if it's a supported carrier
                        if (carrierType == ShippingCarrierCodeType.CustomCode)
                        {
                            // We can't upload tracking details because this isn't a supported carrier, so create a
                            // note with the tracking number instead
                            string notesText = string.Format("Shipped {0} on {1}. Tracking number: {2}",
                                ShippingManager.GetServiceUsed(shipment), shipment.ShipDate.ToShortDateString(), shipment.TrackingNumber);

                            //webClient.AddUserNote(ebayItem.EbayItemID, ebayItem.EbayTransactionID, notesText);
                        }
                    }
                }

                shippingCarrierUsed = useUpsMailInnovationsCarrierType ? "UPS-MI" : carrierType.ToString();

                // If we are DHL, we need to switch to DHL Global Mail as the carrier (DHL goes to DHL.de for some reason)
                if (carrierType == ShippingCarrierCodeType.DHL)
                {
                    shippingCarrierUsed = "DHL Global Mail";
                }
            }

            trackingNumber = tmpTrackingNumber;
            return shippingCarrierUsed;
        }

        /// <summary>
        /// Fake implementation of the WorldShipUtility.DetermineAlternateTracking 
        /// </summary>
        private static void FakeDetermineAlternateTracking(ShipmentEntity shipment, AlternateTrackingLoaded alternativeTracking)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            if (alternativeTracking == null)
            {
                throw new ArgumentNullException("alternativeTracking");
            }

            ShipmentTypeCode shipmentTypeCode = (ShipmentTypeCode)shipment.ShipmentType;
            if (shipmentTypeCode == ShipmentTypeCode.UpsWorldShip || shipmentTypeCode == ShipmentTypeCode.UpsOnLineTools)
            {
                //// we need to inspect the UPS-specific values, get them loaded
                //ShippingManager.EnsureShipmentLoaded(shipment);

                if (UpsUtility.IsUpsMiService((UpsServiceType)shipment.Ups.Service))
                {
                    alternativeTracking(shipment.Ups.UspsTrackingNumber, (UpsServiceType)shipment.Ups.Service);
                }
            }
        }

        /// <summary>
        /// A helper method to obtain the shipping carrier from the shipment
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>A ShippingCarrierCodeType object.</returns>
        private static ShippingCarrierCodeType GetShippingCarrier(ShipmentEntity shipment)
        {
            // default the type to Other
            ShippingCarrierCodeType carrierType = ShippingCarrierCodeType.CustomCode;

            // Is it a USPS shipment?
            switch ((ShipmentTypeCode)shipment.ShipmentType)
            {
                case ShipmentTypeCode.PostalWebTools:
                case ShipmentTypeCode.Express1Endicia:
                case ShipmentTypeCode.Express1Usps:
                case ShipmentTypeCode.Usps:
                case ShipmentTypeCode.Endicia:
                    PostalServiceType service = (PostalServiceType)shipment.Postal.Service;

                    // The shipment is an Endicia/Usps shipment, check to see if it's DHL
                    if (ShipmentTypeManager.IsDhl(service))
                    {
                        // The DHL carrier for Endicia/Usps is:
                        carrierType = ShippingCarrierCodeType.DHL;
                    }
                    else if (ShipmentTypeManager.IsConsolidator(service))
                    {
                        carrierType = ShippingCarrierCodeType.Other;
                    }
                    else
                    {
                        // Use the default carrier for other Endicia/Usps types
                        carrierType = ShippingCarrierCodeType.USPS;
                    }
                    break;

                case ShipmentTypeCode.UpsOnLineTools:
                case ShipmentTypeCode.UpsWorldShip:
                    carrierType = ShippingCarrierCodeType.UPS;
                    break;

                case ShipmentTypeCode.FedEx:
                    carrierType = ShippingCarrierCodeType.FedEx;
                    break;

                case ShipmentTypeCode.Other:
                    CarrierDescription description = ShippingManager.GetOtherCarrierDescription(shipment);

                    return description.IsUPS ? ShippingCarrierCodeType.UPS :
                        description.IsFedEx ? ShippingCarrierCodeType.FedEx :
                        description.IsUSPS ? ShippingCarrierCodeType.USPS :
                        description.IsDHL ? ShippingCarrierCodeType.DHL :
                        ShippingCarrierCodeType.Other;
            }

            return carrierType;
        }
    }

}
