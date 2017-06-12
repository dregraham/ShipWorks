using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.Ups.LocalRating;
using ShipWorks.Shipping.UI.Carriers.Ups.LocalRating;
using ShipWorks.Tests.Shared;
using Syncfusion.XlsIO;
using Xunit;

namespace ShipWorks.Shipping.Tests.Integration.Carriers.Ups.LocalRating
{
    public class UpsImportedRateValidatorTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly UpsImportedRateValidator testObject;

        public UpsImportedRateValidatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<UpsImportedRateValidator>();
        }

        [Fact]
        public void Validate_DoesNotThrowError_WhenValidatingSampleFile()
        {
            var rateTable = mock.CreateMock<IUpsLocalRateTable>(table =>
            {
                table.Setup(t =>
                            t.ReplaceRates(It.IsAny<IEnumerable<UpsPackageRateEntity>>(),
                                It.IsAny<IEnumerable<UpsLetterRateEntity>>(),
                                It.IsAny<IEnumerable<UpsPricePerPoundEntity>>()))
                    .Callback<IEnumerable<UpsPackageRateEntity>, IEnumerable<UpsLetterRateEntity>, IEnumerable<UpsPricePerPoundEntity>>(Validate);
            });

            using (var excelEngine = new ExcelEngine())
            {
                Assembly shippingAssembly = Assembly.GetAssembly(typeof(UpsLocalRatingViewModel));
                IWorkbook sampleExcelFile;

                using (
                    Stream sampleExcelStream =
                        shippingAssembly.GetManifestResourceStream(UpsLocalRatingViewModel.SampleRatesFileResourceName))
                {
                    sampleExcelFile = excelEngine.Excel.Workbooks.Open(sampleExcelStream, ExcelOpenType.Automatic);
                }

                var rateReader = mock.Create<ServiceUpsRateExcelReader>();
                rateReader.Read(sampleExcelFile.Worksheets, rateTable.Object);
            }
        }

        private void Validate(IEnumerable<UpsPackageRateEntity> packageRates, IEnumerable<UpsLetterRateEntity> letterRates, IEnumerable<UpsPricePerPoundEntity> pricesPerPound)
        {
            testObject.Validate(
                packageRates.Cast<IUpsPackageRateEntity>().ToList(),
                letterRates.Cast<IUpsLetterRateEntity>().ToList(),
                pricesPerPound.Cast<IUpsPricePerPoundEntity>().ToList());
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
