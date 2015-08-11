using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Quartz;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Tasks.Common.Enums;

namespace ShipWorks.Tests.Actions.Tasks.Common
{
    public class EmailTaskTest
    {
        [Fact]
        public void Initialize_DeserializesXmlCorrectly_Test()
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
