using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Import;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.ThreeDCart;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.ThreeDCart
{
    public class ThreeDCartRestItemAttributeLoaderTest
    {
        private readonly AutoMock mock;
        private readonly Mock<IOrderElementFactory> orderElementFactory;

        public ThreeDCartRestItemAttributeLoaderTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            orderElementFactory = mock.Mock<IOrderElementFactory>();
        }

        public void LoadItemNameAndAttributes_LoadsItemName()
        {

            var item = new ThreeDCartOrderItemEntity();
            var itemDescription = "Cool hat<br><b>Size:</b>&nbsp;Large - $1.00";
            var testObject = mock.Create<ThreeDCartRestItemAttributeLoader>();

            testObject.LoadItemNameAndAttributes(item, itemDescription);

            Assert.Equal("Cool hat", item.Name);
        }

        [Fact]
        public void LoadItemAttributes_CreatesAttributeWithCorrectValues_WhenAttributeTypeIsSingleValue()
        {
            var item = new ThreeDCartOrderItemEntity();
            var itemDescription = "Cool hat<br><b>Size:</b>&nbsp;Large - $1.00";
            var testObject = mock.Create<ThreeDCartRestItemAttributeLoader>();

            testObject.LoadItemNameAndAttributes(item, itemDescription);

            orderElementFactory.Verify(f => f.CreateItemAttribute(item, "Size", "Large", 1.00m, false), Times.Once);
        }

        [Fact]
        public void LoadItemAttributes_CreatesAttributesWithCorrectValues_WhenAttributeTypeIsMultiValue()
        {
            var item = new ThreeDCartOrderItemEntity();
            var itemDescription = "Cool hat<br><b>Pins:</b>&nbsp;Cool Pin $2.00<br>Sweet Pin $3.00<br>Dude Pin $5.00";
            var testObject = mock.Create<ThreeDCartRestItemAttributeLoader>();

            testObject.LoadItemNameAndAttributes(item, itemDescription);

            orderElementFactory.Verify(f => f.CreateItemAttribute(item, "Pins", "Cool Pin", 2.00m, false), Times.Once);
            orderElementFactory.Verify(f => f.CreateItemAttribute(item, "Pins", "Sweet Pin", 3.00m, false), Times.Once);
            orderElementFactory.Verify(f => f.CreateItemAttribute(item, "Pins", "Dude Pin", 5.00m, false), Times.Once);
        }

        [Fact]
        public void LoadItemAttributes_CreatesCorrectNumberOfAttributes_WhenAttributeTypeIsMultiValue()
        {
            var item = new ThreeDCartOrderItemEntity();
            var itemDescription = "Cool hat<br><b>Pins:</b>&nbsp;Cool Pin $2.00<br>Sweet Pin $3.00<br>Dude Pin $5.00";
            var testObject = mock.Create<ThreeDCartRestItemAttributeLoader>();

            testObject.LoadItemNameAndAttributes(item, itemDescription);

            orderElementFactory.Verify(f => f.CreateItemAttribute(item, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>(), false), Times.Exactly(3));
        }

        [Fact]
        public void LoadItemAttributes_CreatesAttributesWithCorrectValues_WhenAttributeTypeIsFile()
        {
            var item = new ThreeDCartOrderItemEntity();
            var itemDescription = "Cool hat<br><b>Custom Image:</b>&nbsp;<a href=\"/assets/order_images/159(2).jpg\" target=_new>File</a> - $2.00";
            var testObject = mock.Create<ThreeDCartRestItemAttributeLoader>();

            testObject.LoadItemNameAndAttributes(item, itemDescription);

            orderElementFactory.Verify(f => f.CreateItemAttribute(item, "Custom Image", "<a href=\"/assets/order_images/159(2).jpg\" target=_new>File</a>", 2.00m, false), Times.Once);
        }

        [Fact]
        public void LoadItemAttributes_CreatesAttributeWithCorrectValues_WhenAttributeHasNoPrice()
        {
            var item = new ThreeDCartOrderItemEntity();
            var itemDescription = "Cool hat<br><b>Color:</b>&nbsp;Blue";
            var testObject = mock.Create<ThreeDCartRestItemAttributeLoader>();

            testObject.LoadItemNameAndAttributes(item, itemDescription);

            orderElementFactory.Verify(f => f.CreateItemAttribute(item, "Color", "Blue", 0.00m, false), Times.Once);
        }

        [Fact]
        public void LoadItemAttributes_LoadsCorrectNumberOfAttributes_WhenItemDescriptionContainsMultipleMixedTypeAttributes()
        {
            var item = new ThreeDCartOrderItemEntity();
            var itemDescription = "Cool hat<br><b>" +
                                  "Size:</b>&nbsp;Large - $1.00<br><b>" +
                                  "Cool Image:</b>&nbsp;Eagle - $8.00<br><b>" +
                                  "Pins:</b>&nbsp;Cool Pin $2.00<br>Sweet Pin $3.00<br>Dude Pin $5.00<br><b>" +
                                  "Custom Image:</b>&nbsp;<a href=\"/assets/order_images/159(2).jpg\" target=_new>File</a> - $2.00<br><b>" +
                                  "Color:</b>&nbsp;Blue";

            var testObject = mock.Create<ThreeDCartRestItemAttributeLoader>();

            testObject.LoadItemNameAndAttributes(item, itemDescription);

            orderElementFactory.Verify(f => f.CreateItemAttribute(item, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>(), false), Times.Exactly(7));
        }
    }
}