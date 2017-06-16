using System;
using System.Collections;
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

namespace ShipWorks.Shipping.Tests.Integration.Carriers.Ups.LocalRating
{
    public class ServiceUpsRateExcelReaderTest : IDisposable
    {
        readonly AutoMock mock;
        private IEnumerable<UpsPackageRateEntity> readPackageRates;
        private IEnumerable<UpsLetterRateEntity> readLetterRates;
        private IEnumerable<UpsPricePerPoundEntity> readPricesPerPound;
        private readonly ExcelEngine excelEngine;
        private readonly IWorkbook sampleExcelFile;

        public ServiceUpsRateExcelReaderTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            var rateTable = mock.CreateMock<IUpsLocalRateTable>(table =>
            {
                table.Setup(t =>
                            t.ReplaceRates(It.IsAny<IEnumerable<UpsPackageRateEntity>>(),
                                It.IsAny<IEnumerable<UpsLetterRateEntity>>(),
                                It.IsAny<IEnumerable<UpsPricePerPoundEntity>>()))
                    .Callback<IEnumerable<UpsPackageRateEntity>, IEnumerable<UpsLetterRateEntity>, IEnumerable<UpsPricePerPoundEntity>>(SaveReadRates);
            });

            excelEngine = new ExcelEngine();
            Assembly shippingAssembly = Assembly.GetAssembly(typeof(UpsLocalRatingViewModel));
            using (Stream sampleExcelStream = shippingAssembly.GetManifestResourceStream(UpsLocalRatingViewModel.SampleRatesFileResourceName))
            {
                sampleExcelFile = excelEngine.Excel.Workbooks.Open(sampleExcelStream, ExcelOpenType.Automatic);
            }

            ServiceUpsRateExcelReader testObject = mock.Create<ServiceUpsRateExcelReader>();
            testObject.Read(sampleExcelFile.Worksheets, rateTable.Object);
        }

        private void SaveReadRates(IEnumerable<UpsPackageRateEntity> packageRates, IEnumerable<UpsLetterRateEntity> letterRates, IEnumerable<UpsPricePerPoundEntity> pricesPerPound)
        {
            readPackageRates = packageRates;
            readLetterRates = letterRates;
            readPricesPerPound = pricesPerPound;
        }

        [Fact]
        public void NumberOfPackageRatesInSheet_MatchesNumberOfImportedPackageRates()
        {
            int packageRateCalculatedCount = CalculateRateCount(cell => cell.HasNumber);
            
            Assert.True(packageRateCalculatedCount > 0);
            Assert.Equal(packageRateCalculatedCount, readPackageRates.Count());
        }

        [Fact]
        public void NumberOfLetterRatesInSheet_MatchesNumberOfImportedLetterRates()
        {
            int letterRateCalculatedCount = CalculateRateCount(cell => cell.Text == "Letter");

            Assert.True(letterRateCalculatedCount > 0);
            Assert.Equal(letterRateCalculatedCount, readLetterRates.Count());
        }

        [Fact]
        public void NumberOfPricesPerPoundInSheet_MatchesNumberOfPricesPerPound()
        {
            int pricePerPoundCalculatedCount = CalculateRateCount(cell => cell.Text == "Price Per Pound");

            Assert.True(pricePerPoundCalculatedCount > 0);
            Assert.Equal(pricePerPoundCalculatedCount, readPricesPerPound.Count());
        }

        private int CalculateRateCount(Func<IRange, bool> shouldIncludeRowFunc)
        {
            int calculatedRateCount = 0;

            foreach (IWorksheet sheet in ((IEnumerable<IWorksheet>) sampleExcelFile.Worksheets).Where(
                s => s.Range["A1"].Text == "Zones"))
            {
                int zoneCount = sheet.Rows[0].Cells.Count(cell => cell.HasNumber);
                int weightCount = sheet.Columns[0].Cells.Count(shouldIncludeRowFunc);

                calculatedRateCount += zoneCount * weightCount;
            }

            return calculatedRateCount;
        }

        public void Dispose()
        {
            mock.Dispose();
            excelEngine.Dispose();
        }
    }
}