IF EXISTS (SELECT 1 FROM sys.objects WHERE type = 'FN' AND name = 'MonthToDays365')
	BEGIN
		DROP FUNCTION [dbo].[MonthToDays365]
	END
GO

CREATE FUNCTION [dbo].[MonthToDays365] (@month int)
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

IF EXISTS (SELECT 1 FROM sys.objects WHERE type = 'FN' AND name = 'MonthToDays366')
	BEGIN
		DROP FUNCTION [dbo].[MonthToDays366]
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
VALUES (CONVERT(BIGINT, @ActionID), N'PurgeDatabase', CONVERT(xml, N'<Settings><CanTimeout value="True"/><TimeoutInHours value="6"/><RetentionPeriodInDays value="180"/><Purges><Item value="2"/><Item value="3"/><Item value="4"/></Purges><PurgeEmailHistory value="True" /><PurgePrintJobHistory value="True" /><ReclaimDiskSpace value="True" /></Settings>', 1), 0, -1, -1, 0, -1, 0, 0, 0)

PRINT(N'Add 1 row to [dbo].[Scheduling_TRIGGERS]')
INSERT INTO [dbo].[Scheduling_TRIGGERS] ([SCHED_NAME], [TRIGGER_NAME], [TRIGGER_GROUP], [JOB_NAME], [JOB_GROUP], [DESCRIPTION], [NEXT_FIRE_TIME], [PREV_FIRE_TIME], [PRIORITY], [TRIGGER_STATE], [TRIGGER_TYPE], [START_TIME], [END_TIME], [CALENDAR_NAME], [MISFIRE_INSTR], [JOB_DATA])
VALUES (N'QuartzScheduler', @ActionID, N'DEFAULT', @ActionID, N'DEFAULT', NULL, @NextDeleteFireTimeTicks, NULL, 5, N'WAITING', N'CAL_INT', @NextDeleteFireTimeTicks, NULL, NULL, 0, NULL)

PRINT(N'Add 1 row to [dbo].[Scheduling_SIMPROP_TRIGGERS]')
INSERT INTO [dbo].[Scheduling_SIMPROP_TRIGGERS] ([SCHED_NAME], [TRIGGER_NAME], [TRIGGER_GROUP], [STR_PROP_1], [STR_PROP_2], [STR_PROP_3], [INT_PROP_1], [INT_PROP_2], [LONG_PROP_1], [LONG_PROP_2], [DEC_PROP_1], [DEC_PROP_2], [BOOL_PROP_1], [BOOL_PROP_2])
VALUES (N'QuartzScheduler', @ActionID, N'DEFAULT', N'Day', dbo.GetLocalTimezoneName(), NULL, 1, 0, 0, 0, 0.0000, 0.0000, 0, 0)
GO

-- Create DatabaseMaintenance Action

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
VALUES (N'Database Maintenance', 1, 0, '', 0, N'', 6, CONVERT(xml,N'<Settings><DailyActionSchedule><ScheduleType>2</ScheduleType><StartDateTimeInUtc>' + @StartReindexTimeString  + '</StartDateTimeInUtc><FrequencyInDays>7</FrequencyInDays></DailyActionSchedule></Settings>',1), N'DatabaseMaintenance', 'DatabaseMaintenance')
SELECT @ActionID = CONVERT(NVARCHAR(20), SCOPE_IDENTITY())

PRINT(N'Add 1 row to [dbo].[Scheduling_JOB_DETAILS]')
INSERT INTO [dbo].[Scheduling_JOB_DETAILS]
([SCHED_NAME], [JOB_NAME], [JOB_GROUP], [DESCRIPTION], [JOB_CLASS_NAME], [IS_DURABLE], [IS_NONCONCURRENT], [IS_UPDATE_DATA], [REQUESTS_RECOVERY], [JOB_DATA])
VALUES (N'QuartzScheduler', @ActionID, N'DEFAULT', NULL, N'ShipWorks.Actions.Scheduling.QuartzNet.ActionJob, ShipWorks.Core', 0, 0, 0, 0, null)

PRINT (N'Add 1 row to [dbo].[ActionTask]')
INSERT INTO [dbo].[ActionTask] ([ActionID], [TaskIdentifier], [TaskSettings], [StepIndex], [InputSource], [InputFilterNodeID], [FilterCondition], [FilterConditionNodeID], [FlowSuccess], [FlowSkipped], [FlowError])
VALUES (CONVERT(BIGINT, @ActionID), N'DatabaseMaintenance', CONVERT(xml, N'<Settings><DailyActionSchedule><ScheduleType>2</ScheduleType><StartDateTimeInUtc>2014-07-06T07:00:00Z</StartDateTimeInUtc><FrequencyInDays>7</FrequencyInDays></DailyActionSchedule><TimeoutInMinutes value="120" /></Settings>', 1), 0, -1, -1, 0, -1, 0, 0, 0)

