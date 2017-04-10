using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Ups.LocalRating;
using ShipWorks.Shipping.UI.Carriers.Ups.LocalRating;
using ShipWorks.Tests.Shared;
using Syncfusion.XlsIO;
using Xunit;

namespace ShipWorks.Shipping.Tests.Integration.Carriers.Ups
{
    public class ServiceUpsRateExcelReaderTest : IDisposable
    {
        readonly AutoMock mock;
        private IEnumerable<UpsPackageRateEntity> readRates;
        private readonly ExcelEngine excelEngine;
        private readonly IWorkbook sampleExcelFile;
        private readonly Mock<IUpsLocalRateTable> rateTable;

        public ServiceUpsRateExcelReaderTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            rateTable = mock.CreateMock<IUpsLocalRateTable>(table =>
            {
                table.Setup(t => t.AddPackageRates(It.IsAny<IEnumerable<UpsPackageRateEntity>>()))
                    .Callback<IEnumerable<UpsPackageRateEntity>>(rates => readRates = rates);
            });

            excelEngine = new ExcelEngine();
            Assembly shippingAssembly = Assembly.GetAssembly(typeof(UpsLocalRatingViewModel));
            using (Stream sampleExcelStream = shippingAssembly.GetManifestResourceStream(UpsLocalRatingViewModel.SampleFileResourceName))
            {
                sampleExcelFile = excelEngine.Excel.Workbooks.Open(sampleExcelStream, ExcelOpenType.Automatic);
            }
        }

        [Fact]
        public void NumberOfRatesInSheet_MatchesNumberOfImportedRates()
        {
            ServiceUpsRateExcelReader testObject = mock.Create<ServiceUpsRateExcelReader>();
            testObject.Read(sampleExcelFile.Worksheets, rateTable.Object);

            int calculatedRateCount = 0;
            foreach (IWorksheet sheet in sampleExcelFile.Worksheets)
            {
                if (sheet.Range["A1"].Text != "Zones")
                {
                    continue;
                }

                int zoneCount = sheet.Rows[0].Cells.Count(IsValidZone);
                int weightCount = sheet.Columns[0].Cells.Count(IsValidWeight);

                calculatedRateCount += zoneCount * weightCount;
            }

            Assert.Equal(calculatedRateCount, readRates.Count());
        }

        private bool IsValidZone(IRange headerCell)
        {
            return headerCell.HasNumber;
        }

        private bool IsValidWeight(IRange weightCell)
        {
            return (weightCell.HasNumber || weightCell.Text == "Letter" || weightCell.Text == "Price Per Pound");
        }

        public void Dispose()
        {
            mock.Dispose();
            excelEngine.Dispose();
        }
    }
}