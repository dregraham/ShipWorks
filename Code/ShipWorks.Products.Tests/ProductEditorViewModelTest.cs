using System;
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
        public void ShowProductEditor_DelegatesToMessageHelperShowDialog()
        {
            var product = new ProductVariantAliasEntity()
            {
                IsDefault = true,
                ProductVariant = new ProductVariantEntity()
                {
                    Product = new ProductEntity()
                }
            };

            testObject.ShowProductEditor(product.ProductVariant);

            mock.Mock<IMessageHelper>().Verify(m => m.ShowDialog(It.IsAny<IDialog>()));
        }

        [Fact]
        public void SaveProduct_ShowsError_WhenSkuIsBlank()
        {
            var product = new ProductVariantAliasEntity()
            {
                IsDefault = true,
                ProductVariant = new ProductVariantEntity()
                {
                    Product = new ProductEntity()
                }
            };

            testObject.ShowProductEditor(product.ProductVariant);

            testObject.Save.Execute(null);

            mock.Mock<IMessageHelper>().Verify(m => m.ShowError($"The following field is required: {Environment.NewLine}SKU"));
        }
    }
}
