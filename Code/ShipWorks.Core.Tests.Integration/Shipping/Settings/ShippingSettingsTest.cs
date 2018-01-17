using ShipWorks.Data.Connection;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Settings;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShipWorks.Core.Tests.Integration.Shipping.Settings
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class ShippingSettingsTest : IDisposable
    {
        private readonly DataContext context;
        private readonly SqlAdapter adapter;

        public ShippingSettingsTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));

            adapter = new SqlAdapter(false);            
        }

        [Theory]
        [InlineData(ShipmentTypeCode.Amazon)]
        [InlineData(ShipmentTypeCode.DhlExpress)]
        [InlineData(ShipmentTypeCode.Endicia)]
        public void MarkAsConfigured_SetsCarrierAsDefault_IfNoOtherCarrierIsConfigured(ShipmentTypeCode carrierToConfigure)
        {
            var settings = ShippingSettings.Fetch();
            settings.ConfiguredTypes = new List<ShipmentTypeCode>() { ShipmentTypeCode.BestRate };
            settings.DefaultType = (int) ShipmentTypeCode.None;
            ShippingSettings.Save(settings);

            ShippingSettings.MarkAsConfigured(carrierToConfigure);

            settings = ShippingSettings.Fetch();

            Assert.Equal((int) carrierToConfigure, settings.DefaultType);
        }

        [Theory]
        [InlineData(ShipmentTypeCode.UpsOnLineTools, ShipmentTypeCode.Usps)]
        [InlineData(ShipmentTypeCode.UpsWorldShip, ShipmentTypeCode.Usps)]
        [InlineData(ShipmentTypeCode.Usps, ShipmentTypeCode.DhlExpress)]
        public void MarkAsConfigured_DoesNotSetCarrierAsDefault_IfOtherCarrierIsConfigured(ShipmentTypeCode carrierToConfigure, ShipmentTypeCode alreadyConfiguredCarrier)
        {
            var settings = ShippingSettings.Fetch();
            settings.ConfiguredTypes = new List<ShipmentTypeCode>() { alreadyConfiguredCarrier };
            settings.DefaultType = (int) ShipmentTypeCode.None;
            ShippingSettings.Save(settings);

            ShippingSettings.MarkAsConfigured(carrierToConfigure);

            settings = ShippingSettings.Fetch();

            Assert.Equal((int) ShipmentTypeCode.None, settings.DefaultType);
        }

        [Theory]
        [InlineData(ShipmentTypeCode.Amazon)]
        [InlineData(ShipmentTypeCode.DhlExpress)]
        [InlineData(ShipmentTypeCode.Endicia)]
        public void MarkAsConfigured_DoesNotSetCarrierAsDefault_IfNoneNotDefault(ShipmentTypeCode carrierToConfigure)
        {
            var settings = ShippingSettings.Fetch();
            settings.ConfiguredTypes = new List<ShipmentTypeCode>() { ShipmentTypeCode.BestRate };
            settings.DefaultType = (int) ShipmentTypeCode.Usps;
            ShippingSettings.Save(settings);

            ShippingSettings.MarkAsConfigured(carrierToConfigure);

            settings = ShippingSettings.Fetch();

            Assert.Equal((int) ShipmentTypeCode.Usps, settings.DefaultType);
        }

        public void Dispose()
        {
            adapter.Dispose();
            context.Dispose();
        }
    }
}
