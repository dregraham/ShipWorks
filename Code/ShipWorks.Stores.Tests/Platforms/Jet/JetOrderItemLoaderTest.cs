using System;
using System.Collections.Generic;
using ShipWorks.Tests.Shared;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Import;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Jet;
using ShipWorks.Stores.Platforms.Jet.DTO;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Jet
{
    public class JetOrderItemLoaderTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly JetProduct productFromRepo;
        private readonly JetOrderEntity jetOrderEntity;
        private readonly List<JetOrderItemEntity> createdOrderItems = new List<JetOrderItemEntity>();
        private readonly JetOrderItemLoader testObject;
        private readonly JetStoreEntity store;

        public JetOrderItemLoaderTest()
        {
            store = new JetStoreEntity();

            productFromRepo = new JetProduct
            {
                StandardProductCodes = new List<JetProductCode>()
            };

            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            mock.Mock<IOrderElementFactory>()
                .Setup(f => f.CreateItem(It.IsAny<OrderEntity>()))
                .Returns(() =>
                {
                    var newItem = new JetOrderItemEntity {UPC = string.Empty, ISBN = string.Empty};
                    createdOrderItems.Add(newItem);
                    return newItem;
                });

            mock.Mock<IJetProductRepository>()
                .Setup(r => r.GetProduct(It.IsAny<JetOrderItem>(), It.IsAny<JetStoreEntity>()))
                .Returns(productFromRepo);

            jetOrderEntity = new JetOrderEntity();

            testObject = mock.Create<JetOrderItemLoader>();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void LoadItems_CorrectNumberOfItemsCreated(int numberOfItemsReturned)
        {
            var jetOrderDetailsResult = CreateJetOrderDetailsResult(numberOfItemsReturned);

            testObject.LoadItems(jetOrderEntity, jetOrderDetailsResult, store);

            Assert.Equal(numberOfItemsReturned, createdOrderItems.Count);
        }

        [Fact]
        public void LoadItems_SetsName()
        {
            SetupOrderItemTest(item => item.ProductTitle = "title");
            
            Assert.Equal("title", createdOrderItems[0].Name);
        }

        [Fact]
        public void LoadItems_SetsCode()
        {
            SetupOrderItemTest(item => item.ItemTaxCode = "code");

            Assert.Equal("code", createdOrderItems[0].Code);
        }

        [Fact]
        public void LoadItems_SetsSku()
        {
            SetupOrderItemTest(item => item.MerchantSku = "sku");

            Assert.Equal("sku", createdOrderItems[0].SKU);
        }

        [Fact]
        public void LoadItems_SetsJetOrderItemId()
        {
            SetupOrderItemTest(item => item.OrderItemId = "jet id");

            Assert.Equal("jet id", createdOrderItems[0].JetOrderItemID);
        }

        [Fact]
        public void LoadItems_SetsMerchantSku()
        {
            SetupOrderItemTest(item => item.MerchantSku = "sku");

            Assert.Equal("sku", createdOrderItems[0].MerchantSku);
        }

        [Fact]
        public void LoadItems_SetsUnitPrice()
        {
            SetupOrderItemTest(item => item.ItemPrice.BasePrice = 42.42m);

            Assert.Equal(42.42m, createdOrderItems[0].UnitPrice);
        }

        [Fact]
        public void LoadItems_SetsQuantity()
        {
            SetupOrderItemTest(item => item.RequestOrderQuantity = 42.42);

            Assert.Equal(42.42, createdOrderItems[0].Quantity);
        }

        [Fact]
        public void LoadItems_SetsLocation()
        {
            var jetOrderDetailsResult = CreateJetOrderDetailsResult(1);
            jetOrderDetailsResult.FulfillmentNode = "fnode";

            testObject.LoadItems(jetOrderEntity, jetOrderDetailsResult, store);

            Assert.Equal("fnode", createdOrderItems[0].Location);
        }

        [Fact]
        public void LoadItems_SetsDescription()
        {
            var jetOrderDetailsResult = CreateJetOrderDetailsResult(1);
            productFromRepo.ProductDescription = "desc";

            testObject.LoadItems(jetOrderEntity, jetOrderDetailsResult, store);

            Assert.Equal("desc", createdOrderItems[0].Description);
        }

        [Theory]
        [InlineData("UPC", "the upc", "the upc", "")]
        [InlineData("ISBN-10", "the isbn", "", "the isbn")]
        [InlineData("ISBN-13", "the isbn", "", "the isbn")]
        [InlineData("GTIN", "the gtin", "", "")]
        public void LoadItems_SetUPC_AndISBN(string productCodeType, string productCode, string expectedUpc, string expectedIsbn)
        {
            var jetOrderDetailsResult = CreateJetOrderDetailsResult(1);
            productFromRepo.StandardProductCodes = new[]
            {
                new JetProductCode()
                {
                    StandardProductCodeType = productCodeType,
                    StandardProductCode = productCode
                }
            };

            testObject.LoadItems(jetOrderEntity, jetOrderDetailsResult, store);

            Assert.Equal(expectedUpc, createdOrderItems[0].UPC);
            Assert.Equal(expectedIsbn, createdOrderItems[0].ISBN);
        }

        [Fact]
        public void LoadItems_UpcAndIsbnAreBlank_WhenStandardProductCodesIsNull()
        {
            var jetOrderDetailsResult = CreateJetOrderDetailsResult(1);
            productFromRepo.StandardProductCodes = null;

            testObject.LoadItems(jetOrderEntity, jetOrderDetailsResult, store);

            Assert.Empty(createdOrderItems[0].UPC);
            Assert.Empty(createdOrderItems[0].ISBN);
        }

        [Fact]
        public void LoadItems_SetsImage()
        {
            var jetOrderDetailsResult = CreateJetOrderDetailsResult(1);
            productFromRepo.MainImageUrl = "main image";

            testObject.LoadItems(jetOrderEntity, jetOrderDetailsResult, store);

            Assert.Equal("main image", createdOrderItems[0].Image);
        }

        [Fact]
        public void LoadItems_SetsThumbnail()
        {
            var jetOrderDetailsResult = CreateJetOrderDetailsResult(1);
            productFromRepo.SwatchImageUrl = "swatch image";

            testObject.LoadItems(jetOrderEntity, jetOrderDetailsResult, store);

            Assert.Equal("swatch image", createdOrderItems[0].Thumbnail);
        }

        [Fact]
        public void LoadItems_SetsWeight()
        {
            var jetOrderDetailsResult = CreateJetOrderDetailsResult(1);
            productFromRepo.ShippingWeightPounds = 42.42;

            testObject.LoadItems(jetOrderEntity, jetOrderDetailsResult, store);

            Assert.Equal(42.42, createdOrderItems[0].Weight);
        }

        private void SetupOrderItemTest(Action<JetOrderItem> action)
        {
            var jetOrderDetailsResult = CreateJetOrderDetailsResult(1);
            action(jetOrderDetailsResult.OrderItems[0]);

            testObject.LoadItems(jetOrderEntity, jetOrderDetailsResult, store);
        }


        private static JetOrderDetailsResult CreateJetOrderDetailsResult(int numberOfItems)
        {
            var jetOrderDetailsResult = new JetOrderDetailsResult()
            {
                OrderItems = new List<JetOrderItem>()
            };

            for (int i = 0; i < numberOfItems; i++)
            {
                jetOrderDetailsResult.OrderItems.Add(new JetOrderItem()
                {
                    ItemPrice = new JetOrderItemPrice()
                });
            }

            return jetOrderDetailsResult;
        }

        public void Dispose()
        {
            mock.Dispose();
        }

    }
}