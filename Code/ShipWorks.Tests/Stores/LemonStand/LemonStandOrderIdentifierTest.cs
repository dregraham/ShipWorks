using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.LemonStand;

namespace ShipWorks.Tests.Stores.LemonStand
{
    [TestClass]
    public class LemonStandOrderIdentifierTest
    {
        Mock<OrderEntity> order = new Mock<OrderEntity>();
        private LemonStandOrderIdentifier testObject;

        [TestMethod]
        public void ToString_ReturnsCorrectString_WhenCalled_Test()
        {
            testObject = new LemonStandOrderIdentifier("1");

            Assert.AreEqual("LemonStandStoreOrderID:1", testObject.ToString());
        }

        [TestMethod]
        public void ToString_ReturnsWithoutException_WhenLemonStandOrderIDIsNull_Test1()
        {
            testObject = new LemonStandOrderIdentifier(null);
            testObject.ToString();
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public void ApplyTo_ThrowsInvalidOperationException_WhenPassedNullOrderEntity_Test()
        {
            testObject = new LemonStandOrderIdentifier("1");
            testObject.ApplyTo((OrderEntity) null);
        }

        [ExpectedException(typeof(NullReferenceException))]
        [TestMethod]
        public void ApplyTo_ThrowsInvalidOperationException_WhenPassedNullDownloadDetailEntity_Test()
        {
            testObject = new LemonStandOrderIdentifier("1");
            testObject.ApplyTo((DownloadDetailEntity)null);
        }
    }
}
