using System;
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

        public UpsZoneExcelReaderTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<UpsZoneExcelReader>();
            excelEngine = new ExcelEngine();
            excelEngine.Excel.DefaultVersion = ExcelVersion.Excel2013;
        }

        [Fact]
        public void Read_AddsValueAddWorksheetRatesToUpsLocalRateTable()
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
            
            AddRow(worksheet, new []{"004-005","005","305","205","245","135","105"});

            testObject.Read(workbook.Worksheets, rateTable.Object);
            
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