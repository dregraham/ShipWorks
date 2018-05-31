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
using ShipWorks.Stores.Orders.Archive;
using ShipWorks.Tests.Shared.ExtensionMethods;

namespace ShipWorks.Tests.Actions.Tasks.Common
{
    public class AutoArchiveTaskTest
    {
        private AutoArchiveTask testObject;
        private Mock<IOrderArchiver> orderArchiver;
        private Mock<ILog> log;
        private Mock<IDateTimeProvider> dateTimeProvider;
        private ActionStepContext actionStepContext;
        private ActionTaskEntity actionTaskEntity;
        private readonly DateTime DefaultStartDateTimeInUtc = new DateTime(2014, 7, 13, 0, 0, 0, DateTimeKind.Utc);

        public AutoArchiveTaskTest()
        {
            log = new Mock<ILog>();
            log.Setup(l => l.InfoFormat(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<object>()));
            log.Setup(l => l.InfoFormat(It.IsAny<string>()));

            dateTimeProvider = new Mock<IDateTimeProvider>();
            orderArchiver = new Mock<IOrderArchiver>();

            string taskSettings = @"<Settings>
                                              <NumberOfDaysToKeep value=""90"" />
                                              <ExecuteOnDayOfWeek value=""0"" />
                                              <TimeoutInMinutes value=""120"" />
                                           </Settings>";

            actionTaskEntity = new ActionTaskEntity()
            {
                Action = new ActionEntity(),
                TaskSettings = taskSettings,
                TaskIdentifier = "AutoArchiveTask"
            };

            testObject = new AutoArchiveTask(dateTimeProvider.Object);

            ActionQueueEntity queueEntity = new ActionQueueEntity();
            queueEntity.ActionVersion = GetBytes("167C");

            ActionQueueStepEntity stepEntity = new ActionQueueStepEntity();
            stepEntity.TaskSettings = taskSettings;

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
        public void TimeoutInMinutes_IsDefaultedTo120()
        {
            testObject = new AutoArchiveTask(dateTimeProvider.Object);

            Assert.Equal(120, testObject.TimeoutInMinutes);
        }

        [Fact]
        public void NumberOfDaysToKeep_IsDefaultedTo90()
        {
            testObject = new AutoArchiveTask(dateTimeProvider.Object);

            Assert.Equal(90, testObject.NumberOfDaysToKeep);
        }

        [Fact]
        public void ExecuteOnDayOfWeek_IsDefaultedToSunday()
        {
            testObject = new AutoArchiveTask(dateTimeProvider.Object);

            Assert.Equal(DayOfWeek.Sunday, testObject.ExecuteOnDayOfWeek);
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
        public void IsAsync_ReturnsTrue()
        {
            Assert.True(testObject.IsAsync);
        }

        [Fact]
        public void RunAsync_DoesNotArchive_WhenScheduledEndTimeHasLapsed_ByOneHour()
        {
            DateTime elapsedByOneHourDate = DefaultStartDateTimeInUtc.AddHours(testObject.TimeoutInMinutes / 60 + 1);
            dateTimeProvider.Setup(date => date.UtcNow).Returns(elapsedByOneHourDate);

            testObject.Run(new List<long>(), actionStepContext);

            orderArchiver.Verify(m => m.Archive(ParameterShorteners.AnyDate), Times.Never());
        }

        [Fact]
        public void RunAsync_DoesArchive_WhenScheduledEndTimeHasNotLapsed()
        {
            dateTimeProvider.Setup(date => date.UtcNow).Returns(DefaultStartDateTimeInUtc.AddMinutes(1));

            testObject.Run(new List<long>(), actionStepContext);

            orderArchiver.Verify(m => m.Archive(ParameterShorteners.AnyDate), Times.Never());
        }

        [Fact]
        public void CreateEditor_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => testObject.CreateEditor());
        }
    }
}
