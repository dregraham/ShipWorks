using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Shipping.Carriers.Ups.LocalRating;
using ShipWorks.Tests.Shared;
using Syncfusion.XlsIO;
using Xunit;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using Xunit.Sdk;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.LocalRating
{
    public class ServiceUpsRateExcelReaderTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly Mock<IUpsLocalRateTable> rateTable;
        private IEnumerable<UpsPackageRateEntity> readPackageRates;
        private readonly ServiceUpsRateExcelReader testObject;
        private readonly ExcelEngine excelEngine;
        private IEnumerable<UpsLetterRateEntity> readLetterRates;
        private IEnumerable<UpsPricePerPoundEntity> readPricesPerPound;

        public ServiceUpsRateExcelReaderTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            rateTable = mock.CreateMock<IUpsLocalRateTable>(table =>
            {
                table.Setup(
                        t =>
                            t.ReplaceRates(It.IsAny<IEnumerable<UpsPackageRateEntity>>(),
                                It.IsAny<IEnumerable<UpsLetterRateEntity>>(),
                                It.IsAny<IEnumerable<UpsPricePerPoundEntity>>()))
                    .Callback
                    <IEnumerable<UpsPackageRateEntity>, IEnumerable<UpsLetterRateEntity>,
                        IEnumerable<UpsPricePerPoundEntity>>(SaveReadRates);
            });

            testObject = mock.Create<ServiceUpsRateExcelReader>();

            excelEngine = new ExcelEngine();
        }

        private void SaveReadRates(IEnumerable<UpsPackageRateEntity> packageRates, IEnumerable<UpsLetterRateEntity> letterRates, IEnumerable<UpsPricePerPoundEntity> pricesPerPound)
        {
            readPackageRates = packageRates;
            readLetterRates = letterRates;
            readPricesPerPound = pricesPerPound;
        }

        [Fact]
        public void Read_CallsAddRates()
        {
                IWorksheets sheets = SetupSingleRateSheet();
                testObject.Read(sheets, rateTable.Object);

                rateTable.Verify(t => t.ReplaceRates(It.IsAny<IEnumerable<UpsPackageRateEntity>>(),
                                It.IsAny<IEnumerable<UpsLetterRateEntity>>(),
                                It.IsAny<IEnumerable<UpsPricePerPoundEntity>>()), Times.Once);
        }

        [Fact]
        public void Read_StoresValidRateForWeightZoneAndService()
        {
            IWorksheets sheets = SetupSingleRateSheet();
            testObject.Read(sheets, rateTable.Object);

            Assert.Equal(1, readPackageRates.Count());
            Assert.Empty(readPricesPerPound);
            Assert.Empty(readLetterRates);

            UpsPackageRateEntity rate = readPackageRates.Single();
            Assert.Equal(102, rate.Zone);
            Assert.Equal(50, rate.WeightInPounds);
            Assert.Equal((int) UpsServiceType.UpsNextDayAirAM, rate.Service);
            Assert.Equal(42.42m, rate.Rate);
        }

        [Fact]
        public void Read_LetterIsStoredInLetterTable()
        {
            IWorksheets sheets = SetupSingleRateSheet();
            sheets[0].Range["A2"].Text = "Letter";
            
            testObject.Read(sheets, rateTable.Object);

            Assert.Empty(readPackageRates);
            Assert.Empty(readPricesPerPound);
            Assert.Equal(1, readLetterRates.Count());

            var rate = readLetterRates.Single();
            Assert.Equal(102, rate.Zone);
            Assert.Equal((int) UpsServiceType.UpsNextDayAirAM, rate.Service);
            Assert.Equal(42.42m, rate.Rate);
        }

        [Fact]
        public void Read_PricePerPoundIsStoredInPricePerPoundTable()
        {
            IWorksheets sheets = SetupSingleRateSheet();
            sheets[0].Range["A2"].Text = "Price Per Pound";

            testObject.Read(sheets, rateTable.Object);

            Assert.Empty(readPackageRates);
            Assert.Empty(readLetterRates);
            Assert.Equal(1, readPricesPerPound.Count());

            var pricePerPoundEntity = readPricesPerPound.Single();
            Assert.Equal(102, pricePerPoundEntity.Zone);
            Assert.Equal((int) UpsServiceType.UpsNextDayAirAM, pricePerPoundEntity.Service);
            Assert.Equal(42.42m, pricePerPoundEntity.Rate);
        }

        private IWorksheets SetupSingleRateSheet()
        {
            IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);
            IWorksheets sheets = workbook.Worksheets;
            IWorksheet sheet = sheets[0];

            sheet.Name = "NDA Early";
            sheet.Range["A1"].Text = "Zones";
            sheet.Range["B1"].Value2 = 102;
            sheet.Range["A2"].Value2 = 50;
            sheet.Range["B2"].Value2 = 42.42;
            return sheets;
        }

        [Fact]
        public void Read_MultipleZonesStored_WhenSheetHasMultipleZoneColumns()
        {
            IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);
            IWorksheets sheets = workbook.Worksheets;
            IWorksheet sheet = sheets[0];

            sheet.Name = "NDA Early";
            sheet.Range["A1"].Text = "Zones";
            sheet.Range["B1"].Value2= 102;
            sheet.Range["C1"].Value2= 103;

            sheet.Range["A2"].Value2= 50;
            sheet.Range["B2"].Value2= 42.42;
            sheet.Range["C2"].Value2= 3.50;

            testObject.Read(sheets, rateTable.Object);

            Assert.Equal(2, readPackageRates.Count());

            UpsPackageRateEntity rate = readPackageRates.ElementAt(0);
            Assert.Equal(102, rate.Zone);
            Assert.Equal(50, rate.WeightInPounds);
            Assert.Equal((int) UpsServiceType.UpsNextDayAirAM, rate.Service);
            Assert.Equal(42.42m, rate.Rate);

            rate = readPackageRates.ElementAt(1);
            Assert.Equal(103, rate.Zone);
            Assert.Equal(50, rate.WeightInPounds);
            Assert.Equal((int) UpsServiceType.UpsNextDayAirAM, rate.Service);
            Assert.Equal(3.5m, rate.Rate);
        }

        [Fact]
        public void Read_MultipleServicesStored_WhenSheetHasMultipleServices()
        {
            IWorkbook workbook = excelEngine.Excel.Workbooks.Create(2);
            IWorksheets sheets = workbook.Worksheets;

            IWorksheet ndaEarlySheet = sheets[0];
            ndaEarlySheet.Name = "NDA Early";
            ndaEarlySheet.Range["A1"].Text = "Zones";
            ndaEarlySheet.Range["B1"].Value2 = 102;
            ndaEarlySheet.Range["A2"].Value2 = 50;
            ndaEarlySheet.Range["B2"].Value2 = 42.42;

            IWorksheet ndaSheet = sheets[1];
            ndaSheet.Name = "NDA";
            ndaSheet.Range["A1"].Text = "Zones";
            ndaSheet.Range["B1"].Value2 = 400;
            ndaSheet.Range["A2"].Value2 = 6;
            ndaSheet.Range["B2"].Value2 = 7.25;

            testObject.Read(sheets, rateTable.Object);
            Assert.Equal(2, readPackageRates.Count());

            UpsPackageRateEntity rate = readPackageRates.ElementAt(0);
            Assert.Equal(102, rate.Zone);
            Assert.Equal(50, rate.WeightInPounds);
            Assert.Equal((int) UpsServiceType.UpsNextDayAirAM, rate.Service);
            Assert.Equal(42.42m, rate.Rate);

            rate = readPackageRates.ElementAt(1);
            Assert.Equal(400, rate.Zone);
            Assert.Equal(6, rate.WeightInPounds);
            Assert.Equal((int) UpsServiceType.UpsNextDayAir, rate.Service);
            Assert.Equal(7.25m, rate.Rate);
        }

        [Fact]
        public void Read_IgnoresTab_WhenNameNotMappedToService()
        {
            IWorksheets sheets = SetupSingleRateSheet();
            sheets[0].Name = "Not A Service";

            testObject.Read(sheets, rateTable.Object);

            Assert.Empty(readLetterRates);
            Assert.Empty(readPackageRates);
            Assert.Empty(readPricesPerPound);
        }

        [Fact]
        public void Read_ValueIgnoredIfNoHeader()
        {
            IWorksheets sheets = SetupSingleRateSheet();
            sheets[0].Range["B1"].Text = string.Empty;
            sheets[0].Range["B2"].Text = "ignore this";

            testObject.Read(sheets, rateTable.Object);

            Assert.Empty(readLetterRates);
            Assert.Empty(readPackageRates);
            Assert.Empty(readPricesPerPound);
        }

        [Theory]
        [InlineData("bad string value")]
        [InlineData("1.5")]
        public void Read_ThrowsLocalRatingException_IfZoneHeaderNotInt(string headerValue)
        {
            IWorksheets sheets = SetupSingleRateSheet();
            sheets[0].Range["B1"].Text = headerValue;

            var exception = Record.Exception(() => testObject.Read(sheets, rateTable.Object));
            Assert.NotNull(exception);
            Assert.Contains(headerValue, exception.Message);
        }

        [Fact]
        public void Read_ThrowsLocalRatingException_IfRateNotDecimalDashOrEmpty()
        {
            IWorksheets sheets = SetupSingleRateSheet();
            sheets[0].Range["B2"].Text = "blah";

            var exception = Record.Exception(() => testObject.Read(sheets, rateTable.Object));

            Assert.NotNull(exception);
            Assert.Contains("blah", exception.Message);
        }

        [Fact]
        public void Read_SavesRateAsZero_IfRateCellIsDash()
        {
            IWorksheets sheets = SetupSingleRateSheet();
            sheets[0].Range["B2"].Text = "-";

            testObject.Read(sheets, rateTable.Object);
            
            Assert.Equal(0, readPackageRates.ElementAt(0).Rate);
        }

        [Fact]
        public void Read_SavesRateAsZero_IfRateCellIsEmpty()
        {
            IWorksheets sheets = SetupSingleRateSheet();
            sheets[0].Range["B2"].Text = string.Empty;

            testObject.Read(sheets, rateTable.Object);

            Assert.Equal(0, readPackageRates.ElementAt(0).Rate);
        }

        [Fact]
        public void Read_ThrowsLocalRatingException_IfSheetHasNoRows()
        {
            IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);
            IWorksheets sheets = workbook.Worksheets;
            IWorksheet sheet = sheets[0];

            sheet.Name = "NDA Early";

            var exception = Record.Exception(() => testObject.Read(sheets, rateTable.Object));
            Assert.Equal("Sheet 'NDA Early' has no rows.", exception.Message);
        }

        [Fact]
        public void Validate_Throws_WhenDuplicateZone()
        {
            IWorksheets sheets = SetupSingleRateSheet();
            sheets[0].Name = "NDA";
            sheets[0].Range["B1"].Number = 100;
            sheets[0].Range["C1"].Number = 100;

            var exception = Record.Exception(() => testObject.Read(sheets, rateTable.Object));

            Assert.Equal($"Duplicate zone detected in sheet '{sheets[0].Name}'.", exception.Message);
        }

        public void Dispose()
        {
            excelEngine.Dispose();
            mock.Dispose();
        }
    }
}