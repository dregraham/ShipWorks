using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Amazon;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Amazon
{
    public class AmazonCreateOrderSourceViewModelTest
    {
        readonly AutoMock mock;
        private readonly AmazonOrderEntity order;

        public AmazonCreateOrderSourceViewModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            order = new AmazonOrderEntity();
        }

        [Fact]
        public void Load_Empty()
        {
            var unitUnderTest = mock.Create<AmazonCreateOrderSourceViewModel>();

            var store = new AmazonStoreEntity();

            unitUnderTest.Load(store);

            Assert.Equal(string.Empty, unitUnderTest.EncodedOrderSource);
        }

        [Fact]
        public void Load_Valid()
        {
            var unitUnderTest = mock.Create<AmazonCreateOrderSourceViewModel>();

            var store = new AmazonStoreEntity
            {
                MarketplaceID = "ATVPDKIKX0DER",
                MerchantID = "26824e643458",
                OrderSourceID = "56d1cd01-b430-4ebc-8c8e-aeb39888e1cc"
            };

            unitUnderTest.Load(store);

            Assert.Equal("MjY4MjRlNjQzNDU4X0FUVlBES0lLWDBERVJfNTZkMWNkMDEtYjQzMC00ZWJjLThjOGUtYWViMzk4ODhlMWNj", unitUnderTest.EncodedOrderSource);
        }

        [Fact]
        public void Save_Valid()
        {
            var unitUnderTest = mock.Create<AmazonCreateOrderSourceViewModel>();
            unitUnderTest.EncodedOrderSource =
                "MjY4MjRlNjQzNDU4X0FUVlBES0lLWDBERVJfNTZkMWNkMDEtYjQzMC00ZWJjLThjOGUtYWViMzk4ODhlMWNj";

            var store = new AmazonStoreEntity();

            unitUnderTest.Save(store);

            Assert.Equal("ATVPDKIKX0DER", store.MarketplaceID);
            Assert.Equal("26824e643458", store.MerchantID);
            Assert.Equal("56d1cd01-b430-4ebc-8c8e-aeb39888e1cc", store.OrderSourceID);
        }

        [Fact]
        public void Save_Invalid()
        {
            var unitUnderTest = mock.Create<AmazonCreateOrderSourceViewModel>();
            unitUnderTest.EncodedOrderSource = "adfuheiofuabiuea37848shfiuwhe38";

            var store = new AmazonStoreEntity();

            unitUnderTest.Save(store);

            Assert.Equal(null, store.MarketplaceID);
            Assert.Equal(null, store.MerchantID);
            Assert.Equal(null, store.OrderSourceID);
        }
    }
}
