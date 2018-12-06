using System.Linq;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Products.BundleEditor;
using ShipWorks.Products.UI.BundleEditor;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Products.Tests
{
    public class BundleEditorViewModelTest
    {
        private readonly AutoMock mock; 
        private readonly BundleEditorViewModel testObject;

        public BundleEditorViewModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<BundleEditorViewModel>();
        }
        
        [Fact]
        public void Load_SetsBundleLineItemsToEmptyList_WhenGivenProductIsNotABundle()
        {
            ProductVariantEntity productVariant = new ProductVariantEntity();
            productVariant.Product = new ProductEntity();
            productVariant.Product.IsBundle = false;
            
            testObject.Load(productVariant);
        }
        
        [Fact]
        public void Load_LoadsBundleItemsFromGivenProduct_WhenGivenProductIsABundle()
        {
            ProductVariantEntity baseProduct = new ProductVariantEntity();
            baseProduct.Product = new ProductEntity(1);
            baseProduct.Product.IsBundle = true;
            
            ProductVariantEntity bundleChildProduct = new ProductVariantEntity(5);
            bundleChildProduct.Aliases.Add(new ProductVariantAliasEntity {Sku = "a", IsDefault = true});
            
            ProductBundleEntity bundledProduct = new ProductBundleEntity();
            bundledProduct.Quantity = 2;
            bundledProduct.ChildProductVariantID = 5;
            bundledProduct.ChildVariant = bundleChildProduct;
            
            baseProduct.Product.Bundles.Add(bundledProduct);

            testObject.Load(baseProduct);
            
            Assert.Equal(bundledProduct, testObject.BundleLineItems.FirstOrDefault()?.BundledProduct);
        }

        [Fact]
        public void Save_SavesBundleLineItemsToGivenProduct()
        {
            // Load first
            ProductVariantEntity baseProduct = new ProductVariantEntity();
            baseProduct.Product = new ProductEntity(1);
            baseProduct.Product.IsBundle = true;
            
            testObject.Load(baseProduct);

            // add product
            ProductVariantEntity productToAddToBundle = new ProductVariantEntity
            {
                ProductID = 9,
                ProductVariantID = 2,
                Product = new ProductEntity { IsBundle = false }
            };
            
            var sqlAdapter = mock.Mock<ISqlAdapter>();
            mock.Mock<ISqlAdapterFactory>().Setup(x => x.Create()).Returns(sqlAdapter.Object);
            mock.Mock<IProductCatalog>()
                .Setup(x => x.FetchProductVariantEntity(It.IsAny<ISqlAdapter>(), It.IsAny<string>())).Returns(productToAddToBundle);

            testObject.Sku = "a";
            testObject.Quantity = 2;
            testObject.AddSkuToBundleCommand.Execute(null);

            // Save
            testObject.Save();

            Assert.True(
                baseProduct.Product.Bundles.Any(
                    x => x.ProductID == 1 && x.ChildProductVariantID == 2 && x.Quantity == 2));
        }

        [Fact]
        public void AddProductToBundle_DisplaysError_WhenSkuNotFound()
        {
            // Load first
            ProductVariantEntity baseProduct = new ProductVariantEntity();
            baseProduct.Product = new ProductEntity(1);
            baseProduct.Product.IsBundle = false;

            testObject.Load(baseProduct);

            var sqlAdapter = mock.Mock<ISqlAdapter>();
            mock.Mock<ISqlAdapterFactory>().Setup(x => x.Create()).Returns(sqlAdapter.Object);
            mock.Mock<IProductCatalog>()
                .Setup(x => x.FetchProductVariantEntity(It.IsAny<ISqlAdapter>(), It.IsAny<string>())).Returns<ProductVariantEntity>(null);

            testObject.Sku = "a";
            testObject.AddSkuToBundleCommand.Execute(null);
            
            mock.Mock<IMessageHelper>().Verify(x => x.ShowError("SKU a not found"));
        }
        
        [Fact]
        public void AddProductToBundle_AddsProductWithGivenSkuAndQtyToBundleLineItems()
        {
            ProductVariantEntity productToAddToBundle = new ProductVariantEntity { Product = new ProductEntity(2) };
            
            var sqlAdapter = mock.Mock<ISqlAdapter>();
            mock.Mock<ISqlAdapterFactory>().Setup(x => x.Create()).Returns(sqlAdapter.Object);
            mock.Mock<IProductCatalog>()
                .Setup(x => x.FetchProductVariantEntity(It.IsAny<ISqlAdapter>(), It.IsAny<string>())).Returns(productToAddToBundle);

            ProductVariantEntity baseProduct = new ProductVariantEntity();
            baseProduct.Product = new ProductEntity(1);
            baseProduct.Product.IsBundle = true;
            testObject.Load(baseProduct);

            testObject.Sku = "a";
            testObject.Quantity = 2;
            testObject.AddSkuToBundleCommand.Execute(null);

            Assert.True(testObject.BundleLineItems.Any(x => x.DisplayText == "2x a"));
        }
        
        [Fact]
        public void AddProductCommand_IsAllowed()
        {
            Assert.True(testObject.AddSkuToBundleCommand.CanExecute(null));
        }

        [Fact]
        public void RemoveProductFromBundle_RemovesSelectedProductFromBundleLineItems()
        {
            // Load first
            ProductVariantEntity baseProduct = new ProductVariantEntity();
            baseProduct.Product = new ProductEntity(1);
            baseProduct.Product.IsBundle = false;

            testObject.Load(baseProduct);

            var bundledProduct = new ProductBundleDisplayLineItem(new ProductBundleEntity(), "abc");
            
            testObject.BundleLineItems.Add(bundledProduct);
            testObject.SelectedBundleLineItem = bundledProduct;
            testObject.RemoveSkuFromBundleCommand.Execute(null);
            
            Assert.Empty(testObject.BundleLineItems);
        }
        
        [Fact]
        public void RemoveProductCommand_IsAllowed_WhenSelectedProductIsNotNull()
        {
            testObject.SelectedBundleLineItem =
                new ProductBundleDisplayLineItem(new ProductBundleEntity(), string.Empty);
            Assert.True(testObject.RemoveSkuFromBundleCommand.CanExecute(null));
        }
        
        [Fact]
        public void RemoveProductCommand_IsNotAllowed_WhenSelectedProductIsNull()
        {
            testObject.SelectedBundleLineItem = null;
            Assert.False(testObject.RemoveSkuFromBundleCommand.CanExecute(null));
        }
    }
}