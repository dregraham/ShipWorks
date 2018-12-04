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

        [Fact]
        public void ImportProducts_ReturnsSuccess_WhenDialogSucceeds()
        {
            mock.Mock<IMessageHelper>().Setup(x => x.ShowDialog(It.IsAny<IProductImporterDialog>())).Returns(true);

            var result = testObject.ImportProducts();

            Assert.True(result.Success);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(null)]
        public void ImportProducts_ReturnsFailure_WhenDialogDoesNotSucceed(bool? dialogResult)
        {
            mock.Mock<IMessageHelper>().Setup(x => x.ShowDialog(It.IsAny<IProductImporterDialog>())).Returns(dialogResult);

            var result = testObject.ImportProducts();

            Assert.True(result.Failure);
        }

        [Fact]
        public void CloseDialog_CallsCloseOnOpenedDialog()
        {
            var dialog = mock.Mock<IProductImporterDialog>();
            createDialogMock.Setup(f => f(testObject)).Returns(() => dialog.Object);

            testObject.ImportProducts();

            testObject.CloseDialog.Execute(null);

            dialog.Verify(x => x.Close());
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
