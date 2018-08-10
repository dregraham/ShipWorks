using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Autofac.Extras.Moq;
using Interapptive.Shared.Messaging;
using Moq;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping.Services;
using ShipWorks.Tests.Shared;
using Xunit;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Enums;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.Other;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Policies;
using ShipWorks.Stores;
using ShipWorks.Stores.Platforms.Amazon;

namespace ShipWorks.Shipping.Tests.Services
{
    public class ShipmentTypeProviderTest : IDisposable
    {
        readonly AutoMock mock;
        readonly Subject<EnabledCarriersChangedMessage> subject;
        private readonly Mock<IDataProvider> dataProvider;
        private readonly Mock<IStoreManager> storeManager;
        private readonly Mock<ILicenseService> licenseService;
        private readonly Mock<ILicense> license;
        private AmazonPrimeShippingPolicyTarget target;
        private AmazonShipmentShippingPolicy amazonShipmentShippingPolicy;
        private const long nonAmazonOrderID = 1;
        private const long amazonOrderID = 100;
        private const long nonAmazonStoreID = 1005;
        private const long amazonStoreID = 2005;
        private OrderEntity nonAmazonOrder = new OrderEntity(nonAmazonOrderID);
        private StoreEntity nonAmazonStore = new StoreEntity(nonAmazonStoreID);
        private AmazonOrderEntity amazonOrder = new AmazonOrderEntity(amazonOrderID);
        private AmazonStoreEntity amazonStore = new AmazonStoreEntity(amazonStoreID);
        private OtherShipmentType otherShipmentType;
        private UspsShipmentType uspsShipmentType;
        private FedExShipmentType fedExShipmentType;
        private AmazonShipmentType amazonShipmentType;
        private ShipmentEntity shipment;

        public ShipmentTypeProviderTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            subject = new Subject<EnabledCarriersChangedMessage>();
            mock.Provide<IObservable<IShipWorksMessage>>(subject);

            dataProvider = mock.Mock<IDataProvider>();
            dataProvider.Setup(d => d.GetEntity(nonAmazonOrderID, It.IsAny<bool>()))
                .Returns(nonAmazonOrder);
            dataProvider.Setup(d => d.GetEntity(amazonOrderID, It.IsAny<bool>()))
                .Returns(amazonOrder);

            storeManager = mock.Mock<IStoreManager>();
            storeManager.Setup(d => d.GetStore(nonAmazonStoreID))
                .Returns(new StoreEntity());
            storeManager.Setup(d => d.GetStore(amazonStoreID))
                .Returns(new AmazonStoreEntity());

            license = mock.Mock<ILicense>();
            licenseService = mock.Mock<ILicenseService>();
            licenseService.Setup(ls => ls.GetLicenses()).Returns(new[] { license.Object });

            otherShipmentType = mock.Create<OtherShipmentType>();
            uspsShipmentType = mock.Create<UspsShipmentType>();
            fedExShipmentType = mock.Create<FedExShipmentType>();
            amazonShipmentType = mock.Create<AmazonShipmentType>();

            amazonOrder.IsPrime = (int) AmazonIsPrime.No;
            shipment = new ShipmentEntity
            {
                Order = amazonOrder
            };

            target = new AmazonPrimeShippingPolicyTarget()
            {
                Shipment = shipment,
                Allowed = false,
                AmazonOrder = amazonOrder,
                AmazonCredentials = amazonStore as IAmazonCredentials
            };

            amazonShipmentShippingPolicy = new AmazonShipmentShippingPolicy();
            amazonShipmentShippingPolicy.Configure("1");
            amazonShipmentShippingPolicy.Apply(target);

            license
                .Setup(l => l.ApplyShippingPolicy(ShipmentTypeCode.Amazon, It.IsAny<object>()))
                .Callback((ShipmentTypeCode s, object t) => ((AmazonPrimeShippingPolicyTarget) t).Allowed = target.Allowed);

            amazonOrder.Store = amazonStore;
            nonAmazonOrder.Store = nonAmazonStore;
        }

        [Fact]
        public void Constructor_PopulatesAvailableListOfShipments()
        {
            mock.Mock<IShipmentTypeManager>().SetupGet(x => x.EnabledShipmentTypeCodes)
                .Returns(new List<ShipmentTypeCode> { ShipmentTypeCode.Other, ShipmentTypeCode.Usps });

            mock.Create<ShipmentTypeProvider>();

            mock.Mock<IShipmentTypeManager>().Verify(s => s.EnabledShipmentTypeCodes);
        }

