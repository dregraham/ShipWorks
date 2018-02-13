﻿CREATE FUNCTION [dbo].[MonthToDays365] (@month int)
RETURNS int
--WITH SCHEMABINDING
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

CREATE FUNCTION [dbo].[MonthToDays366] (@month int)
RETURNS int
--WITH SCHEMABINDING
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

CREATE FUNCTION [dbo].[TimeToTicks] (@hour int, @minute int, @second int)
RETURNS bigint
--WITH SCHEMABINDING
AS
-- converts the given hour/minute/second to the corresponding ticks
BEGIN
RETURN (((@hour * 3600) + CONVERT(bigint, @minute) * 60) + CONVERT(bigint, @second)) * 10000000
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

-- Create purge action

DECLARE @ActionID NVARCHAR(20)

-- Get dates in UTC
DECLARE @StartDeleteTime DATETIME
DECLARE @StartDeleteTimeString NVARCHAR(50)
DECLARE @NextDeleteFireTime DATETIME
DECLARE @NextDeleteFireTimeTicks BIGINT
SET @StartDeleteTime = DATEADD(hour, 23, CONVERT(DATETIME, CONVERT(DATE, GETDATE())))
SET @NextDeleteFireTime = DATEADD(d, 1, @StartDeleteTime)

-- Convert to UTC
SET @StartDeleteTime = DATEADD(hh, DATEDIFF(hh, GETDATE(), GETUTCDATE()), @StartDeleteTime)
SET @StartDeleteTimeString = CONVERT(NVARCHAR(50), @StartDeleteTime, 127) + 'Z'

-- Convert to UTC, and get ticks
SET @NextDeleteFireTime = DATEADD(hh, DATEDIFF(hh, GETDATE(), GETUTCDATE()), @NextDeleteFireTime)
SET @NextDeleteFireTimeTicks = dbo.GetTicksFromDateTime(@NextDeleteFireTime)

INSERT INTO [dbo].[Action] ([Name], [Enabled], [ComputerLimitedType], [ComputerLimitedList], [StoreLimited], [StoreLimitedList], [TriggerType], [TriggerSettings], [TaskSummary], [InternalOwner])
VALUES (N'Delete Old Data', 1, 0, '', 0, N'', 6, CONVERT(xml,N'<Settings><DailyActionSchedule><ScheduleType>2</ScheduleType><StartDateTimeInUtc>' + @StartDeleteTimeString  + '</StartDateTimeInUtc><FrequencyInDays>1</FrequencyInDays></DailyActionSchedule></Settings>',1), N'PurgeDatabase', NULL)
SELECT @ActionID = CONVERT(NVARCHAR(20), SCOPE_IDENTITY())

PRINT(N'Add 1 row to [dbo].[Scheduling_JOB_DETAILS]')
INSERT INTO [dbo].[Scheduling_JOB_DETAILS]
([SCHED_NAME], [JOB_NAME], [JOB_GROUP], [DESCRIPTION], [JOB_CLASS_NAME], [IS_DURABLE], [IS_NONCONCURRENT], [IS_UPDATE_DATA], [REQUESTS_RECOVERY], [JOB_DATA])
VALUES (N'QuartzScheduler', @ActionID, N'DEFAULT', NULL, N'ShipWorks.Actions.Scheduling.QuartzNet.ActionJob, ShipWorks.Core', 0, 0, 0, 0, null)

PRINT (N'Add 1 row to [dbo].[ActionTask]')
INSERT INTO [dbo].[ActionTask] ([ActionID], [TaskIdentifier], [TaskSettings], [StepIndex], [InputSource], [InputFilterNodeID], [FilterCondition], [FilterConditionNodeID], [FlowSuccess], [FlowSkipped], [FlowError])
VALUES (CONVERT(BIGINT, @ActionID), N'PurgeDatabase', CONVERT(xml, N'<Settings><CanTimeout value="True"/><TimeoutInHours value="6"/><RetentionPeriodInDays value="180"/><Purges><Item value="2"/><Item value="3"/><Item value="4"/></Purges><ReclaimDiskSpace value="True" /></Settings>', 1), 0, -1, -1, 0, -1, 0, 0, 0)

