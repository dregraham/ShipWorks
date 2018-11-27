using Autofac.Extras.Moq;
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
    }
}
