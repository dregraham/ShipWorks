using System;
using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.Products.Import;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Products.Tests.Import
{
    public class ProductImporterViewModelTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly Mock<Func<IProductImporterViewModel, IProductImporterDialog>> createDialogMock;
        private readonly ProductImporterViewModel testObject;

        public ProductImporterViewModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            createDialogMock = mock.MockFunc<IProductImporterViewModel, IProductImporterDialog>();
            testObject = mock.Create<ProductImporterViewModel>();
        }

        [Fact]
        public void ImportProducts_ShowsDialog()
        {
            var dialog = mock.Build<IProductImporterDialog>();
            createDialogMock.Setup(f => f(testObject)).Returns(() => dialog);

            testObject.ImportProducts();

            mock.Mock<IMessageHelper>().Verify(x => x.ShowDialog(dialog));
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void ImportProducts_ReturnsSuccess_BasedOnCurrentState(bool shouldReload, bool expectedSuccess)
        {
            var testState = mock.Mock<IProductImportState>();
            testState.SetupGet(x => x.ShouldReloadProducts).Returns(shouldReload);

            mock.Mock<IMessageHelper>()
                .Setup(x => x.ShowDialog(It.IsAny<IProductImporterDialog>()))
                .Callback(() => testObject.CurrentState = testState.Object);

            var result = testObject.ImportProducts();

            Assert.Equal(expectedSuccess, result.Success);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
