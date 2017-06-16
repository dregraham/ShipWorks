using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Ups.LocalRating;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.UI.Carriers.Ups.LocalRating;
using ShipWorks.Tests.Shared;
using Syncfusion.XlsIO;
using Xunit;

namespace ShipWorks.Shipping.Tests.Integration.Carriers.Ups.LocalRating
{
    public class UpsZoneExcelReaderTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly ExcelEngine excelEngine;
        private readonly IWorkbook sampleExcelFile;
        private IEnumerable<UpsLocalRatingZoneEntity> readZones;
        private readonly Mock<IUpsLocalRateTable> rateTable;

        public UpsZoneExcelReaderTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            excelEngine = new ExcelEngine();
            Assembly shippingAssembly = Assembly.GetAssembly(typeof(UpsLocalRatingViewModel));

            using (Stream sampleExcelStream = shippingAssembly.GetManifestResourceStream(UpsLocalRatingViewModel.SampleZoneFileResourceName))
            {
                sampleExcelFile = excelEngine.Excel.Workbooks.Open(sampleExcelStream, ExcelOpenType.Automatic);
            }

            rateTable = mock.CreateMock<IUpsLocalRateTable>(table =>
            {
                table.Setup(t => t.ReplaceZones(
                        It.IsAny<IEnumerable<UpsLocalRatingZoneEntity>>()))
                    .Callback<IEnumerable<UpsLocalRatingZoneEntity>>(read => readZones = read);
            });

            var testObject = mock.Create<UpsZoneExcelReader>();
            testObject.Read(sampleExcelFile.Worksheets, rateTable.Object);
        }

        [Theory]
        [InlineData("00401", "00599")]
        [InlineData("63001","63399")]
        public void Read_EachZipHasCorrectNumberOfZones(string originZipFloor, string originZipCeiling)
        {
            IWorksheet zipSheet = GetZipSheet(originZipFloor, originZipCeiling);

            IRange zoneRange = zipSheet.Range[2, 2, zipSheet.UsedRange.LastRow, zipSheet.UsedRange.LastColumn];

            int expectedZoneCount = zoneRange.Cells.Count(IsZone);
            int actualZoneCount =
                readZones.Count(z => z.OriginZipFloor == int.Parse(originZipFloor) && z.OriginZipCeiling == int.Parse(originZipCeiling));

            rateTable.Verify(t=>t.ReplaceZones(It.IsAny<IEnumerable<UpsLocalRatingZoneEntity>>()), Times.Once);
            Assert.True(actualZoneCount > 0);
            Assert.Equal(expectedZoneCount, actualZoneCount);
        }

        [Theory]
        [InlineData("00401", "00599", UpsServiceType.UpsGround, 2)]
        [InlineData("00401", "00599", UpsServiceType.Ups3DaySelect, 3)]
        [InlineData("00401", "00599", UpsServiceType.Ups2DayAir, 4)]
        [InlineData("00401", "00599", UpsServiceType.Ups2DayAirAM, 5)]
        [InlineData("00401", "00599", UpsServiceType.UpsNextDayAirSaver, 6)]
        [InlineData("00401", "00599", UpsServiceType.UpsNextDayAir, 7)]
        [InlineData("63001", "63399", UpsServiceType.UpsGround, 2)]
        [InlineData("63001", "63399", UpsServiceType.Ups3DaySelect, 3)]
        [InlineData("63001", "63399", UpsServiceType.Ups2DayAir, 4)]
        [InlineData("63001", "63399", UpsServiceType.Ups2DayAirAM, 5)]
        [InlineData("63001", "63399", UpsServiceType.UpsNextDayAirSaver, 6)]
        [InlineData("63001", "63399", UpsServiceType.UpsNextDayAir, 7)]
        public void Read_NumberOfEntriesPerServicesIsCorrect(string originZipFloor,
            string originZipCeiling,
            UpsServiceType upsServiceType,
            int column)
        {
            var zipSheet = GetZipSheet(originZipFloor, originZipCeiling);

            IRange serviceRange = zipSheet.Range[2, column, zipSheet.UsedRange.LastRow, column];
            int expectedZoneCount = serviceRange.Cells.Count(IsZone);
            int actualZoneCount =
                readZones.Count(
                    z =>
                        z.OriginZipFloor == int.Parse(originZipFloor) &&
                        z.OriginZipCeiling == int.Parse(originZipCeiling) && z.Service == (int) upsServiceType);

            Assert.True(actualZoneCount > 0);
            Assert.Equal(expectedZoneCount, actualZoneCount);

        }

        [Theory]
        [InlineData("00401", "00599", UpsServiceType.UpsGround, 2)]
        [InlineData("00401", "00599", UpsServiceType.Ups3DaySelect, 3)]
        [InlineData("00401", "00599", UpsServiceType.Ups2DayAir, 4)]
        [InlineData("00401", "00599", UpsServiceType.Ups2DayAirAM, 5)]
        [InlineData("00401", "00599", UpsServiceType.UpsNextDayAirSaver, 6)]
        [InlineData("00401", "00599", UpsServiceType.UpsNextDayAir, 7)]
        [InlineData("63001", "63399", UpsServiceType.UpsGround, 2)]
        [InlineData("63001", "63399", UpsServiceType.Ups3DaySelect, 3)]
        [InlineData("63001", "63399", UpsServiceType.Ups2DayAir, 4)]
        [InlineData("63001", "63399", UpsServiceType.Ups2DayAirAM, 5)]
        [InlineData("63001", "63399", UpsServiceType.UpsNextDayAirSaver, 6)]
        [InlineData("63001", "63399", UpsServiceType.UpsNextDayAir, 7)]
        public void Read_NumberOfZonesPerServicesIsCorrect(string originZipFloor,
            string originZipCeiling,
            UpsServiceType upsServiceType,
            int column)
        {
            var zipSheet = GetZipSheet(originZipFloor, originZipCeiling);

            IRange serviceRange = zipSheet.Range[2, column, zipSheet.UsedRange.LastRow, column];
            var expectedNumberOfZones = serviceRange.Cells.Where(IsZone).GroupBy(cell => cell.Value).Select(grp=>new {Zone=grp.Key, EntryCount=grp.Count()});
            var actualNumberOfZones =
                readZones.Where(
                        z =>
                            z.OriginZipFloor == int.Parse(originZipFloor) &&
                            z.OriginZipCeiling == int.Parse(originZipCeiling) && z.Service == (int) upsServiceType)
                    .GroupBy(zoneEntity => zoneEntity.Zone)
                    .Select(grp => new {Zone = grp.Key, EntryCount = grp.Count()});

            Assert.Equal(expectedNumberOfZones, actualNumberOfZones);
        }

        private IWorksheet GetZipSheet(string originZipFloor, string originZipCeiling) => 
            sampleExcelFile.Worksheets[$"{originZipFloor}-{originZipCeiling}"];


        private bool IsZone(IRange cell) => Regex.IsMatch(cell.Value ?? string.Empty, "^[0-9]{3}$");

        public void Dispose()
        {
            mock.Dispose();
            excelEngine.Dispose();
        }
    }
}
