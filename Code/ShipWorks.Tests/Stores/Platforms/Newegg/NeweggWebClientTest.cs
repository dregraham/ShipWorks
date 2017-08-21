﻿using System;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Stores.Platforms.Newegg;
using ShipWorks.Stores.Platforms.Newegg.Net;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Tests.Stores.Newegg
{
    /// <summary>
    /// Summary description for NeweggWebClientTest
    /// </summary>
    public class NeweggWebClientTest
    {
        private IRequestFactory requestFactory;
        private NeweggStoreEntity store;
        private NeweggWebClient testObject;
        private NeweggOrderItemEntity itemEntity;
        private NeweggOrderEntity orderEntity;
        private FedExShipmentEntity fedExEntity;
        private UpsShipmentEntity upsEntity;
        private ShipmentEntity shipmentEntity;
        private PostalShipmentEntity postalShipmentEntity;
        private EndiciaShipmentEntity endiciaShipmentEntity;
        private UspsShipmentEntity uspsShipmentEntity;
        private OtherShipmentEntity otherShipmentEntity;
        private readonly AutoMock mock;

        public NeweggWebClientTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            this.requestFactory = new ShipWorks.Tests.Stores.Newegg.Mocked.Success.MockRequestFactory();
            this.store = new NeweggStoreEntity();

            orderEntity = new NeweggOrderEntity { OrderNumber = 123456 };
            itemEntity = new NeweggOrderItemEntity { Order = orderEntity, SellerPartNumber = "9876ZYXW" };
            fedExEntity = new FedExShipmentEntity { Service = (int) FedExServiceType.FedExGround };
            upsEntity = new UpsShipmentEntity { Service = (int) UpsServiceType.UpsGround, UspsTrackingNumber = "mi tracking #" };
            shipmentEntity = new ShipmentEntity { Order = orderEntity, TrackingNumber = "ABCD1234", ShipDate = DateTime.UtcNow, ShipmentType = (int) ShipmentTypeCode.FedEx, FedEx = fedExEntity };
            uspsShipmentEntity = new UspsShipmentEntity();
            postalShipmentEntity = new PostalShipmentEntity { Service = (int) PostalServiceType.FirstClass };
            otherShipmentEntity = new OtherShipmentEntity { Carrier = "Some other carrier", Service = "Fast Ground" };
            endiciaShipmentEntity = new EndiciaShipmentEntity();

            testObject = mock.Create<NeweggWebClient>(TypedParameter.From(requestFactory));
        }

        [Fact]
        public async Task AreCredentialsValid_ReturnsTrue_WhenAValidResponseIsReceived()
        {
            // The initialization method has configured our test object (NeweggWebClient)
            // to always return a valid response (via our mocked request factory) regardless
            // of the store entity's configuration
            Assert.True(await testObject.AreCredentialsValid(store));
        }

        [Fact]
        public async Task AreCredentialsValid_ReturnsFalse_WhenAnInvalidResponseIsReceived()
        {
            // We're going to override the default configuration that was setup in the
            // initialize method since we're testing for failures
            this.requestFactory = new ShipWorks.Tests.Stores.Newegg.Mocked.Failure.MockRequestFactory();
            testObject = mock.Create<NeweggWebClient>(TypedParameter.From(requestFactory));

            Assert.False(await testObject.AreCredentialsValid(store));
        }

        [Fact]
        public void GetCarrierCode_ReturnsDhl_WhenEndiciaAndDhlServiceUsed()
        {
            postalShipmentEntity.Endicia = endiciaShipmentEntity;
            postalShipmentEntity.Service = (int) PostalServiceType.DhlParcelGround;

            string carrierCode = RunCarrierCodeTest(ShipmentTypeCode.Endicia);

            Assert.Equal("DHL", carrierCode);
        }

        [Fact]
        public void GetCarrierCode_ReturnsDhl_WhenUspsAndDhlServiceUsed()
        {
            postalShipmentEntity.Usps = uspsShipmentEntity;
            postalShipmentEntity.Service = (int) PostalServiceType.DhlParcelGround;

            string carrierCode = RunCarrierCodeTest(ShipmentTypeCode.Usps);

            Assert.Equal("DHL", carrierCode);
        }

        [Fact]
        public void GetCarrierCode_ReturnsUsps_WhenEndiciaAndFirstClass()
        {
            postalShipmentEntity.Endicia = endiciaShipmentEntity;
            postalShipmentEntity.Service = (int) PostalServiceType.FirstClass;

            string carrierCode = RunCarrierCodeTest(ShipmentTypeCode.Endicia);

            Assert.Equal("USPS", carrierCode);
        }

        [Fact]
        public void GetCarrierCode_ReturnsConsolidator_WhenEndiciaAndConsolidator()
        {
            postalShipmentEntity.Endicia = endiciaShipmentEntity;
            postalShipmentEntity.Service = (int) PostalServiceType.ConsolidatorDomestic;

            string carrierCode = RunCarrierCodeTest(ShipmentTypeCode.Endicia);

            Assert.Equal("Consolidator", carrierCode);
        }

        [Fact]
        public void GetCarrierCode_ReturnsUsps_WhenUspsAndFirstClass()
        {
            postalShipmentEntity.Usps = uspsShipmentEntity;
            postalShipmentEntity.Service = (int) PostalServiceType.FirstClass;

            string carrierCode = RunCarrierCodeTest(ShipmentTypeCode.Usps);

            Assert.Equal("USPS", carrierCode);
        }

        [Fact]
        public void GetCarrierCode_ReturnsSpecifiedCarrier_WhenOtherShipmentType()
        {
            string carrierCode = RunCarrierCodeTest(ShipmentTypeCode.Other);

            Assert.Equal(otherShipmentEntity.Carrier, carrierCode);
        }

        [Fact]
        public void GetCarrierCode_ReturnsUps_WhenUpsAndGround()
        {
            shipmentEntity.Ups = upsEntity;

            string carrierCode = RunCarrierCodeTest(ShipmentTypeCode.UpsOnLineTools);

            Assert.Equal("UPS", carrierCode);

            carrierCode = RunCarrierCodeTest(ShipmentTypeCode.UpsWorldShip);

            Assert.Equal("UPS", carrierCode);
        }

        [Fact]
        public void GetCarrierCode_ReturnsUpsMi_WhenUpsAndUpsMiService()
        {
            upsEntity.Service = (int) UpsServiceType.UpsMailInnovationsFirstClass;
            shipmentEntity.Ups = upsEntity;

            string carrierCode = RunCarrierCodeTest(ShipmentTypeCode.UpsOnLineTools);

            Assert.Equal("UPS MI", carrierCode);

            carrierCode = RunCarrierCodeTest(ShipmentTypeCode.UpsWorldShip);

            Assert.Equal("UPS MI", carrierCode);
        }

        private string RunCarrierCodeTest(ShipmentTypeCode shipmentTypeCode)
        {
            testObject = mock.Create<NeweggWebClient>(TypedParameter.From(requestFactory));

            shipmentEntity = new ShipmentEntity
            {
                Order = orderEntity,
                TrackingNumber = "ABCD1234",
                ShipDate = DateTime.UtcNow,
                ShipmentType = (int) shipmentTypeCode,
                Postal = postalShipmentEntity,
                Other = otherShipmentEntity,
                FedEx = fedExEntity,
                Ups = upsEntity
            };

            return NeweggWebClient.GetCarrierCode(shipmentEntity);
        }
    }
}
