using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using Interapptive.Shared.Utility;
using Xunit;
using ShipWorks.Actions.Scheduling.ActionSchedules;
using ShipWorks.Actions.Triggers;
using ShipWorks.Actions.Triggers.Editors;
using ShipWorks.Data.Model;
using ShipWorks.Tests.Actions.Scheduling.ActionSchedules;

namespace ShipWorks.Tests.Actions.Triggers
{
    public class ScheduledTriggerTest
    {
        private ScheduledTrigger testObject;

        private DateTime testDateTime = DateTime.Now;

        public ScheduledTriggerTest()
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

        [Fact]
        public void TriggerType_ReturnsScheduled_Test()
        {
            Assert.Equal(ActionTriggerType.Scheduled, testObject.TriggerType);
        }

        [Fact]
        public void CreateEditor_ReturnsScheduledTriggerEditor_Test()
        {
            Assert.IsAssignableFrom<ScheduledTriggerEditor>(testObject.CreateEditor());
        }

        [Fact]
        public void TriggeringEntityType_IsNull_Test()
        {
            Assert.Null(testObject.TriggeringEntityType);
        }

        [Fact]
        public void Schedule_IsOneTimeActionSchedule_WhenNoSettings_Test()
        {
            testObject = new ScheduledTrigger();

            Assert.IsAssignableFrom<OneTimeActionSchedule>(testObject.Schedule);
        }

        [Fact]
        public void StartDateTimeInUtc_UsesStartDateFromSettings_WhenXmlSettingsContainsStartDate_Test()
        {
            DateTime testTime = DateTime.Parse("6/8/2013 12:07:00 AM");

            const string xmlSettings = "<Settings><WeeklyActionSchedule><ScheduleType>3</ScheduleType><StartDateTimeInUtc>2013-06-08T00:07:00</StartDateTimeInUtc><RecurrenceWeeks>1</RecurrenceWeeks></WeeklyActionSchedule></Settings>";

            testObject = new ScheduledTrigger(xmlSettings);

            Assert.Equal(testTime, testObject.Schedule.StartDateTimeInUtc);
        }

        [Fact]
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

            Assert.Equal(testObject.Schedule.StartDateTimeInUtc, deserializedScheduledTrigger.Schedule.StartDateTimeInUtc);
        }

        [Fact]
        public void DeserializeXml_ReturnsDailyActionSchedule_WhenScheduleTypeIsDaily_Test()
        {
            const string xmlSettings = 
                @"<Settings>
                  <DailyActionSchedule>
                    <ScheduleType>2</ScheduleType>
                    <StartDateTimeInUtc>2013-06-20T17:00:00.567402Z</StartDateTimeInUtc>
                    <FrequencyInDays>30</FrequencyInDays>
                  </DailyActionSchedule>
                </Settings>";

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlSettings);

            XPathNavigator xpath = xmlDocument.CreateNavigator();
            xpath.MoveToFirstChild();

            testObject.DeserializeXml(xpath);

            Assert.IsAssignableFrom<DailyActionSchedule>(testObject.Schedule);
        }

        [Fact]
        public void DeserializeXml_FrequencyInDaysEqualsXmlValue_WhenScheduleTypeIsDaily_Test()
        {
            const string xmlSettings =
                @"<Settings>
                  <DailyActionSchedule>
                    <ScheduleType>2</ScheduleType>
                    <StartDateTimeInUtc>2013-06-20T17:00:00.567402Z</StartDateTimeInUtc>
                    <FrequencyInDays>30</FrequencyInDays>
                  </DailyActionSchedule>
                </Settings>";

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlSettings);

            XPathNavigator xpath = xmlDocument.CreateNavigator();
            xpath.MoveToFirstChild();

            testObject.DeserializeXml(xpath);

            Assert.Equal(30, ((DailyActionSchedule)testObject.Schedule).FrequencyInDays);
        }
    }
}
