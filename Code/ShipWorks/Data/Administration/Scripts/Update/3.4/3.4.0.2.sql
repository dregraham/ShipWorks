SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
GO
CREATE TABLE [dbo].[Scheduler]
(
	[SchedulerID] [bigint] NOT NULL IDENTITY(1096, 1),
	[ComputerID] [bigint] NOT NULL,
	[LastStartDateTime] [datetime] NOT NULL,
	[LastStopDateTime] [datetime] NULL,
	[LastCheckInDateTime] [datetime] NOT NULL,
	[ServiceFullName] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ServiceDisplayName] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
GO
ALTER TABLE [dbo].[Scheduler] ADD CONSTRAINT [PK_Scheduler] PRIMARY KEY CLUSTERED  ([SchedulerID])
GO
GO
ALTER TABLE [dbo].[Scheduler] ADD CONSTRAINT [FK_Scheduler_Computer] FOREIGN KEY ([ComputerID]) REFERENCES [dbo].[Computer] ([ComputerID])
GO
