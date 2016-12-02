﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Services.ShipmentProcessorSteps.LabelRetrieval;
using ShipWorks.Shipping.Settings;
using ShipWorks.Startup;
using ShipWorks.Stores;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests.Services
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class ShipmentProcessorTest : IDisposable
    {
        private ShipmentProcessor testObject;
        private readonly DataContext context;
        private readonly ShipmentEntity shipment;

        public ShipmentProcessorTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));

            context.Mock.SetupDefaultMocksForEnumerable<ILabelRetrievalShipmentValidator>(item =>
                item.Setup(x => x.Validate(It.IsAny<ShipmentEntity>())).Returns(Result.FromSuccess()));

            context.Mock.SetupDefaultMocksForEnumerable<ILabelRetrievalShipmentManipulator>(item =>
                item.Setup(x => x.Manipulate(It.IsAny<ShipmentEntity>())).Returns((ShipmentEntity s) => s));

            context.Mock.Provide<Control>(new Control());
            context.Mock.Provide<Func<Control>>(() => new Control());
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
            settings.ConfiguredTypes = new[] { ShipmentTypeCode.Other };
            settings.ActivatedTypes = new[] { ShipmentTypeCode.Other };
            ShippingSettings.Save(settings);

            ShippingSettings.MarkAsConfigured(ShipmentTypeCode.Other);

            shipment = Create.Shipment(context.Order)
                .AsOther()
                .Save();
        }

        [Fact]
        public async Task Process_ReturnsErrorResult_WhenShipmentCannotBeProcessed()
        {
            testObject = context.Mock.Create<ShipmentProcessor>();

            var result = await testObject.Process(new[] { shipment },
                context.Mock.Create<ICarrierConfigurationShipmentRefresher>(),
                null, null);

            Assert.Equal(1, result.Count());
            Assert.False(result.Single().IsSuccessful);
            Assert.NotNull(result.Single().Error);
        }

        [Fact]
        public async Task Process_DisplaysMessage_WhenShipmentCannotBeProcessed()
        {
            testObject = context.Mock.Create<ShipmentProcessor>();

            await testObject.Process(new[] { shipment },
                context.Mock.Create<ICarrierConfigurationShipmentRefresher>(),
                null, null);

            context.Mock.Mock<IMessageHelper>()
                .Verify(x => x.ShowError(It.IsAny<string>()));
        }

        [Fact]
        public async Task Process_ReturnsSuccessResult_WhenShipmentIsProcessed()
        {
            Modify.Entity(shipment.Other)
                .Set(x => x.Carrier, "Foo")
                .Set(x => x.Service, "Bar")
                .Save();

            testObject = context.Mock.Create<ShipmentProcessor>();

            var result = await testObject.Process(new[] { shipment },
                context.Mock.Create<ICarrierConfigurationShipmentRefresher>(),
                null, null);

            Assert.Equal(1, result.Count());
            Assert.True(result.Single().IsSuccessful);
            Assert.Null(result.Single().Error);
        }

        [Fact]
        public async Task Process_MarksShipmentCopyAsProcessed_WhenShipmentIsProcessed()
        {
            context.Mock.Override<IDateTimeProvider>()
                .Setup(x => x.UtcNow).Returns(new DateTime(2016, 1, 20, 10, 30, 0));

            Modify.Entity(shipment.Other)
                .Set(x => x.Carrier, "Foo")
                .Set(x => x.Service, "Bar")
                .Save();

            testObject = context.Mock.Create<ShipmentProcessor>();

            var result = await testObject.Process(new[] { shipment },
                context.Mock.Create<ICarrierConfigurationShipmentRefresher>(),
                null, null);

            Assert.True(result.Single().Shipment.Processed);
            Assert.Equal(new DateTime(2016, 1, 20, 10, 30, 0), result.Single().Shipment.ProcessedDate);
            Assert.Equal(context.User.UserID, result.Single().Shipment.ProcessedUserID);
            Assert.Equal(context.Computer.ComputerID, result.Single().Shipment.ProcessedComputerID);
        }

        [Fact]
        public async Task Process_ShipmentIsSaved_WhenShipmentIsProcessed()
        {
            context.Mock.Override<IDateTimeProvider>()
                .Setup(x => x.UtcNow).Returns(new DateTime(2016, 1, 20, 10, 30, 0));

            Modify.Entity(shipment.Other)
                .Set(x => x.Carrier, "Foo")
                .Set(x => x.Service, "Bar")
                .Save();

            testObject = context.Mock.Create<ShipmentProcessor>();

            await testObject.Process(new[] { shipment },
                context.Mock.Create<ICarrierConfigurationShipmentRefresher>(),
                null, null);

            using (SqlAdapter adapter = SqlAdapter.Create(false))
            {
                var loadedShipment = new ShipmentEntity(shipment.ShipmentID);
                adapter.FetchEntity(loadedShipment);

                Assert.True(loadedShipment.Processed);
                Assert.Equal(new DateTime(2016, 1, 20, 10, 30, 0), loadedShipment.ProcessedDate);
                Assert.Equal(context.User.UserID, loadedShipment.ProcessedUserID);
                Assert.Equal(context.Computer.ComputerID, loadedShipment.ProcessedComputerID);
            }
        }

        [Fact]
        public async Task Process_DoesNotModifyOriginalShipment_WhenShipmentIsProcessed()
        {
            Modify.Entity(shipment.Other)
                .Set(x => x.Carrier, "Foo")
                .Set(x => x.Service, "Bar")
                .Save();

            testObject = context.Mock.Create<ShipmentProcessor>();

            await testObject.Process(new[] { shipment },
                context.Mock.Create<ICarrierConfigurationShipmentRefresher>(),
                null, null);

            Assert.False(shipment.Processed);
        }

        public void Dispose()
        {
            context?.Dispose();
        }
    }
}
