using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Ups.LocalRating;
using ShipWorks.Tests.Shared;
using Syncfusion.XlsIO;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.LocalRating
{
    public class DeliveryAreaSurchargeUpsZoneExcelReaderTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly ExcelEngine excelEngine;

        private IEnumerable<UpsLocalRatingDeliveryAreaSurchargeEntity> readSurcharges;
        private readonly Mock<IUpsLocalRateTable> rateTable;
        private readonly DeliveryAreaSurchargeUpsZoneExcelReader testObject;

        private readonly string expectedUs48DasHeader = "US 48 DAS";
        private readonly string expectedUs48DasExtendedHeader = "US 48 DAS Extended";
        private readonly string expectedRemoteHiHeader = "Remote HI";
        private readonly string expectedRemoteAkHeader = "Remote AK";

        private const string MissingColumn = "DASzips missing '{0}' column.";
        private const string MissingDasTab = "Spreadsheet missing DASzips Tab";
        private const string TooManyColumns = "DASzips can only have 4 columns. There may be a cell with text outside of the expected 4 columns.";
        private const string DasZipsTabName = "DASzips";
        private const string InvalidZipCode = "DASzips has an invalid zip code for column {0} row {1}.";
        private const string DuplicateColumns = "DASzips can only have 1 '{0}' column.";

        public DeliveryAreaSurchargeUpsZoneExcelReaderTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            rateTable = mock.CreateMock<IUpsLocalRateTable>(table =>
            {
                table.Setup(t => t.ReplaceDeliveryAreaSurcharges(
                        It.IsAny<IEnumerable<UpsLocalRatingDeliveryAreaSurchargeEntity>>()))
                    .Callback<IEnumerable<UpsLocalRatingDeliveryAreaSurchargeEntity>>(read => readSurcharges = read);
            });

            testObject = mock.Create<DeliveryAreaSurchargeUpsZoneExcelReader>();

            excelEngine = new ExcelEngine();
        }

        [Theory]
        [InlineData("00001", UpsDeliveryAreaSurchargeType.Us48Das)]
        [InlineData("00002", UpsDeliveryAreaSurchargeType.Us48DasExtended)]
        [InlineData("00003", UpsDeliveryAreaSurchargeType.UsRemoteHi)]
        [InlineData("00004", UpsDeliveryAreaSurchargeType.UsRemoteAk)]
        public void Read_FirstRowRead_WhenReadingSingleRowDocument(string zip, UpsDeliveryAreaSurchargeType dasType)
        {
            IWorksheets sheets = SetupSheetWithOneZipForEachDas();
            testObject.Read(sheets, rateTable.Object);

            Assert.Equal(1, readSurcharges.Count(s => s.DeliveryAreaType == (int) dasType));
            Assert.Equal(1, readSurcharges.Count(s => s.DeliveryAreaType == (int) dasType && s.DestinationZip == int.Parse(zip)));
        }

        [Fact]
        public void Read_ThrowsLocalRatingException_WhenNoDasZipsTab()
        {
            IWorksheets sheets = SetupSheetWithOneZipForEachDas();
            sheets[0].Name = "60000-70000";
            Exception exception = Record.Exception(() => testObject.Read(sheets, rateTable.Object));

            Assert.NotNull(exception);
            Assert.IsType<UpsLocalRatingException>(exception);
            Assert.Equal(MissingDasTab, exception.Message);
        }

        [Fact]
        public void Read_ThrowsLocalRatingException_WhenDuplicateColumns()
        {
            IWorksheets sheets = SetupSheetWithOneZipForEachDas();
            sheets[0]["B1"].Text = expectedUs48DasHeader;

            string expectedErrorMessage = string.Format(DuplicateColumns, expectedUs48DasHeader);

            Exception exception = Record.Exception(() => testObject.Read(sheets, rateTable.Object));

            Assert.NotNull(exception);
            Assert.IsType<UpsLocalRatingException>(exception);
            Assert.Equal(expectedErrorMessage, exception.Message);
        }

        [Theory]
        [InlineData("E1")]
        [InlineData("E2")]
        public void Read_ThrowsLocalRatingException_WhenMoreThanFourColumns(string columnWithExtraText)
        {
            IWorksheets sheets = SetupSheetWithOneZipForEachDas();
            sheets[0].Range[columnWithExtraText].Text = "Extra";

            Exception exception = Record.Exception(() => testObject.Read(sheets, rateTable.Object));

            Assert.NotNull(exception);
            Assert.IsType<UpsLocalRatingException>(exception);
            Assert.Equal(TooManyColumns, exception.Message);
        }

        [Fact]
        public void Read_ThrowsLocalRatingException_WhenZipcodeIsNotFiveDigits()
        {
            IWorksheets sheets = SetupSheetWithOneZipForEachDas();
            sheets[0].Range["A2"].Text = "BadZip";

            string expectedExceptionMessage = string.Format(InvalidZipCode, expectedUs48DasHeader, 2);

            Exception exception = Record.Exception(() => testObject.Read(sheets, rateTable.Object));

            Assert.NotNull(exception);
            Assert.IsType<UpsLocalRatingException>(exception);
            Assert.Equal(expectedExceptionMessage, exception.Message);
        }

        [Theory]
        [InlineData(UpsDeliveryAreaSurchargeType.Us48Das)]
        [InlineData(UpsDeliveryAreaSurchargeType.Us48DasExtended)]
        [InlineData(UpsDeliveryAreaSurchargeType.UsRemoteAk)]
        [InlineData(UpsDeliveryAreaSurchargeType.UsRemoteHi)]
        public void Read_ThrowsLocalRatingException_WhenMissingDasType(UpsDeliveryAreaSurchargeType missingDasType)
        {
            var sheets = SetupSheetWithMissingColumn(missingDasType);
            Exception exception = Record.Exception(() => testObject.Read(sheets, rateTable.Object));

            string expectedExceptionMessage = string.Format(MissingColumn, EnumHelper.GetApiValue(missingDasType));

            Assert.NotNull(exception);
            Assert.IsType<UpsLocalRatingException>(exception);
            Assert.Equal(expectedExceptionMessage, exception.Message);
        }

        private IWorksheets SetupSheetWithMissingColumn(UpsDeliveryAreaSurchargeType missingDasType)
        {
            IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);
            IWorksheets sheets = workbook.Worksheets;
            IWorksheet sheet = sheets[0];

            sheet.Name = DasZipsTabName;

            var dasTypes = EnumHelper.GetEnumList<UpsDeliveryAreaSurchargeType>(type => type != missingDasType).ToList();
            Assert.Equal(3, dasTypes.Count);

            sheet.Range["A1"].Text = dasTypes[0].ApiValue;
            sheet.Range["B1"].Text = dasTypes[1].ApiValue;
            sheet.Range["C1"].Text = dasTypes[2].ApiValue;
            
            sheet.Range["A2"].Text = "00001";
            sheet.Range["B2"].Text = "00002";
            sheet.Range["C2"].Text = "00003";

            return sheets;
        }

        private IWorksheets SetupSheetWithOneZipForEachDas()
        {
            IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);
            IWorksheets sheets = workbook.Worksheets;
            IWorksheet sheet = sheets[0];

            sheet.Name = DasZipsTabName;
            sheet.Range["A1"].Text = expectedUs48DasHeader;
            sheet.Range["B1"].Text = expectedUs48DasExtendedHeader;
            sheet.Range["C1"].Text = expectedRemoteHiHeader;
            sheet.Range["D1"].Text = expectedRemoteAkHeader;

            sheet.Range["A2"].Text = "00001";
            sheet.Range["B2"].Text = "00002";
            sheet.Range["C2"].Text = "00003";
            sheet.Range["D2"].Text = "00004";
            return sheets;
        }

        public void Dispose()
        {
            mock.Dispose();
            excelEngine.Dispose();
        }
    }
}