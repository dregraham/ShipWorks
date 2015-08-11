using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators.International;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators.International
{
    public class FedExAdmissibilityManipulatorTest
    {
        private FedExAdmissibilityManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private ProcessShipmentRequest nativeRequest;
        private ShipmentEntity shipmentEntity;

        [TestInitialize]
        public void Initialize()
        {
            shipmentEntity = new ShipmentEntity
            {
                FedEx = new FedExShipmentEntity
                {
                    CustomsAdmissibilityPackaging = (int) FedExPhysicalPackagingType.Bag
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
        [ExpectedException(typeof(ArgumentNullException))]
        public void Manipulate_ThrowsArgumentNullException_WhenCarrierRequestIsNull_Test()
        {
            testObject.Manipulate(null);
        }

        [Fact]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNull_Test()
        {
            // Setup the native request to be null
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, null);

            testObject.Manipulate(carrierRequest.Object);
        }

        [Fact]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotProcessShipmentRequest_Test()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, new object());

            testObject.Manipulate(carrierRequest.Object);
        }

        [Fact]
        public void Manipulate_AccountsForNullRequestedShipment_Test()
        {
            // setup the test by setting the requested shipment to null
            nativeRequest.RequestedShipment = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsNotNull(nativeRequest.RequestedShipment);
        }

        [Fact]
        public void Manipulate_AccountsForNullPackageLineItemArray_Test()
        {
            // setup the test by setting the line item array to null
            nativeRequest.RequestedShipment.RequestedPackageLineItems = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsNotNull(nativeRequest.RequestedShipment.RequestedPackageLineItems);
        }

        [Fact]
        public void Manipulate_AccountsForEmptyPackageLineItemArray_Test()
        {
            // setup the test by setting the line item array to null
            nativeRequest.RequestedShipment.RequestedPackageLineItems = new RequestedPackageLineItem[0];
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsNotNull(nativeRequest.RequestedShipment.RequestedPackageLineItems);
        }

        [Fact]
        public void Manipulate_AccountsForNullElementInPackageLineItemArray_Test()
        {
            // setup the test by setting the line item array to null
            nativeRequest.RequestedShipment.RequestedPackageLineItems = new RequestedPackageLineItem[] {null};
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsNotNull(nativeRequest.RequestedShipment.RequestedPackageLineItems[0]);
        }

        [Fact]
        public void Manipulate_PhysicalPackagingTypeSpecifiedIsFalse_WhenShipCountryCodeIsUS_Test()
        {
            shipmentEntity.ShipCountryCode = "US";

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsFalse(nativeRequest.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackagingSpecified);
        }

        [Fact]
        public void Manipulate_PhysicalPackagingTypeSpecifiedIsTrue_WhenShipCountryCodeIsCA_Test()
        {
            shipmentEntity.ShipCountryCode = "CA";

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsTrue(nativeRequest.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackagingSpecified);
        }

        [Fact]
        public void Manipulate_PhysicalPackagingTypeIsBag_WhenShipCountryCodeIsCA_AndFedExTypeIsBag_Test()
        {
            shipmentEntity.ShipCountryCode = "CA";
            shipmentEntity.FedEx.CustomsAdmissibilityPackaging = (int) FedExPhysicalPackagingType.Bag;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(PhysicalPackagingType.BAG, nativeRequest.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackaging);
        }

        [Fact]
        public void Manipulate_PhysicalPackagingTypeIsBarrel_WhenShipCountryCodeIsCA_AndFedExTypeIsBarrel_Test()
        {
            shipmentEntity.ShipCountryCode = "CA";
            shipmentEntity.FedEx.CustomsAdmissibilityPackaging = (int)FedExPhysicalPackagingType.Barrel;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(PhysicalPackagingType.BARREL, nativeRequest.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackaging);
        }

        [Fact]
        public void Manipulate_PhysicalPackagingTypeIsBasket_WhenShipCountryCodeIsCA_AndFedExTypeIsBasketOrHamper_Test()
        {
            shipmentEntity.ShipCountryCode = "CA";
            shipmentEntity.FedEx.CustomsAdmissibilityPackaging = (int)FedExPhysicalPackagingType.BasketOrHamper;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(PhysicalPackagingType.BASKET, nativeRequest.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackaging);
        }

        [Fact]
        public void Manipulate_PhysicalPackagingTypeIsBox_WhenShipCountryCodeIsCA_AndFedExTypeIsBox_Test()
        {
            shipmentEntity.ShipCountryCode = "CA";
            shipmentEntity.FedEx.CustomsAdmissibilityPackaging = (int)FedExPhysicalPackagingType.Box;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(PhysicalPackagingType.BOX, nativeRequest.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackaging);
        }

        [Fact]
        public void Manipulate_PhysicalPackagingTypeIsBucket_WhenShipCountryCodeIsCA_AndFedExTypeIsBucket_Test()
        {
            shipmentEntity.ShipCountryCode = "CA";
            shipmentEntity.FedEx.CustomsAdmissibilityPackaging = (int)FedExPhysicalPackagingType.Bucket;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(PhysicalPackagingType.BUCKET, nativeRequest.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackaging);
        }

        [Fact]
        public void Manipulate_PhysicalPackagingTypeIsBundle_WhenShipCountryCodeIsCA_AndFedExTypeIsBundle_Test()
        {
            shipmentEntity.ShipCountryCode = "CA";
            shipmentEntity.FedEx.CustomsAdmissibilityPackaging = (int)FedExPhysicalPackagingType.Bundle;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(PhysicalPackagingType.BUNDLE, nativeRequest.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackaging);
        }

        [Fact]
        public void Manipulate_PhysicalPackagingTypeIsCarton_WhenShipCountryCodeIsCA_AndFedExTypeIsCarton_Test()
        {
            shipmentEntity.ShipCountryCode = "CA";
            shipmentEntity.FedEx.CustomsAdmissibilityPackaging = (int)FedExPhysicalPackagingType.Carton;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(PhysicalPackagingType.CARTON, nativeRequest.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackaging);
        }

        [Fact]
        public void Manipulate_PhysicalPackagingTypeIsCase_WhenShipCountryCodeIsCA_AndFedExTypeIsCase_Test()
        {
            shipmentEntity.ShipCountryCode = "CA";
            shipmentEntity.FedEx.CustomsAdmissibilityPackaging = (int)FedExPhysicalPackagingType.Case;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(PhysicalPackagingType.CASE, nativeRequest.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackaging);
        }

        [Fact]
        public void Manipulate_PhysicalPackagingTypeIsContainer_WhenShipCountryCodeIsCA_AndFedExTypeIsContainer_Test()
        {
            shipmentEntity.ShipCountryCode = "CA";
            shipmentEntity.FedEx.CustomsAdmissibilityPackaging = (int)FedExPhysicalPackagingType.Container;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(PhysicalPackagingType.CONTAINER, nativeRequest.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackaging);
        }

        [Fact]
        public void Manipulate_PhysicalPackagingTypeIsCrate_WhenShipCountryCodeIsCA_AndFedExTypeIsCrate_Test()
        {
            shipmentEntity.ShipCountryCode = "CA";
            shipmentEntity.FedEx.CustomsAdmissibilityPackaging = (int)FedExPhysicalPackagingType.Crate;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(PhysicalPackagingType.CRATE, nativeRequest.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackaging);
        }

        [Fact]
        public void Manipulate_PhysicalPackagingTypeIsCylinder_WhenShipCountryCodeIsCA_AndFedExTypeIsCylinder_Test()
        {
            shipmentEntity.ShipCountryCode = "CA";
            shipmentEntity.FedEx.CustomsAdmissibilityPackaging = (int)FedExPhysicalPackagingType.Cylinder;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(PhysicalPackagingType.CYLINDER, nativeRequest.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackaging);
        }

        [Fact]
        public void Manipulate_PhysicalPackagingTypeIsDrum_WhenShipCountryCodeIsCA_AndFedExTypeIsDrum_Test()
        {
            shipmentEntity.ShipCountryCode = "CA";
            shipmentEntity.FedEx.CustomsAdmissibilityPackaging = (int)FedExPhysicalPackagingType.Drum;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(PhysicalPackagingType.DRUM, nativeRequest.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackaging);
        }

        [Fact]
        public void Manipulate_PhysicalPackagingTypeIsEnvelope_WhenShipCountryCodeIsCA_AndFedExTypeIsEnvelope_Test()
        {
            shipmentEntity.ShipCountryCode = "CA";
            shipmentEntity.FedEx.CustomsAdmissibilityPackaging = (int)FedExPhysicalPackagingType.Envelope;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(PhysicalPackagingType.ENVELOPE, nativeRequest.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackaging);
        }

        [Fact]
        public void Manipulate_PhysicalPackagingTypeIsPail_WhenShipCountryCodeIsCA_AndFedExTypeIsPail_Test()
        {
            shipmentEntity.ShipCountryCode = "CA";
            shipmentEntity.FedEx.CustomsAdmissibilityPackaging = (int)FedExPhysicalPackagingType.Pail;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(PhysicalPackagingType.PAIL, nativeRequest.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackaging);
        }

        [Fact]
        public void Manipulate_PhysicalPackagingTypeIsPallet_WhenShipCountryCodeIsCA_AndFedExTypeIsPallet_Test()
        {
            shipmentEntity.ShipCountryCode = "CA";
            shipmentEntity.FedEx.CustomsAdmissibilityPackaging = (int)FedExPhysicalPackagingType.Pallet;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(PhysicalPackagingType.PALLET, nativeRequest.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackaging);
        }

        [Fact]
        public void Manipulate_PhysicalPackagingTypeIsPieces_WhenShipCountryCodeIsCA_AndFedExTypeIsPieces_Test()
        {
            shipmentEntity.ShipCountryCode = "CA";
            shipmentEntity.FedEx.CustomsAdmissibilityPackaging = (int)FedExPhysicalPackagingType.Pieces;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(PhysicalPackagingType.PIECE, nativeRequest.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackaging);
        }

        [Fact]
        public void Manipulate_PhysicalPackagingTypeIsReel_WhenShipCountryCodeIsCA_AndFedExTypeIsReel_Test()
        {
            shipmentEntity.ShipCountryCode = "CA";
            shipmentEntity.FedEx.CustomsAdmissibilityPackaging = (int)FedExPhysicalPackagingType.Reel;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(PhysicalPackagingType.REEL, nativeRequest.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackaging);
        }

        [Fact]
        public void Manipulate_PhysicalPackagingTypeIsRoll_WhenShipCountryCodeIsCA_AndFedExTypeIsRoll_Test()
        {
            shipmentEntity.ShipCountryCode = "CA";
            shipmentEntity.FedEx.CustomsAdmissibilityPackaging = (int)FedExPhysicalPackagingType.Roll;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(PhysicalPackagingType.ROLL, nativeRequest.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackaging);
        }

        [Fact]
        public void Manipulate_PhysicalPackagingTypeIsSkid_WhenShipCountryCodeIsCA_AndFedExTypeIsSkid_Test()
        {
            shipmentEntity.ShipCountryCode = "CA";
            shipmentEntity.FedEx.CustomsAdmissibilityPackaging = (int)FedExPhysicalPackagingType.Skid;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(PhysicalPackagingType.SKID, nativeRequest.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackaging);
        }

        [Fact]
        public void Manipulate_PhysicalPackagingTypeIsTank_WhenShipCountryCodeIsCA_AndFedExTypeIsTank_Test()
        {
            shipmentEntity.ShipCountryCode = "CA";
            shipmentEntity.FedEx.CustomsAdmissibilityPackaging = (int)FedExPhysicalPackagingType.Tank;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(PhysicalPackagingType.TANK, nativeRequest.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackaging);
        }

        [Fact]
        public void Manipulate_PhysicalPackagingTypeIsTube_WhenShipCountryCodeIsCA_AndFedExTypeIsTube_Test()
        {
            shipmentEntity.ShipCountryCode = "CA";
            shipmentEntity.FedEx.CustomsAdmissibilityPackaging = (int)FedExPhysicalPackagingType.Tube;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(PhysicalPackagingType.TUBE, nativeRequest.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackaging);
        }

        [Fact]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Manipulate_ThrowsFedExException_WhenShipCountryCodeIsCA_AndFedExTypeIsUnknown_Test()
        {
            shipmentEntity.ShipCountryCode = "CA";
            shipmentEntity.FedEx.CustomsAdmissibilityPackaging = 67;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(PhysicalPackagingType.TUBE, nativeRequest.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackaging);
        }
    }
}
