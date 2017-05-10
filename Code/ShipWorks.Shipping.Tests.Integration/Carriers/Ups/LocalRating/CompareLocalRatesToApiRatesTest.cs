using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Features.Indexed;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.AddressValidation;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Ups.LocalRating;
using ShipWorks.Shipping.Carriers.Ups.LocalRating.ServiceFilters;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using ShipWorks.Shipping.Carriers.UPS.UpsEnvironment;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Shipping.UI.Carriers.Ups.LocalRating;
using ShipWorks.Startup;
using ShipWorks.Tests.Integration.MSTest;
using ShipWorks.Tests.Integration.MSTest.Utilities;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Syncfusion.XlsIO;
using Xunit;
using Xunit.Abstractions;

namespace ShipWorks.Shipping.Tests.Integration.Carriers.Ups.LocalRating
{
    [Collection("DatabaseFixtureWithReusableContext")]
    [Trait("Category", "CompareLocalRatesToApiRatesTest")]
    public class CompareLocalRatesToApiRatesTest
    {
        private readonly ITestOutputHelper output;
        private DataContext context;
        private long localRatingAccountID;
        private long apiRatingAccountID;
        private const string ContextName = "CompareLocalRatesToApiRatesTest";
        
        public CompareLocalRatesToApiRatesTest(DatabaseFixtureWithReusableContext db, ITestOutputHelper output)
        {
            this.output = output;

            if (db.IsContextInitialized(ContextName))
            {
                context = InitializeDataContext(db);
            }
        }

        [Fact]
        public void GetLocalRates()
        {
            RunTest(s => { });
        }

        [Fact]
        public void LargePackage()
        {
            RunTest(s =>
            {
                var package = s.Ups.Packages[0];
                package.DimsLength = 100;
                package.DimsWidth = 10;
                package.DimsHeight = 10;
            });
        }

        [Fact]
        public void AdditionalHandling()
        {
            RunTest(s=>s.Ups.Packages[0].DimsLength=61);
        }

        private void RunTest(Action<ShipmentEntity> setter)
        {
            var localResult = GetRates(CreateShipment(setter), localRatingAccountID, CreateLocalClient());
            var apiResult = GetRates(CreateShipment(setter), apiRatingAccountID, CreateApiClient());

            Assert.True(localResult.Success);
            Assert.True(apiResult.Success);

            Assert.NotNull(localResult.Value.FirstOrDefault());
            Assert.Equal(localResult.Value.Cast<UpsLocalServiceRate>().Count(), localResult.Value.Count);

            Assert.True(ValidateResult(localResult.Value.Cast<UpsLocalServiceRate>().ToList(), apiResult.Value));
        }

        private ShipmentEntity CreateShipment(Action<ShipmentEntity> setter)
        {
            ShipmentEntity shipment = ShipWorksDataMethods.InternalCreateShipment(context.Order,
                ShipmentTypeCode.UpsOnLineTools, 1, 5, "LB");
            Modify.Shipment(shipment).AsUps(builder => builder.WithPackage(pBuilder => pBuilder.Set(p =>
            {
                p.Weight = 10;
                p.DimsWidth = 10;
                p.DimsHeight = 10;
                p.DimsLength = 10;
            }))).Set(s =>
            {
                s.ShipFirstName = "API";
                s.ShipMiddleName = string.Empty;
                s.ShipLastName = "Rates";
                s.ShipCompany = "Shipworks";
                s.ShipStreet1 = "1 S Memorial Drive";
                s.ShipStreet2 = "Suite 2000";
                s.ShipStreet3 = string.Empty;
                s.ShipCity = "St Louis";
                s.ShipStateProvCode = "MO";
                s.ShipCountryCode = "US";
                s.ShipPhone = "314-555-1212";

                s.OriginFirstName = "Joe";
                s.OriginMiddleName = string.Empty;
                s.OriginLastName = "Schmoe";
                s.OriginCompany = "Another Company";
                s.OriginStreet1 = "1 S Memorial Drive";
                s.OriginStreet2 = "Suite 1900";
                s.OriginStreet3 = string.Empty;
                s.OriginCity = "St Louis";
                s.OriginStateProvCode = "MO";
                s.OriginCountryCode = "US";
                s.OriginPhone = "314-555-1212";
                s.OriginOriginID = (int) ShipmentOriginSource.Other;
            }).Set(setter).Save();

            return shipment;
        }

        private bool ValidateResult(List<UpsLocalServiceRate> localRates, List<UpsServiceRate> apiRates)
        {
            var localServiceTypes = new[]
            {
                UpsServiceType.UpsGround,
                UpsServiceType.Ups3DaySelect,
                UpsServiceType.Ups2DayAir,
                UpsServiceType.Ups2DayAirAM,
                UpsServiceType.UpsNextDayAirSaver,
                UpsServiceType.UpsNextDayAir,
                UpsServiceType.UpsNextDayAirAM
            };

            // No rate returned that LocalRates cannot handle
            var extraLocalRates =
                localRates.Select(r => r.Service).Where(service => !localServiceTypes.Contains(service)).ToList();

            if (extraLocalRates.Any())
            {
                output.WriteLine($"LocalRates returned the following unsupported services:\n\n{string.Join("\n", extraLocalRates)}");
            }
            
            // Filter out api rates that LocalRates cannot handle
            apiRates = apiRates.Where(r => localServiceTypes.Contains(r.Service)).ToList();

            var missingLocalRateServices =
                apiRates.Where(apiRate => localRates.None(localRate => localRate.Service == apiRate.Service)).Select(r=>r.Service).ToList();

            if (missingLocalRateServices.Any())
            {
                output.WriteLine($"LocalRates returned the following unsupported services:\n\n{string.Join("\n", missingLocalRateServices)}");
            }

            var allMatched = true;
            foreach (var localRate in localRates)
            {
                var apiRate = apiRates.Single(r => r.Service == localRate.Service);

                if (apiRate.Amount != localRate.Amount)
                {
                    output.WriteLine($"Different amount for service {apiRate.Service}. API: {apiRate.Amount} Local: {localRate.Amount}");

                    StringBuilder sb = new StringBuilder();
                    localRate.Log(sb);
                    output.WriteLine($"Local Rate Calculation:\n{sb}\nEnd Local Rate Calculation");

                    allMatched = false;
                }
            }

            return allMatched;
        }

