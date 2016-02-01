using Interapptive.Shared.Utility;
using log4net;
using Xunit;
using Moq;
using ShipWorks.Actions;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Triggers;
using ShipWorks.Data.Administration.Indexing;
using ShipWorks.Data.Model.EntityClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ShipWorks.Tests.Actions.Tasks.Common
{
    public class RebuildTableIndexTaskTest
    {
        private RebuildTableIndexTask testObject;

        private Mock<IIndexMonitor> indexMonitor;
        private Mock<ILog> log;
        private Mock<IDateTimeProvider> dateTimeProvider;
        private ActionStepContext actionStepContext;
        private ActionTaskEntity actionTaskEntity;
        private readonly DateTime DefaultStartDateTimeInUtc = new DateTime(2014, 7, 13, 2, 0, 0, DateTimeKind.Utc);

        public RebuildTableIndexTaskTest()
        {
            indexMonitor = new Mock<IIndexMonitor>();
            indexMonitor.Setup(m => m.GetIndexesToRebuild())
                        .Returns
                        (
                            new List<TableIndex>
                            {
                                new TableIndex() { IndexName = "Index1", TableName = "Table1" },
                                new TableIndex() {IndexName = "Index2", TableName = "Table2"}
                            }
                        );

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
                TaskIdentifier = "RebuildTableIndex"
            };

            testObject = new RebuildTableIndexTask(indexMonitor.Object, dateTimeProvider.Object, log.Object);

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
        public void TimeoutInMinutes_IsDefaultedTo120()
        {
            testObject = new RebuildTableIndexTask(indexMonitor.Object, dateTimeProvider.Object, log.Object);

            Assert.Equal(120, testObject.TimeoutInMinutes);
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
        public void Run_DoesNotFetchIndexes_WhenScheduledEndTimeHasLapsed_ByOneHour()
        {
            DateTime elapsedByOneHourDate = DefaultStartDateTimeInUtc.AddHours(testObject.TimeoutInMinutes / 60 + 1);
            dateTimeProvider.Setup(date => date.UtcNow).Returns(elapsedByOneHourDate);

            testObject.Run(new List<long>(), actionStepContext);

            indexMonitor.Verify(m => m.GetIndexesToRebuild(), Times.Never());
        }

        [Fact]
        public void Run_DoesNotFetchIndexes_WhenScheduledEndTimeHasLapsed_ByOneMinute()
        {
            DateTime elapsedByOneMinuteDate = DefaultStartDateTimeInUtc.AddMinutes(testObject.TimeoutInMinutes + 1);
            dateTimeProvider.Setup(date => date.UtcNow).Returns(elapsedByOneMinuteDate);

            testObject.Run(new List<long>(), actionStepContext);

            indexMonitor.Verify(m => m.GetIndexesToRebuild(), Times.Never());
        }

        [Fact]
        public void Run_DoesNotFetchIndexes_WhenScheduledEndTimeHasLapsed_ByOneSecond()
        {
            DateTime elapsedByOneSecondDate = DefaultStartDateTimeInUtc.AddSeconds(testObject.TimeoutInMinutes * 60 + 1);
            dateTimeProvider.Setup(date => date.UtcNow).Returns(elapsedByOneSecondDate);

            testObject.Run(new List<long>(), actionStepContext);

            indexMonitor.Verify(m => m.GetIndexesToRebuild(), Times.Never());
        }

        [Fact]
        public void Run_DoesNotFetchIndexes_WhenScheduledEndTimeHasLapsed_ByOneMillisecond()
        {
            DateTime elapsedByOneMillisecondDate = DefaultStartDateTimeInUtc.AddMilliseconds(testObject.TimeoutInMinutes * 60000 + 1);
            dateTimeProvider.Setup(date => date.UtcNow).Returns(elapsedByOneMillisecondDate);

            testObject.Run(new List<long>(), actionStepContext);

            indexMonitor.Verify(m => m.GetIndexesToRebuild(), Times.Never());
        }

        [Fact]
        public void Run_FetchesIndexes_WhenScheduledEndTimeHasNotLapsed()
        {
            DateTime elapsedByOneMinuteDate = DefaultStartDateTimeInUtc.AddSeconds(-5);
            dateTimeProvider.Setup(date => date.UtcNow).Returns(elapsedByOneMinuteDate);

            testObject.Run(new List<long>(), actionStepContext);

            indexMonitor.Verify(m => m.GetIndexesToRebuild(), Times.Once());
        }

        [Fact]
        public void Run_LogsAllIndexesNeedingRebuilt()
        {
            DateTime elapsedByOneMinuteDate = DefaultStartDateTimeInUtc.AddSeconds(-5);
            dateTimeProvider.Setup(date => date.UtcNow).Returns(elapsedByOneMinuteDate);

            testObject.Run(new List<long>(), actionStepContext);

            log.Verify(l => l.Info("The following indexes need to be rebuilt: Table1.Index1, Table2.Index2"), Times.Once());
        }

        [Fact]
        public void Run_LogsMessageWhenNoIndexesNeedRebuilt()
        {
            DateTime elapsedByOneMinuteDate = DefaultStartDateTimeInUtc.AddSeconds(-5);
            dateTimeProvider.Setup(date => date.UtcNow).Returns(elapsedByOneMinuteDate);

            indexMonitor.Setup(m => m.GetIndexesToRebuild()).Returns(new List<TableIndex>());

            testObject.Run(new List<long>(), actionStepContext);

            log.Verify(l => l.Info("No indexes need to be rebuilt at this time."), Times.Once());
        }

        [Fact]
        public void Run_RebuildsEachIndex_WhenThereIsOnlyOneIndexToRebuild()
        {
            DateTime elapsedByOneMinuteDate = DefaultStartDateTimeInUtc.AddSeconds(-5);
            dateTimeProvider.Setup(date => date.UtcNow).Returns(elapsedByOneMinuteDate);

            indexMonitor.Setup(m => m.GetIndexesToRebuild()).Returns(new List<TableIndex> { new TableIndex() });

            testObject.Run(new List<long>(), actionStepContext);

            indexMonitor.Verify(m => m.RebuildIndex(It.IsAny<TableIndex>()), Times.Once());
        }

        [Fact]
        public void Run_LogsEachRebuiltIndex_WhenThereIsOnlyOneIndexToRebuild()
        {
            DateTime elapsedByOneMinuteDate = DefaultStartDateTimeInUtc.AddSeconds(-5);
            dateTimeProvider.Setup(date => date.UtcNow).Returns(elapsedByOneMinuteDate);

            indexMonitor.Setup(m => m.GetIndexesToRebuild()).Returns(new List<TableIndex> { new TableIndex() });

            testObject.Run(new List<long>(), actionStepContext);

            log.Verify(l => l.InfoFormat("Rebuilding index {0}", It.IsAny<TableIndex>()), Times.Once());
        }

        [Fact]
        public void Run_LogsEachRebuiltIndexAfterCompletion_WhenThereIsOnlyOneIndexToRebuild()
        {
            DateTime elapsedByOneMinuteDate = DefaultStartDateTimeInUtc.AddSeconds(-5);
            dateTimeProvider.Setup(date => date.UtcNow).Returns(elapsedByOneMinuteDate);

            indexMonitor.Setup(m => m.GetIndexesToRebuild()).Returns(new List<TableIndex> { new TableIndex() });

            testObject.Run(new List<long>(), actionStepContext);

            log.Verify(l => l.InfoFormat("Finished rebuilding index {0} ({1} ms)", It.IsAny<TableIndex>(), It.IsAny<long>()), Times.Once());
        }

        [Fact]
        public void Run_RebuildsEachIndex_WhenThereAreMultipleIndexesToRebuild()
        {
            DateTime elapsedByOneMinuteDate = DefaultStartDateTimeInUtc.AddSeconds(-5);
            dateTimeProvider.Setup(date => date.UtcNow).Returns(elapsedByOneMinuteDate);

            testObject.Run(new List<long>(), actionStepContext);

            indexMonitor.Verify(m => m.RebuildIndex(It.IsAny<TableIndex>()), Times.Exactly(2));
        }

        [Fact]
        public void Run_LogsEachRebuiltIndex_WhenThereAreMultipleIndexesToRebuild()
        {
            DateTime elapsedByOneMinuteDate = DefaultStartDateTimeInUtc.AddSeconds(-5);
            dateTimeProvider.Setup(date => date.UtcNow).Returns(elapsedByOneMinuteDate);

            testObject.Run(new List<long>(), actionStepContext);

            log.Verify(l => l.InfoFormat("Rebuilding index {0}", It.IsAny<TableIndex>()), Times.Exactly(2));
        }

        [Fact]
        public void Run_LogsEachRebuiltIndexAfterCompletion_WhenThereAreMultipleIndexesToRebuild()
        {
            DateTime elapsedByOneMinuteDate = DefaultStartDateTimeInUtc.AddSeconds(-5);
            dateTimeProvider.Setup(date => date.UtcNow).Returns(elapsedByOneMinuteDate);

            testObject.Run(new List<long>(), actionStepContext);

            log.Verify(l => l.InfoFormat("Finished rebuilding index {0} ({1} ms)", It.IsAny<TableIndex>(), It.IsAny<long>()), Times.Exactly(2));
        }

        [Fact]
        public void Run_MovesToNextIndex_WhenIndexFails_AndThereAreMultipleIndexesToRebuild()
        {
            DateTime elapsedByOneMinuteDate = DefaultStartDateTimeInUtc.AddSeconds(-5);
            dateTimeProvider.Setup(date => date.UtcNow).Returns(elapsedByOneMinuteDate);

            int count = 0;
            indexMonitor.Setup(m => m.RebuildIndex(It.IsAny<TableIndex>()))
                        .Callback(() =>
                        {
                            // Throw an exception the first time rebuild is called
                            if (count == 0)
                            {
                                count++;
                                throw new Exception();
                            }
                        });

            testObject.Run(new List<long>(), actionStepContext);

            // Should have finished building one index
            log.Verify(l => l.InfoFormat("Finished rebuilding index {0} ({1} ms)", It.IsAny<TableIndex>(), It.IsAny<long>()), Times.Once());
        }

        [Fact]
        public void Run_LogsException_WhenIndexFails_AndThereAreMultipleIndexesToRebuild()
        {
            DateTime elapsedByOneMinuteDate = DefaultStartDateTimeInUtc.AddSeconds(-5);
            dateTimeProvider.Setup(date => date.UtcNow).Returns(elapsedByOneMinuteDate);

            int count = 0;
            indexMonitor.Setup(m => m.RebuildIndex(It.IsAny<TableIndex>()))
                        .Callback(() =>
                        {
                            // Throw an exception the first time rebuild is called
                            if (count == 0)
                            {
                                count++;
                                throw new Exception();
                            }
                        });

            testObject.Run(new List<long>(), actionStepContext);

            // Should have caught and logged one exception
            log.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once());
        }

        [Fact]
        public void Run_ThrowsException_WhenUnableToObtainIndexesToRebuild()
        {
            indexMonitor.Setup(m => m.GetIndexesToRebuild()).Throws(new Exception("Mocked exception"));

            Assert.Throws<Exception>(() => testObject.Run(new List<long>(), actionStepContext));
        }

        [Fact]
        public void Run_LogsException_WhenUnableToObtainIndexesToRebuild()
        {
            try
            {
                indexMonitor.Setup(m => m.GetIndexesToRebuild()).Throws(new Exception("Mocked exception"));

                testObject.Run(new List<long>(), actionStepContext);
            }
            catch (Exception ex)
            {
                log.Verify(l => l.Error("An error occurred while running the task to rebuild indexes. Mocked exception", ex), Times.Once());
            }
        }

        [Fact]
        public void CreateEditor_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => testObject.CreateEditor());
        }
    }
}