PRINT(N'Add 1 row to [dbo].[Scheduling_TRIGGERS]')
INSERT INTO [dbo].[Scheduling_TRIGGERS] ([SCHED_NAME], [TRIGGER_NAME], [TRIGGER_GROUP], [JOB_NAME], [JOB_GROUP], [DESCRIPTION], [NEXT_FIRE_TIME], [PREV_FIRE_TIME], [PRIORITY], [TRIGGER_STATE], [TRIGGER_TYPE], [START_TIME], [END_TIME], [CALENDAR_NAME], [MISFIRE_INSTR], [JOB_DATA])
VALUES (N'QuartzScheduler', @ActionID, N'DEFAULT', @ActionID, N'DEFAULT', NULL, @ReindexFireTimeTicks, NULL, 5, N'WAITING', N'CAL_INT', @ReindexFireTimeTicks, NULL, NULL, 0, NULL)

PRINT(N'Add 1 row to [dbo].[Scheduling_SIMPROP_TRIGGERS]')
INSERT INTO [dbo].[Scheduling_SIMPROP_TRIGGERS] ([SCHED_NAME], [TRIGGER_NAME], [TRIGGER_GROUP], [STR_PROP_1], [STR_PROP_2], [STR_PROP_3], [INT_PROP_1], [INT_PROP_2], [LONG_PROP_1], [LONG_PROP_2], [DEC_PROP_1], [DEC_PROP_2], [BOOL_PROP_1], [BOOL_PROP_2])
VALUES (N'QuartzScheduler', @ActionID, N'DEFAULT', N'Day', dbo.GetLocalTimezoneName(), NULL, 7, 0, 0, 0, 0.0000, 0.0000, 0, 0)
GO

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
VALUES (CONVERT(BIGINT, @ActionID), N'ManageIndexState', CONVERT(xml, N'<Settings><DailyActionSchedule><ScheduleType>2</ScheduleType><StartDateTimeInUtc>2014-07-06T07:00:00Z</StartDateTimeInUtc><FrequencyInDays>1</FrequencyInDays></DailyActionSchedule><TimeoutInMinutes value="120" /><DaysBack>14</DaysBack><MinIndexUsage>100</MinIndexUsage></Settings>', 1), 0, -1, -1, 0, -1, 0, 0, 0)

PRINT(N'Add 1 row to [dbo].[Scheduling_TRIGGERS]')
INSERT INTO [dbo].[Scheduling_TRIGGERS] ([SCHED_NAME], [TRIGGER_NAME], [TRIGGER_GROUP], [JOB_NAME], [JOB_GROUP], [DESCRIPTION], [NEXT_FIRE_TIME], [PREV_FIRE_TIME], [PRIORITY], [TRIGGER_STATE], [TRIGGER_TYPE], [START_TIME], [END_TIME], [CALENDAR_NAME], [MISFIRE_INSTR], [JOB_DATA])
VALUES (N'QuartzScheduler', @ActionID, N'DEFAULT', @ActionID, N'DEFAULT', NULL, @ManageIndexFireTimeTicks, NULL, 5, N'WAITING', N'CAL_INT', @ManageIndexFireTimeTicks, NULL, NULL, 0, NULL)

PRINT(N'Add 1 row to [dbo].[Scheduling_SIMPROP_TRIGGERS]')
INSERT INTO [dbo].[Scheduling_SIMPROP_TRIGGERS] ([SCHED_NAME], [TRIGGER_NAME], [TRIGGER_GROUP], [STR_PROP_1], [STR_PROP_2], [STR_PROP_3], [INT_PROP_1], [INT_PROP_2], [LONG_PROP_1], [LONG_PROP_2], [DEC_PROP_1], [DEC_PROP_2], [BOOL_PROP_1], [BOOL_PROP_2])
VALUES (N'QuartzScheduler', @ActionID, N'DEFAULT', N'Day', dbo.GetLocalTimezoneName(), NULL, 1, 0, 0, 0, 0.0000, 0.0000, 0, 0)
GO

-- Create default best rate profile
INSERT INTO [dbo].[ShippingProfile] ([Name], [ShipmentType], [ShipmentTypePrimary], [OriginID], [Insurance], [InsuranceInitialValueSource], [InsuranceInitialValueAmount], [ReturnShipment])
VALUES ('Defaults - Best rate', 14, 1, 0, 0, 0, 0.00, 0)
GO

INSERT INTO [dbo].[BestRateProfile] ([ShippingProfileID], [ServiceLevel])
SELECT TOP 1 ShippingProfileID, 0  FROM ShippingProfile WHERE ShipmentType = 14
GO

