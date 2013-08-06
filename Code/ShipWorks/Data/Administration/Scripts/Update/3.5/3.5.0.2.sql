SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
CREATE TABLE [dbo].[ServiceStatus]
(
	[ServiceStatusID] [bigint] NOT NULL IDENTITY(1096, 1000),
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
ALTER TABLE [dbo].[ServiceStatus] ADD CONSTRAINT [PK_ServiceStatus] PRIMARY KEY CLUSTERED  ([ServiceStatusID])
GO
ALTER TABLE [dbo].[ServiceStatus] ADD CONSTRAINT [IX_ServiceStatus] UNIQUE NONCLUSTERED  ([ComputerID], [ServiceType])
GO
ALTER TABLE [dbo].[ServiceStatus] ENABLE CHANGE_TRACKING
GO
ALTER TABLE [dbo].[ServiceStatus] ADD CONSTRAINT [FK_ServiceStatus_Computer] FOREIGN KEY ([ComputerID]) REFERENCES [dbo].[Computer] ([ComputerID])
GO
