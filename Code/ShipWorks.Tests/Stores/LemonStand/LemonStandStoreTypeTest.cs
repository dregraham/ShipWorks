using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.LemonStand;

namespace ShipWorks.Tests.Stores.LemonStand
{
    [TestClass]
    public class LemonStandStoreTypeTest
    {
        Mock<StoreEntity> storeMock = new Mock<StoreEntity>();
        LemonStandStoreType store;

        [TestInitialize]
        public void Initialize()
        {
            store = new LemonStandStoreType(storeMock.Object);
        }

        [TestMethod]
        public void TestMethod1()
        {
            string result = store.TypeCode.ToString();
            Assert.Equals("LEM", result);
        }
    }
}
