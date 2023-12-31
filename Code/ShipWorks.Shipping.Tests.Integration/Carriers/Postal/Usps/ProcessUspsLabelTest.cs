﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Interapptive.Shared.Business;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.Actions;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;
using ShipWorks.Shipping.Settings;
using ShipWorks.Stores;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Carriers.Postal.Usps;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using ShipWorks.Tests.Shared.ExtensionMethods;
using Xunit;

namespace ShipWorks.Shipping.Tests.Integration.Carriers.Postal.Usps
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class ProcessUspsLabelTest : IDisposable
    {
        private readonly DataContext context;
        private readonly ShipmentEntity shipment;
        private Mock<IUspsWebServiceFactory> webServiceFactory;
        private readonly UspsAccountEntity account;
        private readonly CreateIndiciumResult defaultResponse = new CreateIndiciumResult
        {
            TrackingNumber = string.Empty,
            Rate = new RateV46(),
            ImageData = new[] { Convert.FromBase64String("R0lGODlhAQABAIAAAP///wAAACH5BAEAAAAALAAAAAABAAEAAAICRAEAOw==") },
        };

        public ProcessUspsLabelTest(DatabaseFixture db)
        {
            context = db.CreateDataContext((mock, builder) =>
            {
                webServiceFactory = builder.RegisterMock<IUspsWebServiceFactory>(mock);
                builder.RegisterMock<IDataResourceManager>(mock);
                builder.RegisterMock<IActionDispatcher>(mock);
                builder.RegisterMock<ITangoWebClient>(mock);
                builder.RegisterMock<IMessageHelper>(mock);
                builder.RegisterMock<IMainForm>(mock);
            });

            Modify.Store(context.Store)
                .Set(x => x.Enabled, true)
                .Set(x => x.StoreTypeCode, StoreTypeCode.GenericModule)
                .Set(x => x.License, "I5TKE-5RXP3-NGMEN-ZXHMX-GENERIC-BRIAN@INTERAPPTIVE.COM")
                .Set(x => x.Edition, "QTGyeHaEih1ldH2CBYvYjcUj4YRmteV6oXBCl0s7SwjZ+DtjOHT3JD1Uit/x3sF65o4/EnBuifBA6H1hodXIbIgPMmDQTwXiTIcZj3+53Of8ygIUOvKgurrLmicPNHEmyvwwGLtRRGpkygBh0KCqwVazlsaQ0zFBh34mBLhF3TbVKg8ZYKNzwBIcnXPw1iBLNY3JuuJOd1JOXeC86DGf7ZGlZ5lwQF26Z29Mt6uexBtjAHQup4AX4ORKdjqldEfmiqyh+80AcpMhRPQVeB9gTrWzmVmD+AKuwmdI7j5GrxcKc+1Mmh150RfOhgj8NyR9YKGtbHrrih5D4IuqXUX+BwpuNN5ZjPcOmUrQrjiKP37OlaEdBPRzl5UflPXqBOfSe5iCU/LDKQSbqoxoLu/uC8G/gjvMPavdhAyzOQWwHDOcSNIdIf7QBpPBAkcwucIxcpdRCIgeyc76Tcar7Oc7A+AjjfK/mEZty0ORTDi7WO5k4fPygn5ZK0fbV7D6HF1Rj7rZ0WkHV2zLeSro7ZGIuyz1GN6PMS1uK9cTR/Dm7P/WNeUn9aJ5JaOmqnXOzG+RvG/jrlhc126R5wFg/X/kvfkf9oHn4h72UkLSL3wIj8kiARB8r65qCcw0G0McqXs9WACrQjI+UT12/pZrde8M+D7BvoirfH4GOqEzj7JI8weXiPR62ZzdF4WQ7bYKN/RxLb2KQdH2MMUuU2zSV3Xs/VWGKnmUIXdn7An4pMjhm2WiJLdnQXUjfsHdvOVYTPbZwyFI5vZGX3lDhn4Figoog3potyb+r2HeIJOz2h0NAYJThWhfnQOPqMgc2imFTTjvNnLsVtf0x2dWNECBfY3K0UNC31czVHYJKlxWUS7YqPRP1VtRnPFV4WOfb1WfNC7cQGnpYWZZwItv/8JtINa1J9JxxFKWRGuBZWpZax25M7f4Bd2Ndiil9Rg4Nu+TvpGo5DZ+4yKcGwJOhrnPKVWgEe/xPM5RDNl6lMwZrkMJ/QXebTv3NvY2G97LNWG1SxJc/ywoo8exLWqULu+fbZmysFGiH2Tg2qsN+IjQ/DTh6qPMb40q2Ejl5fARyg++3nP5+bWiDy4vJwmHWX84Sw1XUettUTWmcCEAuuDcuDYrgupkQI0FiRRvu/vq+ZFopJ+TE9lV3lDqA63azhwyboSI9WSSmrjtNqhio28utmp38ohq0WUmwfHN0vNY8JP21vA1g33Jb2t7WG6D+1P26GE9XA/CszrOlVNxF0zd5N7kv1Y88FHl4X5actJh/5sd1pxWrlvN0N6F+dRleaJX7Cua4bRVIJXJW9oN/pnPSUNviY2YFEL89Fbxx3bqvJd44a10Bz25HVTC6ib1QRWiPLruAsDOeAYOuOxz")
                .Save();

            account = Create.CarrierAccount<UspsAccountEntity, IUspsAccountEntity>().Save();
            Create.Profile().AsPrimary().AsPostal(x => x.AsUsps()).Save();

            ShippingSettings.MarkAsConfigured(ShipmentTypeCode.Usps);

            shipment = Create.Shipment(context.Order)
                .AsPostal(postal => postal.AsUsps(usps => usps.Set(x => x.UspsAccountID, account.UspsAccountID)))
                .Set(x => x.TotalWeight, 3)
                .Save();
        }

        [Fact]
        public async Task Process_WithUspsShipment_SavesTrackingNumber()
        {
            Mock<IExtendedSwsimV135> webService = context.Mock.CreateMock<IExtendedSwsimV135>(w =>
            {
                UspsTestHelpers.SetupAddressValidationResponse(w);
                w.Setup(x => x.CreateIndicium(It.IsAny<CreateIndiciumParameters>()))
                    .Returns(new CreateIndiciumResult
                    {
                        TrackingNumber = "FooTracking",
                        Rate = new RateV46(),
                        ImageData = new[] { Convert.FromBase64String("R0lGODlhAQABAIAAAP///wAAACH5BAEAAAAALAAAAAABAAEAAAICRAEAOw==") },
                    });

                AccountInfoV65 accountInfo = new AccountInfoV65()
                {
                    Terms = new Terms()
                    {
                        TermsAR = true,
                        TermsSL = true,
                        TermsGP = true
                    }
                };

                w.Setup(x => x.GetAccountInfo(It.IsAny<Credentials>()))
                    .Returns(new AccountInfoResult(accountInfo, new Address(), ""));
            });

            webServiceFactory.Setup(x => x.Create(It.IsAny<string>(), It.IsAny<LogActionType>()))
                .Returns(webService);

            var result = await context.Container.Resolve<IShipmentProcessor>()
                .Process(new[] { shipment }, context.Mock.Build<ICarrierConfigurationShipmentRefresher>(),
                null, null);

            Assert.True(result.First().IsSuccessful);
            Assert.Equal("FooTracking", result.First().Shipment.TrackingNumber);
        }

        [Theory]
        [InlineData(2, 10, 20, "first")]
        [InlineData(10, 2, 20, "second")]
        [InlineData(10, 20, 2, "third")]
        public async Task Process_UsesAccountWithCheapestRates_WhenRateShopping(decimal first, decimal second, decimal third, string expectedAccount)
        {
            var localAddress = new PersonAdapter { City = "St. Louis", StateProvCode = "MO", CountryCode = "US" };

            Modify.Shipment(shipment)
                .Set(x => x.OriginPerson = localAddress)
                .Set(x => x.ShipPerson = localAddress)
                .Set(x => x.Postal.Usps.RateShop = true)
                .Set(x => x.Postal.Service = (int) PostalServiceType.FirstClass)
                .Save();

            var accounts = new Dictionary<string, long>
            {
                { "first", Modify.Entity(account).Set(x => x.Username, "first").Save().AccountId },
                { "second", Create.CarrierAccount<UspsAccountEntity, IUspsAccountEntity>().Set(x => x.Username, "second").Save().AccountId },
                { "third", Create.CarrierAccount<UspsAccountEntity, IUspsAccountEntity>().Set(x => x.Username, "third").Save().AccountId },
            };

            Mock<IExtendedSwsimV135> webService = context.Mock.CreateMock<IExtendedSwsimV135>(w =>
            {
                UspsTestHelpers.SetupAddressValidationResponse(w);
                w.Setup(x => x.CreateIndicium(It.IsAny<CreateIndiciumParameters>())).Returns(defaultResponse);

                Func<decimal, RateV46[]> createRate = amount => new[] {
                    new RateV46 { ServiceType = ServiceType.USFC, Amount = amount,
                        AddOns = new [] { new AddOnV20 { AddOnType = AddOnTypeV20.USADC } }, DeliverDays = "2" } };

                w.Setup(x => x.GetRates(It.Is<Credentials>(c => c.Username == "first"), It.IsAny<RateV46>(), Carrier.USPS))
                    .Returns(createRate(first));
                w.Setup(x => x.GetRates(It.Is<Credentials>(c => c.Username == "second"), It.IsAny<RateV46>(), Carrier.USPS))
                    .Returns(createRate(second));
                w.Setup(x => x.GetRates(It.Is<Credentials>(c => c.Username == "third"), It.IsAny<RateV46>(), Carrier.USPS))
                    .Returns(createRate(third));

                AccountInfoV65 accountInfo = new AccountInfoV65()
                {
                    Terms = new Terms()
                    {
                        TermsAR = true,
                        TermsSL = true,
                        TermsGP = true
                    }
                };

                w.Setup(x => x.GetAccountInfo(It.IsAny<Credentials>()))
                    .Returns(new AccountInfoResult(accountInfo, new Address(), ""));
            });

            webServiceFactory.Setup(x => x.Create(It.IsAny<string>(), It.IsAny<LogActionType>())).Returns(webService);

            var result = await context.Container.Resolve<IShipmentProcessor>()
                .Process(new[] { shipment }, context.Mock.Build<ICarrierConfigurationShipmentRefresher>(),
                null, null);

            Assert.True(result.First().IsSuccessful, result.First().Error?.Message);
            Assert.Equal(accounts[expectedAccount], result.First().Shipment.Postal.Usps.UspsAccountID);
        }

        public void Dispose() => context.Dispose();
    }
}