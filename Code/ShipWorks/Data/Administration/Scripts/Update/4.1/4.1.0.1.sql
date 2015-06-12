SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Dropping constraints from [dbo].[Scheduling_FIRED_TRIGGERS]'
GO
ALTER TABLE [dbo].[Scheduling_FIRED_TRIGGERS] DROP CONSTRAINT [PK_Scheduling_FIRED_TRIGGERS]
GO
PRINT N'Dropping index [IDX_Scheduling_FT_TRIG_INST_NAME] from [dbo].[Scheduling_FIRED_TRIGGERS]'
GO
DROP INDEX [IDX_Scheduling_FT_TRIG_INST_NAME] ON [dbo].[Scheduling_FIRED_TRIGGERS]
GO
PRINT N'Dropping index [IDX_Scheduling_FT_INST_JOB_REQ_RCVRY] from [dbo].[Scheduling_FIRED_TRIGGERS]'
GO
DROP INDEX [IDX_Scheduling_FT_INST_JOB_REQ_RCVRY] ON [dbo].[Scheduling_FIRED_TRIGGERS]
GO
PRINT N'Dropping index [IDX_Scheduling_FT_JG] from [dbo].[Scheduling_FIRED_TRIGGERS]'
GO
DROP INDEX [IDX_Scheduling_FT_JG] ON [dbo].[Scheduling_FIRED_TRIGGERS]
GO
PRINT N'Dropping index [IDX_Scheduling_FT_J_G] from [dbo].[Scheduling_FIRED_TRIGGERS]'
GO
DROP INDEX [IDX_Scheduling_FT_J_G] ON [dbo].[Scheduling_FIRED_TRIGGERS]
GO
PRINT N'Dropping index [IDX_Scheduling_FT_TG] from [dbo].[Scheduling_FIRED_TRIGGERS]'
GO
DROP INDEX [IDX_Scheduling_FT_TG] ON [dbo].[Scheduling_FIRED_TRIGGERS]
GO
PRINT N'Dropping index [IDX_Scheduling_FT_T_G] from [dbo].[Scheduling_FIRED_TRIGGERS]'
GO
DROP INDEX [IDX_Scheduling_FT_T_G] ON [dbo].[Scheduling_FIRED_TRIGGERS]
GO
PRINT N'Rebuilding [dbo].[Scheduling_FIRED_TRIGGERS]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_Scheduling_FIRED_TRIGGERS]
(
[SCHED_NAME] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ENTRY_ID] [nvarchar] (95) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TRIGGER_NAME] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TRIGGER_GROUP] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[INSTANCE_NAME] [nvarchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FIRED_TIME] [bigint] NOT NULL,
[SCHED_TIME] [bigint] NOT NULL,
[PRIORITY] [int] NOT NULL,
[STATE] [nvarchar] (16) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[JOB_NAME] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[JOB_GROUP] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[IS_NONCONCURRENT] [bit] NULL,
[REQUESTS_RECOVERY] [bit] NULL
)
GO
INSERT INTO [dbo].[tmp_rg_xx_Scheduling_FIRED_TRIGGERS]([SCHED_NAME], [ENTRY_ID], [TRIGGER_NAME], [TRIGGER_GROUP], [INSTANCE_NAME], [FIRED_TIME], [PRIORITY], [STATE], [JOB_NAME], [JOB_GROUP], [IS_NONCONCURRENT], [REQUESTS_RECOVERY],
		SCHED_TIME) 
SELECT [SCHED_NAME], [ENTRY_ID], [TRIGGER_NAME], [TRIGGER_GROUP], [INSTANCE_NAME], [FIRED_TIME], [PRIORITY], [STATE], [JOB_NAME], [JOB_GROUP], [IS_NONCONCURRENT], [REQUESTS_RECOVERY],
		FIRED_TIME
FROM [dbo].[Scheduling_FIRED_TRIGGERS]
GO
DROP TABLE [dbo].[Scheduling_FIRED_TRIGGERS]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_Scheduling_FIRED_TRIGGERS]', N'Scheduling_FIRED_TRIGGERS'
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
PRINT N'Creating primary key [PK_Scheduling_BLOB_TRIGGERS] on [dbo].[Scheduling_BLOB_TRIGGERS]'
GO
ALTER TABLE [dbo].[Scheduling_BLOB_TRIGGERS] ADD CONSTRAINT [PK_Scheduling_BLOB_TRIGGERS] PRIMARY KEY CLUSTERED  ([SCHED_NAME], [TRIGGER_NAME], [TRIGGER_GROUP])
GO
