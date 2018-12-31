using System;
using System.ComponentModel;
using Autofac;
using Autofac.Extras.Moq;
using ShipWorks.Products.Import;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Products.Tests.Import
{
    public class ImportSucceededStateTest : IDisposable
    {
        private readonly AutoMock mock;

        public ImportSucceededStateTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void Constructor_SetsProperties()
        {
            var importResult = mock.Mock<IImportProductsResult>();
            importResult.SetupGet(x => x.ExistingCount).Returns(1);
            importResult.SetupGet(x => x.NewCount).Returns(3);
            importResult.SetupGet(x => x.SuccessCount).Returns(4);
            importResult.SetupGet(x => x.TotalCount).Returns(5);

            var testObject = mock.Create<ImportSucceededState>(TypedParameter.From(importResult));

            Assert.Equal(1, testObject.ExistingCount);
            Assert.Equal(3, testObject.NewCount);
            Assert.Equal(4, testObject.SuccessCount);
        }

        [Fact]
        public void ShouldReloadProducts_ReturnsTrue()
        {
            var testObject = mock.Create<ImportSucceededState>();

            Assert.True(testObject.ShouldReloadProducts);
        }

        [Fact]
        public void CloseRequested_ShouldNotSetCancelToTrue()
        {
            var args = new CancelEventArgs();
            var testObject = mock.Create<ImportSucceededState>();

            testObject.CloseRequested(args);

            Assert.False(args.Cancel);
        }

        [Fact]
        public void CloseDialog_DelegatesToStateManager()
        {
            var testObject = mock.Create<ImportFailedState>();

            testObject.CloseDialog.Execute(null);

            mock.Mock<IProductImporterStateManager>().Verify(x => x.Close());
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
