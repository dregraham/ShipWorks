using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Ebay;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Content
{
    public class CombineOrderAuditTest
    {
        private readonly AutoMock mock;

        public CombineOrderAuditTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Theory]
        [InlineData("anumber", "anumber")]
        [InlineData("a:number", "number")]
        [InlineData("a:number:3", "number:3")]
        [InlineData(":number:3", "number:3")]
        [InlineData("", "")]
        [InlineData(null, "")]
        public void ParseOrderIdentifier_IsCorrect(string testValue, string expectedValue)
        {
            Mock<OrderIdentifier> orderIdentifier = mock.Mock<OrderIdentifier>();
            Mock<StoreType> storeType = mock.Mock<StoreType>();
            Mock<IStoreTypeManager> storeTypeMgr = mock.Mock<IStoreTypeManager>();

            orderIdentifier.Setup(oi => oi.ToString()).Returns(testValue);
            storeType.Setup(st => st.CreateOrderIdentifier(It.IsAny<OrderEntity>())).Returns(orderIdentifier);
            storeTypeMgr.Setup(s => s.GetType(It.IsAny<long>())).Returns(storeType);

            OrderEntity order = storeType.Object.CreateOrder();
            order.OrderNumber = 1;

            CombineOrderAudit testObj = mock.Create<CombineOrderAudit>();
            string value = testObj.ParseOrderIdentifier(storeType.Object, order);

            Assert.Equal(expectedValue, value);
        }
    }
}
