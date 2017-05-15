using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Ups.LocalRating;
using ShipWorks.Shipping.Carriers.Ups.LocalRating.ServiceFilters;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using ShipWorks.Shipping.Carriers.UPS.UpsEnvironment;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Shipping.UI.Carriers.Ups.LocalRating;
using ShipWorks.Startup;
using ShipWorks.Stores;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Tests.Integration.Shared;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using ShipWorks.Tests.Shared.ExtensionMethods;
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
            InitializeDataContext(db);
        }

        [Fact]
        public void GetLocalRates()
        {
            RunTest(s => { });
        }

        [Theory]
        [InlineData(100, 20, 10, 10)] // Dimensional Weight is 144
        [InlineData(71, 23, 7, 10)] // Dimensional Weight is 69, but UPS business rules should have it rated at 90 because it is a large package
        [InlineData(80, 20, 20, 10)] // Billable Weight is 231 - over the 150 limit, but dimensional.
        public void LargePackage(double length, double width, double height, double weight)
        {
            RunTest(s =>
            {
                var package = s.Ups.Packages[0];
                package.DimsLength = length;
                package.DimsWidth = width;
                package.DimsHeight = height;
                package.Weight = weight;
                s.ShipStateProvCode = "CA";
                s.ShipPostalCode = "90012";
            });
        }

        [Theory]
        [InlineData("63110", "MO", "15825", "PA", "53", "21", "19")] // BillWeight = 153
        [InlineData("63110", "MO", "15825", "PA", "55", "30", "25")] // BillWeight = 297
        [InlineData("63110", "MO", "15825", "PA", "66", "20", "20")] // BillWeight = 190
        [InlineData("63110", "MO", "15825", "PA", "74", "20", "20")] // BillWeight = 213
        [InlineData("63110", "MO", "15825", "PA", "74", "30", "15")] // BillWeight = 240
        [InlineData("63110", "MO", "15825", "PA", "80", "18", "18")] // BillWeight = 187
        [InlineData("63110", "MO", "32533", "FL", "50", "21", "20")] // BillWeight = 152
        [InlineData("63110", "MO", "32533", "FL", "57", "30", "24")] // BillWeight = 296
        [InlineData("63110", "MO", "32533", "FL", "65", "20", "20")] // BillWeight = 188
        [InlineData("63110", "MO", "32533", "FL", "73", "20", "20")] // BillWeight = 211
        [InlineData("63110", "MO", "32533", "FL", "73", "28", "18")] // BillWeight = 265
        [InlineData("63110", "MO", "32533", "FL", "79", "18", "18")] // BillWeight = 185
        [InlineData("63110", "MO", "33027", "FL", "56", "20", "19")] // BillWeight = 154
        [InlineData("63110", "MO", "33027", "FL", "67", "20", "20")] // BillWeight = 193
        [InlineData("63110", "MO", "33027", "FL", "67", "22", "20")] // BillWeight = 213
        [InlineData("63110", "MO", "33027", "FL", "75", "20", "20")] // BillWeight = 216
        [InlineData("63110", "MO", "33027", "FL", "75", "30", "10")] // BillWeight = 162
        [InlineData("63110", "MO", "33027", "FL", "81", "18", "18")] // BillWeight = 189
        [InlineData("63110", "MO", "37501", "TN", "53", "20", "20")] // BillWeight = 153
        [InlineData("63110", "MO", "37501", "TN", "59", "30", "23")] // BillWeight = 293
        [InlineData("63110", "MO", "37501", "TN", "64", "20", "20")] // BillWeight = 185
        [InlineData("63110", "MO", "37501", "TN", "72", "20", "20")] // BillWeight = 208
        [InlineData("63110", "MO", "37501", "TN", "72", "27", "19")] // BillWeight = 266
        [InlineData("63110", "MO", "37501", "TN", "78", "18", "18")] // BillWeight = 182
        [InlineData("63110", "MO", "42431", "KY", "55", "20", "19")] // BillWeight = 151
        [InlineData("63110", "MO", "42431", "KY", "63", "20", "20")] // BillWeight = 182
        [InlineData("63110", "MO", "42431", "KY", "67", "28", "21")] // BillWeight = 284
        [InlineData("63110", "MO", "42431", "KY", "71", "20", "20")] // BillWeight = 205
        [InlineData("63110", "MO", "42431", "KY", "71", "26", "20")] // BillWeight = 266
        [InlineData("63110", "MO", "42431", "KY", "77", "18", "18")] // BillWeight = 180
        [InlineData("63110", "MO", "89434", "NV", "60", "20", "18")] // BillWeight = 156
        [InlineData("63110", "MO", "89434", "NV", "68", "20", "20")] // BillWeight = 196
        [InlineData("63110", "MO", "89434", "NV", "68", "23", "20")] // BillWeight = 226
        [InlineData("63110", "MO", "89434", "NV", "71", "28", "19")] // BillWeight = 272
        [InlineData("63110", "MO", "89434", "NV", "76", "20", "20")] // BillWeight = 219
        [InlineData("63110", "MO", "89434", "NV", "76", "31", "10")] // BillWeight = 170
        public void NDATests(string originZip, string originState, string shipZip, string shipState, double length, double width, double height)
        {
            RunTest(s =>
            {
                var package = s.Ups.Packages[0];
                package.DimsLength = length;
                package.DimsWidth = width;
                package.DimsHeight = height;

                s.ShipStateProvCode = shipState;
                s.ShipPostalCode = shipZip;

                s.OriginStateProvCode = originState;
                s.OriginPostalCode = originZip;
            });
        }

        [Theory(Skip = "Zip combo does not support Ups2DayAirAM or UpsNextDayAirSaver")]
        [InlineData("00544", "NY", "81324", "CO", "61", "18", "19")] // BillWeight = 151
        [InlineData("00544", "NY", "81324", "CO", "69", "20", "20")] // BillWeight = 199
        [InlineData("00544", "NY", "81324", "CO", "69", "24", "20")] // BillWeight = 239
        [InlineData("00544", "NY", "81324", "CO", "69", "29", "19")] // BillWeight = 274
        [InlineData("00544", "NY", "81324", "CO", "77", "20", "20")] // BillWeight = 222
        [InlineData("00544", "NY", "81324", "CO", "77", "32", "10")] // BillWeight = 178
        public void FromNyTests(string originZip, string originState, string shipZip, string shipState, double length, double width, double height)
        {
            RunTest(s =>
            {
                var package = s.Ups.Packages[0];
                package.DimsLength = length;
                package.DimsWidth = width;
                package.DimsHeight = height;

                s.ShipStateProvCode = shipState;
                s.ShipPostalCode = shipZip;

                s.OriginStateProvCode = originState;
                s.OriginPostalCode = originZip;
            });
        }

        [Theory(Skip = "HI doesn't appear to support NextDayAirAm")]
        [InlineData("63110", "MO", "96701", "HI", "62", "20", "20")] // BillWeight = 179
        [InlineData("63110", "MO", "96701", "HI", "67", "29", "20")] // BillWeight = 280
        [InlineData("63110", "MO", "96701", "HI", "70", "20", "20")] // BillWeight = 202
        [InlineData("63110", "MO", "96701", "HI", "70", "25", "20")] // BillWeight = 252
        [InlineData("63110", "MO", "96701", "HI", "78", "18", "18")] // BillWeight = 182
        [InlineData("63110", "MO", "96701", "HI", "78", "31", "12")] // BillWeight = 209
        public void ToHiTests(string originZip, string originState, string shipZip, string shipState, double length, double width, double height)
        {
            RunTest(s =>
            {
                var package = s.Ups.Packages[0];
                package.DimsLength = length;
                package.DimsWidth = width;
                package.DimsHeight = height;

                s.ShipStateProvCode = shipState;
                s.ShipPostalCode = shipZip;

                s.OriginStateProvCode = originState;
                s.OriginPostalCode = originZip;
            });
        }

        [Fact]
        public void AdditionalHandling()
        { 
            RunTest(s=>s.Ups.Packages[0].DimsLength=61);
        }

        [Fact]
        public void UseBillableWeightWhenDimensionalWeightDeviserIs139()
        {
            // Dimensional weight is 7lbs
            RunTest(s =>
            {
                var package = s.Ups.Packages[0];
                package.DimsLength = 10;
                package.DimsWidth = 10;
                package.DimsHeight = 10;
                package.DimsWeight = 5;
            });
        }

        [Fact]
        public void UseActualWeightWhenDimensionalWeightDeviserIs139()
        {
            // Billable weight is 7lbs
            RunTest(s =>
            {
                var package = s.Ups.Packages[0];
                package.DimsLength = 10;
                package.DimsWidth = 10;
                package.DimsHeight = 10;
                package.DimsWeight = 10;
            });
        }

        [Theory]
        [InlineData(UpsPackagingType.Custom)]
        [InlineData(UpsPackagingType.Letter)]
        [InlineData(UpsPackagingType.BoxExpress)]
        public void UseDifferentPackaging(UpsPackagingType packagingType)
        {
            RunTest(s => s.Ups.Packages[0].PackagingType = (int) packagingType);
        }

        [Theory]
        [InlineData(UpsDeliveryConfirmationType.None)]
        [InlineData(UpsDeliveryConfirmationType.AdultSignature)]
        [InlineData(UpsDeliveryConfirmationType.NoSignature)]
        [InlineData(UpsDeliveryConfirmationType.Signature)]
        public void UseDifferentConfirmations(UpsDeliveryConfirmationType deliveryConfirmationType)
        {
            RunTest(s=>s.Ups.DeliveryConfirmation = (int) deliveryConfirmationType);
        }


        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ShipperReleaseTest(bool shipperRelease)
        {
            RunTest(s => s.Ups.ShipperRelease = shipperRelease);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CarbonNeutralTest(bool carbonNeutral)
        {
            RunTest(s => s.Ups.CarbonNeutral = carbonNeutral);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void AdditionalHandlingTest(bool additionalHandling)
        {
            RunTest(s => s.Ups.Packages[0].AdditionalHandlingEnabled = additionalHandling);
        }

        /// <remarks>
        /// In order to get dry ice to work:
        /// 1) Remove UpsRatePackageServiceOptionsElementWriter.WriteDryIceElement
        /// 2) In UpsPackageServiceOptionsElementWriter.WriteDryIceElement, change the unit of measurement code
        ///     to use LBS instead of 01. The ship request works with either one...
        /// </remarks>
        [Theory(Skip = "Dry ice isn't supported for our account. See comments...")]
        [InlineData(true, UpsDryIceRegulationSet.Iata, false, 0)]
        [InlineData(true, UpsDryIceRegulationSet.Cfr, false, 5.6)]
        [InlineData(false, UpsDryIceRegulationSet.Iata, false, 10)]
        [InlineData(false, UpsDryIceRegulationSet.Cfr, false, 10)]
        [InlineData(true, UpsDryIceRegulationSet.Cfr, false, 5.5)]
        [InlineData(true, UpsDryIceRegulationSet.Cfr, true, 10)]
        public void DryIceTest(bool enabled,
            UpsDryIceRegulationSet regulationSet,
            bool forMedical,
            double weight)
        {
            
            RunTest(s =>
            {
                UpsPackageEntity package = s.Ups.Packages[0];
                package.DryIceEnabled = enabled;
                package.DryIceWeight = weight;
                package.DryIceIsForMedicalUse = forMedical;
                package.DryIceRegulationSet = (int) regulationSet;
            });
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void VerbalConfirmationTest(bool verbalConfirmation)
        {
            RunTest(s => s.Ups.Packages[0].VerbalConfirmationEnabled = verbalConfirmation);
        }

        [Theory]
        [InlineData(true, 10)]
        [InlineData(false, 0)]
        public void CollectOnDeliveryTest(bool cod, decimal codAmount)
        {
            RunTest(s =>
            {
                s.Ups.CodEnabled = cod;
                s.Ups.CodAmount = codAmount;
            });
        }

        [Theory(Skip = "The zipcode being used doesn't support Saturday Delivery UpsGround and Ups3DaySelect.")]
        [InlineData(true)]
        [InlineData(false)]
        public void SaturdayDelivery(bool saturdayDelivery)
        {
            RunTest(s=>
            {
                s.Ups.SaturdayDelivery = saturdayDelivery;
                s.ShipDate = DateTime.Now.Next(DayOfWeek.Friday);
                s.Ups.Service = (int) UpsServiceType.UpsNextDayAir;
            });
        }

        [Fact]
        public void ShipOnASaturday()
        {
            RunTest(s=>s.ShipDate = DateTime.Now.Next(DayOfWeek.Saturday));
        }

        [Theory]
        [InlineData(UpsPayorType.Receiver)]
        [InlineData(UpsPayorType.Sender)]
        public void ThirdPartyBilling(UpsPayorType payorType)
        {
            // Third party billing rates returned by api do not seem to include the third party billing surcharge
            RunTest(s => s.Ups.PayorType = (int) payorType);
        }

        [Theory]
        // US 48 DAS
        [InlineData("MA", "01007")]
        // US 48 DAS Extended
        [InlineData("MA", "01005")]
        // Remote HI
        [InlineData("HI", "96703")]
        //Remote AK
        [InlineData("AK", "99546")]
        public void RemoteArea(string state, string zip)
        {
            RunTest(s =>
            {
                s.ShipStateProvCode = state;
                s.ShipPostalCode = zip;
            });
        }

        [Fact]
        public void ResidentialTest()
        {
            RunTest(s =>
            {
                s.ShipCompany = string.Empty;
                s.ShipStreet1 = "4012 Shenandoah Ave";
                s.ShipStreet2 = string.Empty;
                s.ShipCity = "St Louis";
                s.ShipStateProvCode = "MO";
                s.ShipPostalCode = "63110";
                s.ShipResidentialStatus = (int) ResidentialDeterminationType.Residential;
            });
        }

        private void RunTest(Action<ShipmentEntity> setter)
        {
            var localResult = GetRates(CreateShipment(setter), localRatingAccountID, CreateLocalClient());
            var apiResult = GetRates(CreateShipment(setter), apiRatingAccountID, CreateApiClient());

            Assert.True(localResult.Success);
            Assert.True(apiResult.Success);


            var firstLocalResult = localResult.Value?.FirstOrDefault();
            Assert.NotNull(firstLocalResult);

            // If this fails, rates were likely from the API.
            Assert.IsType<UpsLocalServiceRate>(firstLocalResult); 

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
            bool valid = true;

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
                output.WriteLine($"LocalRates returned the following unsupported services:\n\n{string.Join("\n", extraLocalRates)}\n");
                valid = false;
            }
            
            // Filter out api rates that LocalRates cannot handle
            apiRates = apiRates.Where(r => localServiceTypes.Contains(r.Service)).ToList();

            var apiRatesNotInLocalRates =
                apiRates.Where(apiRate => localRates.None(localRate => localRate.Service == apiRate.Service)).Select(r=>r.Service).ToList();
            if (apiRatesNotInLocalRates.Any())
            {
                output.WriteLine($"LocalRates client did not return services(s) returned by the api client. Missing services:\n\n{string.Join("\n", apiRatesNotInLocalRates)}\n");
                valid = false;
            }

            var localRatesNotInApiRates =
                localRates.Where(localRate => apiRates.None(apiRate => apiRate.Service == localRate.Service)).Select(r => r.Service).ToList();
            if (localRatesNotInApiRates.Any())
            {
                output.WriteLine($"Api client did not return service(s) returned by the local client. Missing services:\n\n{string.Join("\n", localRatesNotInApiRates)}\n");
                valid = false;
            }

            foreach (var localRate in localRates)
            {
                var apiRate = apiRates.FirstOrDefault(r => r.Service == localRate.Service);
                if (apiRate == null)
                {
                    continue;
                }

                if (apiRate.Amount != localRate.Amount)
                {
                    output.WriteLine($"{apiRate.Service} - Difference = {Math.Abs(apiRate.Amount - localRate.Amount)}");
                    output.WriteLine($"API: {apiRate.Amount} Local: {localRate.Amount}");

                    StringBuilder sb = new StringBuilder();
                    localRate.Log(sb);
                    output.WriteLine($"Local Rate Calculation:\n{sb}\nEnd Local Rate Calculation");

                    valid = false;
                }
            }

            return valid;
        }

        private IUpsRateClient CreateLocalClient()
        {
            return context.Mock.Create<UpsLocalRateClient>();
        }

        private IUpsRateClient CreateApiClient()
        {

            context.Mock.Mock<UpsShipmentType>()
                .Setup(x => x.GetExcludedServiceTypes(It.IsAny<IExcludedServiceTypeRepository>()))
                .Returns(new[]
                {
                    (int) UpsServiceType.UpsSurePost1LbOrGreater,
                    (int) UpsServiceType.UpsSurePostBoundPrintedMatter,
                    (int) UpsServiceType.UpsSurePostLessThan1Lb,
                    (int) UpsServiceType.UpsSurePostMedia
                });

            return context.Mock.Create<UpsApiRateClient>();
        }

        private GenericResult<List<UpsServiceRate>> GetRates(ShipmentEntity shipment,
            long accountID,
            IUpsRateClient client)
        {
            Modify.Shipment(shipment).Set(s=>s.Ups.UpsAccountID = accountID).Save();

            return client.GetRates(shipment);
        }

        private void InitializeDataContext(DatabaseFixtureWithReusableContext db)
        {
            context = db.GetExistingContext(ContextName);
            if (context == null)
            {
                context = db.GetNewDataContext(x => ContainerInitializer.Initialize(x),
                    ShipWorksInitializer.GetShipWorksInstance(), ContextName);

                context.UpdateShippingSetting(s =>
                {
                    s.UpsAccessKey = "YbeKtEkBXqxQYcW0MonRIXPCPFKuLQ6l";
                    s.UpsInsuranceProvider = 1;
                    s.UpsInsurancePennyOne = false;
                    s.ConfiguredTypes = new[] { ShipmentTypeCode.UpsOnLineTools };
                    s.ActivatedTypes = new[] { ShipmentTypeCode.UpsOnLineTools };
                });

                context.Store.Enabled = true;

                var certInspector = new TrustingCertificateInspector();
                context.Mock.Provide<ICertificateInspector>(certInspector);
                context.Mock.Provide<ICarrierSettingsRepository>(new UpsSettingsRepository());

                localRatingAccountID = SetupLocalRatingAccount().UpsAccountID;
                apiRatingAccountID = SetupApiAccount().UpsAccountID;
                UpdateStore();

                // When a constructor has an IEnumberable<IUpsServiceType>, mock returns all expected IServiceFilter's and a mocked version.
                // The mocked version was filtering out all the service types, so this converts the mocked version into a passthrough.
                // Ideally, it wouldn't be returned at all but I found no way of doing this...
                context.Mock.SetupDefaultMocksForEnumerable<IServiceFilter>(item =>
                    item.Setup(
                            x =>
                                x.GetEligibleServices(It.IsAny<UpsShipmentEntity>(),
                                    It.IsAny<IEnumerable<UpsServiceType>>()))
                        .Returns<UpsShipmentEntity, IEnumerable<UpsServiceType>>((_, types) => types));
            }
            else
            {
                using (SqlAdapter adapter = new SqlAdapter())
                {
                    UpsAccountCollection accounts = new UpsAccountCollection();
                    adapter.FetchEntityCollection(accounts, null);

                    apiRatingAccountID = accounts.Single(a => a.Description == "API").UpsAccountID;
                    localRatingAccountID = accounts.Single(a => a.Description == "Local").UpsAccountID;
                }
            }
        }

        /// <summary>
        /// Updates the store so it is a usable ShipWorks store
        /// </summary>
        private void UpdateStore()
        {
            Modify.Store(context.Store)
                .Set(x => x.Enabled, true)
                .Set(x => x.StoreTypeCode, StoreTypeCode.GenericModule)
                .Set(x => x.License, "I5TKE-5RXP3-NGMEN-ZXHMX-GENERIC-BRIAN@INTERAPPTIVE.COM")
                .Set(x => x.Edition,
                    "QTGyeHaEih1ldH2CBYvYjcUj4YRmteV6oXBCl0s7SwjZ+DtjOHT3JD1Uit/x3sF65o4/EnBuifBA6H1hodXIbIgPMmDQTwXiTIcZj3+53Of8ygIUOvKgurrLmicPNHEmyvwwGLtRRGpkygBh0KCqwVazlsaQ0zFBh34mBLhF3TbVKg8ZYKNzwBIcnXPw1iBLNY3JuuJOd1JOXeC86DGf7ZGlZ5lwQF26Z29Mt6uexBtjAHQup4AX4ORKdjqldEfmiqyh+80AcpMhRPQVeB9gTrWzmVmD+AKuwmdI7j5GrxcKc+1Mmh150RfOhgj8NyR9YKGtbHrrih5D4IuqXUX+BwpuNN5ZjPcOmUrQrjiKP37OlaEdBPRzl5UflPXqBOfSe5iCU/LDKQSbqoxoLu/uC8G/gjvMPavdhAyzOQWwHDOcSNIdIf7QBpPBAkcwucIxcpdRCIgeyc76Tcar7Oc7A+AjjfK/mEZty0ORTDi7WO5k4fPygn5ZK0fbV7D6HF1Rj7rZ0WkHV2zLeSro7ZGIuyz1GN6PMS1uK9cTR/Dm7P/WNeUn9aJ5JaOmqnXOzG+RvG/jrlhc126R5wFg/X/kvfkf9oHn4h72UkLSL3wIj8kiARB8r65qCcw0G0McqXs9WACrQjI+UT12/pZrde8M+D7BvoirfH4GOqEzj7JI8weXiPR62ZzdF4WQ7bYKN/RxLb2KQdH2MMUuU2zSV3Xs/VWGKnmUIXdn7An4pMjhm2WiJLdnQXUjfsHdvOVYTPbZwyFI5vZGX3lDhn4Figoog3potyb+r2HeIJOz2h0NAYJThWhfnQOPqMgc2imFTTjvNnLsVtf0x2dWNECBfY3K0UNC31czVHYJKlxWUS7YqPRP1VtRnPFV4WOfb1WfNC7cQGnpYWZZwItv/8JtINa1J9JxxFKWRGuBZWpZax25M7f4Bd2Ndiil9Rg4Nu+TvpGo5DZ+4yKcGwJOhrnPKVWgEe/xPM5RDNl6lMwZrkMJ/QXebTv3NvY2G97LNWG1SxJc/ywoo8exLWqULu+fbZmysFGiH2Tg2qsN+IjQ/DTh6qPMb40q2Ejl5fARyg++3nP5+bWiDy4vJwmHWX84Sw1XUettUTWmcCEAuuDcuDYrgupkQI0FiRRvu/vq+ZFopJ+TE9lV3lDqA63azhwyboSI9WSSmrjtNqhio28utmp38ohq0WUmwfHN0vNY8JP21vA1g33Jb2t7WG6D+1P26GE9XA/CszrOlVNxF0zd5N7kv1Y88FHl4X5actJh/5sd1pxWrlvN0N6F+dRleaJX7Cua4bRVIJXJW9oN/pnPSUNviY2YFEL89Fbxx3bqvJd44a10Bz25HVTC6ib1QRWiPLruAsDOeAYOuOxz")
                .Set(x => x.Company, "ShipWorks")
                .Set(x => x.ManualOrderPostfix, "-M")
                .Set(x => x.ModuleUsername = "kevin")
                .Set(x => x.ModulePassword, "hhWfRUrMqjk=")
                .Set(x => x.ModuleUrl, "http://devsandbox:8880/api/kevin2")
                .Set(x => x.ModuleVersion, "3.10.0")
                .Set(x => x.ModulePlatform, "second")
                .Set(x => x.ModuleDeveloper, "ShipWorks")
                .Set(x => x.ModuleStatusCodes,
                    "<StatusCodes><StatusCode><Code>5</Code><Name>Blah</Name></StatusCode><StatusCode><Code>1</Code><Name>Shipped</Name></StatusCode></StatusCodes>")
                .Set(x => x.ModuleDownloadPageSize, 50)
                .Set(x => x.ModuleRequestTimeout, 60)
                .Set(x => x.ModuleDownloadStrategy = (int) GenericStoreDownloadStrategy.ByModifiedTime)
                .Set(x => x.ModuleOnlineStatusSupport, 2)
                .Set(x => x.ModuleOnlineStatusDataType, 0)
                .Set(x => x.ModuleOnlineCustomerSupport, false)
                .Set(x => x.ModuleOnlineCustomerDataType, 0)
                .Set(x => x.ModuleOnlineStatusDataType, 1)
                .Set(x => x.ModuleHttpExpect100Continue, true)
                .Set(x => x.ModuleResponseEncoding, 0)
                .Set(x => x.SchemaVersion, "1.0.0")
                .Save();
        }

        private UpsAccountEntity SetupLocalRatingAccount()
        {
            UpsAccountEntity upsAccountEntity = Create.CarrierAccount<UpsAccountEntity, IUpsAccountEntity>()
                .Set(a =>
                {
                    a.Description = "Local";
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

        private static UpsAccountEntity SetupApiAccount()
        {
            return Create.CarrierAccount<UpsAccountEntity, IUpsAccountEntity>()
                .Set(a =>
                {
                    a.Description = "API";
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