PRINT(N'Add 1 row to [dbo].[Scheduling_TRIGGERS]')
INSERT INTO [dbo].[Scheduling_TRIGGERS] ([SCHED_NAME], [TRIGGER_NAME], [TRIGGER_GROUP], [JOB_NAME], [JOB_GROUP], [DESCRIPTION], [NEXT_FIRE_TIME], [PREV_FIRE_TIME], [PRIORITY], [TRIGGER_STATE], [TRIGGER_TYPE], [START_TIME], [END_TIME], [CALENDAR_NAME], [MISFIRE_INSTR], [JOB_DATA])
VALUES (N'QuartzScheduler', @ActionID, N'DEFAULT', @ActionID, N'DEFAULT', NULL, @NextDeleteFireTimeTicks, NULL, 5, N'WAITING', N'CAL_INT', @NextDeleteFireTimeTicks, NULL, NULL, 0, NULL)

PRINT(N'Add 1 row to [dbo].[Scheduling_SIMPROP_TRIGGERS]')
INSERT INTO [dbo].[Scheduling_SIMPROP_TRIGGERS] ([SCHED_NAME], [TRIGGER_NAME], [TRIGGER_GROUP], [STR_PROP_1], [STR_PROP_2], [STR_PROP_3], [INT_PROP_1], [INT_PROP_2], [LONG_PROP_1], [LONG_PROP_2], [DEC_PROP_1], [DEC_PROP_2], [BOOL_PROP_1], [BOOL_PROP_2])
VALUES (N'QuartzScheduler', @ActionID, N'DEFAULT', N'Day', dbo.GetLocalTimezoneName(), NULL, 1, 0, 0, 0, 0.0000, 0.0000, 0, 0)
GO

-- Create RebuildTableIndex Action

DECLARE @ActionID NVARCHAR(20)
DECLARE @StartReindexTime DATETIME
DECLARE @StartReindexTimeString NVARCHAR(50)
DECLARE @ReindexFireTimeTicks BIGINT

-- Next Sunday at 2am
SET @StartReindexTime = dateadd(hour, 2, dateadd(week, datediff(week, 0, GETDATE()), 6))


-- Convert to UTC
SET @StartReindexTime = DATEADD(hh, DATEDIFF(hh, GETDATE(), GETUTCDATE()), @StartReindexTime)
SET @StartReindexTimeString = CONVERT(NVARCHAR(50), @StartReindexTime, 127) + 'Z'

-- Convert to UTC, and get ticks
SET @ReindexFireTimeTicks = dbo.GetTicksFromDateTime(@StartReindexTimeString)

INSERT INTO [dbo].[Action] ([Name], [Enabled], [ComputerLimitedType], [ComputerLimitedList], [StoreLimited], [StoreLimitedList], [TriggerType], [TriggerSettings], [TaskSummary], [InternalOwner])
VALUES (N'Reindex Data', 1, 0, '', 0, N'', 6, CONVERT(xml,N'<Settings><DailyActionSchedule><ScheduleType>2</ScheduleType><StartDateTimeInUtc>' + @StartReindexTimeString  + '</StartDateTimeInUtc><FrequencyInDays>7</FrequencyInDays></DailyActionSchedule></Settings>',1), N'RebuildTableIndex', 'ReIndex')
SELECT @ActionID = CONVERT(NVARCHAR(20), SCOPE_IDENTITY())

PRINT(N'Add 1 row to [dbo].[Scheduling_JOB_DETAILS]')
INSERT INTO [dbo].[Scheduling_JOB_DETAILS]
([SCHED_NAME], [JOB_NAME], [JOB_GROUP], [DESCRIPTION], [JOB_CLASS_NAME], [IS_DURABLE], [IS_NONCONCURRENT], [IS_UPDATE_DATA], [REQUESTS_RECOVERY], [JOB_DATA])
VALUES (N'QuartzScheduler', @ActionID, N'DEFAULT', NULL, N'ShipWorks.Actions.Scheduling.QuartzNet.ActionJob, ShipWorks.Core', 0, 0, 0, 0, null)

