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

namespace ShipWorks.Shipping.Tests.Carriers.UPS.LocalRating
{
    public class ServiceUpsRateExcelReaderTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly Mock<IUpsLocalRateTable> rateTable;
        private IEnumerable<UpsPackageRateEntity> readRates;
        private readonly ServiceUpsRateExcelReader testObject;
        private readonly ExcelEngine excelEngine;

        public ServiceUpsRateExcelReaderTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            rateTable = mock.CreateMock<IUpsLocalRateTable>(table =>
            {
                table.Setup(t => t.AddRates(It.IsAny<IEnumerable<UpsPackageRateEntity>>()))
                    .Callback<IEnumerable<UpsPackageRateEntity>>(rates => readRates = rates);
            });

            testObject = mock.Create<ServiceUpsRateExcelReader>();

            excelEngine = new ExcelEngine();
        }

        [Fact]
        public void Read_CallsAddRates()
        {
                IWorksheets sheets = SetupSingleRateSheet();
                testObject.Read(sheets, rateTable.Object);

                rateTable.Verify(t => t.AddRates(It.IsAny<IEnumerable<UpsPackageRateEntity>>()), Times.Once);
        }

        [Fact]
        public void Read_StoresValidRateForWeightZoneAndService()
        {
            IWorksheets sheets = SetupSingleRateSheet();
            testObject.Read(sheets, rateTable.Object);

            Assert.Equal(1, readRates.Count());

            UpsPackageRateEntity rate = readRates.Single();
            Assert.Equal(102, rate.Zone);
            Assert.Equal(50, rate.WeightInPounds);
            Assert.Equal((int) UpsServiceType.UpsNextDayAirAM, rate.Service);
            Assert.Equal(42.42m, rate.Rate);
        }

        [Fact]
        public void Read_LetterIsStoredWithZeroWeight()
        {
            IWorksheets sheets = SetupSingleRateSheet();
            sheets[0].Range["A2"].Text = "Letter";
            
            testObject.Read(sheets, rateTable.Object);

            Assert.Equal(1, readRates.Count());

            UpsPackageRateEntity rate = readRates.Single();
            Assert.Equal(102, rate.Zone);
            Assert.Equal(0, rate.WeightInPounds);
            Assert.Equal((int) UpsServiceType.UpsNextDayAirAM, rate.Service);
            Assert.Equal(42.42m, rate.Rate);
        }

        private IWorksheets SetupSingleRateSheet()
        {
            IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);
            IWorksheets sheets = workbook.Worksheets;
            IWorksheet sheet = sheets[0];

            sheet.Name = "NDA Early";
            sheet.Range["A1"].Text = "Zones";
            sheet.Range["B1"].Text = "102";
            sheet.Range["A2"].Text = "50";
            sheet.Range["B2"].Text = "42.42";
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
            sheet.Range["B1"].Text = "102";
            sheet.Range["C1"].Text = "103";

            sheet.Range["A2"].Text = "50";
            sheet.Range["B2"].Text = "42.42";
            sheet.Range["C2"].Text = "3.50";

            testObject.Read(sheets, rateTable.Object);

            Assert.Equal(2, readRates.Count());

            UpsPackageRateEntity rate = readRates.ElementAt(0);
            Assert.Equal(102, rate.Zone);
            Assert.Equal(50, rate.WeightInPounds);
            Assert.Equal((int) UpsServiceType.UpsNextDayAirAM, rate.Service);
            Assert.Equal(42.42m, rate.Rate);

            rate = readRates.ElementAt(1);
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
            ndaEarlySheet.Range["B1"].Text = "102";
            ndaEarlySheet.Range["A2"].Text = "50";
            ndaEarlySheet.Range["B2"].Text = "42.42";

            IWorksheet ndaSheet = sheets[1];
            ndaSheet.Name = "NDA";
            ndaSheet.Range["A1"].Text = "Zones";
            ndaSheet.Range["B1"].Text = "400";
            ndaSheet.Range["A2"].Text = "6";
            ndaSheet.Range["B2"].Text = "7.25";

            testObject.Read(sheets, rateTable.Object);
            Assert.Equal(2, readRates.Count());

            UpsPackageRateEntity rate = readRates.ElementAt(0);
            Assert.Equal(102, rate.Zone);
            Assert.Equal(50, rate.WeightInPounds);
            Assert.Equal((int) UpsServiceType.UpsNextDayAirAM, rate.Service);
            Assert.Equal(42.42m, rate.Rate);

            rate = readRates.ElementAt(1);
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

            rateTable.Verify(t => t.AddRates(It.IsAny<IEnumerable<UpsPackageRateEntity>>()), Times.Never);
        }

        [Fact]
        public void Read_ValueIgnoredIfNoHeader()
        {
            IWorksheets sheets = SetupSingleRateSheet();
            sheets[0].Range["B1"].Text = string.Empty;
            sheets[0].Range["B2"].Text = "ignore this";

            testObject.Read(sheets, rateTable.Object);

            rateTable.Verify(t => t.AddRates(It.IsAny<IEnumerable<UpsPackageRateEntity>>()), Times.Never);
        }

        [Fact]
        public void Read_ValueIgnoredIfBlank()
        {
            IWorksheets sheets = SetupSingleRateSheet();
            sheets[0].Range["B2"].Value2 = string.Empty;

            testObject.Read(sheets, rateTable.Object);

            rateTable.Verify(t => t.AddRates(It.IsAny<IEnumerable<UpsPackageRateEntity>>()), Times.Never);
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
        public void Read_ThrowsLocalRatingException_IfRateNotDecimal()
        {
            IWorksheets sheets = SetupSingleRateSheet();
            sheets[0].Range["B2"].Text = "blah";

            var exception = Record.Exception(() => testObject.Read(sheets, rateTable.Object));

            Assert.NotNull(exception);
            Assert.Contains("blah", exception.Message);
        }

        [Fact]
        public void Read_ThrowsLocalRatingException_IfSheetHasNoRows()
        {
            IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);
            IWorksheets sheets = workbook.Worksheets;
            IWorksheet sheet = sheets[0];

            sheet.Name = "NDA Early";

            var exception = Record.Exception(() => testObject.Read(sheets, rateTable.Object));
            Assert.Equal("Sheet NDA Early has no rows.", exception.Message);
        }


        public void Dispose()
        {
            excelEngine.Dispose();
            mock.Dispose();
        }
    }
}