using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.UI.ShippingPanel;
using ShipWorks.Shipping.UI.ShippingPanel.ShipmentControl;
using ShipWorks.Startup;
using ShipWorks.Stores;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests.Integration.Services
{
    [SuppressMessage("SonarQube", "S3215: \"interface\" instances should not be cast to concrete types",
        Justification = "We're casting to test results")]
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class ShippingPanelViewModelTest : IDisposable
    {
        private ShippingPanelViewModel testObject;
        private readonly DataContext context;
        private readonly ShipmentEntity shipment;
        private IDisposable subscription;

        public ShippingPanelViewModelTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x),
                mock =>
                {
                    mock.Provide<ISchedulerProvider>(new ImmediateSchedulerProvider());
                    mock.Provide<Control>(new Control());
                    mock.Provide<Func<Control>>(() => new Control());
                });

            context.Mock.Override<ITangoWebClient>();
            context.Mock.Override<IMessageHelper>();

            Modify.Store(context.Store)
                .Set(x => x.Enabled, true)
                .Set(x => x.StoreTypeCode, StoreTypeCode.GenericModule)
                .Set(x => x.License, "I5TKE-5RXP3-NGMEN-ZXHMX-GENERIC-BRIAN@INTERAPPTIVE.COM")
                .Set(x => x.Edition, "QTGyeHaEih1ldH2CBYvYjcUj4YRmteV6oXBCl0s7SwjZ+DtjOHT3JD1Uit/x3sF65o4/EnBuifBA6H1hodXIbIgPMmDQTwXiTIcZj3+53Of8ygIUOvKgurrLmicPNHEmyvwwGLtRRGpkygBh0KCqwVazlsaQ0zFBh34mBLhF3TbVKg8ZYKNzwBIcnXPw1iBLNY3JuuJOd1JOXeC86DGf7ZGlZ5lwQF26Z29Mt6uexBtjAHQup4AX4ORKdjqldEfmiqyh+80AcpMhRPQVeB9gTrWzmVmD+AKuwmdI7j5GrxcKc+1Mmh150RfOhgj8NyR9YKGtbHrrih5D4IuqXUX+BwpuNN5ZjPcOmUrQrjiKP37OlaEdBPRzl5UflPXqBOfSe5iCU/LDKQSbqoxoLu/uC8G/gjvMPavdhAyzOQWwHDOcSNIdIf7QBpPBAkcwucIxcpdRCIgeyc76Tcar7Oc7A+AjjfK/mEZty0ORTDi7WO5k4fPygn5ZK0fbV7D6HF1Rj7rZ0WkHV2zLeSro7ZGIuyz1GN6PMS1uK9cTR/Dm7P/WNeUn9aJ5JaOmqnXOzG+RvG/jrlhc126R5wFg/X/kvfkf9oHn4h72UkLSL3wIj8kiARB8r65qCcw0G0McqXs9WACrQjI+UT12/pZrde8M+D7BvoirfH4GOqEzj7JI8weXiPR62ZzdF4WQ7bYKN/RxLb2KQdH2MMUuU2zSV3Xs/VWGKnmUIXdn7An4pMjhm2WiJLdnQXUjfsHdvOVYTPbZwyFI5vZGX3lDhn4Figoog3potyb+r2HeIJOz2h0NAYJThWhfnQOPqMgc2imFTTjvNnLsVtf0x2dWNECBfY3K0UNC31czVHYJKlxWUS7YqPRP1VtRnPFV4WOfb1WfNC7cQGnpYWZZwItv/8JtINa1J9JxxFKWRGuBZWpZax25M7f4Bd2Ndiil9Rg4Nu+TvpGo5DZ+4yKcGwJOhrnPKVWgEe/xPM5RDNl6lMwZrkMJ/QXebTv3NvY2G97LNWG1SxJc/ywoo8exLWqULu+fbZmysFGiH2Tg2qsN+IjQ/DTh6qPMb40q2Ejl5fARyg++3nP5+bWiDy4vJwmHWX84Sw1XUettUTWmcCEAuuDcuDYrgupkQI0FiRRvu/vq+ZFopJ+TE9lV3lDqA63azhwyboSI9WSSmrjtNqhio28utmp38ohq0WUmwfHN0vNY8JP21vA1g33Jb2t7WG6D+1P26GE9XA/CszrOlVNxF0zd5N7kv1Y88FHl4X5actJh/5sd1pxWrlvN0N6F+dRleaJX7Cua4bRVIJXJW9oN/pnPSUNviY2YFEL89Fbxx3bqvJd44a10Bz25HVTC6ib1QRWiPLruAsDOeAYOuOxz")
                .Save();

            Create.Profile().AsPrimary().AsOther().Save();

            var settings = ShippingSettings.Fetch();
            settings.ConfiguredTypes = new[] { (int) ShipmentTypeCode.Other };
            settings.ActivatedTypes = new[] { (int) ShipmentTypeCode.Other };
            ShippingSettings.Save(settings);

            ShippingSettings.MarkAsConfigured(ShipmentTypeCode.Other);

            shipment = Create.Shipment(context.Order)
                .AsOther()
                .Save();
        }

        [Fact]
        public async Task CreateLabel_ReloadsShipment_WhenProcessingFails()
        {
            testObject = context.Mock.Create<ShippingPanelViewModel>();
            var source = new TaskCompletionSource<ShipmentsProcessedMessage>();

            subscription = Messenger.Current.OfType<ShipmentsProcessedMessage>()
                .Subscribe(x => source.SetResult(x));

            testObject.Populate(context.Mock.Create<CarrierShipmentAdapterFactory>().Get(shipment));
            testObject.CreateLabelCommand.Execute(null);

            var message = await source.Task;

            Assert.Equal(testObject.ShipmentAdapter.Shipment.RowVersion,
                message.Shipments.FirstOrDefault().Shipment.RowVersion);
        }

        [Fact]
        public void Populate_DoesNotModifyShipmentDestination_WhenPreviousShipmentWasLoaded()
        {
            testObject = context.Mock.Create<ShippingPanelViewModel>();
            testObject.Destination.StateProvCode = "Missouri";

            Modify.Shipment(shipment).Set(x => x.ShipStateProvCode, "MO").Save();

            testObject.Populate(context.Mock.Create<CarrierShipmentAdapterFactory>().Get(shipment));

            Assert.Equal("MO", shipment.ShipStateProvCode);
        }

        [Fact]
        public void Populate_DoesNotModifyShipmentOrigin_WhenPreviousShipmentWasLoaded()
        {
            testObject = context.Mock.Create<ShippingPanelViewModel>();
            testObject.Origin.StateProvCode = "Missouri";

            Modify.Shipment(shipment).Set(x => x.OriginStateProvCode, "MO").Save();

            testObject.Populate(context.Mock.Create<CarrierShipmentAdapterFactory>().Get(shipment));

            Assert.Equal("MO", shipment.OriginStateProvCode);
        }

        [Fact]
        public void Populate_LoadsCustoms_WhenShipmentIsInternational()
        {
            testObject = context.Mock.Create<ShippingPanelViewModel>();

            Modify.Shipment(shipment).AsPostal(x => x.AsUsps())
                .WithCustomsItem()
                .Set(x => x.OriginCountryCode, "US")
                .Set(x => x.ShipCountryCode, "UK")
                .Save();

            testObject.Populate(context.Mock.Create<CarrierShipmentAdapterFactory>().Get(shipment));

            Assert.Equal(1, (testObject.ShipmentViewModel as ShipmentViewModel).CustomsItems.Count);
        }

        [Fact]
        public void LoadsCustoms_WhenShipmentSwitchesFromDomesticToInternational()
        {
            testObject = context.Mock.Create<ShippingPanelViewModel>();

            Modify.Shipment(shipment).AsPostal(x => x.AsUsps())
                .WithCustomsItem()
                .Set(x => x.OriginCountryCode, "US")
                .Set(x => x.ShipCountryCode, "US")
                .Save();

            testObject.Populate(context.Mock.Create<CarrierShipmentAdapterFactory>().Get(shipment));

            Assert.Null((testObject.ShipmentViewModel as ShipmentViewModel).CustomsItems);

            testObject.Destination.CountryCode = "UK";

            Assert.Equal(1, (testObject.ShipmentViewModel as ShipmentViewModel).CustomsItems.Count);
        }

        public void Dispose()
        {
            subscription?.Dispose();
        }
    }
}
