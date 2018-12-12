using System.Linq;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Products.AttributeEditor;
using ShipWorks.Tests.Shared;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Products.Tests
{
    public class AttributeEditorViewModelTest
    {
        private readonly AutoMock mock;
        private readonly AttributeEditorViewModel testObject;

        public AttributeEditorViewModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<AttributeEditorViewModel>();
        }

        [Fact]
        public async Task Load_LoadsAttributesNamesFromBaseProduct()
        {
            ProductAttributeEntity attributeEntity = new ProductAttributeEntity(){AttributeName = "foo"};
            ProductVariantEntity productVariant = new ProductVariantEntity();
            ProductEntity productEntity = new ProductEntity();
            productEntity.Variants.Add(productVariant);
            productEntity.Attributes.Add(attributeEntity);

            await testObject.Load(productVariant);

            Assert.Equal("foo", testObject.AttributeNames.SingleOrDefault());
        }

        [Fact]
        public async Task Load_LoadsAttributesFromProductVariant()
        {
            ProductVariantAttributeEntity variantAttributeEntity = new ProductVariantAttributeEntity();

            ProductVariantEntity productVariant = new ProductVariantEntity();
            productVariant.Attributes.Add(variantAttributeEntity);

            ProductEntity productEntity = new ProductEntity();
            productEntity.Variants.Add(productVariant);

            await testObject.Load(productVariant);

            Assert.Equal(variantAttributeEntity, testObject.ProductAttributes.SingleOrDefault());
        }

        [Fact]
        public async Task Save_SavesAttributesToProduct()
        {
            ProductVariantEntity productVariant = new ProductVariantEntity();
            ProductEntity productEntity = new ProductEntity();
            productEntity.Variants.Add(productVariant);

            await testObject.Load(productVariant);

            ProductVariantAttributeEntity variantAttributeEntity = new ProductVariantAttributeEntity();
            testObject.ProductAttributes.Add(variantAttributeEntity);

            testObject.Save();

            Assert.Equal(variantAttributeEntity, productVariant.Attributes.SingleOrDefault());
        }

        [Fact]
        public void AddAttribute_DisplaysError_WhenAttributeNameIsNotEntered()
        {
            ProductVariantEntity productVariant = new ProductVariantEntity();
            ProductEntity productEntity = new ProductEntity();
            productEntity.Variants.Add(productVariant);

            testObject.AddAttributeToProductCommand.Execute(null);

            mock.Mock<IMessageHelper>().Verify(x => x.ShowError("Please enter an attribute name."));
        }

        [Fact]
        public void AddAttribute_DisplaysError_WhenProductAlreadyHasAttributeWithGivenName()
        {
            ProductAttributeEntity productAttributeEntity = new ProductAttributeEntity() {AttributeName = "foo"};

            ProductVariantAttributeEntity variantAttributeEntity = new ProductVariantAttributeEntity();
            variantAttributeEntity.ProductAttribute = productAttributeEntity;
            ProductVariantEntity productVariant = new ProductVariantEntity();
            productVariant.Attributes.Add(variantAttributeEntity);

            ProductEntity productEntity = new ProductEntity();
            productEntity.Variants.Add(productVariant);

            testObject.ProductAttributes.Add(variantAttributeEntity);
            testObject.SelectedAttributeName = "foo";
            testObject.AddAttributeToProductCommand.Execute(null);

            mock.Mock<IMessageHelper>().Verify(x => x.ShowError("This product already contains an attribute named \"foo\""));
        }

        [Fact]
        public async Task AddAttribute_AddsAttributeToProductAttributesList()
        {
            ProductAttributeEntity productAttributeEntity = new ProductAttributeEntity(){ AttributeName = "foo"};
            mock.Mock<IProductCatalog>().Setup(x => x.FetchProductAttribute(It.IsAny<ISqlAdapter>(), "foo", AnyLong))
                .Returns(productAttributeEntity);

            ProductVariantEntity productVariant = new ProductVariantEntity();

            ProductEntity productEntity = new ProductEntity();
            productEntity.Variants.Add(productVariant);

            await testObject.Load(productVariant);

            testObject.SelectedAttributeName = "foo";
            testObject.AttributeValue = "value";
            testObject.AddAttributeToProductCommand.Execute(null);

            ProductVariantAttributeEntity result = testObject.ProductAttributes.FirstOrDefault();
            Assert.Equal(productAttributeEntity.AttributeName, result.ProductAttribute.AttributeName);
            Assert.Equal("value", result.AttributeValue);
        }

        [Fact]
        public async Task AddAttribute_FetchesProductAttribute()
        {
            ProductVariantEntity productVariant = new ProductVariantEntity();
            ProductEntity productEntity = new ProductEntity();
            productEntity.Variants.Add(productVariant);

            await testObject.Load(productVariant);

            testObject.SelectedAttributeName = "foo";
            testObject.AttributeValue = "value";
            testObject.AddAttributeToProductCommand.Execute(null);

            mock.Mock<IProductCatalog>().Verify(x => x.FetchProductAttribute(It.IsAny<ISqlAdapter>(), testObject.SelectedAttributeName, AnyLong));
        }

        [Fact]
        public void RemoveAttribute_RemovesAttributeFromProductAttributesList()
        {
            ProductVariantAttributeEntity variantAttributeEntity = new ProductVariantAttributeEntity();

            ProductVariantEntity productVariant = new ProductVariantEntity();
            productVariant.Attributes.Add(variantAttributeEntity);

            ProductEntity productEntity = new ProductEntity();
            productEntity.Variants.Add(productVariant);

            testObject.ProductAttributes.Add(variantAttributeEntity);
            testObject.SelectedProductAttribute = variantAttributeEntity;
            testObject.RemoveAttributeFromProductCommand.Execute(null);

            Assert.Empty(testObject.ProductAttributes);
        }

        [Fact]
        public void RemoveProductCommand_IsAllowed_WhenSelectedProductIsNotNull()
        {
            testObject.SelectedProductAttribute = new ProductVariantAttributeEntity();
            Assert.True(testObject.RemoveAttributeFromProductCommand.CanExecute(null));
        }

        [Fact]
        public void RemoveProductCommand_IsNotAllowed_WhenSelectedProductIsNull()
        {
            testObject.SelectedProductAttribute = null;
            Assert.False(testObject.RemoveAttributeFromProductCommand.CanExecute(null));
        }
    }
}