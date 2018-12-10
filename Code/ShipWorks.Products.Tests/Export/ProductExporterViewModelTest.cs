using System;
using Autofac.Extras.Moq;
using Interapptive.Shared.IO;
using Interapptive.Shared.Threading;
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

        //[Fact]
        //public void ExportProducts_DelegatesToProduct

        [Fact]
        public void ExportProducts_DoesNotDelegateToProductExporter_WhenFileSelectorIsCanceled()
        {
            mock.Mock<IFileSelector>().Setup(x => x.GetFilePathToSave(AnyString, AnyString)).Returns(GenericResult.FromError<string>("Foo"));
            var testObject = mock.Create<ProductExporterViewModel>();

            testObject.ExportProducts();

            mock.Mock<IProductExporter>().Verify(x => x.Export(AnyString, It.IsAny<IProgressReporter>()), Times.Never);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
