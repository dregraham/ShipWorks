using System;
using Autofac;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Stores;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using ShipWorks.Tests.Shared.ExtensionMethods;
using Xunit;

namespace ShipWorks.Core.Tests.Integration.ApplicationCore.Licensing
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class StoreLicenseTest
    {
        private readonly DataContext context;
        private StoreLicense testObject;
        private Mock<IMessenger> messenger;
        private Mock<IDateTimeProvider> dateTimeProvider;

        public StoreLicenseTest(DatabaseFixture db)
        {
            context = db.CreateDataContext((mock, builder) =>
            {
                messenger = builder.RegisterMock<IMessenger>(mock);
                dateTimeProvider = builder.RegisterMock<IDateTimeProvider>(mock);
                builder.RegisterMock<ITangoWebClient>(mock);
            });

            Modify.Store(context.Store)
                .Set(x => x.Enabled, true)
                .Set(x => x.StoreTypeCode, StoreTypeCode.GenericModule)
                .Set(x => x.License, "I5TKE-5RXP3-NGMEN-ZXHMX-GENERIC-BRIAN@INTERAPPTIVE.COM")
                .Set(x => x.Edition, "QTGyeHaEih1ldH2CBYvYjcUj4YRmteV6oXBCl0s7SwjZ+DtjOHT3JD1Uit/x3sF65o4/EnBuifBA6H1hodXIbIgPMmDQTwXiTIcZj3+53Of8ygIUOvKgurrLmicPNHEmyvwwGLtRRGpkygBh0KCqwVazlsaQ0zFBh34mBLhF3TbVKg8ZYKNzwBIcnXPw1iBLNY3JuuJOd1JOXeC86DGf7ZGlZ5lwQF26Z29Mt6uexBtjAHQup4AX4ORKdjqldEfmiqyh+80AcpMhRPQVeB9gTrWzmVmD+AKuwmdI7j5GrxcKc+1Mmh150RfOhgj8NyR9YKGtbHrrih5D4IuqXUX+BwpuNN5ZjPcOmUrQrjiKP37OlaEdBPRzl5UflPXqBOfSe5iCU/LDKQSbqoxoLu/uC8G/gjvMPavdhAyzOQWwHDOcSNIdIf7QBpPBAkcwucIxcpdRCIgeyc76Tcar7Oc7A+AjjfK/mEZty0ORTDi7WO5k4fPygn5ZK0fbV7D6HF1Rj7rZ0WkHV2zLeSro7ZGIuyz1GN6PMS1uK9cTR/Dm7P/WNeUn9aJ5JaOmqnXOzG+RvG/jrlhc126R5wFg/X/kvfkf9oHn4h72UkLSL3wIj8kiARB8r65qCcw0G0McqXs9WACrQjI+UT12/pZrde8M+D7BvoirfH4GOqEzj7JI8weXiPR62ZzdF4WQ7bYKN/RxLb2KQdH2MMUuU2zSV3Xs/VWGKnmUIXdn7An4pMjhm2WiJLdnQXUjfsHdvOVYTPbZwyFI5vZGX3lDhn4Figoog3potyb+r2HeIJOz2h0NAYJThWhfnQOPqMgc2imFTTjvNnLsVtf0x2dWNECBfY3K0UNC31czVHYJKlxWUS7YqPRP1VtRnPFV4WOfb1WfNC7cQGnpYWZZwItv/8JtINa1J9JxxFKWRGuBZWpZax25M7f4Bd2Ndiil9Rg4Nu+TvpGo5DZ+4yKcGwJOhrnPKVWgEe/xPM5RDNl6lMwZrkMJ/QXebTv3NvY2G97LNWG1SxJc/ywoo8exLWqULu+fbZmysFGiH2Tg2qsN+IjQ/DTh6qPMb40q2Ejl5fARyg++3nP5+bWiDy4vJwmHWX84Sw1XUettUTWmcCEAuuDcuDYrgupkQI0FiRRvu/vq+ZFopJ+TE9lV3lDqA63azhwyboSI9WSSmrjtNqhio28utmp38ohq0WUmwfHN0vNY8JP21vA1g33Jb2t7WG6D+1P26GE9XA/CszrOlVNxF0zd5N7kv1Y88FHl4X5actJh/5sd1pxWrlvN0N6F+dRleaJX7Cua4bRVIJXJW9oN/pnPSUNviY2YFEL89Fbxx3bqvJd44a10Bz25HVTC6ib1QRWiPLruAsDOeAYOuOxz")
                .Save();
        }

        [Fact]
        public void EnforceCapabilities_Refresh_MakesTangoCallFirstTimeAndSecondTime_WhenNextFireDatePassed()
        {
            dateTimeProvider.Setup(d => d.UtcNow).Returns(new DateTime(2018, 1, 1, 1, 0, 0));
            testObject = context.Mock.Create<StoreLicense>(new TypedParameter(typeof(StoreEntity), context.Store), new TypedParameter(typeof(IDateTimeProvider), dateTimeProvider.Object));

            testObject.Refresh();

            context.Mock.Mock<IMessenger>().Verify(m => m.Send(It.IsAny<EnabledCarriersChangedMessage>(), It.IsAny<string>()), Times.Once);

            context.Mock.Mock<IMessenger>().ResetCalls();

            dateTimeProvider.Setup(d => d.UtcNow).Returns(new DateTime(2018, 2, 1, 1, 0, 0));
            testObject = context.Mock.Create<StoreLicense>(new TypedParameter(typeof(StoreEntity), context.Store), new TypedParameter(typeof(IDateTimeProvider), dateTimeProvider.Object));

            testObject.Refresh();

            context.Mock.Mock<IMessenger>().Verify(m => m.Send(It.IsAny<EnabledCarriersChangedMessage>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void EnforceCapabilities_Refresh_MakesTangoCallFirstTimeAndNotSecondTime_WhenNextFireDateHasNotPassed()
        {
            dateTimeProvider.Setup(d => d.UtcNow).Returns(new DateTime(2018, 2, 1, 1, 0, 0));
            testObject = context.Mock.Create<StoreLicense>(new TypedParameter(typeof(StoreEntity), context.Store), new TypedParameter(typeof(IDateTimeProvider), dateTimeProvider.Object));

            testObject.Refresh();

            context.Mock.Mock<IMessenger>().Verify(m => m.Send(It.IsAny<EnabledCarriersChangedMessage>(), It.IsAny<string>()), Times.Once);

            context.Mock.Mock<IMessenger>().ResetCalls();

            dateTimeProvider.Setup(d => d.UtcNow).Returns(new DateTime(2018, 1, 1, 1, 0, 0));
            testObject = context.Mock.Create<StoreLicense>(new TypedParameter(typeof(StoreEntity), context.Store), new TypedParameter(typeof(IDateTimeProvider), dateTimeProvider.Object));

            testObject.Refresh();

            context.Mock.Mock<IMessenger>().Verify(m => m.Send(It.IsAny<EnabledCarriersChangedMessage>(), It.IsAny<string>()), Times.Never);
        }
    }
}