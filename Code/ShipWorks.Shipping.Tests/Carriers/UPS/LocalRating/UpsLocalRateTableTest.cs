using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Ups.LocalRating;
using ShipWorks.Tests.Shared;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.ReadOnlyEntityClasses;
using ShipWorks.Shipping.Carriers.Ups.LocalRating.ServiceFilters;
using ShipWorks.Shipping.Carriers.Ups.LocalRating.Surcharges;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.LocalRating;
using Syncfusion.XlsIO;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.LocalRating
{
    public class UpsLocalRateTableTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly UpsAccountEntity upsAccount;
        private readonly UpsLocalRateTable testObject;

        public UpsLocalRateTableTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            upsAccount = new UpsAccountEntity();
            testObject = mock.Create<UpsLocalRateTable>();
        }

        #region "Replacing And Saving Rates"

        [Fact]
        public void Load_DelegatesToUpsLocalRateTableRepository()
        {
            testObject.Load(upsAccount);

            mock.Mock<IUpsLocalRateTableRepository>().Verify(r => r.GetRateTable(upsAccount));
        }

        [Fact]
        public void SaveRates_SetsUploadDate()
        {
            ReplaceRatesAndSurcharges(testObject);

            Assert.Null(testObject.RateUploadDate);
            testObject.SaveRates(upsAccount);
            Assert.NotNull(testObject.RateUploadDate);
        }

        [Fact]
        public void SaveRates_DelegatesSaveToUpsLocalRateTableRepository()
        {
            ReplaceRatesAndSurcharges(testObject);

            testObject.SaveRates(upsAccount);
            mock.Mock<IUpsLocalRateTableRepository>()
                .Verify(r => r.Save(It.IsAny<UpsRateTableEntity>(), upsAccount), Times.Once);
        }

        [Fact]
        public void SaveRates_DelegatesCleanupToUpsLocalRateTableRepository()
        {
            ReplaceRatesAndSurcharges(testObject);

            testObject.SaveRates(upsAccount);
            mock.Mock<IUpsLocalRateTableRepository>().Verify(r => r.CleanupRates(), Times.Once);
        }

        [Fact]
        public void SaveRates_CallsSave_AfterCleanup()
        {
            ReplaceRatesAndSurcharges(testObject);

            int callOrder = 0;
            int saveCalled = 0;
            int cleanupCalled = 0;

            var rateTableRepo = mock.Mock<IUpsLocalRateTableRepository>();
            rateTableRepo.Setup(r => r.Save(It.IsAny<UpsRateTableEntity>(), upsAccount))
                .Callback(() =>
                {
                    callOrder++;
                    saveCalled = callOrder;
                });

            rateTableRepo.Setup(r => r.CleanupRates())
                .Callback(() =>
                {
                    callOrder++;
                    cleanupCalled = callOrder;
                });

            testObject.SaveRates(upsAccount);

            Assert.Equal(1, saveCalled);
            Assert.Equal(2, cleanupCalled);
        }

        [Fact]
        public void SaveRates_SavesNewRateTable()
        {
            ReplaceRatesAndSurcharges(testObject);

            var rateTableRepo = mock.Mock<IUpsLocalRateTableRepository>();
            rateTableRepo.Setup(r => r.GetRateTable(It.IsAny<UpsAccountEntity>())).Returns(new UpsRateTableEntity(42));

            testObject.Load(upsAccount);
            testObject.SaveRates(upsAccount);
            rateTableRepo
                .Verify(r => r.Save(It.Is<UpsRateTableEntity>(t => t.UpsRateTableID == 0), upsAccount), Times.Once);
        }

        [Fact]
        public void SaveZones_SetsUploadDate()
        {
            testObject.ReplaceDeliveryAreaSurcharges(new List<UpsLocalRatingDeliveryAreaSurchargeEntity> { new UpsLocalRatingDeliveryAreaSurchargeEntity() });
            testObject.ReplaceZones(new List<UpsLocalRatingZoneEntity> { new UpsLocalRatingZoneEntity() });

            Assert.Null(testObject.ZoneUploadDate);
            testObject.SaveZones();
            Assert.NotNull(testObject.ZoneUploadDate);
        }

        [Fact]
        public void SaveZones_DelegatesSaveToUpsLocalRateTableRepository()
        {
            testObject.ReplaceDeliveryAreaSurcharges(new List<UpsLocalRatingDeliveryAreaSurchargeEntity> { new UpsLocalRatingDeliveryAreaSurchargeEntity() });
            testObject.ReplaceZones(new List<UpsLocalRatingZoneEntity> { new UpsLocalRatingZoneEntity() });

            testObject.SaveZones();
            mock.Mock<IUpsLocalRateTableRepository>()
                .Verify(r => r.Save(It.IsAny<UpsLocalRatingZoneFileEntity>()), Times.Once);
        }

        [Fact]
        public void SaveZones_DelegatesCleanupToUpsLocalRateTableRepository()
        {
            testObject.ReplaceDeliveryAreaSurcharges(new List<UpsLocalRatingDeliveryAreaSurchargeEntity> { new UpsLocalRatingDeliveryAreaSurchargeEntity() });
            testObject.ReplaceZones(new List<UpsLocalRatingZoneEntity> { new UpsLocalRatingZoneEntity() });

            testObject.SaveZones();
            mock.Mock<IUpsLocalRateTableRepository>().Verify(r => r.CleanupZones(), Times.Once);
        }

        [Fact]
        public void SaveZones_CallsCleanupZones_AfterSaveZones()
        {
            // Adds zones and surcharges to save prior to testing save functionality below
            testObject.ReplaceDeliveryAreaSurcharges(new List<UpsLocalRatingDeliveryAreaSurchargeEntity> { new UpsLocalRatingDeliveryAreaSurchargeEntity() });
            testObject.ReplaceZones(new List<UpsLocalRatingZoneEntity> { new UpsLocalRatingZoneEntity() });

            int callOrder = 0;
            int saveCalled = 0;
            int cleanupCalled = 0;

            var rateTableRepo = mock.Mock<IUpsLocalRateTableRepository>();
            rateTableRepo.Setup(r => r.Save(It.IsAny<UpsLocalRatingZoneFileEntity>()))
                .Callback(() =>
                {
                    callOrder++;
                    saveCalled = callOrder;
                });

            rateTableRepo.Setup(r => r.CleanupZones())
                .Callback(() =>
                {
                    callOrder++;
                    cleanupCalled = callOrder;
                });


            testObject.SaveZones();
            Assert.Equal(1, saveCalled);
            Assert.Equal(2, cleanupCalled);
        }

        [Fact]
        public void AddRates_DelegatesToImportedRateValidator()
        {
            ReplaceRatesAndSurcharges(testObject);

            mock.Mock<IUpsImportedRateValidator>()
                .Verify(v => v.Validate(
                        It.Is<List<IUpsPackageRateEntity>>(
                            r => (r.Single() as ReadOnlyUpsPackageRateEntity).UpsPackageRateID == 2),
                        It.Is<List<IUpsLetterRateEntity>>(
                            r => (r.Single() as ReadOnlyUpsLetterRateEntity).UpsLetterRateID == 3),
                        It.Is<List<IUpsPricePerPoundEntity>>(
                            r => (r.Single() as ReadOnlyUpsPricePerPoundEntity).UpsPricePerPoundID == 4)),
                    Times.Once);
        }

        [Fact]
        public void LoadRates_ThrowsUpsLocalRatingException_WhenStreamIsNull()
        {
            var exception = Assert.Throws<UpsLocalRatingException>(() => testObject.LoadRates(null));
            Assert.Equal("Error loading rate file from stream.", exception.Message);
        }

        [Fact]
        public void LoadRates_CallsReadOnExcelReaders()
        {
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IWorkbook workbook = excelEngine.Excel.Workbooks.Create(2);
                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Position = 0;
                    var reader1 = mock.CreateMock<IUpsRateExcelReader>();
                    var reader2 = mock.CreateMock<IUpsRateExcelReader>();
                    var excelReaderFactory = mock.Mock<IUpsLocalRateExcelReaderFactory>();
                    excelReaderFactory.Setup(e => e.CreateRateExcelReaders())
                        .Returns(new[] { reader1.Object, reader2.Object });

                    var localTestObject = mock.Create<UpsLocalRateTable>();
                    localTestObject.LoadRates(stream);

                    reader1.Verify(r => r.Read(It.IsAny<IWorksheets>(), localTestObject), Times.Once);
                    reader2.Verify(r => r.Read(It.IsAny<IWorksheets>(), localTestObject), Times.Once);
                }
            }
        }

        [Fact]
        public void LoadZones_CallsReadOnExcelReaders()
        {
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IWorkbook workbook = excelEngine.Excel.Workbooks.Create(2);
                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Position = 0;
                    var reader1 = mock.CreateMock<IUpsZoneExcelReader>();
                    var reader2 = mock.CreateMock<IUpsZoneExcelReader>();

                    var excelReaderFactory = mock.Mock<IUpsLocalRateExcelReaderFactory>();
                    excelReaderFactory.Setup(e => e.CreateZoneExcelReaders())
                        .Returns(new[] { reader1.Object, reader2.Object });

                    var localTestObject = mock.Create<UpsLocalRateTable>();
                    localTestObject.LoadZones(stream);

                    reader1.Verify(r => r.Read(It.IsAny<IWorksheets>(), localTestObject), Times.Once);
                    reader2.Verify(r => r.Read(It.IsAny<IWorksheets>(), localTestObject), Times.Once);
                }
            }
        }

        [Fact]
        public void LoadZones_SavesFileContentToEntity()
        {
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IWorkbook workbook = excelEngine.Excel.Workbooks.Create(2);
                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Position = 0;
                    UpsLocalRatingZoneFileEntity zoneFileEntity = new UpsLocalRatingZoneFileEntity();

                    mock.Mock<IUpsLocalRateTableRepository>()
                        .Setup(r => r.Save(It.IsAny<UpsLocalRatingZoneFileEntity>()))
                        .Callback<UpsLocalRatingZoneFileEntity>(entity => zoneFileEntity = entity);

                    testObject.ReplaceDeliveryAreaSurcharges(new List<UpsLocalRatingDeliveryAreaSurchargeEntity> { new UpsLocalRatingDeliveryAreaSurchargeEntity() });
                    testObject.ReplaceZones(new List<UpsLocalRatingZoneEntity> { new UpsLocalRatingZoneEntity() });
                    testObject.LoadZones(stream);
                    testObject.SaveZones();

                    Assert.Equal(stream.ToArray(), zoneFileEntity.FileContent);
                }
            }
        }

        [Fact]
        public void LoadZones_ThrowsUpsLocalRatingException_WhenStreamIsNull()
        {
            var exception = Assert.Throws<UpsLocalRatingException>(() => testObject.LoadZones(null));
            Assert.Equal("Error loading zone file from stream.", exception.Message);
        }

        private static void ReplaceRatesAndSurcharges(UpsLocalRateTable testObject)
        {
            List<UpsPackageRateEntity> packageRates = new List<UpsPackageRateEntity> { new UpsPackageRateEntity(2) };
            List<UpsLetterRateEntity> letterRate = new List<UpsLetterRateEntity> { new UpsLetterRateEntity(3) };
            List<UpsPricePerPoundEntity> pricesPerPound = new List<UpsPricePerPoundEntity> { new UpsPricePerPoundEntity(4) };
            List<UpsRateSurchargeEntity> surcharges = new List<UpsRateSurchargeEntity> { new UpsRateSurchargeEntity(5) };

            testObject.ReplaceRates(packageRates, letterRate, pricesPerPound);
            testObject.ReplaceSurcharges(surcharges);
        }

        #endregion "Replacing And Saving Rates"

        #region Calculate Rates


        [Fact]
        public void CalculateRates_ThrowsUpsLocalRatingException_WhenShipmentPostalCodeIsNotValid()
        {
            UpsShipmentEntity upsShipment =
                new UpsShipmentEntity
                {
                    Packages = { new UpsPackageEntity() },
                    Shipment = new ShipmentEntity()
                };

            upsShipment.Shipment.OriginPostalCode = "12345";
            upsShipment.Shipment.ShipPostalCode = "abcde";

            var result = testObject.CalculateRates(upsShipment.Shipment);

            Assert.True(result.Failure);
            Assert.Equal("Unable to find zone using destination postal code abcde.", result.Message);
        }

        [Fact]
        public void CalculateRates_ReturnsRateListWithRatesFromAllPackages_WhenShipmentHasMultiplePackages()
        {
            var package1 = new UpsPackageEntity()
            {
                PackagingType = (int) UpsPackagingType.Custom,
                Weight = 10
            };

            var package2 = new UpsPackageEntity()
            {
                PackagingType = (int) UpsPackagingType.Custom,
                Weight = 12
            };

            var shipment = new ShipmentEntity()
            {
                Ups = new UpsShipmentEntity()
                {
                    UpsAccountID = 42,
                    Packages = {package1, package2}
                },
                OriginPostalCode = "12345",
                ShipPostalCode = "45678",
            };

            // Define a service filter to return all services it receives
            mock.Mock<IServiceFilter>()
                .Setup(f => f.GetEligibleServices(shipment.Ups, It.IsAny<IEnumerable<UpsServiceType>>()))
                .Returns<UpsShipmentEntity, IEnumerable<UpsServiceType>>((_, types) => types);

            var repo = mock.Mock<IUpsLocalRateTableRepository>();
            string[] zones = new[] {"123"};
            repo.Setup(r => r.GetZones(12345, 45678))
                .Returns(zones);

            var rateForPackage1 = new UpsLocalServiceRate(UpsServiceType.UpsGround, "1", 10, "10");
            repo.Setup(r => r.GetPricePerPoundRates(42, zones, 151)).Returns(new[] {rateForPackage1});
            repo.Setup(r => r.GetPackageRates(42, zones, 10)).Returns(new[] {rateForPackage1});

            var rateForPackage2 = new UpsLocalServiceRate(UpsServiceType.UpsGround, "1", 22, "10");
            repo.Setup(r => r.GetPricePerPoundRates(42, zones, 151)).Returns(new[] {rateForPackage2});
            repo.Setup(r => r.GetPackageRates(42, zones, 12)).Returns(new[] {rateForPackage2});

            var calculatedRates = testObject.CalculateRates(shipment);

            Assert.True(calculatedRates.Success);
            Assert.Equal(rateForPackage1, calculatedRates.Value.Single());
            Assert.Equal(10 + 22, rateForPackage1.Amount);
        }

        [Fact]
        public void CalculateRates_ReturnsLetterRate_WhenPackageTypeIsLetter()
        {
            var shipment = new ShipmentEntity()
            {
                Ups = new UpsShipmentEntity()
                {
                    UpsAccountID = 42,
                },
                OriginPostalCode = "12345",
                ShipPostalCode = "45678",
            };

            shipment.Ups.Packages.Add(new UpsPackageEntity()
            {
                PackagingType = (int) UpsPackagingType.Letter,
                Weight = .1
            });

            // Define a service filter to return all services it receives
            mock.Mock<IServiceFilter>()
                .Setup(f => f.GetEligibleServices(shipment.Ups, It.IsAny<IEnumerable<UpsServiceType>>()))
                .Returns<UpsShipmentEntity, IEnumerable<UpsServiceType>>((_, types) => types);

            var repo = mock.Mock<IUpsLocalRateTableRepository>();
            string[] zones = new[] {"123"};
            repo.Setup(r => r.GetZones(12345, 45678))
                .Returns(zones);

            var rate = new UpsLocalServiceRate(UpsServiceType.Ups2DayAir, "1", 42, "10");
            repo.Setup(r => r.GetLetterRates(42, zones))
                .Returns(new[] {rate});

            var result = testObject.CalculateRates(shipment);

            Assert.True(result.Success);
            Assert.Equal(rate, result.Value.Single());
        }

        [Fact]
        public void CalculateRates_ReturnsPricePerPound_WhenShipmentIsMoreThan150LBS()
        {
            var shipment = new ShipmentEntity()
            {
                Ups = new UpsShipmentEntity()
                {
                    UpsAccountID = 42,
                },
                OriginPostalCode = "12345",
                ShipPostalCode = "45678",
            };

            shipment.Ups.Packages.Add(new UpsPackageEntity()
            {
                PackagingType = (int) UpsPackagingType.Letter,
                Weight = 151
            });

            // Define a service filter to return all services it receives
            mock.Mock<IServiceFilter>()
                .Setup(f => f.GetEligibleServices(shipment.Ups, It.IsAny<IEnumerable<UpsServiceType>>()))
                .Returns<UpsShipmentEntity, IEnumerable<UpsServiceType>>((_, types) => types);

            string[] zones = new[] { "123" };
            
            
            var repo = mock.Mock<IUpsLocalRateTableRepository>();
            repo.Setup(r => r.GetZones(12345, 45678))
                .Returns(zones);

            var rate = new UpsLocalServiceRate(UpsServiceType.Ups2DayAir, "1", 42, "10");
            repo.Setup(r => r.GetPricePerPoundRates(42, zones, 151))
                .Returns(new[] { rate });

            var result = testObject.CalculateRates(shipment);

            Assert.True(result.Success);
            Assert.Equal(rate, result.Value.Single());
        }

        [Fact]
        public void CalculateRates_ReturnsFailedGenericResult_WhenNoRatesExistForZone()
        {
            var shipment = new ShipmentEntity()
            {
                Ups = new UpsShipmentEntity()
                {
                    UpsAccountID = 42,
                },
                OriginPostalCode = "12345",
                ShipPostalCode = "45678",
            };

            shipment.Ups.Packages.Add(new UpsPackageEntity()
            {
                PackagingType = (int) UpsPackagingType.Letter,
                Weight = 151
            });

            // Define a service filter to return all services it receives
            mock.Mock<IServiceFilter>()
                .Setup(f => f.GetEligibleServices(shipment.Ups, It.IsAny<IEnumerable<UpsServiceType>>()))
                .Returns<UpsShipmentEntity, IEnumerable<UpsServiceType>>((_, types) => types);

            string[] zones = { "123" };
            var repo = mock.Mock<IUpsLocalRateTableRepository>();
            repo.Setup(r => r.GetZones(12345, 45678))
                .Returns(zones);

            repo.Setup(r => r.GetPricePerPoundRates(42L, zones, 151))
                .Returns(new UpsLocalServiceRate[0]);

            var result = testObject.CalculateRates(shipment);

            Assert.True(result.Failure);
            Assert.Equal("No local rates found.", result.Message);

        }

        [Fact]
        public void CalculateRates_ReturnsFailedGenericResult_WhenZoneCannotBeFoundBetweenDestinationAndOrigin()
        {
            var shipment = new ShipmentEntity()
            {
                Ups = new UpsShipmentEntity()
                {
                    UpsAccountID = 42,
                },
                OriginPostalCode = "12345",
                ShipPostalCode = "45678",
            };

            var repo = mock.Mock<IUpsLocalRateTableRepository>();
            repo.Setup(r => r.GetZones(12345, 45678))
                .Returns(new string[0]);

            var result = testObject.CalculateRates(shipment);

            Assert.True(result.Failure);
            Assert.Equal("Unable to find zone using origin postal code 12345 and destination postal code 45678.", result.Message);
        }

        [Fact]
        public void Calculate_SurchargesAppliedToRates()
        {
            var shipment = new ShipmentEntity()
            {
                Ups = new UpsShipmentEntity()
                {
                    UpsAccountID = 42,
                },
                OriginPostalCode = "12345",
                ShipPostalCode = "45678",
            };

            shipment.Ups.Packages.Add(new UpsPackageEntity()
            {
                PackagingType = (int) UpsPackagingType.Letter,
                Weight = .1
            });

            // Define a service filter to return all services it receives
            mock.Mock<IServiceFilter>()
                .Setup(f => f.GetEligibleServices(shipment.Ups, It.IsAny<IEnumerable<UpsServiceType>>()))
                .Returns<UpsShipmentEntity, IEnumerable<UpsServiceType>>((_, types) => types);

            string[] zones = new[] { "123" };
            var repo = mock.Mock<IUpsLocalRateTableRepository>();
            repo.Setup(r => r.GetZones(12345, 45678))
                .Returns(zones);

            var rate1 = new UpsLocalServiceRate(UpsServiceType.Ups2DayAir, "1", 42, "10");
            var rate2 = new UpsLocalServiceRate(UpsServiceType.UpsGround, "1", 42, "10");
            repo.Setup(r => r.GetLetterRates(42L, zones)).Returns(new[] { rate1, rate2 });
            
            Mock<IUpsSurcharge> surcharge = mock.CreateMock<IUpsSurcharge>();
            mock.Mock<IUpsSurchargeFactory>()
                .Setup(f => f.Get(It.IsAny<IDictionary<UpsSurchargeType, double>>(), It.IsAny<UpsLocalRatingZoneFileEntity>()))
                .Returns(new[] { surcharge.Object, surcharge.Object });

            testObject.CalculateRates(shipment);

            surcharge.Verify(s => s.Apply(shipment.Ups, rate1), Times.Exactly(2));
            surcharge.Verify(s => s.Apply(shipment.Ups, rate2), Times.Exactly(2));
        }

        [Fact]
        public void CalculateRates_FiltersOutRates_WithServicesExcludedInServiceFilter()
        {
            var shipment = new ShipmentEntity()
            {
                Ups = new UpsShipmentEntity()
                {
                    UpsAccountID = 42,
                },
                OriginPostalCode = "12345",
                ShipPostalCode = "45678",
            };

            shipment.Ups.Packages.Add(new UpsPackageEntity()
            {
                PackagingType = (int) UpsPackagingType.Letter,
                Weight = .1
            });

            // Define a service filter to return all services it receives
            mock.Mock<IServiceFilter>()
                .Setup(f => f.GetEligibleServices(shipment.Ups, It.IsAny<IEnumerable<UpsServiceType>>()))
                .Returns<UpsShipmentEntity, IEnumerable<UpsServiceType>>(
                    (_, types) => types.Where(t => t != UpsServiceType.Ups2DayAir));

            var repo = mock.Mock<IUpsLocalRateTableRepository>();
            string[] zones = new[] {"123"};
            repo.Setup(r => r.GetZones(12345, 45678))
                .Returns(zones);

            var rate1 = new UpsLocalServiceRate(UpsServiceType.Ups2DayAir, "1", 42, "10");
            var rate2 = new UpsLocalServiceRate(UpsServiceType.UpsGround, "1", 42, "10");

            repo.Setup(r => r.GetLetterRates(42, zones))
                .Returns(new[] { rate1, rate2 });

            var result = testObject.CalculateRates(shipment);
            Assert.True(result.Success);
            Assert.Equal(rate2, result.Value.Single());
        }

        [Fact]
        public void CalculateRates_ReturnsGenericResultWithFailure_WhenRepoThrowsUpsLocalRatingException()
        {
            var shipment = new ShipmentEntity()
            {
                Ups = new UpsShipmentEntity()
                {
                    UpsAccountID = 42,
                },
                OriginPostalCode = "12345",
                ShipPostalCode = "45678",
            };

            mock.Mock<IUpsLocalRateTableRepository>()
                .Setup(r => r.GetZones(12345, 45678))
                .Throws(new UpsLocalRatingException("my error"));

            var result = testObject.CalculateRates(shipment);
            Assert.True(result.Failure);
            Assert.Equal("my error", result.Message);
        }

        #endregion Calculate Rates

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}