        [Fact]
        public void Constructor_UpdatesAvailableShipmentTypesFromManager_WhenEnabledCarriersChangedMessageReceived()
        {
            mock.Mock<IShipmentTypeManager>().SetupGet(x => x.ShipmentTypes)
                .Returns(new List<ShipmentType> { otherShipmentType, uspsShipmentType, fedExShipmentType });
            mock.Mock<IShipmentTypeManager>().SetupSequence(x => x.EnabledShipmentTypeCodes)
                .Returns(new List<ShipmentTypeCode> { ShipmentTypeCode.Other, ShipmentTypeCode.Usps })
                .Returns(new List<ShipmentTypeCode> { ShipmentTypeCode.FedEx, ShipmentTypeCode.Usps });

            ShipmentTypeProvider testObject = mock.Create<ShipmentTypeProvider>();

            subject.OnNext(new EnabledCarriersChangedMessage(null, new List<ShipmentTypeCode>(), new List<ShipmentTypeCode>()));

            var carrierAdapter = mock.Mock<ICarrierShipmentAdapter>();
            carrierAdapter.SetupGet(c => c.ShipmentTypeCode).Returns(ShipmentTypeCode.Other);

            Assert.Equal(2, testObject.GetAvailableShipmentTypes(carrierAdapter.Object).Count());
            Assert.Contains(ShipmentTypeCode.FedEx, testObject.GetAvailableShipmentTypes(carrierAdapter.Object));
            Assert.Contains(ShipmentTypeCode.Usps, testObject.GetAvailableShipmentTypes(carrierAdapter.Object));
        }

        [Fact]
        public void Constructor_RemovesExcludedShipmentTypes_WhenMessageContainsCarriersToExclude()
        {
            mock.Mock<IShipmentTypeManager>().SetupGet(x => x.EnabledShipmentTypeCodes)
                .Returns(new List<ShipmentTypeCode> { ShipmentTypeCode.Other, ShipmentTypeCode.Usps });

            ShipmentTypeProvider testObject = mock.Create<ShipmentTypeProvider>();

            subject.OnNext(new EnabledCarriersChangedMessage(null, new List<ShipmentTypeCode>(), new[] { ShipmentTypeCode.Other }));

            var carrierAdapter = mock.Mock<ICarrierShipmentAdapter>();
            carrierAdapter.SetupGet(c => c.ShipmentTypeCode).Returns(ShipmentTypeCode.Other);

            Assert.DoesNotContain(ShipmentTypeCode.Other, testObject.GetAvailableShipmentTypes(carrierAdapter.Object));
        }

        [Fact]
        public void Constructor_AddsShipmentTypes_WhenMessageContainsCarriersToAdd()
        {
            mock.Mock<IShipmentTypeManager>().SetupGet(x => x.EnabledShipmentTypeCodes)
                .Returns(new List<ShipmentTypeCode> { ShipmentTypeCode.Other, ShipmentTypeCode.Usps });
            mock.Mock<IShipmentTypeManager>().SetupGet(x => x.ShipmentTypes)
                .Returns(new List<ShipmentType> { otherShipmentType, uspsShipmentType, fedExShipmentType });

            ShipmentTypeProvider testObject = mock.Create<ShipmentTypeProvider>();

            subject.OnNext(new EnabledCarriersChangedMessage(null, new[] { ShipmentTypeCode.FedEx }, new List<ShipmentTypeCode>()));

            var carrierAdapter = mock.Mock<ICarrierShipmentAdapter>();
            carrierAdapter.SetupGet(c => c.ShipmentTypeCode).Returns(ShipmentTypeCode.Other);

            Assert.Contains(ShipmentTypeCode.FedEx, testObject.GetAvailableShipmentTypes(carrierAdapter.Object));
        }

        [Fact]
        public void Constructor_RemovesDuplicates_WhenAddedShipmentTypeAlreadyExistsInList()
        {
            mock.Mock<IShipmentTypeManager>().SetupGet(x => x.EnabledShipmentTypeCodes)
                .Returns(new List<ShipmentTypeCode> { ShipmentTypeCode.Other, ShipmentTypeCode.Usps });
            mock.Mock<IShipmentTypeManager>().SetupGet(x => x.ShipmentTypes)
                .Returns(new List<ShipmentType> { otherShipmentType, uspsShipmentType });

            ShipmentTypeProvider testObject = mock.Create<ShipmentTypeProvider>();

            subject.OnNext(new EnabledCarriersChangedMessage(null, new[] { ShipmentTypeCode.Other }, new List<ShipmentTypeCode>()));

            var carrierAdapter = mock.Mock<ICarrierShipmentAdapter>();
            carrierAdapter.SetupGet(c => c.ShipmentTypeCode).Returns(ShipmentTypeCode.Other);

            Assert.Equal(2, testObject.GetAvailableShipmentTypes(carrierAdapter.Object).Count());
        }

