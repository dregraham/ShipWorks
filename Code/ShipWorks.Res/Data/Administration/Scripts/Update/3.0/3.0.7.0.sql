SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
PRINT N'Disabling table change tracking'
GO
ALTER TABLE [dbo].[EmailAccount] DISABLE CHANGE_TRACKING
GO
PRINT N'Dropping foreign keys from [dbo].[GenericFileStore]'
GO
ALTER TABLE [dbo].[GenericFileStore] DROP
CONSTRAINT [FK_GenericFileStore_Store]
GO
PRINT N'Dropping constraints from [dbo].[EmailAccount]'
GO
ALTER TABLE [dbo].[EmailAccount] DROP CONSTRAINT [PK_EmailAccount]
GO
PRINT N'Dropping constraints from [dbo].[GenericFileStore]'
GO
ALTER TABLE [dbo].[GenericFileStore] DROP CONSTRAINT [PK_GenericFileStore]
GO
PRINT N'Rebuilding [dbo].[GenericFileStore]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_GenericFileStore]
(
[StoreID] [bigint] NOT NULL,
[FileFormat] [int] NOT NULL,
[FileSource] [int] NOT NULL,
[DiskFolder] [nvarchar] (355) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FtpFolder] [nvarchar] (355) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FtpUsername] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FtpPassword] [nvarchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[EmailAccountID] [bigint] NULL,
[EmailFolder] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_GenericFileStore_EmailIncomingFolder] DEFAULT (''),
[EmailFolderValidityID] [bigint] NOT NULL,
[EmailFolderLastMessageID] [bigint] NOT NULL,
[EmailOnlyUnread] [bit] NOT NULL,
[NamePatternMatch] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[NamePatternSkip] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[SuccessAction] [int] NOT NULL,
[SuccessMoveFolder] [nvarchar] (355) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ErrorAction] [int] NOT NULL,
[ErrorMoveFolder] [nvarchar] (355) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[XmlXsltFileName] [nvarchar] (355) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[XmlXsltContent] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[FlatImportMap] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
INSERT INTO [dbo].[tmp_rg_xx_GenericFileStore]([StoreID], [FileFormat], [FileSource], [DiskFolder], [FtpFolder], [FtpUsername], [FtpPassword], [EmailAccountID], [EmailFolder], [EmailFolderValidityID], [EmailFolderLastMessageID], [EmailOnlyUnread], [NamePatternMatch], [NamePatternSkip], [SuccessAction], [SuccessMoveFolder], [ErrorAction], [ErrorMoveFolder], [XmlXsltFileName], [XmlXsltContent], [FlatImportMap]) SELECT [StoreID], [FileFormat], [FileSource], [DiskFolder], [FtpFolder], [FtpUsername], [FtpPassword], [EmailAccountID], '', 0, 0, 1, [NamePatternMustMatch], [NamePatternCantMatch], [SuccessAction], [SuccessMoveFolder], [ErrorAction], [ErrorMoveFolder], [XmlXsltFileName], [XmlXsltContent], [FlatImportMap] FROM [dbo].[GenericFileStore]
GO
DROP TABLE [dbo].[GenericFileStore]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_GenericFileStore]', N'GenericFileStore'
GO
PRINT N'Creating primary key [PK_GenericFileStore] on [dbo].[GenericFileStore]'
GO
ALTER TABLE [dbo].[GenericFileStore] ADD CONSTRAINT [PK_GenericFileStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Rebuilding [dbo].[EmailAccount]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_EmailAccount]
(
[EmailAccountID] [bigint] NOT NULL IDENTITY(1034, 1000),
[RowVersion] [timestamp] NOT NULL,
[AccountName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DisplayName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[EmailAddress] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IncomingServer] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IncomingServerType] [int] NOT NULL,
[IncomingPort] [int] NOT NULL,
[IncomingSecurityType] [int] NOT NULL,
[IncomingUsername] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IncomingPassword] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OutgoingServer] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OutgoingPort] [int] NOT NULL,
[OutgoingSecurityType] [int] NOT NULL,
[OutgoingCredentialSource] [int] NOT NULL,
[OutgoingUsername] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OutgoingPassword] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AutoSend] [bit] NOT NULL,
[AutoSendMinutes] [int] NOT NULL,
[AutoSendLastTime] [datetime] NOT NULL,
[LimitMessagesPerConnection] [bit] NOT NULL,
[LimitMessagesPerConnectionQuantity] [int] NOT NULL,
[LimitMessagesPerHour] [bit] NOT NULL,
[LimitMessagesPerHourQuantity] [int] NOT NULL,
[LimitMessageInterval] [bit] NOT NULL,
[LimitMessageIntervalSeconds] [int] NOT NULL,
[InternalOwnerID] [bigint] NULL
)
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_EmailAccount] ON
GO
INSERT INTO [dbo].[tmp_rg_xx_EmailAccount]([EmailAccountID], [AccountName], [DisplayName], [EmailAddress], [IncomingServer], [IncomingServerType], [IncomingPort], [IncomingSecurityType], [IncomingUsername], [IncomingPassword], [OutgoingServer], [OutgoingPort], [OutgoingSecurityType], [OutgoingCredentialSource], [OutgoingUsername], [OutgoingPassword], [AutoSend], [AutoSendMinutes], [AutoSendLastTime], [LimitMessagesPerConnection], [LimitMessagesPerConnectionQuantity], [LimitMessagesPerHour], [LimitMessagesPerHourQuantity], [LimitMessageInterval], [LimitMessageIntervalSeconds], [InternalOwnerID]) SELECT [EmailAccountID], [AccountName], [DisplayName], [EmailAddress], [IncomingServer], 0, [IncomingPort], [IncomingSecurityType], [IncomingUsername], [IncomingPassword], [OutgoingServer], [OutgoingPort], [OutgoingSecurityType], [OutgoingCredentialSource], [OutgoingUsername], [OutgoingPassword], [AutoSend], [AutoSendMinutes], [AutoSendLastTime], [LimitMessagesPerConnection], [LimitMessagesPerConnectionQuantity], [LimitMessagesPerHour], [LimitMessagesPerHourQuantity], [LimitMessageInterval], [LimitMessageIntervalSeconds], [InternalOwnerID] FROM [dbo].[EmailAccount]
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_EmailAccount] OFF
GO
DECLARE @idVal BIGINT
SELECT @idVal = IDENT_CURRENT(N'[dbo].[EmailAccount]')
IF @idVal IS NOT NULL
    DBCC CHECKIDENT(N'[dbo].[tmp_rg_xx_EmailAccount]', RESEED, @idVal)
GO
DROP TABLE [dbo].[EmailAccount]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_EmailAccount]', N'EmailAccount'
GO
PRINT N'Creating primary key [PK_EmailAccount] on [dbo].[EmailAccount]'
GO
ALTER TABLE [dbo].[EmailAccount] ADD CONSTRAINT [PK_EmailAccount] PRIMARY KEY CLUSTERED  ([EmailAccountID])
GO
ALTER TABLE [dbo].[EmailAccount] ENABLE CHANGE_TRACKING
GO
PRINT N'Adding foreign keys to [dbo].[GenericFileStore]'
GO
ALTER TABLE [dbo].[GenericFileStore] ADD
CONSTRAINT [FK_GenericFileStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
