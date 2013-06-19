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
                    RecurrenceHours = 42
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
        public void ScheduleIsNull_IsNow_WhenNoSettings_Test()
        {
            testObject = new ScheduledTrigger();

            Assert.IsNull(testObject.Schedule);
        }

        [TestMethod]
        public void StartDateTimeInUtc_UsesStartDateFromSettings_WhenXmlSettingsContainsStartDate_Test()
        {
            DateTime testTime = DateTime.Parse("6/8/2013 12:07:00 AM");

            const string xmlSettings = "<?xml version=\"1.0\"?>\r\n<OneTimeActionSchedule>\r\n  <StartDateTimeInUtc>2013-06-08T00:07:00</StartDateTimeInUtc>\r\n</OneTimeActionSchedule>";

            testObject = new ScheduledTrigger(xmlSettings);

            Assert.AreEqual(testTime, testObject.Schedule.StartDateTimeInUtc);
        }

        [TestMethod]
        public void StartDateTimeCorrect_IsNow_WhenSerializedAndDeserailized_Test()
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
