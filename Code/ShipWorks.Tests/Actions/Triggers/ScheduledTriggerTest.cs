using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Actions.Triggers;
using ShipWorks.Actions.Triggers.Editors;
using ShipWorks.Data.Model;

namespace ShipWorks.Tests.Actions.Triggers
{
    [TestClass]
    public class ScheduledTriggerTest
    {
        private ScheduledTrigger testObject;

        [TestInitialize]
        public void Initialize()
        {
            testObject = new ScheduledTrigger();
        }

        [TestMethod]
        public void TriggerType_ReturnsScheduled_Test()
        {
            Assert.AreEqual(ActionTriggerType.Scheduled, testObject.TriggerType);
        }

        [TestMethod]
        public void CreateEditor_ReturnsScheduledTriggerEditor_Test()
        {
            Assert.IsInstanceOfType(testObject.CreateEditor(), typeof(ScheduledTriggerEditor));
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

            testObject = new ScheduledTrigger(null);
            
            // A little fuzzy logic to try to make sure the start date was 
            // initialized in the constructor
            Assert.IsTrue(testObject.StartDateTimeInUtc >= now);
        }

        [TestMethod]
        public void StartDateTimeInUtc_IsNow_WhenXmlSettingsIsEmptyString_Test()
        {
            DateTime now = DateTime.UtcNow;

            testObject = new ScheduledTrigger(string.Empty);

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

            testObject = new ScheduledTrigger(xmlSettings);

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

            testObject = new ScheduledTrigger(xmlSettings);

            Assert.AreEqual(DateTime.Parse("6/8/2013 12:07:00 AM"), testObject.StartDateTimeInUtc);
        }
    }
}
