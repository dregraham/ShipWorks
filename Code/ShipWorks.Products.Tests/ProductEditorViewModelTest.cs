using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
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
        public void ShowProductEditor_SetsProductVarient_WhenProductVariantIsNull()
        {
            var product = new ProductVariantAliasEntity()
            {
                ProductVariant = null
            };

            testObject.ShowProductEditor(product);

            Assert.NotNull(product.ProductVariant);
        }

        [Fact]
        public void ShowProductEditor_SetsProductVarientProduct_WhenProductVariantProductIsNull()
        {
            var product = new ProductVariantAliasEntity()
            {
                ProductVariant = new ProductVariantEntity()
                {
                    Product = null
                }
            };

            testObject.ShowProductEditor(product);

            Assert.NotNull(product.ProductVariant.Product);
        }

        [Fact]
        public void ShowProductEditor_DelegatesToMessageHelperShowDialog()
        {
            var product = new ProductVariantAliasEntity()
            {
                ProductVariant = new ProductVariantEntity()
                {
                    Product = new ProductEntity()
                }
            };

            testObject.ShowProductEditor(product);

            mock.Mock<IMessageHelper>().Verify(m => m.ShowDialog(It.IsAny<IDialog>()));
        }

        [Fact]
        public void SaveProduct_ShowsError_WhenSkuIsBlank()
        {
            var product = new ProductVariantAliasEntity()
            {
                ProductVariant = new ProductVariantEntity()
                {
                    Product = new ProductEntity()
                }
            };

            testObject.ShowProductEditor(product);

            testObject.Save.Execute(null);

            mock.Mock<IMessageHelper>().Verify(m => m.ShowError("Please enter a value for SKU."));
        }
    }
}
