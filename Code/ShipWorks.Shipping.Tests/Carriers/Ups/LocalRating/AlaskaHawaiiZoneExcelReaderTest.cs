﻿using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Ups.LocalRating;
using ShipWorks.Tests.Shared;
using Syncfusion.XlsIO;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.LocalRating
{
    public class AlaskaHawaiiZoneExcelReaderTest : IDisposable
    {
        readonly AutoMock mock;
        readonly AlaskaHawaiiZoneExcelReader testObject;
        readonly ExcelEngine excelEngine;

        public AlaskaHawaiiZoneExcelReaderTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<AlaskaHawaiiZoneExcelReader>();
            excelEngine = new ExcelEngine();
            excelEngine.Excel.DefaultVersion = ExcelVersion.Excel2013;
        }

        [Fact]
        public void Read_ReplacesZonesOnUpsLocalRateTable()
        {
            IWorkbook workbook = GetDefaultWorkbook();

            IEnumerable<UpsLocalRatingZoneEntity> result = testObject.GetAlaskaHawaiiZones(workbook.Worksheets);

            Assert.Equal(48, result.Count());
        }

        [Fact]
        public void Read_ThrowsUpsRatingException_WhenInvalidZipcode()
        {
            IWorkbook workbook = GetDefaultWorkbook();
            workbook.Worksheets["HI"].Range["A13"].Value = "631";

            UpsLocalRatingException ex = Assert.Throws<UpsLocalRatingException>(() => testObject.GetAlaskaHawaiiZones(workbook.Worksheets));
            Assert.Equal("Invalid zip code found in sheet HI, cell A13.", ex.Message);
        }


        [Fact]
        public void Read_ThrowsUpsRatingException_WhenGroundNotInA1()
        {
            IWorkbook workbook = GetDefaultWorkbook();
            workbook.Worksheets["HI"].Range["A1"].Value = "Overnight";

            UpsLocalRatingException ex = Assert.Throws<UpsLocalRatingException>(() => testObject.GetAlaskaHawaiiZones(workbook.Worksheets));
            Assert.Equal("In worksheet 'HI', cell A1 should be Ground.", ex.Message);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(8)]
        public void Read_ThrowsUpsRatingException_WhenWorksheetsSectionIsMissingGroundZone(int row)
        {
            IWorkbook workbook = GetDefaultWorkbook();

            workbook.Worksheets["AK"].Range[$"B{row}"].Text = string.Empty;

            UpsLocalRatingException ex = Assert.Throws<UpsLocalRatingException>(() => testObject.GetAlaskaHawaiiZones(workbook.Worksheets));

            string expectedErrorMessage = "Error reading worksheet AK \n\n" +
                                          $"Missing Ground zone for section starting at row {row}.";
            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        [Fact]
        public void Read_ThrowsUpsRatingException_WhenWorksheetsSectionIsMissingGroundLabel()
        {
            IWorkbook workbook = GetDefaultWorkbook();

            workbook.Worksheets["AK"].Range["A8"].Text = string.Empty;

            UpsLocalRatingException ex = Assert.Throws<UpsLocalRatingException>(() => testObject.GetAlaskaHawaiiZones(workbook.Worksheets));

            string expectedErrorMessage = "Error reading worksheet \'AK.\'\n\n" +
                                          "Each section must start with a Ground zone definition in column A.";
            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        [Fact]
        public void Read_ThrowsUpsRatingException_WhenWorksheetsSectionStartsWithAServiceOtherThanGround()
        {
            IWorkbook workbook = GetDefaultWorkbook();

            workbook.Worksheets["AK"].Range["A8"].Text = "Next Day Air"; 
            workbook.Worksheets["AK"].Range["A9"].Text = "Ground";

            UpsLocalRatingException ex = Assert.Throws<UpsLocalRatingException>(() => testObject.GetAlaskaHawaiiZones(workbook.Worksheets));

            string expectedErrorMessage = "Error reading worksheet 'AK' \n\n" +
                                          "The zones for section starting at around row 9 should be in the order of:\n" +
                                          " - Ground\n - Next Day Air\n - Second Day Air\n - Postal Codes:";

            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        [Theory]
        [InlineData("HI")]
        [InlineData("AK")]
        public void Read_ThrowsUpsRatingException_WhenWorksheetsIsMissingAkOrHi(string state)
        {
            IWorkbook workbook = GetDefaultWorkbook();

            workbook.Worksheets.Remove(state);
            
            UpsLocalRatingException ex = Assert.Throws<UpsLocalRatingException>(() => testObject.GetAlaskaHawaiiZones(workbook.Worksheets));
            Assert.Equal($"Zone file must have a '{state}' worksheet.", ex.Message);
        }

        [Fact]
        public void Read_ThrowsUpsRatingException_WhenFirstColumnRemoved()
        {
            IWorkbook workbook = GetDefaultWorkbook();
            workbook.Worksheets["HI"].DeleteColumn(1);

            string expectedError = "In worksheet 'HI', cell A1 should be Ground.";

            UpsLocalRatingException ex = Assert.Throws<UpsLocalRatingException>(() => testObject.GetAlaskaHawaiiZones(workbook.Worksheets));
            Assert.Equal(expectedError, ex.Message);

        }

        private IWorkbook GetDefaultWorkbook()
        {
            IWorkbook workbook = excelEngine.Excel.Workbooks.Create(2);
            IWorksheet hiWorksheet = workbook.Worksheets[0];

            hiWorksheet.Name = "HI";
            hiWorksheet.Range["C1"].Text = "";
            hiWorksheet.Range["D1"].Text = "";
            hiWorksheet.Range["E1"].Text = "";
            hiWorksheet.Range["F1"].Text = "";
            hiWorksheet.Range["G1"].Text = "";
            hiWorksheet.Range["H1"].Text = "";
            hiWorksheet.Range["I1"].Text = "";
            hiWorksheet.Range["J1"].Text = "";
            hiWorksheet.Range["K1"].Text = "";
            hiWorksheet.Range["A1"].Text = "Ground";
            hiWorksheet.Range["B1"].Text = "44";
            hiWorksheet.Range["A2"].Text = "Next Day Air";
            hiWorksheet.Range["B2"].Text = "124";
            hiWorksheet.Range["A3"].Text = "Second Day Air";
            hiWorksheet.Range["B3"].Text = "224";
            hiWorksheet.Range["A4"].Text = "Postal Codes:";
            hiWorksheet.Range["A5"].Text = "23456";
            hiWorksheet.Range["A6"].Text = "34567";
            hiWorksheet.Range["A7"].Text = "45678";
            hiWorksheet.Range["A8"].Text = "Ground";
            hiWorksheet.Range["B8"].Text = "55";
            hiWorksheet.Range["A9"].Text = "Next Day Air";
            hiWorksheet.Range["B9"].Text = "155";
            hiWorksheet.Range["A10"].Text = "Second Day Air";
            hiWorksheet.Range["B10"].Text = "15";
            hiWorksheet.Range["A11"].Text = "Postal Codes:";
            hiWorksheet.Range["A12"].Text = "12345";
            hiWorksheet.Range["A13"].Text = "67890";
            hiWorksheet.Range["A14"].Text = "12345";
            hiWorksheet.Range["A15"].Text = "56789";
            hiWorksheet.Range["A16"].Text = "01234";

            IWorksheet akWorksheet = workbook.Worksheets[1];

            akWorksheet.Name = "AK";
            akWorksheet.Range["C1"].Text = "";
            akWorksheet.Range["D1"].Text = "";
            akWorksheet.Range["E1"].Text = "";
            akWorksheet.Range["F1"].Text = "";
            akWorksheet.Range["G1"].Text = "";
            akWorksheet.Range["H1"].Text = "";
            akWorksheet.Range["I1"].Text = "";
            akWorksheet.Range["J1"].Text = "";
            akWorksheet.Range["K1"].Text = "";
            akWorksheet.Range["A1"].Text = "Ground";
            akWorksheet.Range["B1"].Text = "44";
            akWorksheet.Range["A2"].Text = "Next Day Air";
            akWorksheet.Range["B2"].Text = "124";
            akWorksheet.Range["A3"].Text = "Second Day Air";
            akWorksheet.Range["B3"].Text = "224";
            akWorksheet.Range["A4"].Text = "Postal Codes:";
            akWorksheet.Range["A5"].Text = "23456";
            akWorksheet.Range["A6"].Text = "34567";
            akWorksheet.Range["A7"].Text = "45678";
            akWorksheet.Range["A8"].Text = "Ground";
            akWorksheet.Range["B8"].Text = "55";
            akWorksheet.Range["A9"].Text = "Next Day Air";
            akWorksheet.Range["B9"].Text = "155";
            akWorksheet.Range["A10"].Text = "Second Day Air";
            akWorksheet.Range["B10"].Text = "15";
            akWorksheet.Range["A11"].Text = "Postal Codes:";
            akWorksheet.Range["A12"].Text = "12345";
            akWorksheet.Range["A13"].Text = "67890";
            akWorksheet.Range["A14"].Text = "12345";
            akWorksheet.Range["A15"].Text = "56789";
            akWorksheet.Range["A16"].Text = "01234";

            return workbook;
        }

        public void Dispose()
        {
            mock?.Dispose();
            excelEngine?.Dispose();
        }
    }
}