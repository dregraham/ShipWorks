using System.Linq;
using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Products.AliasEditor;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Products.Tests
{
    public class AliasEditorViewModelTest
    {
        private readonly AutoMock mock;
        private readonly AliasEditorViewModel testObject;

        public AliasEditorViewModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<AliasEditorViewModel>();
        }

        [Fact]
        public void Load_DoesNotLoadDefaultAlias()
        {
            ProductVariantAliasEntity defaultAliasEntity = new ProductVariantAliasEntity{ IsDefault = true };
            ProductVariantAliasEntity variantAliasEntity = new ProductVariantAliasEntity();

            ProductVariantEntity productVariant = new ProductVariantEntity();
            productVariant.Aliases.Add(defaultAliasEntity);
            productVariant.Aliases.Add(variantAliasEntity);

            testObject.Load(productVariant);

            Assert.DoesNotContain(defaultAliasEntity, testObject.ProductAliases);
        }

        [Fact]
        public void Load_LoadsAliasesFromProductVariant()
        {
            ProductVariantAliasEntity variantAliasEntity = new ProductVariantAliasEntity();

            ProductVariantEntity productVariant = new ProductVariantEntity();
            productVariant.Aliases.Add(variantAliasEntity);

            testObject.Load(productVariant);

            Assert.Equal(variantAliasEntity, testObject.ProductAliases.SingleOrDefault());
        }

        [Fact]
        public void Save_SavesAliasesToProductVariant()
        {
            ProductVariantEntity productVariant = new ProductVariantEntity();

            testObject.Load(productVariant);

            ProductVariantAliasEntity variantAliasEntity = new ProductVariantAliasEntity();
            testObject.ProductAliases.Add(variantAliasEntity);

            testObject.Save();

            Assert.Equal(variantAliasEntity, productVariant.Aliases.SingleOrDefault());
        }

        [Fact]
        public void Save_DoesNotRemoveDefaultAlias()
        {
            ProductVariantAliasEntity defaultAliasEntity = new ProductVariantAliasEntity {IsDefault = true};
            ProductVariantEntity productVariant = new ProductVariantEntity();
            productVariant.Aliases.Add(defaultAliasEntity);
            
            testObject.Load(productVariant);

            ProductVariantAliasEntity variantAliasEntity = new ProductVariantAliasEntity();
            testObject.ProductAliases.Add(variantAliasEntity);

            testObject.Save();

            Assert.Contains(defaultAliasEntity, productVariant.Aliases);
        }
        
        [Fact]
        public void AddAlias_DisplaysError_WhenAliasSkuIsNotEntered()
        {
            testObject.AddAliasCommand.Execute(null);

            mock.Mock<IMessageHelper>().Verify(x => x.ShowError("Please enter an alias sku."));
        }

        [Fact]
        public void AddAlias_DisplaysError_WhenEnteredAliasSkuMatchesDefaultAliasSku()
        {
            ProductVariantAliasEntity defaultAliasEntity = new ProductVariantAliasEntity
            {
                IsDefault = true, 
                Sku = "foo"
            };

            ProductVariantEntity productVariant = new ProductVariantEntity();
            productVariant.Aliases.Add(defaultAliasEntity);

            testObject.Load(productVariant);

            testObject.AliasSku = "foo";
            
            testObject.AddAliasCommand.Execute(null);
            
            mock.Mock<IMessageHelper>().Verify(x => x.ShowError("\"foo\" is already the default sku for this product."));
        }

        [Fact]
        public void AddAlias_DisplaysError_WhenProductAlreadyHasAliasWithGivenSku()
        {
            ProductVariantAliasEntity defaultAliasEntity = new ProductVariantAliasEntity
            {
                IsDefault = false, 
                Sku = "foo"
            };

            ProductVariantEntity productVariant = new ProductVariantEntity();
            productVariant.Aliases.Add(defaultAliasEntity);

            testObject.Load(productVariant);

            testObject.AliasSku = "foo";
            
            testObject.AddAliasCommand.Execute(null);
            
            mock.Mock<IMessageHelper>().Verify(x => x.ShowError("This product already contains an alias with the sku \"foo\"."));
        }

        [Fact]
        public void AddAlias_AddsAliasToProductAliasesList()
        {
            ProductVariantEntity productVariant = new ProductVariantEntity();
            ProductEntity productEntity = new ProductEntity();
            productEntity.Variants.Add(productVariant);

            testObject.Load(productVariant);

            testObject.AliasName = "foo";
            testObject.AliasSku = "bar";
            testObject.AddAliasCommand.Execute(null);

            ProductVariantAliasEntity result = testObject.ProductAliases.FirstOrDefault();
            Assert.Equal("foo", result.AliasName);
            Assert.Equal("bar", result.Sku);
        }

        [Fact]
        public void AddAlias_ClearsAliasNameAndSku_WhenAliasAddedSuccessfully()
        {
            ProductVariantEntity productVariant = new ProductVariantEntity();

            testObject.Load(productVariant);

            testObject.AliasName = "foo";
            testObject.AliasSku = "bar";
            testObject.AddAliasCommand.Execute(null);
            
            Assert.Empty(testObject.AliasName);
            Assert.Empty(testObject.AliasSku);
        }
        
        [Fact]
        public void AddAlias_DoesNotClearAliasNameAndSku_WhenAliasFailsToAdd()
        {
            ProductVariantAliasEntity defaultAliasEntity = new ProductVariantAliasEntity
            {
                IsDefault = true, 
                Sku = "bar"
            };
            ProductVariantEntity productVariant = new ProductVariantEntity();
            productVariant.Aliases.Add(defaultAliasEntity);

            testObject.Load(productVariant);

            testObject.AliasName = "foo";
            testObject.AliasSku = "bar";
            
            // This will fail to actually add, due to AliasSku matching default sku
            testObject.AddAliasCommand.Execute(null);
            
            Assert.Equal("foo", testObject.AliasName);
            Assert.Equal("bar", testObject.AliasSku);
        }

        [Fact]
        public void RemoveAlias_RemovesAliasFromProductAliasesList()
        {
            ProductVariantAliasEntity variantAliasEntity = new ProductVariantAliasEntity();

            ProductVariantEntity productVariant = new ProductVariantEntity();
            productVariant.Aliases.Add(variantAliasEntity);

            testObject.ProductAliases.Add(variantAliasEntity);
            testObject.SelectedProductAlias = variantAliasEntity;
            testObject.RemoveAliasCommand.Execute(null);

            Assert.Empty(testObject.ProductAliases);
        }

        [Fact]
        public void RemoveProductCommand_IsAllowed_WhenSelectedProductIsNotNull()
        {
            testObject.SelectedProductAlias = new ProductVariantAliasEntity();
            Assert.True(testObject.RemoveAliasCommand.CanExecute(null));
        }

        [Fact]
        public void RemoveProductCommand_IsNotAllowed_WhenSelectedProductIsNull()
        {
            testObject.SelectedProductAlias = null;
            Assert.False(testObject.RemoveAliasCommand.CanExecute(null));
        }
    }
}