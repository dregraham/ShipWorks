using System;
using System.Collections.Generic;
using System.ComponentModel;
using Autofac;
using Autofac.Extras.Moq;
using ShipWorks.Products.Import;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Products.Tests.Import
{
    public class ImportFailedStateTest : IDisposable
    {
        private readonly AutoMock mock;

        public ImportFailedStateTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void Constructor_SetsFailureReason()
        {
            var testObject = mock.Create<ImportFailedState>(TypedParameter.From(new Exception("Foo")));

            Assert.Equal("Foo", testObject.FailureReason);
        }

        [Fact]
        public void Constructor_SetsExtraDetails_WhenExceptionIsFailedProductImportException()
        {
            var importResult = mock.Mock<IImportProductsResult>();
            importResult.SetupGet(x => x.ExistingCount).Returns(1);
            importResult.SetupGet(x => x.FailedCount).Returns(2);
            importResult.SetupGet(x => x.NewCount).Returns(3);
            importResult.SetupGet(x => x.SuccessCount).Returns(4);
            importResult.SetupGet(x => x.TotalCount).Returns(5);
            importResult.SetupGet(x => x.FailureResults).Returns(new Dictionary<string, string> { { "foo", "bar" } });

            Exception exception = new FailedProductImportException(importResult.Object);
            var testObject = mock.Create<ImportFailedState>(TypedParameter.From(exception));

            Assert.Equal(1, testObject.ExistingCount);
            Assert.Equal(2, testObject.FailedCount);
            Assert.Equal(3, testObject.NewCount);
            Assert.Equal(4, testObject.SuccessCount);
            Assert.Equal("bar", testObject.ImportErrors["foo"]);
        }

        [Fact]
        public void HasImportErrors_ReturnsFalse_WhenThereAreNotImportErrors()
        {
            var importResult = mock.Mock<IImportProductsResult>();
            Exception exception = new FailedProductImportException(importResult.Object);
            var testObject = mock.Create<ImportFailedState>(TypedParameter.From(exception));

            Assert.False(testObject.HasImportErrors);
        }

        [Fact]
        public void HasImportErrors_ReturnsTrue_WhenThereAreImportErrors()
        {
            var importResult = mock.Mock<IImportProductsResult>();
            importResult.SetupGet(x => x.FailureResults).Returns(new Dictionary<string, string> { { "foo", "bar" } });

            Exception exception = new FailedProductImportException(importResult.Object);
            var testObject = mock.Create<ImportFailedState>(TypedParameter.From(exception));

            Assert.True(testObject.HasImportErrors);
        }

        [Fact]
        public void ShouldReloadProducts_ReturnsTrue()
        {
            var testObject = mock.Create<ImportFailedState>();

            Assert.True(testObject.ShouldReloadProducts);
        }

        [Fact]
        public void CloseRequested_ShouldNotSetCancelToTrue()
        {
            var args = new CancelEventArgs();
            var testObject = mock.Create<ImportFailedState>();

            testObject.CloseRequested(args);

            Assert.False(args.Cancel);
        }

        [Fact]
        public void StartImport_DelegatesToFileSelector()
        {
            var stateManager = mock.Mock<IProductImporterStateManager>();
            var testObject = mock.Create<ImportFailedState>();

            testObject.StartImport.Execute(null);

            mock.Mock<IProductImportFileSelector>().Verify(x => x.ChooseFileToImport(stateManager.Object));
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