        [Fact]
        public void GetAvailableShipmentTypes_ReturnsOnlyAmazon_WhenShipmentTypeCodeIsAmazon()
        {
            mock.Mock<IShipmentTypeManager>().SetupGet(x => x.EnabledShipmentTypeCodes)
                .Returns(new List<ShipmentTypeCode> { ShipmentTypeCode.Other, ShipmentTypeCode.Usps, ShipmentTypeCode.Amazon });
            mock.Mock<IShipmentTypeManager>().SetupGet(x => x.ShipmentTypes)
                .Returns(new List<ShipmentType> { otherShipmentType, uspsShipmentType, amazonShipmentType });

            amazonOrder.IsPrime = (int) AmazonIsPrime.Yes;
            target.AmazonOrder = amazonOrder;

            ShipmentTypeProvider testObject = mock.Create<ShipmentTypeProvider>();

            subject.OnNext(new EnabledCarriersChangedMessage(null, new[] { ShipmentTypeCode.Other }, new List<ShipmentTypeCode>()));

            var carrierAdapter = mock.Mock<ICarrierShipmentAdapter>();
            carrierAdapter.SetupGet(c => c.ShipmentTypeCode).Returns(ShipmentTypeCode.Amazon);
            carrierAdapter.SetupGet(c => c.Shipment).Returns(shipment);

            amazonShipmentShippingPolicy.Apply(target);
            Assert.Equal(1, testObject.GetAvailableShipmentTypes(carrierAdapter.Object).Count());
            Assert.Equal(ShipmentTypeCode.Amazon, testObject.GetAvailableShipmentTypes(carrierAdapter.Object).First());
        }


        [Fact]
        public void GetAvailableShipmentTypes_ReturnsNoneAmazon_WhenShipmentTypeCodeIsNotAmazon()
        {
            mock.Mock<IShipmentTypeManager>().SetupGet(x => x.EnabledShipmentTypeCodes)
                .Returns(new List<ShipmentTypeCode> { ShipmentTypeCode.Other, ShipmentTypeCode.Usps });
            mock.Mock<IShipmentTypeManager>().SetupGet(x => x.ShipmentTypes)
                .Returns(new List<ShipmentType> { otherShipmentType, uspsShipmentType, fedExShipmentType });

            ShipmentTypeProvider testObject = mock.Create<ShipmentTypeProvider>();

            subject.OnNext(new EnabledCarriersChangedMessage(null, new[] { ShipmentTypeCode.Other }, new List<ShipmentTypeCode>()));

            var carrierAdapter = mock.Mock<ICarrierShipmentAdapter>();
            carrierAdapter.SetupGet(c => c.ShipmentTypeCode).Returns(ShipmentTypeCode.Other);

            Assert.Equal(2, testObject.GetAvailableShipmentTypes(carrierAdapter.Object).Count());
            Assert.True(testObject.GetAvailableShipmentTypes(carrierAdapter.Object).None(c => c == ShipmentTypeCode.Amazon));
        }

