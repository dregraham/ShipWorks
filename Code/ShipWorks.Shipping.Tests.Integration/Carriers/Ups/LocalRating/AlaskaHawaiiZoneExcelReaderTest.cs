using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Ups.LocalRating;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.UI.Carriers.Ups.LocalRating;
using ShipWorks.Tests.Shared;
using Syncfusion.XlsIO;
using Xunit;

namespace ShipWorks.Shipping.Tests.Integration.Carriers.Ups.LocalRating
{
    public class AlaskaHawaiiZoneExcelReaderTest
    {
        private readonly AutoMock mock;
        private readonly IEnumerable<UpsLocalRatingZoneEntity> readAlaskaHawaiiZones;
        private readonly ExcelEngine excelEngine;
        private readonly IWorkbook sampleExcelFile;

        public AlaskaHawaiiZoneExcelReaderTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            excelEngine = new ExcelEngine();
            Assembly shippingAssembly = Assembly.GetAssembly(typeof(UpsLocalRatingViewModel));

            using (Stream sampleExcelStream = shippingAssembly.GetManifestResourceStream(UpsLocalRatingViewModel.SampleZoneFileResourceName))
            {
                sampleExcelFile = excelEngine.Excel.Workbooks.Open(sampleExcelStream, ExcelOpenType.Automatic);
            }

            var testObject = mock.Create<AlaskaHawaiiZoneExcelReader>();
            readAlaskaHawaiiZones = testObject.GetAlaskaHawaiiZones(sampleExcelFile.Worksheets);
        }

        [Theory]
        [InlineData("AK", "99")]
        [InlineData("HI", "96")]
        public void Read_ReturnsCorrectNumberOfZones(string sheetName, string zipPrefix)
        {
            IWorksheet dasSheet = sampleExcelFile.Worksheets[sheetName];
            int zipCodeCount = dasSheet.Cells.Count(IsZip);
            
            // each zip code found in this sheet should have 3 services associated with it.
            int expectedZonesForState = zipCodeCount * 3;

            Assert.Equal(expectedZonesForState, readAlaskaHawaiiZones.Count(z=>z.DestinationZipCeiling.ToString().StartsWith(zipPrefix)));
        }

        [Theory]
        [InlineData("AK", "99", UpsServiceType.UpsGround, "44", 5, 11)]
        [InlineData("AK", "99", UpsServiceType.UpsNextDayAir, "124", 5, 11)]
        [InlineData("AK", "99", UpsServiceType.Ups2DayAir, "224", 5, 11)]
        [InlineData("AK", "99", UpsServiceType.UpsGround, "46", 17, 41)]
        [InlineData("AK", "99", UpsServiceType.UpsNextDayAir, "126", 17, 41)]
        [InlineData("AK", "99", UpsServiceType.Ups2DayAir, "226", 17, 41)]
        [InlineData("HI", "96", UpsServiceType.UpsGround, "44", 5, 13)]
        [InlineData("HI", "96", UpsServiceType.UpsNextDayAir, "124", 5, 13)]
        [InlineData("HI", "96", UpsServiceType.Ups2DayAir, "224", 5, 13)]
        [InlineData("HI", "96", UpsServiceType.UpsGround, "46", 19, 26)]
        [InlineData("HI", "96", UpsServiceType.UpsNextDayAir, "126", 19, 26)]
        [InlineData("HI", "96", UpsServiceType.Ups2DayAir, "226", 19, 26)]
        public void Read_SavesCorrectNumberOfUS48Das(string sheetName, string zipPrefix, UpsServiceType service, string zone, int startRow, int endRow)
        {
            IWorksheet dasSheet = sampleExcelFile.Worksheets[sheetName];
            int zipCodeCount = dasSheet.Rows.Where(r => r.Row >= startRow && r.Row <= endRow).Sum(row => row.Cells.Count(IsZip));

            Assert.Equal(zipCodeCount, readAlaskaHawaiiZones.Count(z=>MatchesCriteria(z, zipPrefix, service, zone)));
        }

        private static bool MatchesCriteria(UpsLocalRatingZoneEntity upsLocalRatingZoneEntity, string zipPrefix, UpsServiceType service, string zone)
        {
            // service doesn't match
            if (upsLocalRatingZoneEntity.Service != (int) service)
            {
                return false;
            }

            if (!upsLocalRatingZoneEntity.DestinationZipCeiling.ToString().StartsWith(zipPrefix))
            {
                return false;
            }

            if (upsLocalRatingZoneEntity.Zone != zone)
            {
                return false;
            }

            return true;
        }

        private static bool IsZip(IRange cell)
        {
            return Regex.IsMatch(cell?.Text ?? cell?.Value ?? string.Empty, "^[0-9]{5}$");
        }

        public void Dispose()
        {
            mock.Dispose();
            excelEngine.Dispose();
        }
    }
}
