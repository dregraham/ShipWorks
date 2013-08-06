﻿

-- Add the new ActionQueueType to the ActionQueue table, defaulting to UI type of 0
ALTER TABLE [dbo].[ActionQueue] ADD
[ActionQueueType] [int] NOT NULL CONSTRAINT [DF_ActionQueue_ActionQueueType] DEFAULT ((0))
GO


/*
Create the Quartz schema tables for job scheduling.
*/

SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Creating [dbo].[Scheduling_TRIGGERS]'
GO
CREATE TABLE [dbo].[Scheduling_TRIGGERS]
(
[SCHED_NAME] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TRIGGER_NAME] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TRIGGER_GROUP] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[JOB_NAME] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[JOB_GROUP] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DESCRIPTION] [nvarchar] (250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[NEXT_FIRE_TIME] [bigint] NULL,
[PREV_FIRE_TIME] [bigint] NULL,
[PRIORITY] [int] NULL,
[TRIGGER_STATE] [nvarchar] (16) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TRIGGER_TYPE] [nvarchar] (8) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[START_TIME] [bigint] NOT NULL,
[END_TIME] [bigint] NULL,
[CALENDAR_NAME] [nvarchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[MISFIRE_INSTR] [int] NULL,
[JOB_DATA] [image] NULL
)
GO
PRINT N'Creating primary key [PK_Scheduling_TRIGGERS] on [dbo].[Scheduling_TRIGGERS]'
GO
ALTER TABLE [dbo].[Scheduling_TRIGGERS] ADD CONSTRAINT [PK_Scheduling_TRIGGERS] PRIMARY KEY CLUSTERED  ([SCHED_NAME], [TRIGGER_NAME], [TRIGGER_GROUP])
GO
PRINT N'Creating index [IDX_Scheduling_T_C] on [dbo].[Scheduling_TRIGGERS]'
GO
CREATE NONCLUSTERED INDEX [IDX_Scheduling_T_C] ON [dbo].[Scheduling_TRIGGERS] ([SCHED_NAME], [CALENDAR_NAME])
GO
PRINT N'Creating index [IDX_Scheduling_T_JG] on [dbo].[Scheduling_TRIGGERS]'
GO
CREATE NONCLUSTERED INDEX [IDX_Scheduling_T_JG] ON [dbo].[Scheduling_TRIGGERS] ([SCHED_NAME], [JOB_GROUP])
GO
PRINT N'Creating index [IDX_Scheduling_T_J] on [dbo].[Scheduling_TRIGGERS]'
GO
CREATE NONCLUSTERED INDEX [IDX_Scheduling_T_J] ON [dbo].[Scheduling_TRIGGERS] ([SCHED_NAME], [JOB_NAME], [JOB_GROUP])
GO
PRINT N'Creating index [IDX_Scheduling_T_NFT_MISFIRE] on [dbo].[Scheduling_TRIGGERS]'
GO
CREATE NONCLUSTERED INDEX [IDX_Scheduling_T_NFT_MISFIRE] ON [dbo].[Scheduling_TRIGGERS] ([SCHED_NAME], [MISFIRE_INSTR], [NEXT_FIRE_TIME])
GO
PRINT N'Creating index [IDX_Scheduling_T_NFT_ST_MISFIRE_GRP] on [dbo].[Scheduling_TRIGGERS]'
GO
CREATE NONCLUSTERED INDEX [IDX_Scheduling_T_NFT_ST_MISFIRE_GRP] ON [dbo].[Scheduling_TRIGGERS] ([SCHED_NAME], [MISFIRE_INSTR], [NEXT_FIRE_TIME], [TRIGGER_GROUP], [TRIGGER_STATE])
GO
PRINT N'Creating index [IDX_Scheduling_T_NFT_ST_MISFIRE] on [dbo].[Scheduling_TRIGGERS]'
GO
CREATE NONCLUSTERED INDEX [IDX_Scheduling_T_NFT_ST_MISFIRE] ON [dbo].[Scheduling_TRIGGERS] ([SCHED_NAME], [MISFIRE_INSTR], [NEXT_FIRE_TIME], [TRIGGER_STATE])
GO
PRINT N'Creating index [IDX_Scheduling_T_NEXT_FIRE_TIME] on [dbo].[Scheduling_TRIGGERS]'
GO
CREATE NONCLUSTERED INDEX [IDX_Scheduling_T_NEXT_FIRE_TIME] ON [dbo].[Scheduling_TRIGGERS] ([SCHED_NAME], [NEXT_FIRE_TIME])
GO
PRINT N'Creating index [IDX_Scheduling_T_G] on [dbo].[Scheduling_TRIGGERS]'
GO
CREATE NONCLUSTERED INDEX [IDX_Scheduling_T_G] ON [dbo].[Scheduling_TRIGGERS] ([SCHED_NAME], [TRIGGER_GROUP])
GO
PRINT N'Creating index [IDX_Scheduling_T_N_G_STATE] on [dbo].[Scheduling_TRIGGERS]'
GO
CREATE NONCLUSTERED INDEX [IDX_Scheduling_T_N_G_STATE] ON [dbo].[Scheduling_TRIGGERS] ([SCHED_NAME], [TRIGGER_GROUP], [TRIGGER_STATE])
GO
PRINT N'Creating index [IDX_Scheduling_T_N_STATE] on [dbo].[Scheduling_TRIGGERS]'
GO
CREATE NONCLUSTERED INDEX [IDX_Scheduling_T_N_STATE] ON [dbo].[Scheduling_TRIGGERS] ([SCHED_NAME], [TRIGGER_NAME], [TRIGGER_GROUP], [TRIGGER_STATE])
GO
PRINT N'Creating index [IDX_Scheduling_T_STATE] on [dbo].[Scheduling_TRIGGERS]'
GO
CREATE NONCLUSTERED INDEX [IDX_Scheduling_T_STATE] ON [dbo].[Scheduling_TRIGGERS] ([SCHED_NAME], [TRIGGER_STATE])
GO
PRINT N'Creating index [IDX_Scheduling_T_NFT_ST] on [dbo].[Scheduling_TRIGGERS]'
GO
CREATE NONCLUSTERED INDEX [IDX_Scheduling_T_NFT_ST] ON [dbo].[Scheduling_TRIGGERS] ([SCHED_NAME], [TRIGGER_STATE], [NEXT_FIRE_TIME])
GO
PRINT N'Creating [dbo].[Scheduling_CRON_TRIGGERS]'
GO
CREATE TABLE [dbo].[Scheduling_CRON_TRIGGERS]
(
[SCHED_NAME] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TRIGGER_NAME] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TRIGGER_GROUP] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CRON_EXPRESSION] [nvarchar] (120) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TIME_ZONE_ID] [nvarchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
PRINT N'Creating primary key [PK_Scheduling_CRON_TRIGGERS] on [dbo].[Scheduling_CRON_TRIGGERS]'
GO
ALTER TABLE [dbo].[Scheduling_CRON_TRIGGERS] ADD CONSTRAINT [PK_Scheduling_CRON_TRIGGERS] PRIMARY KEY CLUSTERED  ([SCHED_NAME], [TRIGGER_NAME], [TRIGGER_GROUP])
GO
PRINT N'Creating [dbo].[Scheduling_SIMPLE_TRIGGERS]'
GO
CREATE TABLE [dbo].[Scheduling_SIMPLE_TRIGGERS]
(
[SCHED_NAME] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TRIGGER_NAME] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TRIGGER_GROUP] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[REPEAT_COUNT] [int] NOT NULL,
[REPEAT_INTERVAL] [bigint] NOT NULL,
[TIMES_TRIGGERED] [int] NOT NULL
)
GO
PRINT N'Creating primary key [PK_Scheduling_SIMPLE_TRIGGERS] on [dbo].[Scheduling_SIMPLE_TRIGGERS]'
GO
ALTER TABLE [dbo].[Scheduling_SIMPLE_TRIGGERS] ADD CONSTRAINT [PK_Scheduling_SIMPLE_TRIGGERS] PRIMARY KEY CLUSTERED  ([SCHED_NAME], [TRIGGER_NAME], [TRIGGER_GROUP])
GO
PRINT N'Creating [dbo].[Scheduling_SIMPROP_TRIGGERS]'
GO
CREATE TABLE [dbo].[Scheduling_SIMPROP_TRIGGERS]
(
[SCHED_NAME] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TRIGGER_NAME] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TRIGGER_GROUP] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[STR_PROP_1] [nvarchar] (512) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[STR_PROP_2] [nvarchar] (512) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[STR_PROP_3] [nvarchar] (512) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[INT_PROP_1] [int] NULL,
[INT_PROP_2] [int] NULL,
[LONG_PROP_1] [bigint] NULL,
[LONG_PROP_2] [bigint] NULL,
[DEC_PROP_1] [numeric] (13, 4) NULL,
[DEC_PROP_2] [numeric] (13, 4) NULL,
[BOOL_PROP_1] [bit] NULL,
[BOOL_PROP_2] [bit] NULL
)
GO
PRINT N'Creating primary key [PK_Scheduling_SIMPROP_TRIGGERS] on [dbo].[Scheduling_SIMPROP_TRIGGERS]'
GO
ALTER TABLE [dbo].[Scheduling_SIMPROP_TRIGGERS] ADD CONSTRAINT [PK_Scheduling_SIMPROP_TRIGGERS] PRIMARY KEY CLUSTERED  ([SCHED_NAME], [TRIGGER_NAME], [TRIGGER_GROUP])
GO
PRINT N'Creating [dbo].[Scheduling_JOB_DETAILS]'
GO
CREATE TABLE [dbo].[Scheduling_JOB_DETAILS]
(
[SCHED_NAME] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[JOB_NAME] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[JOB_GROUP] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DESCRIPTION] [nvarchar] (250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[JOB_CLASS_NAME] [nvarchar] (250) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IS_DURABLE] [bit] NOT NULL,
[IS_NONCONCURRENT] [bit] NOT NULL,
[IS_UPDATE_DATA] [bit] NOT NULL,
[REQUESTS_RECOVERY] [bit] NOT NULL,
[JOB_DATA] [image] NULL
)
GO
PRINT N'Creating primary key [PK_Scheduling_JOB_DETAILS] on [dbo].[Scheduling_JOB_DETAILS]'
GO
ALTER TABLE [dbo].[Scheduling_JOB_DETAILS] ADD CONSTRAINT [PK_Scheduling_JOB_DETAILS] PRIMARY KEY CLUSTERED  ([SCHED_NAME], [JOB_NAME], [JOB_GROUP])
GO
PRINT N'Creating [dbo].[Scheduling_BLOB_TRIGGERS]'
GO
CREATE TABLE [dbo].[Scheduling_BLOB_TRIGGERS]
(
[SCHED_NAME] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TRIGGER_NAME] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TRIGGER_GROUP] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BLOB_DATA] [image] NULL
)
GO
PRINT N'Creating [dbo].[Scheduling_CALENDARS]'
GO
CREATE TABLE [dbo].[Scheduling_CALENDARS]
(
[SCHED_NAME] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CALENDAR_NAME] [nvarchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CALENDAR] [image] NOT NULL
)
GO
PRINT N'Creating primary key [PK_Scheduling_CALENDARS] on [dbo].[Scheduling_CALENDARS]'
GO
ALTER TABLE [dbo].[Scheduling_CALENDARS] ADD CONSTRAINT [PK_Scheduling_CALENDARS] PRIMARY KEY CLUSTERED  ([SCHED_NAME], [CALENDAR_NAME])
GO
PRINT N'Creating [dbo].[Scheduling_FIRED_TRIGGERS]'
GO
CREATE TABLE [dbo].[Scheduling_FIRED_TRIGGERS]
(
[SCHED_NAME] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ENTRY_ID] [nvarchar] (95) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TRIGGER_NAME] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TRIGGER_GROUP] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[INSTANCE_NAME] [nvarchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FIRED_TIME] [bigint] NOT NULL,
[PRIORITY] [int] NOT NULL,
[STATE] [nvarchar] (16) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[JOB_NAME] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[JOB_GROUP] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[IS_NONCONCURRENT] [bit] NULL,
[REQUESTS_RECOVERY] [bit] NULL
)
GO
PRINT N'Creating primary key [PK_Scheduling_FIRED_TRIGGERS] on [dbo].[Scheduling_FIRED_TRIGGERS]'
GO
ALTER TABLE [dbo].[Scheduling_FIRED_TRIGGERS] ADD CONSTRAINT [PK_Scheduling_FIRED_TRIGGERS] PRIMARY KEY CLUSTERED  ([SCHED_NAME], [ENTRY_ID])
GO
PRINT N'Creating index [IDX_Scheduling_FT_TRIG_INST_NAME] on [dbo].[Scheduling_FIRED_TRIGGERS]'
GO
CREATE NONCLUSTERED INDEX [IDX_Scheduling_FT_TRIG_INST_NAME] ON [dbo].[Scheduling_FIRED_TRIGGERS] ([SCHED_NAME], [INSTANCE_NAME])
GO
PRINT N'Creating index [IDX_Scheduling_FT_INST_JOB_REQ_RCVRY] on [dbo].[Scheduling_FIRED_TRIGGERS]'
GO
CREATE NONCLUSTERED INDEX [IDX_Scheduling_FT_INST_JOB_REQ_RCVRY] ON [dbo].[Scheduling_FIRED_TRIGGERS] ([SCHED_NAME], [INSTANCE_NAME], [REQUESTS_RECOVERY])
GO
PRINT N'Creating index [IDX_Scheduling_FT_JG] on [dbo].[Scheduling_FIRED_TRIGGERS]'
GO
CREATE NONCLUSTERED INDEX [IDX_Scheduling_FT_JG] ON [dbo].[Scheduling_FIRED_TRIGGERS] ([SCHED_NAME], [JOB_GROUP])
GO
PRINT N'Creating index [IDX_Scheduling_FT_J_G] on [dbo].[Scheduling_FIRED_TRIGGERS]'
GO
CREATE NONCLUSTERED INDEX [IDX_Scheduling_FT_J_G] ON [dbo].[Scheduling_FIRED_TRIGGERS] ([SCHED_NAME], [JOB_NAME], [JOB_GROUP])
GO
PRINT N'Creating index [IDX_Scheduling_FT_TG] on [dbo].[Scheduling_FIRED_TRIGGERS]'
GO
CREATE NONCLUSTERED INDEX [IDX_Scheduling_FT_TG] ON [dbo].[Scheduling_FIRED_TRIGGERS] ([SCHED_NAME], [TRIGGER_GROUP])
GO
PRINT N'Creating index [IDX_Scheduling_FT_T_G] on [dbo].[Scheduling_FIRED_TRIGGERS]'
GO
CREATE NONCLUSTERED INDEX [IDX_Scheduling_FT_T_G] ON [dbo].[Scheduling_FIRED_TRIGGERS] ([SCHED_NAME], [TRIGGER_NAME], [TRIGGER_GROUP])
GO
PRINT N'Creating [dbo].[Scheduling_LOCKS]'
GO
CREATE TABLE [dbo].[Scheduling_LOCKS]
(
[SCHED_NAME] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[LOCK_NAME] [nvarchar] (40) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_Scheduling_LOCKS] on [dbo].[Scheduling_LOCKS]'
GO
ALTER TABLE [dbo].[Scheduling_LOCKS] ADD CONSTRAINT [PK_Scheduling_LOCKS] PRIMARY KEY CLUSTERED  ([SCHED_NAME], [LOCK_NAME])
GO
PRINT N'Creating [dbo].[Scheduling_PAUSED_TRIGGER_GRPS]'
GO
CREATE TABLE [dbo].[Scheduling_PAUSED_TRIGGER_GRPS]
(
[SCHED_NAME] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TRIGGER_GROUP] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_Scheduling_PAUSED_TRIGGER_GRPS] on [dbo].[Scheduling_PAUSED_TRIGGER_GRPS]'
GO
ALTER TABLE [dbo].[Scheduling_PAUSED_TRIGGER_GRPS] ADD CONSTRAINT [PK_Scheduling_PAUSED_TRIGGER_GRPS] PRIMARY KEY CLUSTERED  ([SCHED_NAME], [TRIGGER_GROUP])
GO
PRINT N'Creating [dbo].[Scheduling_SCHEDULER_STATE]'
GO
CREATE TABLE [dbo].[Scheduling_SCHEDULER_STATE]
(
[SCHED_NAME] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[INSTANCE_NAME] [nvarchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[LAST_CHECKIN_TIME] [bigint] NOT NULL,
[CHECKIN_INTERVAL] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_Scheduling_SCHEDULER_STATE] on [dbo].[Scheduling_SCHEDULER_STATE]'
GO
ALTER TABLE [dbo].[Scheduling_SCHEDULER_STATE] ADD CONSTRAINT [PK_Scheduling_SCHEDULER_STATE] PRIMARY KEY CLUSTERED  ([SCHED_NAME], [INSTANCE_NAME])
GO
PRINT N'Adding foreign keys to [dbo].[Scheduling_CRON_TRIGGERS]'
GO
ALTER TABLE [dbo].[Scheduling_CRON_TRIGGERS] ADD CONSTRAINT [FK_Scheduling_CRON_TRIGGERS_Scheduling_TRIGGERS] FOREIGN KEY ([SCHED_NAME], [TRIGGER_NAME], [TRIGGER_GROUP]) REFERENCES [dbo].[Scheduling_TRIGGERS] ([SCHED_NAME], [TRIGGER_NAME], [TRIGGER_GROUP]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[Scheduling_TRIGGERS]'
GO
ALTER TABLE [dbo].[Scheduling_TRIGGERS] ADD CONSTRAINT [FK_Scheduling_TRIGGERS_Scheduling_JOB_DETAILS] FOREIGN KEY ([SCHED_NAME], [JOB_NAME], [JOB_GROUP]) REFERENCES [dbo].[Scheduling_JOB_DETAILS] ([SCHED_NAME], [JOB_NAME], [JOB_GROUP])
GO
PRINT N'Adding foreign keys to [dbo].[Scheduling_SIMPLE_TRIGGERS]'
GO
ALTER TABLE [dbo].[Scheduling_SIMPLE_TRIGGERS] ADD CONSTRAINT [FK_Scheduling_SIMPLE_TRIGGERS_Scheduling_TRIGGERS] FOREIGN KEY ([SCHED_NAME], [TRIGGER_NAME], [TRIGGER_GROUP]) REFERENCES [dbo].[Scheduling_TRIGGERS] ([SCHED_NAME], [TRIGGER_NAME], [TRIGGER_GROUP]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[Scheduling_SIMPROP_TRIGGERS]'
GO
ALTER TABLE [dbo].[Scheduling_SIMPROP_TRIGGERS] ADD CONSTRAINT [FK_Scheduling_SIMPROP_TRIGGERS_Scheduling_TRIGGERS] FOREIGN KEY ([SCHED_NAME], [TRIGGER_NAME], [TRIGGER_GROUP]) REFERENCES [dbo].[Scheduling_TRIGGERS] ([SCHED_NAME], [TRIGGER_NAME], [TRIGGER_GROUP]) ON DELETE CASCADE
GO