        [Theory]
        [InlineData(Ordertype.Amazon, ShipmentTypeCode.Amazon, RestrictionType.Hidden, AmazonIsPrime.Yes, false, false)]
        [InlineData(Ordertype.Amazon, ShipmentTypeCode.Amazon, RestrictionType.Hidden, AmazonIsPrime.No, false, true)]
        [InlineData(Ordertype.Amazon, ShipmentTypeCode.Amazon, RestrictionType.Hidden, AmazonIsPrime.Unknown, false, true)]
        [InlineData(Ordertype.GenericModule, ShipmentTypeCode.Amazon, RestrictionType.Hidden, AmazonIsPrime.Yes, false, false)]
        [InlineData(Ordertype.GenericModule, ShipmentTypeCode.Amazon, RestrictionType.Hidden, AmazonIsPrime.No, false, true)]
        [InlineData(Ordertype.GenericModule, ShipmentTypeCode.Amazon, RestrictionType.Hidden, AmazonIsPrime.Unknown, false, true)]
        [InlineData(Ordertype.BigCommerce, ShipmentTypeCode.Amazon, RestrictionType.Hidden, AmazonIsPrime.No, false, true)]
        [InlineData(Ordertype.Amazon, ShipmentTypeCode.Other, RestrictionType.Hidden, AmazonIsPrime.Yes, false, false)]
        [InlineData(Ordertype.Amazon, ShipmentTypeCode.Other, RestrictionType.Hidden, AmazonIsPrime.No, false, true)]
        [InlineData(Ordertype.Amazon, ShipmentTypeCode.Other, RestrictionType.Hidden, AmazonIsPrime.Unknown, false, true)]
        [InlineData(Ordertype.GenericModule, ShipmentTypeCode.Other, RestrictionType.Hidden, AmazonIsPrime.Yes, false, false)]
        [InlineData(Ordertype.GenericModule, ShipmentTypeCode.Other, RestrictionType.Hidden, AmazonIsPrime.No, false, true)]
        [InlineData(Ordertype.GenericModule, ShipmentTypeCode.Other, RestrictionType.Hidden, AmazonIsPrime.Unknown, false, true)]
        [InlineData(Ordertype.BigCommerce, ShipmentTypeCode.Other, RestrictionType.Hidden, AmazonIsPrime.No, false, true)]
        [InlineData(Ordertype.Amazon, ShipmentTypeCode.Amazon, RestrictionType.PrimeOnly, AmazonIsPrime.Yes, true, false)]
        [InlineData(Ordertype.Amazon, ShipmentTypeCode.Amazon, RestrictionType.PrimeOnly, AmazonIsPrime.No, false, true)]
        [InlineData(Ordertype.Amazon, ShipmentTypeCode.Amazon, RestrictionType.PrimeOnly, AmazonIsPrime.Unknown, false, true)]
        [InlineData(Ordertype.GenericModule, ShipmentTypeCode.Amazon, RestrictionType.PrimeOnly, AmazonIsPrime.Yes, true, false)]
        [InlineData(Ordertype.GenericModule, ShipmentTypeCode.Amazon, RestrictionType.PrimeOnly, AmazonIsPrime.No, false, true)]
        [InlineData(Ordertype.GenericModule, ShipmentTypeCode.Amazon, RestrictionType.PrimeOnly, AmazonIsPrime.Unknown, false, true)]
        [InlineData(Ordertype.BigCommerce, ShipmentTypeCode.Amazon, RestrictionType.PrimeOnly, AmazonIsPrime.No, false, true)]
        [InlineData(Ordertype.Amazon, ShipmentTypeCode.Other, RestrictionType.PrimeOnly, AmazonIsPrime.Yes, true, false)]
        [InlineData(Ordertype.Amazon, ShipmentTypeCode.Other, RestrictionType.PrimeOnly, AmazonIsPrime.No, false, true)]
        [InlineData(Ordertype.Amazon, ShipmentTypeCode.Other, RestrictionType.PrimeOnly, AmazonIsPrime.Unknown, false, true)]
        [InlineData(Ordertype.GenericModule, ShipmentTypeCode.Other, RestrictionType.PrimeOnly, AmazonIsPrime.Yes, true, false)]
        [InlineData(Ordertype.GenericModule, ShipmentTypeCode.Other, RestrictionType.PrimeOnly, AmazonIsPrime.No, false, true)]
        [InlineData(Ordertype.GenericModule, ShipmentTypeCode.Other, RestrictionType.PrimeOnly, AmazonIsPrime.Unknown, false, true)]
        [InlineData(Ordertype.BigCommerce, ShipmentTypeCode.Other, RestrictionType.PrimeOnly, AmazonIsPrime.No, false, true)]
        [InlineData(Ordertype.Amazon, ShipmentTypeCode.Amazon, RestrictionType.AnyAmazon, AmazonIsPrime.Yes, true, false)]
        [InlineData(Ordertype.Amazon, ShipmentTypeCode.Amazon, RestrictionType.AnyAmazon, AmazonIsPrime.No, true, true)]
        [InlineData(Ordertype.Amazon, ShipmentTypeCode.Amazon, RestrictionType.AnyAmazon, AmazonIsPrime.Unknown, true, true)]
        [InlineData(Ordertype.GenericModule, ShipmentTypeCode.Amazon, RestrictionType.AnyAmazon, AmazonIsPrime.Yes, true, false)]
        [InlineData(Ordertype.GenericModule, ShipmentTypeCode.Amazon, RestrictionType.AnyAmazon, AmazonIsPrime.No, false, true)]
        [InlineData(Ordertype.GenericModule, ShipmentTypeCode.Amazon, RestrictionType.AnyAmazon, AmazonIsPrime.Unknown, false, true)]
        [InlineData(Ordertype.BigCommerce, ShipmentTypeCode.Amazon, RestrictionType.AnyAmazon, AmazonIsPrime.No, false, true)]
        [InlineData(Ordertype.Amazon, ShipmentTypeCode.Other, RestrictionType.AnyAmazon, AmazonIsPrime.Yes, true, false)]
        [InlineData(Ordertype.Amazon, ShipmentTypeCode.Other, RestrictionType.AnyAmazon, AmazonIsPrime.No, true, true)]
        [InlineData(Ordertype.Amazon, ShipmentTypeCode.Other, RestrictionType.AnyAmazon, AmazonIsPrime.Unknown, true, true)]
        [InlineData(Ordertype.GenericModule, ShipmentTypeCode.Other, RestrictionType.AnyAmazon, AmazonIsPrime.Yes, true, false)]
        [InlineData(Ordertype.GenericModule, ShipmentTypeCode.Other, RestrictionType.AnyAmazon, AmazonIsPrime.No, false, true)]
        [InlineData(Ordertype.GenericModule, ShipmentTypeCode.Other, RestrictionType.AnyAmazon, AmazonIsPrime.Unknown, false, true)]
        [InlineData(Ordertype.BigCommerce, ShipmentTypeCode.Other, RestrictionType.AnyAmazon, AmazonIsPrime.No, false, true)]
        public void GetAvailableShipmentTypes_ReturnsCorrectValues(Ordertype orderType, ShipmentTypeCode shipmentTypeCode, 
                                                                   RestrictionType restrictionType, AmazonIsPrime isPrime, 
                                                                   bool expectedAmazonAllowed, bool otherCarriersAllowed)
        {
            List<ShipmentTypeCode> enabledShipmentTypeCodes = new List<ShipmentTypeCode> { ShipmentTypeCode.Other, ShipmentTypeCode.Usps};
            List<ShipmentType> shipmentTypes = new List<ShipmentType> {otherShipmentType, uspsShipmentType};

            if (restrictionType != RestrictionType.Hidden)
            {
                shipmentTypes.Add(amazonShipmentType);
            }
            enabledShipmentTypeCodes.Add(ShipmentTypeCode.Amazon);

            mock.Mock<IShipmentTypeManager>().SetupGet(x => x.EnabledShipmentTypeCodes)
                .Returns(enabledShipmentTypeCodes);
            mock.Mock<IShipmentTypeManager>().SetupGet(x => x.ShipmentTypes)
                .Returns(shipmentTypes);


            OrderEntity orderToTest = (orderType == Ordertype.Amazon || orderType == Ordertype.GenericModule && isPrime == AmazonIsPrime.Yes) ? amazonOrder : nonAmazonOrder;

            amazonOrder.IsPrime = (int) isPrime;
            shipment = new ShipmentEntity
            {
                Order = orderToTest,
                ShipmentTypeCode = shipmentTypeCode
            };

            target = new AmazonPrimeShippingPolicyTarget()
            {
                Shipment = shipment,
                Allowed = false,
                AmazonOrder = orderToTest as IAmazonOrder,
                AmazonCredentials = orderToTest.Store as IAmazonCredentials
            };

            amazonShipmentShippingPolicy = new AmazonShipmentShippingPolicy();
            if (restrictionType != RestrictionType.Hidden)
            {
                amazonShipmentShippingPolicy.Configure(restrictionType == RestrictionType.PrimeOnly ? "1" : "2");
            }
            amazonShipmentShippingPolicy.Apply(target);

            license
                .Setup(l => l.ApplyShippingPolicy(ShipmentTypeCode.Amazon, It.IsAny<object>()))
                .Callback((ShipmentTypeCode s, object t) => ((AmazonPrimeShippingPolicyTarget) t).Allowed = target.Allowed);

            ShipmentTypeProvider testObject = mock.Create<ShipmentTypeProvider>();

            var carrierAdapter = mock.Mock<ICarrierShipmentAdapter>();
            carrierAdapter.SetupGet(c => c.ShipmentTypeCode).Returns(shipmentTypeCode);
            carrierAdapter.SetupGet(c => c.Shipment).Returns(shipment);

            var availableTypes = testObject.GetAvailableShipmentTypes(carrierAdapter.Object);

            Assert.Equal(expectedAmazonAllowed, availableTypes.Any(c => c == ShipmentTypeCode.Amazon));
            Assert.Equal(otherCarriersAllowed, availableTypes.Count() > 1);
        }

        public enum Ordertype
        {
            Amazon = 0,
            GenericModule = 1,
            BigCommerce = 2
        }

        public enum RestrictionType
        {
            Hidden = 0,
            PrimeOnly = 1,
            AnyAmazon = 2
        }

        public void Dispose()
        {
            mock.Dispose();
            subject.Dispose();
        }
    }
}
