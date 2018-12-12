using System;
using Autofac.Extras.Moq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Products.Import;
using ShipWorks.Tests.Shared;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Products.Tests
{
    public class ProductsModeTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly ProductsMode testObject;

        public ProductsModeTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<ProductsMode>();
        }

        [Fact]
        public void ImportProductsCommand_DelegatesToProductImporterViewModel()
        {
            testObject.ImportProducts.Execute(null);

            mock.Mock<IProductImporterViewModel>().Verify(x => x.ImportProducts());
        }

        [Fact]
        public void ImportProductsCommand_DoesNotReloadProducts_WhenImportIsCanceled()
        {
            mock.Mock<IProductImporterViewModel>().Setup(x => x.ImportProducts()).Returns(Result.FromError("Foo"));
            mock.Mock<IProductsCollectionFactory>().ResetCalls();

            testObject.ImportProducts.Execute(null);

            mock.Mock<IProductsCollectionFactory>().Verify(x => x.Create(AnyBool, AnyString, It.IsAny<IBasicSortDefinition>()), Times.Never);
        }

        [Fact]
        public void ImportProductsCommand_ReloadProducts_WhenImportIsCompleted()
        {
            mock.Mock<IProductImporterViewModel>().Setup(x => x.ImportProducts()).Returns(Result.FromSuccess);
            mock.Mock<IProductsCollectionFactory>().ResetCalls();

            testObject.ImportProducts.Execute(null);

            mock.Mock<IProductsCollectionFactory>().Verify(x => x.Create(AnyBool, AnyString, It.IsAny<IBasicSortDefinition>()));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