PRINT (N'Add 1 row to [dbo].[ActionTask]')
INSERT INTO [dbo].[ActionTask] ([ActionID], [TaskIdentifier], [TaskSettings], [StepIndex], [InputSource], [InputFilterNodeID], [FilterCondition], [FilterConditionNodeID], [FlowSuccess], [FlowSkipped], [FlowError])
VALUES (CONVERT(BIGINT, @ActionID), N'RebuildTableIndex', CONVERT(xml, N'<Settings><DailyActionSchedule><ScheduleType>2</ScheduleType><StartDateTimeInUtc>2014-07-06T07:00:00Z</StartDateTimeInUtc><FrequencyInDays>7</FrequencyInDays></DailyActionSchedule><TimeoutInMinutes value="120" /></Settings>', 1), 0, -1, -1, 0, -1, 0, 0, 0)

PRINT(N'Add 1 row to [dbo].[Scheduling_TRIGGERS]')
INSERT INTO [dbo].[Scheduling_TRIGGERS] ([SCHED_NAME], [TRIGGER_NAME], [TRIGGER_GROUP], [JOB_NAME], [JOB_GROUP], [DESCRIPTION], [NEXT_FIRE_TIME], [PREV_FIRE_TIME], [PRIORITY], [TRIGGER_STATE], [TRIGGER_TYPE], [START_TIME], [END_TIME], [CALENDAR_NAME], [MISFIRE_INSTR], [JOB_DATA])
VALUES (N'QuartzScheduler', @ActionID, N'DEFAULT', @ActionID, N'DEFAULT', NULL, @ReindexFireTimeTicks, NULL, 5, N'WAITING', N'CAL_INT', @ReindexFireTimeTicks, NULL, NULL, 0, NULL)

PRINT(N'Add 1 row to [dbo].[Scheduling_SIMPROP_TRIGGERS]')
INSERT INTO [dbo].[Scheduling_SIMPROP_TRIGGERS] ([SCHED_NAME], [TRIGGER_NAME], [TRIGGER_GROUP], [STR_PROP_1], [STR_PROP_2], [STR_PROP_3], [INT_PROP_1], [INT_PROP_2], [LONG_PROP_1], [LONG_PROP_2], [DEC_PROP_1], [DEC_PROP_2], [BOOL_PROP_1], [BOOL_PROP_2])
VALUES (N'QuartzScheduler', @ActionID, N'DEFAULT', N'Day', dbo.GetLocalTimezoneName(), NULL, 7, 0, 0, 0, 0.0000, 0.0000, 0, 0)
GO

-- Create default best rate profile
INSERT INTO [dbo].[ShippingProfile] ([Name], [ShipmentType], [ShipmentTypePrimary], [OriginID], [Insurance], [InsuranceInitialValueSource], [InsuranceInitialValueAmount], [ReturnShipment])
VALUES ('Defaults - Best rate', 14, 1, 0, 0, 0, 0.00, 0)
GO

INSERT INTO [dbo].[BestRateProfile] ([ShippingProfileID], [ServiceLevel])
SELECT TOP 1 ShippingProfileID, 0  FROM ShippingProfile WHERE ShipmentType = 14
GO

-- Cleanup functions
DROP FUNCTION [dbo].[GetTicksFromDateTime]
DROP FUNCTION [dbo].[DateToTicks]
DROP FUNCTION [dbo].[TimeToTicks]
DROP FUNCTION [dbo].[MonthToDays]
DROP FUNCTION [dbo].[MonthToDays366]
DROP FUNCTION [dbo].[MonthToDays365]
DROP FUNCTION dbo.GetLocalTimezoneName



