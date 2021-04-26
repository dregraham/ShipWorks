PRINT N'Creating [dbo].[Device]'
GO
IF OBJECT_ID(N'[dbo].[Device]', 'U') IS NULL
CREATE TABLE [dbo].[Device]
(
[DeviceID] [bigint] NOT NULL IDENTITY(1106, 1000),
[ComputerID] [bigint] NOT NULL,
[Model] [smallint] NOT NULL,
[IPAddress] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PortNumber] [smallint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_Device] on [dbo].[Device]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PK_Device]', 'PK') AND parent_object_id = OBJECT_ID(N'[dbo].[Device]', 'U'))
ALTER TABLE [dbo].[Device] ADD CONSTRAINT [PK_Device] PRIMARY KEY CLUSTERED  ([DeviceID])
GO
PRINT N'Adding foreign keys to [dbo].[Device]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Device_Computer]','F') AND parent_object_id = OBJECT_ID(N'[dbo].[Device]', 'U'))
ALTER TABLE [dbo].[Device] ADD CONSTRAINT [FK_Device_Computer] FOREIGN KEY ([ComputerID]) REFERENCES [dbo].[Computer] ([ComputerID]) ON DELETE CASCADE
GO
