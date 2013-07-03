SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
CREATE TABLE [dbo].[WindowsService]
(
	[WindowsServiceID] [bigint] NOT NULL IDENTITY(1096, 1000),
	[RowVersion] [timestamp] NOT NULL,
	[ComputerID] [bigint] NOT NULL,
	[ServiceType] [int] NOT NULL,
	[LastStartDateTime] [datetime] NULL,
	[LastStopDateTime] [datetime] NULL,
	[LastCheckInDateTime] [datetime] NULL,
	[ServiceFullName] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ServiceDisplayName] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
ALTER TABLE [dbo].[WindowsService] ADD CONSTRAINT [PK_Scheduler] PRIMARY KEY CLUSTERED  ([WindowsServiceID])
GO
ALTER TABLE [dbo].[WindowsService] ADD CONSTRAINT [IX_WindowsService] UNIQUE NONCLUSTERED  ([ComputerID], [ServiceType])
GO
ALTER TABLE [dbo].[WindowsService] ENABLE CHANGE_TRACKING
GO
ALTER TABLE [dbo].[WindowsService] ADD CONSTRAINT [FK_Scheduler_Computer] FOREIGN KEY ([ComputerID]) REFERENCES [dbo].[Computer] ([ComputerID])
GO
