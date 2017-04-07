using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Shipping.Carriers.Ups.LocalRating;
using ShipWorks.Tests.Shared;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.LocalRating
{
    public class SurchargeUpsRateExcelReaderTest : IDisposable
    {
    //    readonly AutoMock mock;
    //    readonly SurchargeUpsRateExcelReader testObject;
    //    readonly ExcelEngine excelEngine;

    //    public SurchargeUpsRateExcelReaderTest()
    //    {
    //        mock = AutoMockExtensions.GetLooseThatReturnsMocks();
    //        testObject = mock.Create<SurchargeUpsRateExcelReader>();
    //        excelEngine = new ExcelEngine();
    //    }

    //    [Fact]
    //    public void Read_AddsValueAddWorksheetRatesToUpsLocalRateTable()
    //    {
    //        var foo = CreateWorkbook("Value Add");
    //        var worksheet = foo.Worksheets["Value Add"];
    //        AddRow(worksheet, new[] { "asdf", "asdf" });

    //        //testObject.Read(rateWorksheets.Object, upsLocalRateTable.Object);
    //    }

    //    private IWorkbook CreateWorkbook(string name)
    //    {
    //        IWorkbook workbook = excelEngine.Excel.Workbooks.Create();
    //        workbook.Worksheets.Create(name);
    //        return workbook;
    //    }

    //    private void AddRow(IWorksheet worksheet, string[] values)
    //    {
    //        worksheet.InsertRow(worksheet.Rows.Length + 1);
    //        IRange newRow = worksheet.Rows.Last();


    //        for (int i = 0; i < values.Length; i++)
    //        {
    //            newRow.Cells[i].Value = values[i];
    //        }
    //    }

    //    public void Dispose()
    //    {
    //        mock.Dispose();
    //        excelEngine.Dispose();
    //    }
    }
}