INSERT INTO [dbo].[PackageProfile] (ShippingProfileID, [Weight], DimsProfileID, DimsLength, DimsWidth, DimsHeight, DimsWeight, DimsAddWeight)
SELECT TOP 1 ShippingProfileID, 0, 0, 0, 0, 0, 0, 0 FROM ShippingProfile WHERE ShipmentType = 14

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

PRINT (N'Add Initial AmazonSFPServiceTypes')
GO
INSERT INTO dbo.AmazonSFPServiceType
(ApiValue, Description, PlatformApiCode)
VALUES
('USPS_PTP_FC', 'USPS First Class Mail', 'amazon_usps_first_class_mail'),
('USPS_PTP_MM', 'USPS Media Mail', 'amazon_usps_media_mail'),
('USPS_PTP_PRI', 'USPS Priority Mail', 'amazon_usps_priority_mail_package'),
('USPS_PTP_PRI_SFRB', 'USPS Priority Mail Small Flat Rate Box', 'amazon_usps_priority_mail_small_flat_rate_box'),
('USPS_PTP_PRI_MFRB', 'USPS Priority Mail Medium Flat Rate Box', 'amazon_usps_priority_mail_medium_flat_rate_box'),
('USPS_PTP_PRI_LFRB', 'USPS Priority Mail Large Flat Rate Box', 'amazon_usps_priority_mail_large_flat_rate_box'),
('USPS_PTP_PRI_FRE', 'USPS Priority Mail Flat Rate Envelope', 'amazon_usps_priority_mail_flat_rate_envelope'),
('USPS_PTP_PRI_LFRE', 'USPS Priority Mail Legal Flat Rate Envelope', 'amazon_usps_priority_mail_legal_flat_rate_envelope'),
('USPS_PTP_PRI_PFRE', 'USPS Priority Mail Padded Flat Rate Envelope', 'amazon_usps_priority_mail_padded_flat_rate_envelope'),
('USPS_PTP_PRI_RA', 'USPS Priority Mail Regional Rate Box A', 'amazon_usps_priority_mail_regional_rate_box_a'),
('USPS_PTP_PRI_RB', 'USPS Priority Mail Regional Rate Box B', 'amazon_usps_priority_mail_regional_rate_box_b'),
('USPS_PTP_EXP', 'USPS Priority Mail Express', 'amazon_stamps_usps_priority_mail_express_package'),
('USPS_PTP_EXP_FRE', 'USPS Priority Mail Express Flat Rate Envelope', 'amazon_usps_priority_mail_express_flat_rate_envelope'),
('USPS_PTP_EXP_LFRE', 'USPS Priority Mail Express Legal Flat Rate Envelope', 'amazon_usps_priority_mail_express_legal_flat_rate_envelope'),
('USPS_PTP_EXP_PFRE', 'USPS Priority Mail Express Padded Flat Rate Envelope', 'amazon_usps_priority_mail_express_padded_flat_rate_envelope'),
('USPS_PTP_PSBN', 'USPS Parcel Select', 'amazon_usps_parcel_select'),
('STAMPS_US_PMI_PACKAGE', 'USPS Priority Mail International', 'amazon_stamps_usps_priority_mail_international_package'),
('STAMPS_US_PMI_FRB', 'USPS Priority Mail International Flat Rate Box', 'amazon_stamps_usps_priority_mail_international_flat_rate_box'),
('STAMPS_US_PMI_LFRB', 'USPS Priority Mail International Large Flat Rate B', 'amazon_stamps_usps_priority_mail_international_large_flat_rate_box'),
('STAMPS_US_EMI_PACKAGE', 'USPS Priority Mail Express International', 'amazon_stamps_usps_priority_mail_express_international_package'),
('FEDEX_PTP_GROUND', 'FedEx Ground®', 'amazon_fedex_ground'),
('FEDEX_PTP_HOME_DELIVERY', 'FedEx Home Delivery®', 'amazon_fedex_home_delivery'),
('FEDEX_PTP_SECOND_DAY', 'FedEx 2Day®', 'amazon_fedex_2day'),
('FEDEX_PTP_SECOND_DAY_AM', 'FedEx 2Day® A.M.', 'amazon_fedex_2day_am'),
('FEDEX_PTP_EXPRESS_SAVER', 'FedEx Express Saver®', 'amazon_fedex_express_saver'),
('FEDEX_PTP_STANDARD_OVERNIGHT', 'FedEx Standard Overnight®', 'amazon_fedex_standard_overnight'),
('FEDEX_PTP_PRIORITY_OVERNIGHT', 'FedEx Priority Overnight®', 'amazon_fedex_priority_overnight'),
('UPS_PTP_GND', 'UPS Ground', 'amazon_ups_ground'),
('UPS_PTP_3DAY_SELECT', 'UPS 3 Day Select', 'amazon_ups_3_day_select'),
('UPS_PTP_2ND_DAY_AIR', 'UPS 2nd Day Air', 'amazon_ups_2nd_day_air'),
('UPS_PTP_NEXT_DAY_AIR_SAVER', 'UPS Next Day Air Saver', 'amazon_ups_next_day_air_saver'),
('UPS_PTP_NEXT_DAY_AIR', 'UPS Next Day Air', 'amazon_ups_next_day_air'),
('FEDEX_PTP_EXPRESS_SAVER_ONE_RATE', 'FedEx Express Saver One Rate®', 'amazon_fedex_express_saver_one_rate'),
('FEDEX_PTP_PRIORITY_OVERNIGHT_ONE_RATE', 'FedEx Priority Overnight One Rate®', 'amazon_fedex_priority_overnight_rate_one'),
('FEDEX_PTP_STANDARD_OVERNIGHT_ONE_RATE', 'FedEx Standard Overnight One Rate®', 'amazon_fedex_standard_overnight_one_rate'),
('FEDEX_PTP_SECOND_DAY_ONE_RATE', 'FedEx Second Day One Rate®', 'amazon_fedex_second_day_one_rate'),
('FEDEX_PTP_SECOND_DAY_AM_ONE_RATE', 'FedEx Second Day AM One Rate®', 'amazon_fedex_second_day_am_one_rate'),
('ONTRAC_MFN_GROUND', 'OnTrac Ground', 'amazon_ontrac_ground'),
('USPS_PTP_PRI_CUBIC', 'USPS Priority Mail Cubic', 'amazon_usps_priority_mail_cubic'),
('DYNAMEX_PTP_RUSH', 'DYNAMEX Rush', 'amazon_dynamex_rush'),
('DYNAMEX_PTP_SAME', 'DYNAMEX Same Day', 'amazon_dynamex_sameday'),
('ROYAL_MFN_TRACKED_24_LTR', 'RM T24 Letter', 'amazon_rm_t24_ltr'),
('ROYAL_MFN_TRACKED_48_LTR', 'RM T48 Letter', 'amazon_rm_t48_ltr'),
('ROYAL_MFN_TRACKED_24', 'RM T24 Parcel', 'amazon_rm_t24_prcl'),
('ROYAL_MFN_TRACKED_48', 'RM T48 Parcel', 'amazon_rm_t48_prcl'),
('DPD_UK_MFN_PAK', 'DPD Next Day Expresspak', 'amazon_dpd_uk_expak'),
('DPD_UK_MFN_PAK_SAT', 'DPD Next Day Expresspak Saturday', 'amazon_dpd_uk_expak_sat'),
('DPD_UK_MFN_PARCEL', 'DPD Next Day Parcel', 'amazon_dpd_uk_parc'),
('DPD_UK_MFN_PARCEL_SAT', 'DPD Next Day Parcel Saturday', 'amazon_dpd_uk_parc_sat'),
('prime-premium-uk-mfn', 'Amazon Shipping Prime Premium', 'amazon_as_uk_prime_prem'),
('econ-uk-mfn', 'Amazon Shipping Economy', 'amazon_as_uk_econ'),
('UPS_PTP_NEXT_DAY_AIR_SAT', 'UPS Next Day Air® (Saturday)', 'amazon_ups_next_day_air_sat'),
('UPS_PTP_2ND_DAY_AIR_SAT', 'UPS 2nd Day Air® (Saturday)', 'amazon_ups_second_day_air_sat'),
('FEDEX_PTP_PRI_OVERNIGHT_SAT', 'FedEx Priority Overnight® (Saturday)', 'amazon_fedex_priority_overnight_sat'),
('FEDEX_PTP_PRI_OVERN_ONE_R_SAT', 'FedEx Priority Overnight® One Rate (Saturday)', 'amazon_fedex_priority_overnight_one_rate_sat'),
('FEDEX_PTP_SECOND_DAY_SAT', 'FedEx 2Day® (Saturday)', 'amazon_fedex_two_day_sat'),
('FEDEX_PTP_SEC_DAY_ONE_RATE_SAT', 'Fedex 2Day® One Rate (Saturday)', 'amazon_fedex_two_day_one_rate_sat')
GO

PRINT N'Adding Shortcuts'
GO
INSERT INTO Shortcut
(ModifierKeys, VirtualKey, Barcode, [Action])
VALUES
(3, 87, '', 0),
(0, 121, '-PL-', 3),
(null, null, '-TB-', 4),
(null, null, '-ES-', 5),
(null, null, '-CR-', 6),
(3, 65, '-AP-', 7),
(3, 70, '-FO-', 1),
(3, 67, '-CL-', 8),
(3, 80, '' , 9),
(null, null, '-NP-', 10),
(null, null, '-NS-', 11),
(null, null, '-UV-', 12);
GO