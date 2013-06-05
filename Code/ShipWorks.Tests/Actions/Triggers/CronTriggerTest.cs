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

        [TestMethod]
        public void StartDateTimeInUtc_IsNow_WhenXmlSettingsIsNull_Test()
        {
            DateTime now = DateTime.UtcNow;

            testObject = new CronTrigger(null);
            
            // A little fuzzy logic to try to make sure the start date was 
            // initialized in the constructor
            Assert.IsTrue(testObject.StartDateTimeInUtc >= now);
        }

        [TestMethod]
        public void StartDateTimeInUtc_IsNow_WhenXmlSettingsIsEmptyString_Test()
        {
            DateTime now = DateTime.UtcNow;

            testObject = new CronTrigger(string.Empty);

            // A little fuzzy logic to try to make sure the start date was 
            // initialized in the constructor
            Assert.IsTrue(testObject.StartDateTimeInUtc >= now);
        }

        [TestMethod]
        public void StartDateTimeInUtc_IsNow_WhenXmlSettingsDoesNotContainStartDate_Test()
        {
            DateTime now = DateTime.UtcNow;

            const string xmlSettings = @"
                <Settings>
                  <SomeDateValue value=""6/8/2013 12:07:00 AM"" />
                </Settings>";

            testObject = new CronTrigger(xmlSettings);

            // A little fuzzy logic to try to make sure the start date was 
            // initialized in the constructor
            Assert.IsTrue(testObject.StartDateTimeInUtc >= now);
        }

        [TestMethod]
        public void StartDateTimeInUtc_UsesStartDateFromSettings_WhenXmlSettingsContainsStartDate_Test()
        {
            const string xmlSettings = @"
                <Settings>
                  <StartDateTimeInUtc value=""6/8/2013 12:07:00 AM"" />
                </Settings>";

            testObject = new CronTrigger(xmlSettings);

            Assert.AreEqual(DateTime.Parse("6/8/2013 12:07:00 AM"), testObject.StartDateTimeInUtc);
        }
    }
}
