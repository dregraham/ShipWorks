using Interapptive.Shared.Utility;
using log4net;
using Xunit;
using Moq;
using ShipWorks.Actions;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Triggers;
using ShipWorks.Data.Model.EntityClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using ShipWorks.Actions.Tasks;

namespace ShipWorks.Tests.Actions.Tasks.Common
{
    public class DatabaseMaintenanceTaskTest
    {
        private DatabaseMaintenanceTask testObject;

        private Mock<ILog> log;
        private Mock<IDateTimeProvider> dateTimeProvider;
        private ActionStepContext actionStepContext;
        private ActionTaskEntity actionTaskEntity;
        private readonly DateTime DefaultStartDateTimeInUtc = new DateTime(2014, 7, 13, 2, 0, 0, DateTimeKind.Utc);

        public DatabaseMaintenanceTaskTest()
        {
            log = new Mock<ILog>();
            log.Setup(l => l.InfoFormat(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<object>()));
            log.Setup(l => l.InfoFormat(It.IsAny<string>()));

            dateTimeProvider = new Mock<IDateTimeProvider>();

            string taskSettingTemplate = @"<Settings>
                                  <DailyActionSchedule>
                                    <ScheduleType>2</ScheduleType>
                                    <StartDateTimeInUtc>{0}</StartDateTimeInUtc>
                                    <FrequencyInDays>1</FrequencyInDays>
                                  </DailyActionSchedule>
                                  <TimeoutInMinutes value=""120"" />
                                </Settings>";

            actionTaskEntity = new ActionTaskEntity()
            {
                Action = new ActionEntity(),
                TaskSettings = string.Format(taskSettingTemplate, XmlConvert.ToString(DefaultStartDateTimeInUtc, XmlDateTimeSerializationMode.Utc)),
                TaskIdentifier = "DatabaseMaintenance"
            };

            testObject = new DatabaseMaintenanceTask(dateTimeProvider.Object, t => LogManager.GetLogger(typeof(DatabaseMaintenanceTask)) );

            ActionQueueEntity queueEntity = new ActionQueueEntity();
            queueEntity.ActionVersion = GetBytes("167C");

            ActionQueueStepEntity stepEntity = new ActionQueueStepEntity();
            stepEntity.TaskSettings = string.Format(taskSettingTemplate, XmlConvert.ToString(DefaultStartDateTimeInUtc, XmlDateTimeSerializationMode.Utc));

            actionStepContext = new ActionStepContext(queueEntity, stepEntity, null);

        }

        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        [Fact]
        public void InputRequirement_ReturnsNone()
        {
            Assert.Equal(ActionTaskInputRequirement.None, testObject.InputRequirement);
        }

        [Fact]
        public void TimeoutInMinutes_IsDefaultedTo180()
        {
            testObject = new DatabaseMaintenanceTask(dateTimeProvider.Object, t => LogManager.GetLogger(typeof(DatabaseMaintenanceTask)));

            Assert.Equal(180, testObject.TimeoutInMinutes);
        }

        [Fact]
        public void IsAllowedForTrigger_ReturnsTrue_WhenTriggerTypeIsScheduled()
        {
            bool isAllowed = testObject.IsAllowedForTrigger(ActionTriggerType.Scheduled);

            Assert.True(isAllowed);
        }

        [Fact]
        public void IsAllowedForTrigger_ReturnsFalse_WhenTriggerTypeIsDownloadFinished()
        {
            bool isAllowed = testObject.IsAllowedForTrigger(ActionTriggerType.DownloadFinished);

            Assert.False(isAllowed);
        }

        [Fact]
        public void IsAllowedForTrigger_ReturnsFalse_WhenTriggerTypeIsFilterContentChanged()
        {
            bool isAllowed = testObject.IsAllowedForTrigger(ActionTriggerType.FilterContentChanged);

            Assert.False(isAllowed);
        }

        [Fact]
        public void IsAllowedForTrigger_ReturnsFalse_WhenTriggerTypeIsOrderDownloaded()
        {
            bool isAllowed = testObject.IsAllowedForTrigger(ActionTriggerType.OrderDownloaded);

            Assert.False(isAllowed);
        }

        [Fact]
        public void IsAllowedForTrigger_ReturnsFalse_WhenTriggerTypeIsShipmentProcessed()
        {
            bool isAllowed = testObject.IsAllowedForTrigger(ActionTriggerType.ShipmentProcessed);

            Assert.False(isAllowed);
        }

        [Fact]
        public void IsAllowedForTrigger_ReturnsFalse_WhenTriggerTypeIsShipmentVoided()
        {
            bool isAllowed = testObject.IsAllowedForTrigger(ActionTriggerType.ShipmentVoided);

            Assert.False(isAllowed);
        }

        [Fact]
        public void IsAllowedForTrigger_ReturnsFalse_WhenTriggerTypeIsUserInitiated()
        {
            bool isAllowed = testObject.IsAllowedForTrigger(ActionTriggerType.UserInitiated);

            Assert.False(isAllowed);
        }

        [Fact]
        public void CreateEditor_ReturnsActionTaskEditor()
        {
            var editor = testObject.CreateEditor();
            Assert.Equal(typeof(ActionTaskEditor), editor.GetType());
        }
    }
}
