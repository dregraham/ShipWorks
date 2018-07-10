PRINT N'Add Manage Index State Task'
GO
IF EXISTS (SELECT 1 FROM sys.objects WHERE type = 'FN' AND name = 'MonthToDays365')
	BEGIN
		DROP FUNCTION [dbo].[MonthToDays365]
	END
GO

CREATE FUNCTION [dbo].[MonthToDays365] (@month int)
RETURNS int
AS
-- converts the given month (0-12) to the corresponding number of days into the year (by end of month)
-- this function is for non-leap years
BEGIN 
RETURN
	CASE @month
		WHEN 0 THEN 0
		WHEN 1 THEN 31
		WHEN 2 THEN 59
		WHEN 3 THEN 90
		WHEN 4 THEN 120
		WHEN 5 THEN 151
		WHEN 6 THEN 181
		WHEN 7 THEN 212
		WHEN 8 THEN 243
		WHEN 9 THEN 273
		WHEN 10 THEN 304
		WHEN 11 THEN 334
		WHEN 12 THEN 365
		ELSE 0
	END
END
GO

IF EXISTS (SELECT 1 FROM sys.objects WHERE type = 'FN' AND name = 'MonthToDays366')
	BEGIN
		DROP FUNCTION [dbo].[MonthToDays366]
	END
GO

CREATE FUNCTION [dbo].[MonthToDays366] (@month int)
RETURNS int 
AS
-- converts the given month (0-12) to the corresponding number of days into the year (by end of month)
-- this function is for leap years
BEGIN 
RETURN
	CASE @month
		WHEN 0 THEN 0
		WHEN 1 THEN 31
		WHEN 2 THEN 60
		WHEN 3 THEN 91
		WHEN 4 THEN 121
		WHEN 5 THEN 152
		WHEN 6 THEN 182
		WHEN 7 THEN 213
		WHEN 8 THEN 244
		WHEN 9 THEN 274
		WHEN 10 THEN 305
		WHEN 11 THEN 335
		WHEN 12 THEN 366
		ELSE 0
	END
END
GO

IF EXISTS (SELECT 1 FROM sys.objects WHERE type = 'FN' AND name = 'MonthToDays')
	BEGIN
		DROP FUNCTION [dbo].[MonthToDays]
	END
GO

CREATE FUNCTION [dbo].[MonthToDays] (@year int, @month int)
RETURNS int
--WITH SCHEMABINDING
AS
-- converts the given month (0-12) to the corresponding number of days into the year (by end of month)
-- this function is for non-leap years
BEGIN 
RETURN 
	-- determine whether the given year is a leap year
	CASE 
		WHEN (@year % 4 = 0) and ((@year % 100  != 0) or ((@year % 100 = 0) and (@year % 400 = 0))) THEN dbo.MonthToDays366(@month)
		ELSE dbo.MonthToDays365(@month)
	END
END
GO

IF EXISTS (SELECT 1 FROM sys.objects WHERE type = 'FN' AND name = 'TimeToTicks')
	BEGIN
		DROP FUNCTION [dbo].[TimeToTicks]
	END
GO

CREATE FUNCTION [dbo].[TimeToTicks] (@hour int, @minute int, @second int)  
RETURNS bigint 
--WITH SCHEMABINDING
AS 
-- converts the given hour/minute/second to the corresponding ticks
BEGIN 
RETURN (((@hour * 3600) + CONVERT(bigint, @minute) * 60) + CONVERT(bigint, @second)) * 10000000
END
GO

IF EXISTS (SELECT 1 FROM sys.objects WHERE type = 'FN' AND name = 'DateToTicks')
	BEGIN
		DROP FUNCTION [dbo].[DateToTicks]
	END
GO

CREATE FUNCTION [dbo].[DateToTicks] (@year int, @month int, @day int)
RETURNS bigint
--WITH SCHEMABINDING
AS
-- converts the given year/month/day to the corresponding ticks
BEGIN 
RETURN CONVERT(bigint, (((((((@year - 1) * 365) + ((@year - 1) / 4)) - ((@year - 1) / 100)) + ((@year - 1) / 400)) + dbo.MonthToDays(@year, @month - 1)) + @day) - 1) * 864000000000;
END
GO

IF EXISTS (SELECT 1 FROM sys.objects WHERE type = 'FN' AND name = 'GetTicksFromDateTime')
	BEGIN
		DROP FUNCTION [dbo].[GetTicksFromDateTime]
	END
GO

CREATE FUNCTION [dbo].[GetTicksFromDateTime] (@d datetime)
RETURNS bigint
--WITH SCHEMABINDING
AS
-- converts the given datetime to .NET-compatible ticks
-- see http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpref/html/frlrfsystemdatetimeclasstickstopic.asp
BEGIN 
RETURN 
	dbo.DateToTicks(DATEPART(yyyy, @d), DATEPART(mm, @d), DATEPART(dd, @d)) +
	dbo.TimeToTicks(DATEPART(hh, @d), DATEPART(mi, @d), DATEPART(ss, @d)) +
	(CONVERT(bigint, DATEPART(ms, @d)) * CONVERT(bigint,10000));
END
GO

IF EXISTS (SELECT 1 FROM sys.objects WHERE type = 'FN' AND name = 'GetLocalTimezoneName')
	BEGIN
		DROP FUNCTION [dbo].[GetLocalTimezoneName]
	END
GO

