using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Ups.LocalRating;
using ShipWorks.Shipping.UI.Carriers.Ups.LocalRating;
using ShipWorks.Tests.Shared;
using Syncfusion.XlsIO;
using Xunit;

namespace ShipWorks.Shipping.Tests.Integration.Carriers.Ups.LocalRating
{
    public class DeliveryAreaSurchargeUpsZoneExcelReaderTest : IDisposable
    {
        private readonly AutoMock mock;
        private IEnumerable<UpsLocalRatingDeliveryAreaSurchargeEntity> readSurcharges;
        private readonly ExcelEngine excelEngine;
        private readonly IWorkbook sampleExcelFile;

        public DeliveryAreaSurchargeUpsZoneExcelReaderTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            Mock<IUpsLocalRateTable> rateTable = mock.CreateMock<IUpsLocalRateTable>(table =>
            {
                table.Setup(t => t.ReplaceDeliveryAreaSurcharges(
                        It.IsAny<IEnumerable<UpsLocalRatingDeliveryAreaSurchargeEntity>>()))
                    .Callback<IEnumerable<UpsLocalRatingDeliveryAreaSurchargeEntity>>(read => readSurcharges = read);
            });
            
            excelEngine = new ExcelEngine();
            Assembly shippingAssembly = Assembly.GetAssembly(typeof(UpsLocalRatingViewModel));

            using (Stream sampleExcelStream = shippingAssembly.GetManifestResourceStream(UpsLocalRatingViewModel.SampleZoneFileResourceName))
            {
                sampleExcelFile = excelEngine.Excel.Workbooks.Open(sampleExcelStream, ExcelOpenType.Automatic);
            }

            var testObject = mock.Create<DeliveryAreaSurchargeUpsZoneExcelReader>();
            testObject.Read(sampleExcelFile.Worksheets, rateTable.Object);
        }

        [Fact]
        public void Read_SavesCorrectNumberOfDeliveryAreaSurcharges()
        {
            IWorksheet dasSheet = sampleExcelFile.Worksheets["DASzips"];
            int count = dasSheet.Cells.Count(c => Regex.IsMatch(c.Text ?? string.Empty, "^[0-9]{5}$"));

            Assert.Equal(count, readSurcharges.Count());
        }

        [Theory]
        [InlineData(0, UpsDeliveryAreaSurchargeType.Us48Das)]
        [InlineData(1, UpsDeliveryAreaSurchargeType.Us48DasExtended)]
        [InlineData(2, UpsDeliveryAreaSurchargeType.UsRemoteHi)]
        [InlineData(3, UpsDeliveryAreaSurchargeType.UsRemoteAk)]
        public void Read_SavesCorrectNumberOfUS48Das(int dasColumn, UpsDeliveryAreaSurchargeType surchargeType)
        {
            IWorksheet dasSheet = sampleExcelFile.Worksheets["DASzips"];
            int count = dasSheet.Columns[dasColumn].Cells.Count(IsZip);

            Assert.Equal(count, readSurcharges.Count(s=>s.DeliveryAreaType == (int) surchargeType));
        }

        private static bool IsZip(IRange cell)
        {
            return Regex.IsMatch(cell?.Text ?? string.Empty, "^[0-9]{5}$");
        }

        public void Dispose()
        {
            mock.Dispose();
            excelEngine.Dispose();
        }
    }
}
