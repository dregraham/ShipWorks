using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Rating;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Services.Builders;
using ShipWorks.Shipping.UI.ShippingPanel.ShipmentControl;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.ShippingPanel.ShipmentControl
{
    public class ShipmentViewModelTest
    {
        private readonly ShipmentEntity shipment = new ShipmentEntity();
        private Mock<ICarrierShipmentAdapter> shipmentAdapter;
        private Mock<IShipmentServicesBuilder> shipmentServicesBuilder;
        private Mock<IShipmentServicesBuilderFactory> shipmentServicesBuilderFactory;
        private Mock<IShipmentPackageTypesBuilder> shipmentPackageTypesBuilder;
        private Mock<IShipmentPackageTypesBuilderFactory> shipmentPackageTypesBuilderFactory;
        private Mock<IRateSelectionFactory> rateSelectionFactory;
        private Dictionary<int, string> expectedServices = new Dictionary<int, string>();
        private Dictionary<int, string> expectedPackageTypes = new Dictionary<int, string>();
        private readonly List<IPackageAdapter> packageAdapters = new List<IPackageAdapter>();

        [Fact]
        public void ShipDate_MatchesShipmentAdapterValue_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                CreateDefaultShipmentAdapter(mock, 2);

                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
                testObject.Load(shipmentAdapter.Object);

                Assert.Equal(shipmentAdapter.Object.ShipDate, testObject.ShipDate);
            }
        }

        [Fact]
        public void TotalWeight_MatchesShipmentAdapterValue_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                CreateDefaultShipmentAdapter(mock, 2);

                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
                testObject.Load(shipmentAdapter.Object);

                Assert.Equal(shipmentAdapter.Object.TotalWeight, testObject.TotalWeight);
            }
        }

        [Fact]
        public void UsingInsurance_MatchesShipmentAdapterValue_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                CreateDefaultShipmentAdapter(mock, 2);

                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
                testObject.Load(shipmentAdapter.Object);

                Assert.Equal(shipmentAdapter.Object.UsingInsurance, testObject.UsingInsurance);
            }
        }

        [Fact]
        public void ServiceType_MatchesShipmentAdapterValue_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                CreateDefaultShipmentAdapter(mock, 2);

                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
                testObject.Load(shipmentAdapter.Object);

                Assert.Equal(shipmentAdapter.Object.ServiceType, testObject.ServiceType);
            }
        }

        [Fact]
        public void SupportsMultiplePackages_MatchesShipmentAdapterValue_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                CreateDefaultShipmentAdapter(mock, 2);

                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
                testObject.Load(shipmentAdapter.Object);

                Assert.Equal(shipmentAdapter.Object.SupportsMultiplePackages, testObject.SupportsMultiplePackages);
            }
        }

        [Fact]
        public void SupportsPackageTypes_MatchesShipmentAdapterValue_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                CreateDefaultShipmentAdapter(mock, 2);

                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
                testObject.Load(shipmentAdapter.Object);

                Assert.Equal(shipmentAdapter.Object.SupportsPackageTypes, testObject.SupportsPackageTypes);
            }
        }

        [Fact]
        public void PackageCountList_HasTwentyFiveEntries_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                CreateDefaultShipmentAdapter(mock, 2);

                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
                testObject.Load(shipmentAdapter.Object);

                Assert.Equal(25, testObject.PackageCountList.Count());
            }
        }

        [Fact]
        public void NumberOfPackages_AddsPackageAdapters_WhenNumberRequestedGreaterThanCurrentCount_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                CreateDefaultShipmentAdapter(mock, 2);

                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
                testObject.Load(shipmentAdapter.Object);

                int numOfPackages = testObject.NumberOfPackages;
                numOfPackages++;
                testObject.NumberOfPackages = numOfPackages;

                Assert.Equal(numOfPackages, packageAdapters.Count);
            }
        }

        [Fact]
        public void NumberOfPackages_RemovesPackageAdapters_WhenNumberRequestedLessThanCurrentCount_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                CreateDefaultShipmentAdapter(mock, 2);

                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
                testObject.Load(shipmentAdapter.Object);

                int numOfPackages = testObject.NumberOfPackages;
                numOfPackages--;
                testObject.NumberOfPackages = numOfPackages;

                Assert.Equal(numOfPackages, packageAdapters.Count);
            }
        }

        [Fact]
        public void SelectedPackageAdapter_DefaultsToFirstInList_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                CreateDefaultShipmentAdapter(mock, 2);

                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
                testObject.Load(shipmentAdapter.Object);

                Assert.Equal(packageAdapters[0].Index, testObject.SelectedPackageAdapter.Index);
            }
        }

        [Fact]
        public void Load_GetsServices_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                expectedServices = new Dictionary<int, string>();
                expectedServices.Add(0, "Service 0");
                expectedServices.Add(1, "Service 1");
                
                CreateDefaultShipmentAdapter(mock, 2);

                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
                testObject.Load(shipmentAdapter.Object);
                Assert.Equal(expectedServices.Count, testObject.Services.Count);
            }
        }

        [Fact]
        public void RefreshServiceTypes_UpdatesServices_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                expectedServices = new Dictionary<int, string>();
                expectedServices.Add(0, "Service 0");
                expectedServices.Add(1, "Service 1");

                CreateDefaultShipmentAdapter(mock, 2);

                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
                testObject.Load(shipmentAdapter.Object);
                Assert.Equal(expectedServices.Count, testObject.Services.Count);

                expectedServices.Add(3, "Service 3");
                testObject.RefreshServiceTypes();
                Assert.Equal(expectedServices.Count, testObject.Services.Count);
            }
        }

        [Fact]
        public void RefreshServiceTypes_ReturnsErrorService_WhenInvalidRateGroupShippingException_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                CreateDefaultShipmentAdapter(mock, 2);
                shipmentServicesBuilder.Setup(sb => sb.BuildServiceTypeDictionary(It.IsAny<IEnumerable<ShipmentEntity>>()))
                    .Throws(new InvalidRateGroupShippingException(new RateGroup(Enumerable.Empty<RateResult>())));

                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
                testObject.Load(shipmentAdapter.Object);

                Assert.Equal(1, testObject.Services.Count);
                Assert.Contains("error", testObject.Services.First().Value, StringComparison.OrdinalIgnoreCase);
            }
        }

        [Fact]
        public void RefreshServiceTypes_SetsServiceType_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                CreateDefaultShipmentAdapter(mock, 2);

                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
                testObject.Load(shipmentAdapter.Object);
                testObject.ServiceType = -1;

                testObject.RefreshServiceTypes();

                Assert.Equal(shipmentAdapter.Object.ServiceType, testObject.ServiceType);
            }
        }

        [Fact]
        public void Load_GetsPackageTypes_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                expectedPackageTypes = new Dictionary<int, string>();
                expectedPackageTypes.Add(0, "Package Type 0");
                expectedPackageTypes.Add(1, "Package Type 1");

                CreateDefaultShipmentAdapter(mock, 2);

                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
                testObject.Load(shipmentAdapter.Object);

                Assert.Equal(expectedPackageTypes.Count, testObject.PackageTypes.Count);
            }
        }

        [Fact]
        public void RefreshPackageTypes_UpdatesPackageTypes_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                expectedPackageTypes = new Dictionary<int, string>();
                expectedPackageTypes.Add(0, "Package Type 0");
                expectedPackageTypes.Add(1, "Package Type 1");

                CreateDefaultShipmentAdapter(mock, 2);

                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
                testObject.Load(shipmentAdapter.Object);
                Assert.Equal(expectedPackageTypes.Count, testObject.PackageTypes.Count);

                expectedServices.Add(3, "Package Type 3");
                testObject.RefreshServiceTypes();
                Assert.Equal(expectedPackageTypes.Count, testObject.PackageTypes.Count);
            }
        }

        [Fact]
        public void Save_UpdatesShipmentAdapter_WithViewModelValue_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                CreateDefaultShipmentAdapter(mock, 2);

                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
                testObject.Load(shipmentAdapter.Object);

                testObject.ShipDate = testObject.ShipDate.AddDays(1);
                testObject.UsingInsurance = !testObject.UsingInsurance;
                testObject.ServiceType = testObject.ServiceType++;

                testObject.Save();

                shipmentAdapter.VerifySet(sa => sa.ShipDate = testObject.ShipDate, Times.Once());
                shipmentAdapter.VerifySet(sa => sa.UsingInsurance = testObject.UsingInsurance);
                shipmentAdapter.VerifySet(sa => sa.ServiceType = testObject.ServiceType);
            }
        }


        [Fact]
        public void SelectedRateChangedMessage_DelegatesTo_HandleSelectedRateChangedMessageAndUpdatesServiceType_Test()
        {
            IMessenger messenger = new TestMessenger();

            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                mock.Provide(messenger);

                Mock<IRateSelection> rateSelection = mock.Mock<IRateSelection>();
                rateSelection.Setup(rs => rs.ServiceType).Returns(99);

                rateSelectionFactory = mock.Mock<IRateSelectionFactory>();
                rateSelectionFactory.Setup(r => r.CreateRateSelection(It.IsAny<RateResult>()))
                    .Returns(rateSelection.Object);

                CreateDefaultShipmentAdapter(mock, 2);

                shipmentAdapter.SetupSet(sa => sa.ServiceType = rateSelection.Object.ServiceType);

                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
                testObject.Load(shipmentAdapter.Object);

                SelectedRateChangedMessage message = new SelectedRateChangedMessage(this, new RateResult("test message", "1", 1, "test"));
                messenger.Send(message);

                shipmentAdapter.Verify(sa => sa.ServiceType);
                Assert.Equal(rateSelection.Object.ServiceType, testObject.ServiceType);
            }
        }

        private Dictionary<int, string> CreateDefaultShipmentAdapter(AutoMock mock, int numberOfPackages)
        {


            shipmentServicesBuilder = mock.Mock<IShipmentServicesBuilder>();
            shipmentServicesBuilder.Setup(sb => sb.BuildServiceTypeDictionary(It.IsAny<IEnumerable<ShipmentEntity>>())).Returns(expectedServices);

            shipmentServicesBuilderFactory = mock.Mock<IShipmentServicesBuilderFactory>();
            shipmentServicesBuilderFactory.Setup(sbf => sbf.Get(It.IsAny<ShipmentTypeCode>())).Returns(shipmentServicesBuilder.Object);

            shipmentPackageTypesBuilder = mock.Mock<IShipmentPackageTypesBuilder>();
            shipmentPackageTypesBuilder.Setup(sb => sb.BuildPackageTypeDictionary(It.IsAny<IEnumerable<ShipmentEntity>>())).Returns(expectedPackageTypes);

            shipmentPackageTypesBuilderFactory = mock.Mock<IShipmentPackageTypesBuilderFactory>();
            shipmentPackageTypesBuilderFactory.Setup(sbf => sbf.Get(It.IsAny<ShipmentTypeCode>())).Returns(shipmentPackageTypesBuilder.Object);
            
            CreatePackageAdapters(mock, numberOfPackages);

            shipmentAdapter = mock.Mock<ICarrierShipmentAdapter>();
            shipmentAdapter.Setup(sa => sa.ShipmentTypeCode).Returns(ShipmentTypeCode.UpsOnLineTools);
            shipmentAdapter.Setup(sa => sa.ServiceType).Returns((int) UpsServiceType.UpsGround);
            shipmentAdapter.Setup(sa => sa.ShipDate).Returns(new DateTime(2015, 1, 1, 1, 1, 1));
            shipmentAdapter.Setup(sa => sa.TotalWeight).Returns(0.5);
            shipmentAdapter.Setup(sa => sa.UsingInsurance).Returns(false);
            shipmentAdapter.Setup(sa => sa.SupportsPackageTypes).Returns(true);
            shipmentAdapter.Setup(sa => sa.SupportsAccounts).Returns(true);
            shipmentAdapter.Setup(sa => sa.SupportsMultiplePackages).Returns(true);
            shipmentAdapter.Setup(sa => sa.GetPackageAdapters()).Returns(packageAdapters);
            shipmentAdapter.Setup(sa => sa.GetPackageAdapters(It.IsAny<int>())).Returns((int x) =>
                {
                    CreatePackageAdapters(mock, x);
                    return packageAdapters;
                });

            return expectedServices;
        }

        private void CreatePackageAdapters(AutoMock mock, int numberOfPackages)
        {
            packageAdapters.Clear();

            for (int i = 1; i <= numberOfPackages; i++)
            {
                Mock<IPackageAdapter> packageAdapter = mock.Mock<IPackageAdapter>();
                packageAdapter.Setup(pa => pa.PackagingType).Returns(new PackageTypeBinding() {PackageTypeID = (int) UpsPackagingType.Custom, Name = "Your Pakaging"});
                packageAdapter.Setup(pa => pa.AdditionalWeight).Returns(0.1*i);
                packageAdapter.Setup(pa => pa.ApplyAdditionalWeight).Returns(false);
                packageAdapter.Setup(pa => pa.Index).Returns(1*i);
                packageAdapter.Setup(pa => pa.DimsHeight).Returns(2*i);
                packageAdapter.Setup(pa => pa.DimsLength).Returns(2*i);
                packageAdapter.Setup(pa => pa.DimsWidth).Returns(1*i);
                packageAdapter.Setup(pa => pa.Weight).Returns(0.5*i);

                packageAdapters.Add(packageAdapter.Object);
            }
        }
    }
}
