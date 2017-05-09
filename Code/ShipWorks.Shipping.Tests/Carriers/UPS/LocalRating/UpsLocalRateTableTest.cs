using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autofac.Extras.Moq;
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
using ShipWorks.Tests.Shared.EntityBuilders;
using Syncfusion.XlsIO;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.LocalRating
{
    public class UpsLocalRateTableTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly UpsAccountEntity upsAccount;
        private readonly UpsLocalRateTable testObject;
        private readonly ShipmentEntity shipment;

        public UpsLocalRateTableTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            upsAccount = new UpsAccountEntity();
            testObject = mock.Create<UpsLocalRateTable>();
            shipment = Create.Shipment().AsUps().Build();
        }

        #region "Replacing And Saving Rates"

        [Fact]
        public void Load_DelegatesToUpsLocalRateTableRepository()
        {
            testObject.Load(upsAccount);

            mock.Mock<IUpsLocalRateTableRepository>().Verify(r => r.Get(upsAccount));
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
            rateTableRepo.Setup(r => r.Get(It.IsAny<UpsAccountEntity>())).Returns(new UpsRateTableEntity(42));

            testObject.Load(upsAccount);
            testObject.SaveRates(upsAccount);
            rateTableRepo
                .Verify(r => r.Save(It.Is<UpsRateTableEntity>(t => t.UpsRateTableID == 0), upsAccount), Times.Once);
        }
        
        [Fact]
        public void SaveZones_SetsUploadDate()
        {
            testObject.ReplaceDeliveryAreaSurcharges(new List<UpsLocalRatingDeliveryAreaSurchargeEntity>{new UpsLocalRatingDeliveryAreaSurchargeEntity()});
            testObject.ReplaceZones(new List<UpsLocalRatingZoneEntity>{new UpsLocalRatingZoneEntity()});

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

            int callOrder =0;
            int saveCalled=0;
            int cleanupCalled=0;

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
                    mock.Provide<IEnumerable<IUpsRateExcelReader>>(new[] {reader1.Object, reader2.Object});

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
                    mock.Provide<IEnumerable<IUpsZoneExcelReader>>(new[] { reader1.Object, reader2.Object });

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
        public void Calculate_ReturnsRatesFromRepository()
        {
            var serviceRates = new[] { new UpsLocalServiceRate(UpsServiceType.Ups2DayAir, 3.50M, false, 1) };

            mock.Mock<IUpsLocalRateTableRepository>()
                .Setup(r => r.GetServiceRates(shipment.Ups))
                .Returns(serviceRates);

            // Define a service filter to return all services it receives
            mock.Mock<IServiceFilter>()
                .Setup(f => f.GetEligibleServices(shipment.Ups, It.IsAny<IEnumerable<UpsServiceType>>()))
                .Returns<UpsShipmentEntity, IEnumerable<UpsServiceType>>((_, types) => types);

            var result = testObject.CalculateRates(shipment);
            Assert.Equal(serviceRates[0], result.Value.Single());
        }

        [Fact]
        public void Calculate_SurchargesAppliedToRates()
        {
            var serviceRates = new[]
            {
                new UpsLocalServiceRate(UpsServiceType.Ups2DayAir, 3.50M, false, 1),
                new UpsLocalServiceRate(UpsServiceType.UpsGround, 3.50M, false, 1)
            };

            mock.Mock<IUpsLocalRateTableRepository>()
                .Setup(r => r.GetServiceRates(shipment.Ups))
                .Returns(serviceRates);

            // Define a service filter to return all services it receives
            mock.Mock<IServiceFilter>()
                .Setup(f => f.GetEligibleServices(shipment.Ups, It.IsAny<IEnumerable<UpsServiceType>>()))
                .Returns<UpsShipmentEntity, IEnumerable<UpsServiceType>>((_, types) => types);

            Mock<IUpsSurcharge> surcharge = mock.CreateMock<IUpsSurcharge>();
            mock.Mock<IUpsSurchargeFactory>()
                .Setup(f => f.Get(It.IsAny<IDictionary<UpsSurchargeType, double>>(), It.IsAny<UpsLocalRatingZoneFileEntity>()))
                .Returns(new[] { surcharge.Object, surcharge.Object });

            testObject.CalculateRates(shipment);

            surcharge.Verify(s => s.Apply(shipment.Ups, serviceRates[0]), Times.Exactly(2));
            surcharge.Verify(s => s.Apply(shipment.Ups, serviceRates[1]), Times.Exactly(2));
        }

        [Fact]
        public void CalculateRates_FiltersOutRates_WithServicesExcludedInServiceFilter()
        {
            var serviceRates = new[]
            {
                new UpsLocalServiceRate(UpsServiceType.Ups2DayAir, 3.50M, false, 1),
                new UpsLocalServiceRate(UpsServiceType.UpsGround, 3.50M, false, 1)
            };

            mock.Mock<IUpsLocalRateTableRepository>()
                .Setup(r => r.GetServiceRates(shipment.Ups))
                .Returns(serviceRates);

            var filterredServiceTypes = new[] { UpsServiceType.UpsGround };
            mock.Mock<IServiceFilter>()
                .Setup(f => f.GetEligibleServices(shipment.Ups, It.IsAny<IEnumerable<UpsServiceType>>()))
                .Returns<UpsShipmentEntity, IEnumerable<UpsServiceType>>((_, types) => filterredServiceTypes);

            var rateResults = testObject.CalculateRates(shipment);
            var expectedRateResults =
                serviceRates.Where(r => r.Service == UpsServiceType.UpsGround);

            Assert.Equal(expectedRateResults, rateResults.Value);
        }

        [Fact]
        public void CalculateRates_ReturnsGenericResultWithFailure_WhenRepoThrowsUpsLocalRatingException()
        {
            mock.Mock<IUpsLocalRateTableRepository>()
                .Setup(r => r.GetServiceRates(shipment.Ups))
                .Throws(new UpsLocalRatingException("my error"));

            var result = testObject.CalculateRates(shipment);
            Assert.True(result.Failure);
            Assert.Equal("my error", result.Message);
        }

        [Fact]
        public void CalculateRates_ReturnsGenericResultWithFailure_WhenNoRatesReturned()
        {
            var serviceRates = new UpsLocalServiceRate[0];
            var filterredServiceTypes = new UpsServiceType[0];

            mock.Mock<IUpsLocalRateTableRepository>()
                .Setup(r => r.GetServiceRates(shipment.Ups))
                .Returns(serviceRates);

            mock.Mock<IServiceFilter>()
                .Setup(f => f.GetEligibleServices(shipment.Ups, It.IsAny<IEnumerable<UpsServiceType>>()))
                .Returns<UpsShipmentEntity, IEnumerable<UpsServiceType>>((_, types) => filterredServiceTypes);

            var result = testObject.CalculateRates(shipment);

            Assert.True(result.Failure);
            Assert.Equal("No local rates found.", result.Message);
        }

        #endregion Calculate Rates

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}