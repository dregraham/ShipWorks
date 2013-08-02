DECLARE @ActionID NVARCHAR(20)

-- Get dates in UTC
DECLARE @StartDateTime DATETIME
DECLARE @StartDateTimeString NVARCHAR(50)
DECLARE @NextFireTime DATETIME
DECLARE @NextFireTimeTicks BIGINT
SET @StartDateTime = DATEADD(hour, 23, CONVERT(DATETIME, CONVERT(DATE, GETDATE())))
SET @NextFireTime = DATEADD(d, 1, @StartDateTime)

-- Convert to UTC
SET @StartDateTime = DATEADD(hh, DATEDIFF(hh, GETDATE(), GETUTCDATE()), @StartDateTime)
SET @StartDateTimeString = CONVERT(NVARCHAR(50), @StartDateTime, 127) + 'Z'

-- Convert to UTC, and get ticks
SET @NextFireTime = DATEADD(hh, DATEDIFF(hh, GETDATE(), GETUTCDATE()), @NextFireTime)
SET @NextFireTimeTicks = dbo.GetTicksFromDateTime(@NextFireTime)

INSERT INTO [dbo].[Action] ([Name], [Enabled], [ComputerLimitedType], [ComputerLimitedList], [StoreLimited], [StoreLimitedList], [TriggerType], [TriggerSettings], [TaskSummary], [InternalOwner]) 
VALUES (N'Delete Old Data', 1, 0, '', 0, N'', 6, CONVERT(xml,N'<Settings><DailyActionSchedule><ScheduleType>2</ScheduleType><StartDateTimeInUtc>' + @StartDateTimeString  + '</StartDateTimeInUtc><FrequencyInDays>1</FrequencyInDays></DailyActionSchedule></Settings>',1), N'PurgeDatabase', NULL)
SELECT @ActionID = CONVERT(NVARCHAR(20), SCOPE_IDENTITY())

PRINT(N'Add 1 row to [dbo].[Scheduling_JOB_DETAILS]')
INSERT INTO [dbo].[Scheduling_JOB_DETAILS] 
([SCHED_NAME], [JOB_NAME], [JOB_GROUP], [DESCRIPTION], [JOB_CLASS_NAME], [IS_DURABLE], [IS_NONCONCURRENT], [IS_UPDATE_DATA], [REQUESTS_RECOVERY], [JOB_DATA]) 
VALUES (N'QuartzScheduler', @ActionID, N'DEFAULT', NULL, N'ShipWorks.Actions.Scheduling.QuartzNet.ActionJob, ShipWorks', 0, 0, 0, 0, null)

PRINT(N'Add 1 row to [dbo].[ActionTask]')
INSERT INTO [dbo].[ActionTask] ([ActionID], [TaskIdentifier], [TaskSettings], [StepIndex], [InputSource], [InputFilterNodeID], [FilterCondition], [FilterConditionNodeID], [FlowSuccess], [FlowSkipped], [FlowError]) 
VALUES (CONVERT(BIGINT, @ActionID), N'PurgeDatabase', CONVERT(xml,N'<Settings><CanTimeout value="True"/><TimeoutInHours value="3"/><RetentionPeriodInDays value="30"/><Purges><Item type="ShipWorks.Actions.Tasks.Common.PurgeDatabaseType" value="3"/></Purges></Settings>',1), 0, 1, -1, 0, -1, 0, 0, 0)
 
PRINT (N'Add 1 row to [dbo].[ActionTask]')
INSERT INTO [dbo].[ActionTask] ([ActionID], [TaskIdentifier], [TaskSettings], [StepIndex], [InputSource], [InputFilterNodeID], [FilterCondition], [FilterConditionNodeID], [FlowSuccess], [FlowSkipped], [FlowError])
VALUES (CONVERT(BIGINT, @ActionID), N'PurgeDatabase', CONVERT(xml, N'<Settings><CanTimeout value="True"/><TimeoutInHours value="3"/><RetentionPeriodInDays value="180"/><Purges><Item type="ShipWorks.Actions.Tasks.Common.PurgeDatabaseType" value="2"/><Item type="ShipWorks.Actions.Tasks.Common.PurgeDatabaseType" value="4"/></Purges></Settings>', 1), 0, 1, -1, 0, -1, 0, 0, 0)

PRINT(N'Add 1 row to [dbo].[Scheduling_TRIGGERS]')
INSERT INTO [dbo].[Scheduling_TRIGGERS] ([SCHED_NAME], [TRIGGER_NAME], [TRIGGER_GROUP], [JOB_NAME], [JOB_GROUP], [DESCRIPTION], [NEXT_FIRE_TIME], [PREV_FIRE_TIME], [PRIORITY], [TRIGGER_STATE], [TRIGGER_TYPE], [START_TIME], [END_TIME], [CALENDAR_NAME], [MISFIRE_INSTR], [JOB_DATA]) 
VALUES (N'QuartzScheduler', @ActionID, N'DEFAULT', @ActionID, N'DEFAULT', NULL, @NextFireTimeTicks, NULL, 5, N'WAITING', N'CAL_INT', @NextFireTimeTicks, NULL, NULL, 0, NULL)

PRINT(N'Add 1 row to [dbo].[Scheduling_SIMPROP_TRIGGERS]')
INSERT INTO [dbo].[Scheduling_SIMPROP_TRIGGERS] ([SCHED_NAME], [TRIGGER_NAME], [TRIGGER_GROUP], [STR_PROP_1], [STR_PROP_2], [STR_PROP_3], [INT_PROP_1], [INT_PROP_2], [LONG_PROP_1], [LONG_PROP_2], [DEC_PROP_1], [DEC_PROP_2], [BOOL_PROP_1], [BOOL_PROP_2]) 
VALUES (N'QuartzScheduler', @ActionID, N'DEFAULT', N'Day', dbo.GetLocalTimezoneName(), NULL, 1, 0, 0, 0, 0.0000, 0.0000, 0, 0)
