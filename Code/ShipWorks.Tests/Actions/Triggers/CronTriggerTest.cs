using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Actions.Triggers;
using ShipWorks.Actions.Triggers.Editors;
using ShipWorks.Data.Model;

namespace ShipWorks.Tests.Actions.Triggers
{
    [TestClass]
    public class CronTriggerTest
    {
        private CronTrigger testObject;

        [TestInitialize]
        public void Initialize()
        {
            testObject = new CronTrigger();
        }

        [TestMethod]
        public void TriggerType_ReturnsCron_Test()
        {
            Assert.AreEqual(ActionTriggerType.Cron, testObject.TriggerType);
        }

        [TestMethod]
        public void CreateEditor_ReturnsCronTriggerEditor_Test()
        {
            Assert.IsInstanceOfType(testObject.CreateEditor(), typeof(CronTriggerEditor));
        }

        [TestMethod]
        public void TriggeringEntityType_IsNull_Test()
        {
            Assert.IsNull(testObject.TriggeringEntityType);
        }
    }
}