DECLARE @ExecStatement NVARCHAR(4000), @SQLString NVARCHAR(4000)
SET @ExecStatement = 'EXEC sp_executesql @SQLString'
SET @SQLString = N'CREATE PROCEDURE [GetDatabaseGuid] WITH ENCRYPTION AS SELECT convert(uniqueidentifier,''' + CONVERT(varchar(255),NEWID()) + ''')'
EXEC sp_executesql  @ExecStatement, N'@SQLString NVARCHAR(4000)', @SQLString=@SQLString


-- Create finish shipment processing batch action

DECLARE @ActionID BIGINT

INSERT INTO [dbo].[Action] ([Name], [Enabled], [ComputerLimitedType], [ComputerLimitedList], [StoreLimited], [StoreLimitedList], [TriggerType], [TriggerSettings], [TaskSummary], [InternalOwner])
VALUES (N'Finish Processing Batch', 1, 1, '', 0, N'', 8, CONVERT(xml,N'<Settings>
  <RestrictStandardReturn value="True" />
  <ReturnShipmentsOnly value="False" />
  <RestrictType value="False" />
</Settings>',1), N'FinishProcessingBatch', N'FinishProcessingBatch')
SELECT @ActionID = SCOPE_IDENTITY()

PRINT (N'Add 1 row to [dbo].[ActionTask]')
INSERT INTO [dbo].[ActionTask] ([ActionID], [TaskIdentifier], [TaskSettings], [StepIndex], [InputSource], [InputFilterNodeID], [FilterCondition], [FilterConditionNodeID], [FlowSuccess], [FlowSkipped], [FlowError])
VALUES (@ActionID, N'FinishProcessingBatch', CONVERT(xml, N'<Settings />', 1), 0, -1, -1, 0, -1, 0, 0, 0)
GO

PRINT (N'Add Initial AmazonServiceTypes')
GO
INSERT INTO dbo.AmazonServiceType
(ApiValue, Description)
VALUES
('USPS_PTP_FC', 'USPS First Class'),
('USPS_PTP_PRI', 'USPS Priority Mail®'),
('USPS_PTP_PRI_SFRB', 'USPS Priority Mail® Small Flat Rate Box'),
('USPS_PTP_PRI_MFRB', 'USPS Priority Mail® Flat Rate Box'),
('USPS_PTP_PRI_LFRB', 'USPS Priority Mail® Large Flat Rate Box'),
('USPS_PTP_PRI_FRE', 'USPS Priority Mail® Flat Rate Envelope'),
('USPS_PTP_PRI_LFRE', 'USPS Priority Mail Legal Flat Rate Envelope'),
('USPS_PTP_PRI_PFRE', 'USPS Priority Mail Padded Flat Rate Envelope'),
('USPS_PTP_PRI_RA', 'USPS Priority Mail Regional Rate Box A'),
('USPS_PTP_PRI_RB', 'USPS Priority Mail Regional Rate Box B'),
('USPS_PTP_EXP', 'USPS Priority Mail Express®'),
('USPS_PTP_EXP_FRE', 'USPS Priority Mail Express® Flat Rate Envelope'),
('USPS_PTP_EXP_LFRE', 'USPS Priority Mail Express Legal Flat Rate Envelope'),
('USPS_PTP_EXP_PFRE', 'USPS Priority Mail Express Padded Flat Rate Envelope'),
('USPS_PTP_PSBN', 'USPS Parcel Select'),
('FEDEX_PTP_GROUND', 'FedEx Ground®'),
('FEDEX_PTP_SECOND_DAY', 'FedEx 2Day®'),
('FEDEX_PTP_SECOND_DAY_AM', 'FedEx 2Day®A.M.'),
('FEDEX_PTP_EXPRESS_SAVER', 'FedEx Express Saver®'),
('FEDEX_PTP_STANDARD_OVERNIGHT', 'FedEx Standard Overnight®'),
('FEDEX_PTP_PRIORITY_OVERNIGHT', 'FedEx Priority Overnight®'),
('UPS_PTP_GND', 'UPS Ground'),
('UPS_PTP_3DAY_SELECT', 'UPS 3 Day Select'),
('UPS_PTP_2ND_DAY_AIR', 'UPS 2nd Day Air'),
('UPS_PTP_NEXT_DAY_AIR_SAVER', 'UPS Next Day Air Saver'),
('UPS_PTP_NEXT_DAY_AIR', 'UPS Next Day Air');
GO