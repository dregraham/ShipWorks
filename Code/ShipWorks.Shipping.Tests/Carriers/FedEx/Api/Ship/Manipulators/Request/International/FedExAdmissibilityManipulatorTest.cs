using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators.International;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using System;
using System.Collections.Generic;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators.International
{
    public class FedExAdmissibilityManipulatorTest
    {
        private FedExAdmissibilityManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private ProcessShipmentRequest nativeRequest;
        private ShipmentEntity shipmentEntity;

        public FedExAdmissibilityManipulatorTest()
        {
            shipmentEntity = new ShipmentEntity
            {
                FedEx = new FedExShipmentEntity
                {
                    CustomsAdmissibilityPackaging = (int)FedExPhysicalPackagingType.Bag
                }
            };

            nativeRequest = new ProcessShipmentRequest()
            {
                RequestedShipment = new RequestedShipment()
                {
                    RequestedPackageLineItems = new RequestedPackageLineItem[]
                    {
                        new RequestedPackageLineItem()
                    }
                }
            };

            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            testObject = new FedExAdmissibilityManipulator();
        }

        [Fact]
        public void Manipulate_ThrowsArgumentNullException_WhenCarrierRequestIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.Manipulate(null));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNull()
        {
            // Setup the native request to be null
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, null);

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotProcessShipmentRequest()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, new object());

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_AccountsForNullRequestedShipment()
        {
            // setup the test by setting the requested shipment to null
            nativeRequest.RequestedShipment = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            Assert.NotNull(nativeRequest.RequestedShipment);
        }

        [Fact]
        public void Manipulate_AccountsForNullPackageLineItemArray()
        {
            // setup the test by setting the line item array to null
            nativeRequest.RequestedShipment.RequestedPackageLineItems = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            Assert.NotNull(nativeRequest.RequestedShipment.RequestedPackageLineItems);
        }

        [Fact]
        public void Manipulate_AccountsForEmptyPackageLineItemArray()
        {
            // setup the test by setting the line item array to null
            nativeRequest.RequestedShipment.RequestedPackageLineItems = new RequestedPackageLineItem[0];
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            Assert.NotNull(nativeRequest.RequestedShipment.RequestedPackageLineItems);
        }

        [Fact]
        public void Manipulate_AccountsForNullElementInPackageLineItemArray()
        {
            // setup the test by setting the line item array to null
            nativeRequest.RequestedShipment.RequestedPackageLineItems = new RequestedPackageLineItem[] { null };
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            Assert.NotNull(nativeRequest.RequestedShipment.RequestedPackageLineItems[0]);
        }

        [Fact]
        public void Manipulate_PhysicalPackagingTypeSpecifiedIsFalse_WhenShipCountryCodeIsUS()
        {
            shipmentEntity.ShipCountryCode = "US";

            testObject.Manipulate(carrierRequest.Object);

            Assert.False(nativeRequest.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackagingSpecified);
        }

        [Fact]
        public void Manipulate_PhysicalPackagingTypeSpecifiedIsTrue_WhenShipCountryCodeIsCA()
        {
            shipmentEntity.ShipCountryCode = "CA";

            testObject.Manipulate(carrierRequest.Object);

            Assert.True(nativeRequest.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackagingSpecified);
        }

        [Fact]
        public void Manipulate_PhysicalPackagingTypeIsBag_WhenShipCountryCodeIsCA_AndFedExTypeIsBag()
        {
            shipmentEntity.ShipCountryCode = "CA";
            shipmentEntity.FedEx.CustomsAdmissibilityPackaging = (int)FedExPhysicalPackagingType.Bag;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(PhysicalPackagingType.BAG, nativeRequest.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackaging);
        }

        [Fact]
        public void Manipulate_PhysicalPackagingTypeIsBarrel_WhenShipCountryCodeIsCA_AndFedExTypeIsBarrel()
        {
            shipmentEntity.ShipCountryCode = "CA";
            shipmentEntity.FedEx.CustomsAdmissibilityPackaging = (int)FedExPhysicalPackagingType.Barrel;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(PhysicalPackagingType.BARREL, nativeRequest.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackaging);
        }

        [Fact]
        public void Manipulate_PhysicalPackagingTypeIsBasket_WhenShipCountryCodeIsCA_AndFedExTypeIsBasketOrHamper()
        {
            shipmentEntity.ShipCountryCode = "CA";
            shipmentEntity.FedEx.CustomsAdmissibilityPackaging = (int)FedExPhysicalPackagingType.BasketOrHamper;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(PhysicalPackagingType.BASKET, nativeRequest.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackaging);
        }

        [Fact]
        public void Manipulate_PhysicalPackagingTypeIsBox_WhenShipCountryCodeIsCA_AndFedExTypeIsBox()
        {
            shipmentEntity.ShipCountryCode = "CA";
            shipmentEntity.FedEx.CustomsAdmissibilityPackaging = (int)FedExPhysicalPackagingType.Box;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(PhysicalPackagingType.BOX, nativeRequest.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackaging);
        }

        [Fact]
        public void Manipulate_PhysicalPackagingTypeIsBucket_WhenShipCountryCodeIsCA_AndFedExTypeIsBucket()
        {
            shipmentEntity.ShipCountryCode = "CA";
            shipmentEntity.FedEx.CustomsAdmissibilityPackaging = (int)FedExPhysicalPackagingType.Bucket;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(PhysicalPackagingType.BUCKET, nativeRequest.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackaging);
        }

        [Fact]
        public void Manipulate_PhysicalPackagingTypeIsBundle_WhenShipCountryCodeIsCA_AndFedExTypeIsBundle()
        {
            shipmentEntity.ShipCountryCode = "CA";
            shipmentEntity.FedEx.CustomsAdmissibilityPackaging = (int)FedExPhysicalPackagingType.Bundle;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(PhysicalPackagingType.BUNDLE, nativeRequest.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackaging);
        }

        [Fact]
        public void Manipulate_PhysicalPackagingTypeIsCarton_WhenShipCountryCodeIsCA_AndFedExTypeIsCarton()
        {
            shipmentEntity.ShipCountryCode = "CA";
            shipmentEntity.FedEx.CustomsAdmissibilityPackaging = (int)FedExPhysicalPackagingType.Carton;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(PhysicalPackagingType.CARTON, nativeRequest.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackaging);
        }

        [Fact]
        public void Manipulate_PhysicalPackagingTypeIsCase_WhenShipCountryCodeIsCA_AndFedExTypeIsCase()
        {
            shipmentEntity.ShipCountryCode = "CA";
            shipmentEntity.FedEx.CustomsAdmissibilityPackaging = (int)FedExPhysicalPackagingType.Case;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(PhysicalPackagingType.CASE, nativeRequest.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackaging);
        }

        [Fact]
        public void Manipulate_PhysicalPackagingTypeIsContainer_WhenShipCountryCodeIsCA_AndFedExTypeIsContainer()
        {
            shipmentEntity.ShipCountryCode = "CA";
            shipmentEntity.FedEx.CustomsAdmissibilityPackaging = (int)FedExPhysicalPackagingType.Container;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(PhysicalPackagingType.CONTAINER, nativeRequest.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackaging);
        }

        [Fact]
        public void Manipulate_PhysicalPackagingTypeIsCrate_WhenShipCountryCodeIsCA_AndFedExTypeIsCrate()
        {
            shipmentEntity.ShipCountryCode = "CA";
            shipmentEntity.FedEx.CustomsAdmissibilityPackaging = (int)FedExPhysicalPackagingType.Crate;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(PhysicalPackagingType.CRATE, nativeRequest.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackaging);
        }

        [Fact]
        public void Manipulate_PhysicalPackagingTypeIsCylinder_WhenShipCountryCodeIsCA_AndFedExTypeIsCylinder()
        {
            shipmentEntity.ShipCountryCode = "CA";
            shipmentEntity.FedEx.CustomsAdmissibilityPackaging = (int)FedExPhysicalPackagingType.Cylinder;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(PhysicalPackagingType.CYLINDER, nativeRequest.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackaging);
        }

        [Fact]
        public void Manipulate_PhysicalPackagingTypeIsDrum_WhenShipCountryCodeIsCA_AndFedExTypeIsDrum()
        {
            shipmentEntity.ShipCountryCode = "CA";
            shipmentEntity.FedEx.CustomsAdmissibilityPackaging = (int)FedExPhysicalPackagingType.Drum;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(PhysicalPackagingType.DRUM, nativeRequest.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackaging);
        }

        [Fact]
        public void Manipulate_PhysicalPackagingTypeIsEnvelope_WhenShipCountryCodeIsCA_AndFedExTypeIsEnvelope()
        {
            shipmentEntity.ShipCountryCode = "CA";
            shipmentEntity.FedEx.CustomsAdmissibilityPackaging = (int)FedExPhysicalPackagingType.Envelope;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(PhysicalPackagingType.ENVELOPE, nativeRequest.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackaging);
        }

        [Fact]
        public void Manipulate_PhysicalPackagingTypeIsPail_WhenShipCountryCodeIsCA_AndFedExTypeIsPail()
        {
            shipmentEntity.ShipCountryCode = "CA";
            shipmentEntity.FedEx.CustomsAdmissibilityPackaging = (int)FedExPhysicalPackagingType.Pail;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(PhysicalPackagingType.PAIL, nativeRequest.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackaging);
        }

        [Fact]
        public void Manipulate_PhysicalPackagingTypeIsPallet_WhenShipCountryCodeIsCA_AndFedExTypeIsPallet()
        {
            shipmentEntity.ShipCountryCode = "CA";
            shipmentEntity.FedEx.CustomsAdmissibilityPackaging = (int)FedExPhysicalPackagingType.Pallet;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(PhysicalPackagingType.PALLET, nativeRequest.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackaging);
        }

        [Fact]
        public void Manipulate_PhysicalPackagingTypeIsPieces_WhenShipCountryCodeIsCA_AndFedExTypeIsPieces()
        {
            shipmentEntity.ShipCountryCode = "CA";
            shipmentEntity.FedEx.CustomsAdmissibilityPackaging = (int)FedExPhysicalPackagingType.Pieces;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(PhysicalPackagingType.PIECE, nativeRequest.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackaging);
        }

        [Fact]
        public void Manipulate_PhysicalPackagingTypeIsReel_WhenShipCountryCodeIsCA_AndFedExTypeIsReel()
        {
            shipmentEntity.ShipCountryCode = "CA";
            shipmentEntity.FedEx.CustomsAdmissibilityPackaging = (int)FedExPhysicalPackagingType.Reel;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(PhysicalPackagingType.REEL, nativeRequest.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackaging);
        }

        [Fact]
        public void Manipulate_PhysicalPackagingTypeIsRoll_WhenShipCountryCodeIsCA_AndFedExTypeIsRoll()
        {
            shipmentEntity.ShipCountryCode = "CA";
            shipmentEntity.FedEx.CustomsAdmissibilityPackaging = (int)FedExPhysicalPackagingType.Roll;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(PhysicalPackagingType.ROLL, nativeRequest.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackaging);
        }

        [Fact]
        public void Manipulate_PhysicalPackagingTypeIsSkid_WhenShipCountryCodeIsCA_AndFedExTypeIsSkid()
        {
            shipmentEntity.ShipCountryCode = "CA";
            shipmentEntity.FedEx.CustomsAdmissibilityPackaging = (int)FedExPhysicalPackagingType.Skid;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(PhysicalPackagingType.SKID, nativeRequest.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackaging);
        }

        [Fact]
        public void Manipulate_PhysicalPackagingTypeIsTank_WhenShipCountryCodeIsCA_AndFedExTypeIsTank()
        {
            shipmentEntity.ShipCountryCode = "CA";
            shipmentEntity.FedEx.CustomsAdmissibilityPackaging = (int)FedExPhysicalPackagingType.Tank;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(PhysicalPackagingType.TANK, nativeRequest.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackaging);
        }

        [Fact]
        public void Manipulate_PhysicalPackagingTypeIsTube_WhenShipCountryCodeIsCA_AndFedExTypeIsTube()
        {
            shipmentEntity.ShipCountryCode = "CA";
            shipmentEntity.FedEx.CustomsAdmissibilityPackaging = (int)FedExPhysicalPackagingType.Tube;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(PhysicalPackagingType.TUBE, nativeRequest.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackaging);
        }

        [Fact]
        public void Manipulate_PhysicalPackagingTypeIsHamper_WhenShipCountryCodeIsCA_AndFedExTypeIsHamper()
        {
            shipmentEntity.ShipCountryCode = "CA";
            shipmentEntity.FedEx.CustomsAdmissibilityPackaging = (int)FedExPhysicalPackagingType.Hamper;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(PhysicalPackagingType.HAMPER, nativeRequest.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackaging);
        }

        [Fact]
        public void Manipulate_PhysicalPackagingTypeIsOther_WhenShipCountryCodeIsCA_AndFedExTypeIsOther()
        {
            shipmentEntity.ShipCountryCode = "CA";
            shipmentEntity.FedEx.CustomsAdmissibilityPackaging = (int)FedExPhysicalPackagingType.Other;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(PhysicalPackagingType.OTHER, nativeRequest.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackaging);
        }

        [Fact]
        public void Manipulate_NoPhysicalPackagingTypeThrows_WhenShipCountryCodeIsCA_AndAnyFedExPackageTypeIsUsed()
        {
            shipmentEntity.ShipCountryCode = "CA";
            foreach (var fedExPhysicalPackagingType in EnumHelper.GetEnumList<FedExPhysicalPackagingType>())
            {
                shipmentEntity.FedEx.CustomsAdmissibilityPackaging = (int) fedExPhysicalPackagingType.Value;
                testObject.Manipulate(carrierRequest.Object);
            }
        }

        [Fact]
        public void Manipulate_ThrowsFedExException_WhenShipCountryCodeIsCA_AndFedExTypeIsUnknown()
        {
            shipmentEntity.ShipCountryCode = "CA";
            shipmentEntity.FedEx.CustomsAdmissibilityPackaging = 67;

            Assert.Throws<InvalidOperationException>(() => testObject.Manipulate(carrierRequest.Object));
        }
    }
}
