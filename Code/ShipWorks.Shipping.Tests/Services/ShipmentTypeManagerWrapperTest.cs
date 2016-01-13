using System;
using System.Linq;
using Autofac.Extras.Moq;
using Autofac.Features.Indexed;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Settings;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Services
{
    public class ShipmentTypeManagerWrapperTest : IDisposable
    {
        readonly AutoMock mock;
        readonly IShipmentTypeManager testObject;
        readonly ShippingSettingsEntity shippingSettings;

        public ShipmentTypeManagerWrapperTest()
        {
            shippingSettings = new ShippingSettingsEntity();

            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            mock.Override<IIndex<ShipmentTypeCode, ShipmentType>>();
            mock.Mock<IShippingSettings>().Setup(x => x.Fetch()).Returns(shippingSettings);

            testObject = mock.Create<ShipmentTypeManagerWrapper>();
        }

        [Fact]
        public void ShipmentTypesSupportingAccounts_AreCorrect_Test()
        {
            Assert.False(testObject.ShipmentTypesSupportingAccounts.Contains(ShipmentTypeCode.BestRate));
            Assert.False(testObject.ShipmentTypesSupportingAccounts.Contains(ShipmentTypeCode.Other));
            Assert.False(testObject.ShipmentTypesSupportingAccounts.Contains(ShipmentTypeCode.PostalWebTools));
            Assert.False(testObject.ShipmentTypesSupportingAccounts.Contains(ShipmentTypeCode.None));

            Assert.True(testObject.ShipmentTypesSupportingAccounts.Contains(ShipmentTypeCode.OnTrac));
            Assert.True(testObject.ShipmentTypesSupportingAccounts.Contains(ShipmentTypeCode.Endicia));
            Assert.True(testObject.ShipmentTypesSupportingAccounts.Contains(ShipmentTypeCode.Express1Endicia));
            Assert.True(testObject.ShipmentTypesSupportingAccounts.Contains(ShipmentTypeCode.Express1Usps));
            Assert.True(testObject.ShipmentTypesSupportingAccounts.Contains(ShipmentTypeCode.FedEx));
            Assert.True(testObject.ShipmentTypesSupportingAccounts.Contains(ShipmentTypeCode.UpsOnLineTools));
            Assert.True(testObject.ShipmentTypesSupportingAccounts.Contains(ShipmentTypeCode.UpsWorldShip));
            Assert.True(testObject.ShipmentTypesSupportingAccounts.Contains(ShipmentTypeCode.Usps));
            Assert.True(testObject.ShipmentTypesSupportingAccounts.Contains(ShipmentTypeCode.iParcel));
        }

        [Theory]
        [InlineData(ShipmentTypeCode.Usps)]
        [InlineData(ShipmentTypeCode.None)]
        public void InitialShipmentType_ReturnsDefaultFromShippingSettings_WhenNoRulesExist(ShipmentTypeCode code)
        {
            ShipmentEntity shipment = new ShipmentEntity();
            shippingSettings.DefaultType = (int) code;
            Mock<ShipmentType> shipmentType = mock.CreateMock<ShipmentType>();
            shipmentType.Setup(x => x.IsAllowedFor(shipment)).Returns(true);

            mock.Mock<IIndex<ShipmentTypeCode, ShipmentType>>().Setup(x => x[code]).Returns(shipmentType.Object);

            ShipmentType type = testObject.InitialShipmentType(shipment);

            Assert.Same(shipmentType.Object, type);
        }

        [Fact]
        public void InitialShipmentType_ReturnsNoneShipmentType_WhenShipmentIsNotAllowedForShipmentType()
        {
            shippingSettings.DefaultType = (int) ShipmentTypeCode.Usps;
            var shipment = new ShipmentEntity();

            var shipmentType = mock.CreateMock<ShipmentType>();
            shipmentType.Setup(x => x.IsAllowedFor(shipment)).Returns(false);

            testObject.InitialShipmentType(shipment);

            mock.Mock<IIndex<ShipmentTypeCode, ShipmentType>>()
                .Verify(x => x[ShipmentTypeCode.None]);
        }

        [Fact]
        public void InitialShipmentType_DelegatesToFilterHelper_WithEachRule()
        {
            var rule1 = new ShippingProviderRuleEntity();
            var rule2 = new ShippingProviderRuleEntity();
            var rule3 = new ShippingProviderRuleEntity();

            mock.Mock<IShippingProviderRuleManager>()
                .Setup(x => x.GetRules())
                .Returns(new[] { rule1, rule2, rule3 });

            testObject.InitialShipmentType(new ShipmentEntity { OrderID = 6 });

            mock.Mock<IFilterHelper>().Verify(x => x.IsObjectInFilterContent(6, rule1));
            mock.Mock<IFilterHelper>().Verify(x => x.IsObjectInFilterContent(6, rule2));
            mock.Mock<IFilterHelper>().Verify(x => x.IsObjectInFilterContent(6, rule3));
        }

        [Fact]
        public void InitialShipmentType_SetsInitialShipmentTypeToLastMatch_WhenShipmentIsInRule()
        {
            var rule1 = new ShippingProviderRuleEntity { ShipmentType = (int) ShipmentTypeCode.FedEx };
            var rule2 = new ShippingProviderRuleEntity { ShipmentType = (int) ShipmentTypeCode.Amazon };

            mock.Mock<IShippingProviderRuleManager>()
                .Setup(x => x.GetRules())
                .Returns(new[] { new ShippingProviderRuleEntity { FilterNodeID = 12 }, rule1, rule2 });

            var helper = mock.Mock<IFilterHelper>();
            helper.Setup(x => x.IsObjectInFilterContent(6, rule1)).Returns(true);
            helper.Setup(x => x.IsObjectInFilterContent(6, rule2)).Returns(true);

            testObject.InitialShipmentType(new ShipmentEntity { OrderID = 6 });

            mock.Mock<IIndex<ShipmentTypeCode, ShipmentType>>().Verify(x => x[ShipmentTypeCode.Amazon]);
        }

        public void Dispose() => mock.Dispose();
    }
}
