using System;
using Autofac.Extras.Moq;
using Interapptive.Shared.IO;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Products.Export;
using ShipWorks.Tests.Shared;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Products.Tests.Export
{
    public class ProductExporterViewModelTest : IDisposable
    {
        private readonly AutoMock mock;

        public ProductExporterViewModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void ExportProducts_DelegatesToFileSelector()
        {
            mock.Mock<IDateTimeProvider>().SetupGet(x => x.Now).Returns(new DateTime(2018, 12, 10, 11, 7, 13));
            var testObject = mock.Create<ProductExporterViewModel>();

            testObject.ExportProducts();

            mock.Mock<IFileSelector>().Verify(x => x.GetFilePathToSave("Comma Separated|*.csv|All Files|*.*", "ProductExport_2018-12-10_110713.csv"));
        }

        [Fact]
        public void ExportProducts_DoesNotDelegateToProductExporter_WhenFileSelectorIsCanceled()
        {
            mock.Mock<IFileSelector>().Setup(x => x.GetFilePathToSave(AnyString, AnyString)).Returns(GenericResult.FromError<string>("Foo"));
            var testObject = mock.Create<ProductExporterViewModel>();

            testObject.ExportProducts();

            mock.Mock<IProductExporter>().Verify(x => x.Export(AnyString, It.IsAny<IProgressReporter>()), Times.Never);
        }

        [Fact]
        public void ExportProducts_DelegatesToProductExporter_WhenFileNameIsSelected()
        {
            mock.Mock<IFileSelector>().Setup(x => x.GetFilePathToSave(AnyString, AnyString)).Returns("Bar");
            var testObject = mock.Create<ProductExporterViewModel>();

            testObject.ExportProducts();

            mock.Mock<IProductExporter>().Verify(x => x.Export("Bar", It.IsAny<IProgressReporter>()));
        }

        [Fact]
        public void ExportProducts_ShowsInformationMessage_WhenExportSucceed()
        {
            mock.Mock<IFileSelector>().Setup(x => x.GetFilePathToSave(AnyString, AnyString)).Returns("Bar");
            var testObject = mock.Create<ProductExporterViewModel>();

            testObject.ExportProducts();

            mock.Mock<IMessageHelper>().Verify(x => x.ShowInformation("Product export succeeded"));
        }

        [Fact]
        public void ExportProducts_ShowEsrror_WhenExportFails()
        {
            var exception = new Exception("Foo");
            mock.Mock<IProductExporter>().Setup(x => x.Export(AnyString, It.IsAny<IProgressReporter>())).ThrowsAsync(exception);
            mock.Mock<IFileSelector>().Setup(x => x.GetFilePathToSave(AnyString, AnyString)).Returns("Bar");
            var testObject = mock.Create<ProductExporterViewModel>();

            testObject.ExportProducts();

            mock.Mock<IMessageHelper>().Verify(x => x.ShowError("Foo", exception));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
