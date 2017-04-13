using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Ups.LocalRating;
using ShipWorks.Shipping.Carriers.UPS.LocalRating;
using ShipWorks.Tests.Shared;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.LocalRating
{
    public class SurchargeUpsRateExcelReaderTest : IDisposable
    {
        readonly AutoMock mock;
        readonly SurchargeUpsRateExcelReader testObject;
        readonly ExcelEngine excelEngine;

        public SurchargeUpsRateExcelReaderTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<SurchargeUpsRateExcelReader>();
            excelEngine = new ExcelEngine();
            excelEngine.Excel.DefaultVersion = ExcelVersion.Excel2013;
        }

        [Fact]
        public void Read_AddsValueAddWorksheetRatesToUpsLocalRateTable()
        {
            Mock<IUpsLocalRateTable> rateTable = mock.Mock<IUpsLocalRateTable>();
            
            IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);
            IWorksheet worksheet = workbook.Worksheets[0];

            worksheet.Name = "Value Add";
            worksheet.Range["A1"].Text = "Value Added Service";
            worksheet.Range["B1"].Text = "Rate";
            AddRow(worksheet, new[] { "No Signature", "2.00" });

            testObject.Read(workbook.Worksheets, rateTable.Object);
            
            rateTable.Verify(r => r.AddSurcharges(It.Is<IEnumerable<UpsRateSurchargeEntity>>(e => e.First().Amount == 2.00 && e.First().SurchargeType == (int)UpsSurchargeType.NoSignature)));
        }

        [Fact]
        public void Read_AddsSurchargesWorksheetRatesToUpsLocalRateTable()
        {
            Mock<IUpsLocalRateTable> rateTable = mock.Mock<IUpsLocalRateTable>();

            IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);
            IWorksheet worksheet = workbook.Worksheets[0];

            worksheet.Name = "Surcharges";
            worksheet.Range["A1"].Text = "Surcharge";
            worksheet.Range["B1"].Text = "Rate";
            AddRow(worksheet, new[] { "Large Package", "2.00" });

            testObject.Read(workbook.Worksheets, rateTable.Object);

            rateTable.Verify(r => r.AddSurcharges(It.Is<IEnumerable<UpsRateSurchargeEntity>>(e => e.First().Amount == 2.00 && e.First().SurchargeType == (int)UpsSurchargeType.LargePackage)));
        }


        [Fact]
        public void Read_AddsSurchargesWorksheetRatesToUpsLocalRateTable_WhenSurchargeContainsDollarSigns()
        {
            Mock<IUpsLocalRateTable> rateTable = mock.Mock<IUpsLocalRateTable>();

            IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);
            IWorksheet worksheet = workbook.Worksheets[0];

            worksheet.Name = "Surcharges";
            worksheet.Range["A1"].Text = "Surcharge";
            worksheet.Range["B1"].Text = "Rate";
            AddRow(worksheet, new[] { "Large Package", "$2.00" });

            testObject.Read(workbook.Worksheets, rateTable.Object);

            rateTable.Verify(r => r.AddSurcharges(It.Is<IEnumerable<UpsRateSurchargeEntity>>(e => e.First().Amount == 2.00 && e.First().SurchargeType == (int)UpsSurchargeType.LargePackage)));
        }

        [Fact]
        public void Read_ThrowsUpsLocalRatingException_WhenSurchargeRateIsNotANumber()
        {
            Mock<IUpsLocalRateTable> rateTable = mock.Mock<IUpsLocalRateTable>();

            IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);
            IWorksheet worksheet = workbook.Worksheets[0];

            worksheet.Name = "Value Add";
            worksheet.Range["A1"].Text = "Value Added Service";
            worksheet.Range["B1"].Text = "Rate";
            AddRow(worksheet, new[] { "Large Package", "yes very" });

            Exception ex = Assert.Throws<UpsLocalRatingException>(() => testObject.Read(workbook.Worksheets, rateTable.Object));
            Assert.Equal("The rate for Large Package is invalid 'yes very'.", ex.Message);
        }

        [Fact]
        public void Read_ThrowsUpsLocalRatingException_WhenSurchargeTypeDuplicated()
        {
            Mock<IUpsLocalRateTable> rateTable = mock.Mock<IUpsLocalRateTable>();

            IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);
            IWorksheet worksheet = workbook.Worksheets[0];

            worksheet.Name = "Value Add";
            worksheet.Range["A1"].Text = "Value Added Service";
            worksheet.Range["B1"].Text = "Rate";
            AddRow(worksheet, new[] { "Large Package", "2.00" });
            AddRow(worksheet, new[] { "Large Package", "2.00" });
            
            Exception ex = Assert.Throws<UpsLocalRatingException>(() => testObject.Read(workbook.Worksheets, rateTable.Object));
            Assert.Equal("The surcharge Large Package was specified more than once.", ex.Message);
        }

        [Fact]
        public void Read_ThrowsUpsLocalRatingException_WhenSurchargeTypeIsUnknown()
        {
            Mock<IUpsLocalRateTable> rateTable = mock.Mock<IUpsLocalRateTable>();

            IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);
            IWorksheet worksheet = workbook.Worksheets[0];

            worksheet.Name = "Value Add";
            worksheet.Range["A1"].Text = "Value Added Service";
            worksheet.Range["B1"].Text = "Rate";
            AddRow(worksheet, new[] { "unknown surcharge", "2.00" });
            
            Exception ex = Assert.Throws<UpsLocalRatingException>(() => testObject.Read(workbook.Worksheets, rateTable.Object));
            Assert.Equal("Unknown Surcharge or Value Add unknown surcharge", ex.Message);
        }

        private void AddRow(IWorksheet workSheet , string[] values)
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
            mock.Dispose();
            excelEngine.Dispose();
        }
    }
}