CREATE FUNCTION dbo.GetLocalTimezoneName()
RETURNS nvarchar(100)
AS
BEGIN
	DECLARE @timeZoneOffSet int
	DECLARE @timeZoneName NVARCHAR(100)
	SET @timeZoneOffSet = datediff(hh, getdate(), getutcdate())

	SELECT @timeZoneName = 
	CASE
		WHEN @timeZoneOffSet = 4 THEN 'Eastern Standard Time'
		WHEN @timeZoneOffSet = 6 THEN 'Mountain Standard Time'
		WHEN @timeZoneOffSet = 7 THEN 'Pacific Standard Time'
		ELSE 'Central Standard Time'
	END

	RETURN @timeZoneName
END
GO

IF NOT EXISTS(SELECT * FROM [Action] WHERE Name = N'Manage Index State')
BEGIN

	DECLARE @ActionID NVARCHAR(20)
	DECLARE @StartManageIndexTime DATETIME
	DECLARE @StartManageIndexTimeString NVARCHAR(50)
	DECLARE @ManageIndexFireTimeTicks BIGINT

	-- Next Sunday at 12am
	SET @StartManageIndexTime = dateadd(hour, 0, dateadd(week, datediff(week, 0, GETDATE()), 6))

	-- Convert to UTC
	SET @StartManageIndexTime = DATEADD(hh, DATEDIFF(hh, GETDATE(), GETUTCDATE()), @StartManageIndexTime)
	SET @StartManageIndexTimeString = CONVERT(NVARCHAR(50), @StartManageIndexTime, 127) + 'Z'

	-- Convert to UTC, and get ticks
	SET @ManageIndexFireTimeTicks = dbo.GetTicksFromDateTime(@StartManageIndexTimeString)

	INSERT INTO [dbo].[Action] ([Name], [Enabled], [ComputerLimitedType], [ComputerLimitedList], [StoreLimited], [StoreLimitedList], [TriggerType], [TriggerSettings], [TaskSummary], [InternalOwner])
	VALUES (N'Manage Index State', 1, 0, '', 0, N'', 6, CONVERT(xml,N'<Settings><DailyActionSchedule><ScheduleType>2</ScheduleType><StartDateTimeInUtc>' + @StartManageIndexTimeString  + '</StartDateTimeInUtc><FrequencyInDays>1</FrequencyInDays></DailyActionSchedule></Settings>',1), N'ManageIndexState', 'ManageIndexState')
	SELECT @ActionID = CONVERT(NVARCHAR(20), SCOPE_IDENTITY())

	PRINT(N'Add 1 row to [dbo].[Scheduling_JOB_DETAILS]')
	INSERT INTO [dbo].[Scheduling_JOB_DETAILS]
	([SCHED_NAME], [JOB_NAME], [JOB_GROUP], [DESCRIPTION], [JOB_CLASS_NAME], [IS_DURABLE], [IS_NONCONCURRENT], [IS_UPDATE_DATA], [REQUESTS_RECOVERY], [JOB_DATA])
	VALUES (N'QuartzScheduler', @ActionID, N'DEFAULT', NULL, N'ShipWorks.Actions.Scheduling.QuartzNet.ActionJob, ShipWorks.Core', 0, 0, 0, 0, null)

	PRINT (N'Add 1 row to [dbo].[ActionTask]')
	INSERT INTO [dbo].[ActionTask] ([ActionID], [TaskIdentifier], [TaskSettings], [StepIndex], [InputSource], [InputFilterNodeID], [FilterCondition], [FilterConditionNodeID], [FlowSuccess], [FlowSkipped], [FlowError])
	VALUES (CONVERT(BIGINT, @ActionID), N'ManageIndexState', CONVERT(xml, N'<Settings><DailyActionSchedule><ScheduleType>2</ScheduleType><StartDateTimeInUtc>2014-07-06T07:00:00Z</StartDateTimeInUtc><FrequencyInDays>1</FrequencyInDays></DailyActionSchedule><TimeoutInMinutes value="120" /><DaysBack>14</DaysBack></Settings>', 1), 0, -1, -1, 0, -1, 0, 0, 0)

	PRINT(N'Add 1 row to [dbo].[Scheduling_TRIGGERS]')
	INSERT INTO [dbo].[Scheduling_TRIGGERS] ([SCHED_NAME], [TRIGGER_NAME], [TRIGGER_GROUP], [JOB_NAME], [JOB_GROUP], [DESCRIPTION], [NEXT_FIRE_TIME], [PREV_FIRE_TIME], [PRIORITY], [TRIGGER_STATE], [TRIGGER_TYPE], [START_TIME], [END_TIME], [CALENDAR_NAME], [MISFIRE_INSTR], [JOB_DATA])
	VALUES (N'QuartzScheduler', @ActionID, N'DEFAULT', @ActionID, N'DEFAULT', NULL, @ManageIndexFireTimeTicks, NULL, 5, N'WAITING', N'CAL_INT', @ManageIndexFireTimeTicks, NULL, NULL, 0, NULL)

	PRINT(N'Add 1 row to [dbo].[Scheduling_SIMPROP_TRIGGERS]')
	INSERT INTO [dbo].[Scheduling_SIMPROP_TRIGGERS] ([SCHED_NAME], [TRIGGER_NAME], [TRIGGER_GROUP], [STR_PROP_1], [STR_PROP_2], [STR_PROP_3], [INT_PROP_1], [INT_PROP_2], [LONG_PROP_1], [LONG_PROP_2], [DEC_PROP_1], [DEC_PROP_2], [BOOL_PROP_1], [BOOL_PROP_2])
	VALUES (N'QuartzScheduler', @ActionID, N'DEFAULT', N'Day', dbo.GetLocalTimezoneName(), NULL, 1, 0, 0, 0, 0.0000, 0.0000, 0, 0)
END
GO



