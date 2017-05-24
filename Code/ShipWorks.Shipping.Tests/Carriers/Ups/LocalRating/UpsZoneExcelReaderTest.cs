using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Ups.LocalRating;
using ShipWorks.Shipping.Carriers.UPS.LocalRating;
using ShipWorks.Tests.Shared;
using Syncfusion.XlsIO;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.LocalRating
{
    public class UpsZoneExcelReaderTest : IDisposable
    {
        readonly AutoMock mock;
        readonly UpsZoneExcelReader testObject;
        readonly ExcelEngine excelEngine;
        private int numberOfSupportedServices = 7;

        public UpsZoneExcelReaderTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<UpsZoneExcelReader>();
            excelEngine = new ExcelEngine();
            excelEngine.Excel.DefaultVersion = ExcelVersion.Excel2013;
        }

        [Fact]
        public void Read_ThrowsUpsLocalRatingException_WhenWorksheetsHasNoZoneInfo()
        {
            Mock<IUpsLocalRateTable> rateTable = mock.Mock<IUpsLocalRateTable>();

            IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);
            IWorksheet worksheet = workbook.Worksheets[0];

            worksheet.Name = "EomeEmptyWorksheet";

            UpsLocalRatingException ex = Assert.Throws<UpsLocalRatingException>(() => testObject.Read(workbook.Worksheets, rateTable.Object));
            Assert.Equal("The zone file contains no worksheets that follow the zone worksheet naming convention of '#####-#####'", ex.Message);
        }

        [Fact]
        public void Read_ThrowsUpsLocalRatingException_WhenWorksheetDestinationZipIsInvalid()
        {
            Mock<IUpsLocalRateTable> rateTable = mock.Mock<IUpsLocalRateTable>();

            IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);
            IWorksheet worksheet = workbook.Worksheets[0];

            worksheet.Name = "12345-12345";
            worksheet.Range["A1"].Text = "Dest. ZIP";
            worksheet.Range["B1"].Text = "Ground";
            worksheet.Range["C1"].Text = "3 Day Select";
            worksheet.Range["D1"].Text = "2nd Day Air";
            worksheet.Range["E1"].Text = "2nd Day Air A.M.";
            worksheet.Range["F1"].Text = "Next Day Air Saver";
            worksheet.Range["G1"].Text = "Next Day Air";
            worksheet.Range["H1"].Text = "Next Day Air Early";

            AddRow(worksheet, new[] { "0a4-0b5", "005", "305", "205", "-", "135", "-", "-" });
            
            UpsLocalRatingException ex = Assert.Throws<UpsLocalRatingException>(() => testObject.Read(workbook.Worksheets, rateTable.Object));
            Assert.Equal("Worksheet 12345-12345 has an invalid destination zip value 0a4-0b5.", ex.Message);
        }

        [Fact]
        public void Read_SkipsZoneColumnsWithDash()
        {
            Mock<IUpsLocalRateTable> rateTable = mock.Mock<IUpsLocalRateTable>();

            IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);
            IWorksheet worksheet = workbook.Worksheets[0];

            worksheet.Name = "12345-12345";
            worksheet.Range["A1"].Text = "Dest. ZIP";
            worksheet.Range["B1"].Text = "Ground";
            worksheet.Range["C1"].Text = "3 Day Select";
            worksheet.Range["D1"].Text = "2nd Day Air";
            worksheet.Range["E1"].Text = "2nd Day Air A.M.";
            worksheet.Range["F1"].Text = "Next Day Air Saver";
            worksheet.Range["G1"].Text = "Next Day Air";
            worksheet.Range["H1"].Text = "Next Day Air Early";

            AddRow(worksheet, new[] { "004-005", "005", "305", "205", "-", "135", "-", "-" });

            testObject.Read(workbook.Worksheets, rateTable.Object);

            rateTable.Verify(r => r.ReplaceZones(
                It.Is<List<UpsLocalRatingZoneEntity>>(
                    z => z.Count == 4 && z.All(a => a.OriginZipFloor == 12345 &&
                                                    a.OriginZipCeiling == 12345 &&
                                                    a.DestinationZipFloor == 00400 &&
                                                    a.DestinationZipCeiling == 00599))));
        }

        [Fact]
        public void Read_DelegatestoAlaskaHawaiiZoneReader()
        {
            Mock<IUpsLocalRateTable> rateTable = mock.Mock<IUpsLocalRateTable>();

            IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);
            IWorksheet worksheet = workbook.Worksheets[0];

            worksheet.Name = "12345-12345";
            worksheet.Range["A1"].Text = "Dest. ZIP";
            worksheet.Range["B1"].Text = "Ground";
            worksheet.Range["C1"].Text = "3 Day Select";
            worksheet.Range["D1"].Text = "2nd Day Air";
            worksheet.Range["E1"].Text = "2nd Day Air A.M.";
            worksheet.Range["F1"].Text = "Next Day Air Saver";
            worksheet.Range["G1"].Text = "Next Day Air";
            worksheet.Range["H1"].Text = "Next Day Air Early";

            AddRow(worksheet, new[] { "004-005", "005", "305", "205", "-", "135", "-", "-" });

            testObject.Read(workbook.Worksheets, rateTable.Object);

            mock.Mock<IAlaskaHawaiiZoneExcelReader>().Verify(a => a.GetAlaskaHawaiiZones(workbook.Worksheets));
        }

        [Fact]
        public void Read_ReplacesZonesOnUpsLocalRateTable_WhenDestinationRange()
        {
            Mock<IUpsLocalRateTable> rateTable = mock.Mock<IUpsLocalRateTable>();

            IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);
            IWorksheet worksheet = workbook.Worksheets[0];

            worksheet.Name = "12345-12345";
            worksheet.Range["A1"].Text = "Dest. ZIP";
            worksheet.Range["B1"].Text = "Ground";
            worksheet.Range["C1"].Text = "3 Day Select";
            worksheet.Range["D1"].Text = "2nd Day Air";
            worksheet.Range["E1"].Text = "2nd Day Air A.M.";
            worksheet.Range["F1"].Text = "Next Day Air Saver";
            worksheet.Range["G1"].Text = "Next Day Air";
            worksheet.Range["H1"].Text = "Next Day Air Early";
            
            AddRow(worksheet, new []{"004-005","005","305","205","245","135","105", "105"});

            testObject.Read(workbook.Worksheets, rateTable.Object);

            rateTable.Verify(r => r.ReplaceZones(
                It.Is<List<UpsLocalRatingZoneEntity>>(
                    z => z.Count == numberOfSupportedServices && z.All(a => a.OriginZipFloor == 12345 &&
                                                    a.OriginZipCeiling == 12345 &&
                                                    a.DestinationZipFloor == 00400 &&
                                                    a.DestinationZipCeiling == 00599))));
        }

        [Fact]
        public void Read_ReplacesZonesOnUpsLocalRateTable_WhenSingleDestination()
        {
            Mock<IUpsLocalRateTable> rateTable = mock.Mock<IUpsLocalRateTable>();

            IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);
            IWorksheet worksheet = workbook.Worksheets[0];

            worksheet.Name = "12345-12345";
            worksheet.Range["A1"].Text = "Dest. ZIP";
            worksheet.Range["B1"].Text = "Ground";
            worksheet.Range["C1"].Text = "3 Day Select";
            worksheet.Range["D1"].Text = "2nd Day Air";
            worksheet.Range["E1"].Text = "2nd Day Air A.M.";
            worksheet.Range["F1"].Text = "Next Day Air Saver";
            worksheet.Range["G1"].Text = "Next Day Air";
            worksheet.Range["H1"].Text = "Next Day Air Early";

            AddRow(worksheet, new[] { "004", "005", "305", "205", "245", "135", "105", "105" });

            testObject.Read(workbook.Worksheets, rateTable.Object);

            rateTable.Verify(r => r.ReplaceZones(
                It.Is<List<UpsLocalRatingZoneEntity>>(
                    z => z.Count == numberOfSupportedServices && z.All(a => a.OriginZipFloor == 12345 &&
                                                    a.OriginZipCeiling == 12345 &&
                                                    a.DestinationZipFloor == 00400 &&
                                                    a.DestinationZipCeiling == 00499))));
        }

        [Fact]
        public void Read_ThrowsUpsLocalRatingException_WhenWorksheetIsMissingAColumHeader()
        {
            Mock<IUpsLocalRateTable> rateTable = mock.Mock<IUpsLocalRateTable>();

            IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);
            IWorksheet worksheet = workbook.Worksheets[0];

            worksheet.Name = "12345-12345";
            worksheet.Range["A1"].Text = "Dest. ZIP";
            worksheet.Range["B1"].Text = "Ground";
            worksheet.Range["C1"].Text = "3 Day Select";
            worksheet.Range["D1"].Text = "2nd Day Air";
            worksheet.Range["E1"].Text = "2nd Day Air A.M.";
            worksheet.Range["F1"].Text = "Next Day Air Saver";

            AddRow(worksheet, new[] { "004", "005", "305", "205", "245", "135" });

            UpsLocalRatingException ex = Assert.Throws<UpsLocalRatingException>(() => testObject.Read(workbook.Worksheets, rateTable.Object));
            Assert.Equal("12345-12345 is missing the required Next Day Air header at G1.", ex.Message);
        }

        [Fact]
        public void Read_ThrowsUpsLocalRatingException_WhenWorksheetIsMissingMultipleColumHeader()
        {
            Mock<IUpsLocalRateTable> rateTable = mock.Mock<IUpsLocalRateTable>();

            IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);
            IWorksheet worksheet = workbook.Worksheets[0];

            worksheet.Name = "12345-12345";
            worksheet.Range["A1"].Text = "Dest. ZIP";
            worksheet.Range["B1"].Text = "Ground";

            AddRow(worksheet, new[] { "004", "005" });

            UpsLocalRatingException ex = Assert.Throws<UpsLocalRatingException>(() => testObject.Read(workbook.Worksheets, rateTable.Object));
            Assert.Equal("12345-12345 is missing the required 3 Day Select header at C1.", ex.Message);
        }

        [Fact]
        public void Read_SkipsEmptyRow_WhenBlankLineBeforeSingleDestination()
        {
            Mock<IUpsLocalRateTable> rateTable = mock.Mock<IUpsLocalRateTable>();

            IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);
            IWorksheet worksheet = workbook.Worksheets[0];

            worksheet.Name = "12345-12345";
            worksheet.Range["A1"].Text = "Dest. ZIP";
            worksheet.Range["B1"].Text = "Ground";
            worksheet.Range["C1"].Text = "3 Day Select";
            worksheet.Range["D1"].Text = "2nd Day Air";
            worksheet.Range["E1"].Text = "2nd Day Air A.M.";
            worksheet.Range["F1"].Text = "Next Day Air Saver";
            worksheet.Range["G1"].Text = "Next Day Air";
            worksheet.Range["H1"].Text = "Next Day Air Early";

            AddRow(worksheet, Enumerable.Repeat(string.Empty, 7).ToArray());
            AddRow(worksheet, new[] { "004", "005", "305", "205", "245", "135", "105", "105" });

            testObject.Read(workbook.Worksheets, rateTable.Object);

            rateTable.Verify(r => r.ReplaceZones(
                It.Is<List<UpsLocalRatingZoneEntity>>(
                    z => z.Count == numberOfSupportedServices && z.All(a => a.OriginZipFloor == 12345 &&
                                                    a.OriginZipCeiling == 12345 &&
                                                    a.DestinationZipFloor == 00400 &&
                                                    a.DestinationZipCeiling == 00499))));
        }

        [Fact]
        public void Read_ThrowsUpsLocalRatingException_WhenNumericValueAsDestinationZip()
        {
            Mock<IUpsLocalRateTable> rateTable = mock.Mock<IUpsLocalRateTable>();

            IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);
            IWorksheet worksheet = workbook.Worksheets[0];

            worksheet.Name = "12345-12345";
            worksheet.Range["A1"].Text = "Dest. ZIP";
            worksheet.Range["B1"].Text = "Ground";
            worksheet.Range["C1"].Text = "3 Day Select";
            worksheet.Range["D1"].Text = "2nd Day Air";
            worksheet.Range["E1"].Text = "2nd Day Air A.M.";
            worksheet.Range["F1"].Text = "Next Day Air Saver";
            worksheet.Range["G1"].Text = "Next Day Air";
            worksheet.Range["H1"].Text = "Next Day Air Early";

            AddRow(worksheet, new[] { "", "005", "305", "205", "245", "135", "105", "105" });
            // reset A2 (which was empty string from line above) to a numeric value.
            worksheet.Range["A2"].Value2 = 4;

            var ex = Assert.Throws<UpsLocalRatingException>(() => testObject.Read(workbook.Worksheets, rateTable.Object));
            Assert.Equal("Worksheet 12345-12345 has an invalid destination zip value 4.", ex.Message);
        }

        private void AddRow(IWorksheet workSheet, string[] values)
        {
            workSheet.InsertRow(workSheet.Rows.Length + 1);
            IRange row = workSheet.Rows.Last();

            for (int i = 0; i < values.Length; i++)
            {
                row.Cells[i].Text = values[i];
            }
        }

        public void Dispose()
        {
            mock?.Dispose();
            excelEngine?.Dispose();
        }
    }
}