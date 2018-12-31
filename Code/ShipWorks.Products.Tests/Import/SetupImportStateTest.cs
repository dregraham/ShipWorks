using System;
using System.ComponentModel;
using Autofac.Extras.Moq;
using ShipWorks.Products.Import;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Products.Tests.Import
{
    public class SetupImportStateTest : IDisposable
    {
        private readonly AutoMock mock;

        public SetupImportStateTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void ShouldReloadProducts_ReturnsFalse()
        {
            var testObject = mock.Create<SetupImportState>();

            Assert.False(testObject.ShouldReloadProducts);
        }

        [Fact]
        public void CloseRequested_ShouldNotSetCancelToTrue()
        {
            var args = new CancelEventArgs();
            var testObject = mock.Create<SetupImportState>();

            testObject.CloseRequested(args);

            Assert.False(args.Cancel);
        }

        [Fact]
        public void SaveSample_DelegatesToProductImportFileSelector()
        {
            var testObject = mock.Create<SetupImportState>();

            testObject.SaveSample.Execute(null);

            mock.Mock<IProductImportFileSelector>().Verify(x => x.SaveSample());
        }

        [Fact]
        public void StartImport_DelegatesToProductImportFileSelector()
        {
            var stateManager = mock.Mock<IProductImporterStateManager>().Object;
            var testObject = mock.Create<SetupImportState>();

            testObject.StartImport.Execute(null);

            mock.Mock<IProductImportFileSelector>().Verify(x => x.ChooseFileToImport(stateManager));
        }

        [Fact]
        public void CloseDialog_DelegatesToStateManager()
        {
            var testObject = mock.Create<SetupImportState>();

            testObject.CloseDialog.Execute(null);

            mock.Mock<IProductImporterStateManager>().Verify(x => x.Close());
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
