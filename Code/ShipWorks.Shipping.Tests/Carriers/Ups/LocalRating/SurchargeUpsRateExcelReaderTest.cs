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
        readonly AutoMock mock;
        readonly SurchargeUpsRateExcelReader testObject;
        readonly ExcelEngine excelEngine;

        public SurchargeUpsRateExcelReaderTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<SurchargeUpsRateExcelReader>();
            excelEngine = new ExcelEngine();
        }

        [Fact]
        public void Read_AddsValueAddWorksheetRatesToUpsLocalRateTable()
        {
            Mock<IWorksheets> rateWorksheets = mock.Mock<IWorksheets>();
            Mock<IUpsLocalRateTable> upsLocalRateTable = mock.Mock<IUpsLocalRateTable>();

            Mock<IRange> range = mock.Mock<IRange>();
            //range.Setup()
            //var foo = new IRange[] { new Ra};

            //rateWorksheets.Setup(s => s["Value Add"]).Returns(null);


            testObject.Read(rateWorksheets.Object, upsLocalRateTable.Object);
        }

        public IWorksheet CreateWorkbook(string name)
        {
            IWorkbook foo = excelEngine.Excel.Workbooks.Create();

            foo.Worksheets.Create(name);
        }

        public void Dispose()
        {
            mock.Dispose();
            excelEngine.Dispose();
        }
    }
}
