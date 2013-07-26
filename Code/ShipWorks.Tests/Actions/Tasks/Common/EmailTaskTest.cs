using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quartz;
using ShipWorks.Actions.Tasks.Common;

namespace ShipWorks.Tests.Actions.Tasks.Common
{
    [TestClass]
    public class EmailTaskTest
    {
        [TestMethod]
        public void DeserializeXml_ShouldDeserializeCorrectly()
        {
            // Create a new purge database task to serialize
            EmailTask initialObject = new EmailTask();
            initialObject.DelayDelivery = true;
            initialObject.DelayQuantity = 6;
            initialObject.DelayTimeOfDay = new TimeSpan(123456);
            initialObject.DelayType = EmailDelayType.TimeHours;

            string serializedObject = initialObject.SerializeSettings();

            // Create a test purge database task and deserialize its settings
            EmailTask testObject = new EmailTask();
            testObject.Initialize(serializedObject);

            Assert.AreEqual(true, testObject.DelayDelivery);
            Assert.AreEqual(6, testObject.DelayQuantity);
            Assert.AreEqual(new TimeSpan(123456), testObject.DelayTimeOfDay);
            Assert.AreEqual(EmailDelayType.TimeHours, initialObject.DelayType);
        }
    }
}
