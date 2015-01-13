using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Enums;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    [TestClass]
    public class FedExPriorityAlertManipulatorTest
    {
        private FedExPriorityAlertManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;

        private ProcessShipmentRequest nativeRequest;
        private ShipmentEntity shipmentEntity;


        [TestInitialize]
        public void Initialize()
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


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Manipulate_ThrowsArgumentNullException_WhenCarrierRequestIsNull_Test()
        {
            testObject.Manipulate(null);
        }

        [TestMethod]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNull_Test()
        {
            // Setup the native request to be null
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, null);

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotProcessShipmentRequest_Test()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, new object());

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        public void Manipulate_AccountsForNullRequestedShipment_Test()
        {
            // Setup the test by configuring the native request to have a null requested shipment property and re-initialize
            // the carrier request with the updated native request
            nativeRequest.RequestedShipment = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            // The requested shipment property should be created now
            Assert.IsNotNull(nativeRequest.RequestedShipment);
        }

        [TestMethod]
        public void Manipulate_AccountsForNullRequestedPackageLineItems_Test()
        {
            // Setup the test by configuring the native request to have a null requested package line items
            // property and re-initialize the carrier request with the updated native request
            nativeRequest.RequestedShipment.RequestedPackageLineItems = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            // The requested package line items property should be created now
            Assert.IsNotNull(nativeRequest.RequestedShipment.RequestedPackageLineItems);
        }

        [TestMethod]
        public void Manipulate_AccountsForEmptyRequestedPackageLineItems_Test()
        {
            // Setup the test by configuring the native request to have an empty arrary for the requested 
            // package line items property and re-initialize the carrier request with the updated native request
            nativeRequest.RequestedShipment.RequestedPackageLineItems = new RequestedPackageLineItem[0];
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            // The requested package line items property should have one item in the array
            Assert.AreEqual(1, nativeRequest.RequestedShipment.RequestedPackageLineItems.Length);
        }

        [TestMethod]
        public void Manipulate_AccountsForNullSpecialServicesRequested_Test()
        {
            // Setup the test by configuring the native request to a null value for the customer references
            // property and re-initialize the carrier request with the updated native request
            nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            // The special services property should be created now
            Assert.IsNotNull(nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested);
        }

        [TestMethod]
        public void Manipulate_EnhancementTypeArrayIsNull_WhenShipmentWithSinglePackage_HasEnhancementTypeOfNone_Test()
        {
            // Setup only includes clearing out the package list and add one with none as enhancment type
            shipmentEntity.FedEx.Packages.Clear();
            shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity() { PriorityAlertEnhancementType = (int)FedExPriorityAlertEnhancementType.None, DimsHeight = 2, DimsWidth = 2, DimsLength = 2 });

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsNull(nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.PriorityAlertDetail.EnhancementTypes);
        }

        [TestMethod]
        public void Manipulate_EnhancementTypeArrayIsNull_WhenShipmentWithMultiplePackages_HavingEnhancementTypeOfNone_Test()
        {
            // No additional setup needed
            testObject.Manipulate(carrierRequest.Object);

            List<RequestedPackageLineItem> lineItems = nativeRequest.RequestedShipment.RequestedPackageLineItems.ToList();
            Assert.AreEqual(0, lineItems.Count(l => l.SpecialServicesRequested.PriorityAlertDetail.EnhancementTypes != null));
        }

        [TestMethod]
        public void Manipulate_EnhancementTypeArrayIsAssigned_WhenShipment_WithSinglePackage_HasEnhancementTypeOfAlertPlus_Test()
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

            Assert.AreEqual(PriorityAlertEnhancementType.PRIORITY_ALERT_PLUS, nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.PriorityAlertDetail.EnhancementTypes[0]);
        }
        
        [TestMethod]
        public void Manipulate_ContentLengthIsOne_WhenEnhancementTypeIsPriorityPlus_Test()
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

            Assert.AreEqual(1, nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.PriorityAlertDetail.Content.Length);
        }

        [TestMethod]
        public void Manipulate_SetsContent_WhenEnhancementTypeIsPriorityPlus_Test()
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

            Assert.AreEqual(shipmentEntity.FedEx.Packages[0].PriorityAlertDetailContent, nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.PriorityAlertDetail.Content[0]);
        }

        [TestMethod]
        public void Manipulate_ContentIsNotNull_WhenEnhancementTypeIsNone_Test()
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

            Assert.IsNotNull(nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.PriorityAlertDetail.Content);
        }

        [TestMethod]
        public void Manipulate_SpecialServiceTypesIsCorrect_WhenEnhancementTypeIsNoneButPriorityAlertIsTrue_Test()
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

            Assert.AreEqual(nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.PriorityAlertDetail.Content[0], "Some Content");
            Assert.IsNull(nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.PriorityAlertDetail.EnhancementTypes);
            Assert.AreEqual(nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SpecialServiceTypes.Count(), 1);
            Assert.AreEqual(nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SpecialServiceTypes[0], PackageSpecialServiceType.PRIORITY_ALERT);
        }
    }
}