        private IUpsRateClient CreateLocalClient()
        {
            return context.Mock.Create<UpsLocalRateClient>();
        }

        private IUpsRateClient CreateApiClient()
        {
            return context.Mock.Create<UpsApiRateClient>();
        }

        private GenericResult<List<UpsServiceRate>> GetRates(ShipmentEntity shipment,
            long accountID,
            IUpsRateClient client)
        {
            Modify.Shipment(shipment).Set(s=>s.Ups.UpsAccountID = accountID).Save();

            return client.GetRates(shipment);
        }

        private DataContext InitializeDataContext(DatabaseFixtureWithReusableContext db)
        {
            context = db.GetReusableDataContext(x => ContainerInitializer.Initialize(x),
                ShipWorksInitializer.GetShipWorksInstance(), ContextName);

            context.UpdateShippingSetting(s =>
            {
                s.UpsAccessKey = "YbeKtEkBXqxQYcW0MonRIXPCPFKuLQ6l";
                s.UpsInsuranceProvider = 1;
                s.UpsInsurancePennyOne = false;
            });

            var certInspector = new TrustingCertificateInspector();
            context.Mock.Provide<ICertificateInspector>(certInspector);
            context.Mock.Provide<ICarrierSettingsRepository>(new UpsSettingsRepository());

            localRatingAccountID = SetupLocalRating().UpsAccountID;
            apiRatingAccountID = SetupApi().UpsAccountID;

            // When a constructor has an IEnumberable<IUpsServiceType>, mock returns all expected IServiceFilter's and a mocked version.
            // The mocked version was filtering out all the service types, so this converts the mocked version into a passthrough.
            // Ideally, it wouldn't be returned at all but I found no way of doing this...
            context.Mock.SetupDefaultMocksForEnumerable<IServiceFilter>(item =>
                item.Setup(x => x.GetEligibleServices(It.IsAny<UpsShipmentEntity>(), It.IsAny<IEnumerable<UpsServiceType>>()))
                .Returns<UpsShipmentEntity, IEnumerable<UpsServiceType>>((_, types) => types));

            return context;
        }

        private UpsAccountEntity SetupLocalRating()
        {
            UpsAccountEntity upsAccountEntity = Create.CarrierAccount<UpsAccountEntity, IUpsAccountEntity>()
                            .Set(a =>
                            {
                                a.AccountNumber = "TT9723";
                                a.UserID = "7de7b76b15ed443e";
                                a.Password = "11d0e976";
                                a.RateType = 0;
                                a.InvoiceAuth = false;
                                a.FirstName = "API";
                                a.MiddleName = string.Empty;
                                a.LastName = "Rates";
                                a.Company = "Shipworks";
                                a.Street1 = "1 S Memorial Drive";
                                a.Street2 = "Suite 2000";
                                a.Street3 = string.Empty;
                                a.City = "St Louis";
                                a.StateProvCode = "MO";
                                a.CountryCode = "US";
                                a.Phone = "314-555-1212";
                                a.Email = "junk@shipworks.com";
                                a.Website = "www.shipworks.com";
                                a.PromoStatus = 0;
                                a.LocalRatingEnabled = true;
                                a.UpsRateTableID = null;
                            }).Save();

            Assembly shippingAssembly = Assembly.GetAssembly(typeof(UpsLocalRatingViewModel));

            var table = context.Mock.Create<IUpsLocalRateTable>();

            using (Stream zoneStream = shippingAssembly.GetManifestResourceStream(UpsLocalRatingViewModel.SampleZoneFileResourceName))
            {
                table.LoadZones(zoneStream);
            }

            using (Stream rateStream = shippingAssembly.GetManifestResourceStream(UpsLocalRatingViewModel.SampleRatesFileResourceName))
            {
                table.LoadRates(rateStream);
            }

            table.SaveRates(upsAccountEntity);
            table.SaveZones();
            return upsAccountEntity;
        }

        private static UpsAccountEntity SetupApi()
        {
            return Create.CarrierAccount<UpsAccountEntity, IUpsAccountEntity>()
                .Set(a =>
                {
                    a.AccountNumber = "TT9723";
                    a.UserID = "7de7b76b15ed443e";
                    a.Password = "11d0e976";
                    a.RateType = 0;
                    a.InvoiceAuth = false;
                    a.FirstName = "API";
                    a.MiddleName = string.Empty;
                    a.LastName = "Rates";
                    a.Company = "Shipworks";
                    a.Street1 = "1 S Memorial Drive";
                    a.Street2 = "Suite 2000";
                    a.Street3 = string.Empty;
                    a.City = "St Louis";
                    a.StateProvCode = "MO";
                    a.CountryCode = "US";
                    a.Phone = "314-555-1212";
                    a.Email = "junk@shipworks.com";
                    a.Website = "www.shipworks.com";
                    a.PromoStatus = 0;
                    a.LocalRatingEnabled = false;
                    a.UpsRateTableID = null;
                }).Save();
        }
    }
}
