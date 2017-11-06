using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using Xunit;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Enums;
using ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Ship.Manipulators.Request
{
    public class FedExPriorityAlertManipulatorTest
    {
        private readonly ProcessShipmentRequest processShipmentRequest;
        private readonly ShipmentEntity shipment;
        private readonly FedExPriorityAlertManipulator testObject;

        public FedExPriorityAlertManipulatorTest()
        {
            AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            // Setup the carrier request that will be sent to the test object
            processShipmentRequest = new ProcessShipmentRequest()
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

            shipment = Create.Shipment().AsFedEx().Build();

            // Set the quantity of FedEx packages of the shipment entity equal to that in the native request for the happy path
            shipment.FedEx.Packages.Add(new FedExPackageEntity() { PriorityAlertEnhancementType = (int)FedExPriorityAlertEnhancementType.None, DimsHeight = 2, DimsWidth = 2, DimsLength = 2 });
            shipment.FedEx.Packages.Add(new FedExPackageEntity() { PriorityAlertEnhancementType = (int)FedExPriorityAlertEnhancementType.None, DimsHeight = 2, DimsWidth = 2, DimsLength = 2 });

            testObject = mock.Create<FedExPriorityAlertManipulator>();
        }

        [Theory]
        [InlineData(0, true)]
        [InlineData(1, true)]
        [InlineData(2, false)]
        public void ShouldApply_ReturnsCorrectValue(int sequenceNumber, bool expectedValue)
        {
            Assert.Equal(expectedValue, testObject.ShouldApply(shipment, sequenceNumber));
        }

        [Fact]
        public void Manipulate_ThrowsArgumentNullException_WhenShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.Manipulate(null, new ProcessShipmentRequest(), 0));
        }

        [Fact]
        public void Manipulate_ThrowsArgumentNullException_WhenProcessShipmentRequestIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.Manipulate(new ShipmentEntity(), null, 0));
        }

        [Fact]
        public void Manipulate_AccountsForNullRequestedShipment()
        {
            // Setup the test by configuring the native request to have a null requested shipment property and re-initialize
            // the carrier request with the updated native request
            processShipmentRequest.RequestedShipment = null;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // The requested shipment property should be created now
            Assert.NotNull(processShipmentRequest.RequestedShipment);
        }

        [Fact]
        public void Manipulate_AccountsForNullRequestedPackageLineItems()
        {
            // Setup the test by configuring the native request to have a null requested package line items
            // property and re-initialize the carrier request with the updated native request
            processShipmentRequest.RequestedShipment.RequestedPackageLineItems = null;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // The requested package line items property should be created now
            Assert.NotNull(processShipmentRequest.RequestedShipment.RequestedPackageLineItems);
        }

        [Fact]
        public void Manipulate_AccountsForEmptyRequestedPackageLineItems()
        {
            // Setup the test by configuring the native request to have an empty arrary for the requested 
            // package line items property and re-initialize the carrier request with the updated native request
            processShipmentRequest.RequestedShipment.RequestedPackageLineItems = new RequestedPackageLineItem[0];

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // The requested package line items property should have one item in the array
            Assert.Equal(1, processShipmentRequest.RequestedShipment.RequestedPackageLineItems.Length);
        }

        [Fact]
        public void Manipulate_AccountsForNullSpecialServicesRequested()
        {
            // Setup the test by configuring the native request to a null value for the customer references
            // property and re-initialize the carrier request with the updated native request
            processShipmentRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested = null;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // The special services property should be created now
            Assert.NotNull(processShipmentRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested);
        }

        [Fact]
        public void Manipulate_EnhancementTypeArrayIsNull_WhenShipmentWithSinglePackage_HasEnhancementTypeOfNone()
        {
            // Setup only includes clearing out the package list and add one with none as enhancment type
            shipment.FedEx.Packages.Clear();
            shipment.FedEx.Packages.Add(new FedExPackageEntity() { PriorityAlertEnhancementType = (int)FedExPriorityAlertEnhancementType.None, DimsHeight = 2, DimsWidth = 2, DimsLength = 2 });

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Null(processShipmentRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.PriorityAlertDetail.EnhancementTypes);
        }

        [Fact]
        public void Manipulate_EnhancementTypeArrayIsNull_WhenShipmentWithMultiplePackages_HavingEnhancementTypeOfNone()
        {
            // No additional setup needed
            testObject.Manipulate(shipment, processShipmentRequest, 0);

            List<RequestedPackageLineItem> lineItems = processShipmentRequest.RequestedShipment.RequestedPackageLineItems.ToList();
            Assert.Equal(0, lineItems.Count(l => l.SpecialServicesRequested.PriorityAlertDetail.EnhancementTypes != null));
        }

        [Fact]
        public void Manipulate_EnhancementTypeArrayIsAssigned_WhenShipment_WithSinglePackage_HasEnhancementTypeOfAlertPlus()
        {
            // Setup only includes clearing out the package list and add one with priorty alert plus
            shipment.FedEx.Packages.Clear();
            shipment.FedEx.Packages.Add(new FedExPackageEntity()
            {
                PriorityAlertEnhancementType = (int)FedExPriorityAlertEnhancementType.PriorityAlertPlus,
                PriorityAlertDetailContent = "Some Content",
                PriorityAlert = true,
                DimsHeight = 2,
                DimsWidth = 2,
                DimsLength = 2
            });

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal(PriorityAlertEnhancementType.PRIORITY_ALERT_PLUS, processShipmentRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.PriorityAlertDetail.EnhancementTypes[0]);
        }

        [Fact]
        public void Manipulate_ContentLengthIsOne_WhenEnhancementTypeIsPriorityPlus()
        {
            // Setup only includes clearing out the package list and add one with the priorty alert plus enhancment types
            shipment.FedEx.Packages.Clear();
            shipment.FedEx.Packages.Add
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

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal(1, processShipmentRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.PriorityAlertDetail.Content.Length);
        }

        [Fact]
        public void Manipulate_SetsContent_WhenEnhancementTypeIsPriorityPlus()
        {
            // Setup only includes clearing out the package list and add one with the priorty alert plus enhancment types
            shipment.FedEx.Packages.Clear();
            shipment.FedEx.Packages.Add
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

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal(shipment.FedEx.Packages[0].PriorityAlertDetailContent, processShipmentRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.PriorityAlertDetail.Content[0]);
        }

        [Fact]
        public void Manipulate_ContentIsNotNull_WhenEnhancementTypeIsNone()
        {
            // Setup only includes clearing out the package list and add one with the priorty alert plus enhancment types
            shipment.FedEx.Packages.Clear();
            shipment.FedEx.Packages.Add
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

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.NotNull(processShipmentRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.PriorityAlertDetail.Content);
        }

        [Fact]
        public void Manipulate_SpecialServiceTypesIsCorrect_WhenEnhancementTypeIsNoneButPriorityAlertIsTrue()
        {
            // Setup only includes clearing out the package list and add one with the priorty alert plus enhancment types
            shipment.FedEx.Packages.Clear();
            shipment.FedEx.Packages.Add
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

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal(processShipmentRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.PriorityAlertDetail.Content[0], "Some Content");
            Assert.Null(processShipmentRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.PriorityAlertDetail.EnhancementTypes);
            Assert.Equal(processShipmentRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SpecialServiceTypes.Count(), 1);
            Assert.Equal(processShipmentRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SpecialServiceTypes[0], PackageSpecialServiceType.PRIORITY_ALERT);
        }
    }
}
