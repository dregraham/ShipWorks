using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using Interapptive.Shared.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Actions.Scheduling.ActionSchedules;
using ShipWorks.Actions.Triggers;
using ShipWorks.Actions.Triggers.Editors;
using ShipWorks.Data.Model;
using ShipWorks.Tests.Actions.Scheduling.ActionSchedules;

namespace ShipWorks.Tests.Actions.Triggers
{
    [TestClass]
    public class ScheduledTriggerTest
    {
        private ScheduledTrigger testObject;

        private DateTime testDateTime = DateTime.Now;

        [TestInitialize]
        public void Initialize()
        {
            testObject = new ScheduledTrigger()
            {
                Schedule = new HourlyActionSchedule()
                {
                    StartDateTimeInUtc = testDateTime,
                    FrequencyInHours = 42
                }
            };
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
        public void Schedule_IsOneTimeActionSchedule_WhenNoSettings_Test()
        {
            testObject = new ScheduledTrigger();

            Assert.IsInstanceOfType(testObject.Schedule, typeof(OneTimeActionSchedule));
        }

        [TestMethod]
        public void StartDateTimeInUtc_UsesStartDateFromSettings_WhenXmlSettingsContainsStartDate_Test()
        {
            DateTime testTime = DateTime.Parse("6/8/2013 12:07:00 AM");

            const string xmlSettings = "<Settings><WeeklyActionSchedule><ScheduleType>3</ScheduleType><StartDateTimeInUtc>2013-06-08T00:07:00</StartDateTimeInUtc><RecurrenceWeeks>1</RecurrenceWeeks></WeeklyActionSchedule></Settings>";

            testObject = new ScheduledTrigger(xmlSettings);

            Assert.AreEqual(testTime, testObject.Schedule.StartDateTimeInUtc);
        }

        [TestMethod]
        public void StartDateTimeInUtc_IsCorrect_WhenSerializedAndDeserailized_Test()
        {

            MemoryStream stream = new MemoryStream();
            XmlTextWriter xmlWriter = new XmlTextWriter(stream, Encoding.ASCII);

            testObject.SerializeXml(xmlWriter);

            xmlWriter.Flush();
            stream.Position = 0;

            StreamReader streamReader = new StreamReader(stream);
            string xml = streamReader.ReadToEnd();

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);

            ScheduledTrigger deserializedScheduledTrigger = new ScheduledTrigger(xml);

            Assert.AreEqual(testObject.Schedule.StartDateTimeInUtc, deserializedScheduledTrigger.Schedule.StartDateTimeInUtc);

        }
    }
}
