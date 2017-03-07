SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
IF OBJECT_ID(N'[dbo].[UserShortcutOverride]', 'U') IS NULL
CREATE TABLE [dbo].[UserShortcutOverride]
(
[UserShortcutOverrideID] [bigint] NOT NULL IDENTITY(1000, 1099),
[UserID] [bigint] NOT NULL,
[CommandType] [int] NOT NULL,
[Alt] [bit] NOT NULL CONSTRAINT [DF_UserShortcutOverride_Alt] DEFAULT ((0)),
[Ctrl] [bit] NOT NULL CONSTRAINT [DF_UserShortcutOverride_Ctrl] DEFAULT ((0)),
[Shift] [bit] NOT NULL CONSTRAINT [DF_UserShortcutOverride_Shift] DEFAULT ((0)),
[KeyValue] [nvarchar] (3) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'PK_UserShortcutOverride' AND object_id = OBJECT_ID(N'[dbo].[UserShortcutOverride]'))
ALTER TABLE [dbo].[UserShortcutOverride] ADD CONSTRAINT [PK_UserShortcutOverride] PRIMARY KEY CLUSTERED  ([UserShortcutOverrideID])
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_UniqueKeys' AND object_id = OBJECT_ID(N'[dbo].[UserShortcutOverride]'))
CREATE UNIQUE NONCLUSTERED INDEX [IX_UniqueKeys] ON [dbo].[UserShortcutOverride] ([KeyValue], [Alt], [Ctrl], [Shift], [CommandType])
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserShortcutOverride_User]', 'F') AND parent_object_id = OBJECT_ID(N'[dbo].[UserShortcutOverride]', 'U'))
ALTER TABLE [dbo].[UserShortcutOverride] ADD CONSTRAINT [FK_UserShortcutOverride_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([UserID])
GO
