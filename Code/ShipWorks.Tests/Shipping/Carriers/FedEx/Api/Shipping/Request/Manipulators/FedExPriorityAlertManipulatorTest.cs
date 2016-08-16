using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Enums;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    public class FedExPriorityAlertManipulatorTest
    {
        private FedExPriorityAlertManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;

        private ProcessShipmentRequest nativeRequest;
        private ShipmentEntity shipmentEntity;


        public FedExPriorityAlertManipulatorTest()
        {
            // Setup the carrier request that will be sent to the test object
            nativeRequest = new ProcessShipmentRequest
            {
                RequestedShipment = new RequestedShipment
                {
                    RequestedPackageLineItems = new RequestedPackageLineItem[2]
                    {
                        new RequestedPackageLineItem()
                        {
                            SpecialServicesRequested = new PackageSpecialServicesRequested() {PriorityAlertDetail = new PriorityAlertDetail()}
                        },
                        new RequestedPackageLineItem()
                        {
                            SpecialServicesRequested = new PackageSpecialServicesRequested() {PriorityAlertDetail = new PriorityAlertDetail()}
                        }
                    }
                }
            };

            shipmentEntity = new ShipmentEntity
            {
                FedEx = new FedExShipmentEntity()
            };

            // Set the quantity of FedEx packages of the shipment entity equal to that in the native request for the happy path
            shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity() { PriorityAlertEnhancementType = (int)FedExPriorityAlertEnhancementType.None, DimsHeight = 2, DimsWidth = 2, DimsLength = 2 });
            shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity() { PriorityAlertEnhancementType = (int)FedExPriorityAlertEnhancementType.None, DimsHeight = 2, DimsWidth = 2, DimsLength = 2 });

            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            testObject = new FedExPriorityAlertManipulator();
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
            // Setup the test by configuring the native request to have a null requested shipment property and re-initialize
            // the carrier request with the updated native request
            nativeRequest.RequestedShipment = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            // The requested shipment property should be created now
            Assert.NotNull(nativeRequest.RequestedShipment);
        }

        [Fact]
        public void Manipulate_AccountsForNullRequestedPackageLineItems()
        {
            // Setup the test by configuring the native request to have a null requested package line items
            // property and re-initialize the carrier request with the updated native request
            nativeRequest.RequestedShipment.RequestedPackageLineItems = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            // The requested package line items property should be created now
            Assert.NotNull(nativeRequest.RequestedShipment.RequestedPackageLineItems);
        }

        [Fact]
        public void Manipulate_AccountsForEmptyRequestedPackageLineItems()
        {
            // Setup the test by configuring the native request to have an empty arrary for the requested 
            // package line items property and re-initialize the carrier request with the updated native request
            nativeRequest.RequestedShipment.RequestedPackageLineItems = new RequestedPackageLineItem[0];
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            // The requested package line items property should have one item in the array
            Assert.Equal(1, nativeRequest.RequestedShipment.RequestedPackageLineItems.Length);
        }

        [Fact]
        public void Manipulate_AccountsForNullSpecialServicesRequested()
        {
            // Setup the test by configuring the native request to a null value for the customer references
            // property and re-initialize the carrier request with the updated native request
            nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            // The special services property should be created now
            Assert.NotNull(nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested);
        }

        [Fact]
        public void Manipulate_EnhancementTypeArrayIsNull_WhenShipmentWithSinglePackage_HasEnhancementTypeOfNone()
        {
            // Setup only includes clearing out the package list and add one with none as enhancment type
            shipmentEntity.FedEx.Packages.Clear();
            shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity() { PriorityAlertEnhancementType = (int)FedExPriorityAlertEnhancementType.None, DimsHeight = 2, DimsWidth = 2, DimsLength = 2 });

            testObject.Manipulate(carrierRequest.Object);

            Assert.Null(nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.PriorityAlertDetail.EnhancementTypes);
        }

        [Fact]
        public void Manipulate_EnhancementTypeArrayIsNull_WhenShipmentWithMultiplePackages_HavingEnhancementTypeOfNone()
        {
            // No additional setup needed
            testObject.Manipulate(carrierRequest.Object);

            List<RequestedPackageLineItem> lineItems = nativeRequest.RequestedShipment.RequestedPackageLineItems.ToList();
            Assert.Equal(0, lineItems.Count(l => l.SpecialServicesRequested.PriorityAlertDetail.EnhancementTypes != null));
        }

        [Fact]
        public void Manipulate_EnhancementTypeArrayIsAssigned_WhenShipment_WithSinglePackage_HasEnhancementTypeOfAlertPlus()
        {
            // Setup only includes clearing out the package list and add one with priorty alert plus
            shipmentEntity.FedEx.Packages.Clear();
            shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity()
            {
                PriorityAlertEnhancementType = (int)FedExPriorityAlertEnhancementType.PriorityAlertPlus,
                PriorityAlertDetailContent = "Some Content",
                PriorityAlert = true,
                DimsHeight = 2,
                DimsWidth = 2,
                DimsLength = 2
            });

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(PriorityAlertEnhancementType.PRIORITY_ALERT_PLUS, nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.PriorityAlertDetail.EnhancementTypes[0]);
        }

        [Fact]
        public void Manipulate_ContentLengthIsOne_WhenEnhancementTypeIsPriorityPlus()
        {
            // Setup only includes clearing out the package list and add one with the priorty alert plus enhancment types
            shipmentEntity.FedEx.Packages.Clear();
            shipmentEntity.FedEx.Packages.Add
                (
                    new FedExPackageEntity()
                    {
                        PriorityAlertEnhancementType = (int)FedExPriorityAlertEnhancementType.PriorityAlertPlus,
                        PriorityAlertDetailContent = "Some Content",
                        PriorityAlert = true,
                        DimsHeight = 2,
                        DimsWidth = 2,
                        DimsLength = 2
                    }
                );

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(1, nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.PriorityAlertDetail.Content.Length);
        }

        [Fact]
        public void Manipulate_SetsContent_WhenEnhancementTypeIsPriorityPlus()
        {
            // Setup only includes clearing out the package list and add one with the priorty alert plus enhancment types
            shipmentEntity.FedEx.Packages.Clear();
            shipmentEntity.FedEx.Packages.Add
                (
                    new FedExPackageEntity()
                    {
                        PriorityAlertEnhancementType = (int)FedExPriorityAlertEnhancementType.PriorityAlertPlus,
                        PriorityAlertDetailContent = "Some Content",
                        PriorityAlert = true,
                        DimsHeight = 2,
                        DimsWidth = 2,
                        DimsLength = 2
                    }
                );

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(shipmentEntity.FedEx.Packages[0].PriorityAlertDetailContent, nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.PriorityAlertDetail.Content[0]);
        }

        [Fact]
        public void Manipulate_ContentIsNotNull_WhenEnhancementTypeIsNone()
        {
            // Setup only includes clearing out the package list and add one with the priorty alert plus enhancment types
            shipmentEntity.FedEx.Packages.Clear();
            shipmentEntity.FedEx.Packages.Add
                (
                    new FedExPackageEntity()
                    {
                        PriorityAlertEnhancementType = (int)FedExPriorityAlertEnhancementType.None,
                        PriorityAlertDetailContent = "Some Content",
                        PriorityAlert = true,
                        DimsHeight = 2,
                        DimsWidth = 2,
                        DimsLength = 2
                    }
                );

            testObject.Manipulate(carrierRequest.Object);

            Assert.NotNull(nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.PriorityAlertDetail.Content);
        }

        [Fact]
        public void Manipulate_SpecialServiceTypesIsCorrect_WhenEnhancementTypeIsNoneButPriorityAlertIsTrue()
        {
            // Setup only includes clearing out the package list and add one with the priorty alert plus enhancment types
            shipmentEntity.FedEx.Packages.Clear();
            shipmentEntity.FedEx.Packages.Add
                (
                    new FedExPackageEntity()
                    {
                        PriorityAlertEnhancementType = (int)FedExPriorityAlertEnhancementType.None,
                        PriorityAlertDetailContent = "Some Content",
                        PriorityAlert = true,
                        DimsHeight = 2,
                        DimsWidth = 2,
                        DimsLength = 2
                    }
                );

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.PriorityAlertDetail.Content[0], "Some Content");
            Assert.Null(nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.PriorityAlertDetail.EnhancementTypes);
            Assert.Equal(nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SpecialServiceTypes.Count(), 1);
            Assert.Equal(nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SpecialServiceTypes[0], PackageSpecialServiceType.PRIORITY_ALERT);
        }
    }
}
