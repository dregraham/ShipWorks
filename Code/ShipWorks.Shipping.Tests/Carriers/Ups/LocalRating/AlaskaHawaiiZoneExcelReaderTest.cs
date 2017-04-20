using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Ups.LocalRating;
using ShipWorks.Tests.Shared;
using Syncfusion.XlsIO;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.LocalRating
{
    public class AlaskaHawaiiZoneExcelReaderTest
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
            Mock<IUpsLocalRateTable> rateTable = mock.Mock<IUpsLocalRateTable>();

            IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);
            IWorksheet worksheet = workbook.Worksheets[0];

            worksheet.Name = "HI";
            worksheet.Range["C1"].Text = "";
            worksheet.Range["D1"].Text = "";
            worksheet.Range["E1"].Text = "";
            worksheet.Range["F1"].Text = "";
            worksheet.Range["G1"].Text = "";
            worksheet.Range["H1"].Text = "";
            worksheet.Range["I1"].Text = "";
            worksheet.Range["J1"].Text = "";
            worksheet.Range["K1"].Text = "";
            worksheet.Range["A1"].Text = "Ground";
            worksheet.Range["B1"].Text = "44";
            worksheet.Range["A2"].Text = "Next Day Air";
            worksheet.Range["B2"].Text = "124";
            worksheet.Range["A3"].Text = "Second Day Air";
            worksheet.Range["B3"].Text = "224";
            worksheet.Range["A4"].Text = "Postal Codes:";
            worksheet.Range["A5"].Text = "23456";
            worksheet.Range["A6"].Text = "34567";
            worksheet.Range["A7"].Text = "45678";
            worksheet.Range["A8"].Text = "Ground";
            worksheet.Range["B8"].Text = "55";
            worksheet.Range["A9"].Text = "Next Day Air";
            worksheet.Range["B9"].Text = "155";
            worksheet.Range["A10"].Text = "Second Day Air";
            worksheet.Range["B10"].Text = "15";
            worksheet.Range["A11"].Text = "Postal Codes:";
            worksheet.Range["A12"].Text = "12345";
            worksheet.Range["A13"].Text = "67890";
            worksheet.Range["A14"].Text = "12345";
            worksheet.Range["A15"].Text = "56789";
            worksheet.Range["A16"].Text = "01234";
            
            IEnumerable<UpsLocalRatingZoneEntity> result = testObject.GetAlaskaHawaiiZones(workbook.Worksheets);

            Assert.Equal(24, result.Count());
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