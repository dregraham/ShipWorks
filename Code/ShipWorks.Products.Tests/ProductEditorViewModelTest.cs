using System;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Products.ProductEditor;
using ShipWorks.Products.UI;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.OrderLookup.Tests
{
    public class ProductEditorViewModelTest
    {
        private readonly AutoMock mock;
        private readonly ProductEditorViewModel testObject;

        public ProductEditorViewModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            testObject = mock.Create<ProductEditorViewModel>();
        }

        [Fact]
        public async Task ShowProductEditor_DelegatesToMessageHelperShowDialog()
        {
            var product = new ProductVariantAliasEntity()
            {
                IsDefault = true,
                ProductVariant = new ProductVariantEntity()
                {
                    Product = new ProductEntity()
                }
            };

            await testObject.ShowProductEditor(product.ProductVariant).ConfigureAwait(true);

            mock.Mock<IMessageHelper>().Verify(m => m.ShowDialog(It.IsAny<IDialog>()));
        }
    }
}
