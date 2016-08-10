using System;
using Moq;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.None;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Express1;
using ShipWorks.Shipping.Carriers.Postal.WebTools;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;
using ShipWorks.Shipping.Editing;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using ShipWorks.UI.Controls;
using ShipWorks.Users;
using Xunit;

namespace ShipWorks.Core.Tests.Integration.Shipping
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class CustomsControlCacheTest : IDisposable
    {
        private readonly DataContext context;
        private readonly ShipmentEntity shipment;
        private readonly SqlAdapter adapter;
        private readonly CustomsControlCache customsControlCache;

        public CustomsControlCacheTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));

            shipment = Create.Shipment(context.Order)
                .AsOther()
                .Set(x => x.ShipCountryCode, "UK")
                .Save();

            adapter = new SqlAdapter(false);

            UserSettingsEntity userSettings = new UserSettingsEntity();
            userSettings.ShippingWeightFormat = (int)WeightDisplayFormat.FractionalPounds;

            UserEntity user = new UserEntity();
            user.Settings = userSettings;

            Mock<IUserSession> session = new Mock<IUserSession>();
            session.Setup(s => s.User).Returns(user);

            context.Mock.Provide<IUserSession>(session.Object);

            customsControlCache = new CustomsControlCache();
        }

        [Fact]
        public void Get_ReturnsPostalCustomsControl_WhenShipmentTypeIsUsps()
        {
            CustomsControlBase customsControl = customsControlCache.Get(new UspsShipmentType());

            Assert.True(customsControl is PostalCustomsControl);
        }

        [Fact]
        public void Get_ReturnsPostalCustomsControl_WhenShipmentTypeIsExpress1Usps()
        {
            CustomsControlBase customsControl = customsControlCache.Get(new Express1UspsShipmentType());

            Assert.True(customsControl is PostalCustomsControl);
        }

        [Fact]
        public void Get_ReturnsPostalCustomsControl_WhenShipmentTypeIsEndicia()
        {
            CustomsControlBase customsControl = customsControlCache.Get(new EndiciaShipmentType());

            Assert.True(customsControl is PostalCustomsControl);
        }

        [Fact]
        public void Get_ReturnsPostalCustomsControl_WhenShipmentTypeIsExpress1Endicia()
        {
            CustomsControlBase customsControl = customsControlCache.Get(new Express1EndiciaShipmentType());

            Assert.True(customsControl is PostalCustomsControl);
        }

        [Fact]
        public void Get_ReturnsPostalCustomsControl_WhenShipmentTypeIsPostalWebTools()
        {
            CustomsControlBase customsControl = customsControlCache.Get(new PostalWebShipmentType());

            Assert.True(customsControl is PostalCustomsControl);
        }

        [Fact]
        public void Get_ReturnsFedExCustomsControl_WhenShipmentTypeIsFedEx()
        {
            CustomsControlBase customsControl = customsControlCache.Get(new FedExShipmentType());

            Assert.True(customsControl is FedExCustomsControl);
        }

        [Fact]
        public void Get_ReturnsUpsCustomsControl_WhenShipmentTypeIsUpsOnLineTools()
        {
            CustomsControlBase customsControl = customsControlCache.Get(new UpsOltShipmentType());

            Assert.True(customsControl is UpsCustomsControl);
        }

        [Fact]
        public void Get_ReturnsUpsCustomsControl_WhenShipmentTypeIsUpsWorldShip()
        {
            CustomsControlBase customsControl = customsControlCache.Get(new WorldShipShipmentType());

            Assert.True(customsControl is UpsCustomsControl);
        }

        [Fact]
        public void Get_ReturnsCustomsControlBase_WhenShipmentTypeIsNull()
        {
            CustomsControlBase customsControl = customsControlCache.Get(null);

            Assert.True(customsControl != null);
            Assert.False(customsControl is UpsCustomsControl);
            Assert.False(customsControl is FedExCustomsControl);
            Assert.False(customsControl is PostalCustomsControl);
        }

        [Fact]
        public void Get_ReturnsCustomsControlBase_WhenShipmentTypeIsNone()
        {
            CustomsControlBase customsControl = customsControlCache.Get(new NoneShipmentType());

            Assert.True(customsControl != null);
            Assert.False(customsControl is UpsCustomsControl);
            Assert.False(customsControl is FedExCustomsControl);
            Assert.False(customsControl is PostalCustomsControl);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            adapter.Dispose();
            context.Dispose();
        }
    }
}
