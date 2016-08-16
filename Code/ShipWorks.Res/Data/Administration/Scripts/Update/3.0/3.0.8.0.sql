SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
PRINT N'Dropping foreign keys from [dbo].[GenericFileStore]'
GO
ALTER TABLE [dbo].[GenericFileStore] DROP
CONSTRAINT [FK_GenericFileStore_Store]
GO
PRINT N'Dropping constraints from [dbo].[GenericFileStore]'
GO
ALTER TABLE [dbo].[GenericFileStore] DROP CONSTRAINT [PK_GenericFileStore]
GO
PRINT N'Dropping constraints from [dbo].[GenericFileStore]'
GO
ALTER TABLE [dbo].[GenericFileStore] DROP CONSTRAINT [DF_GenericFileStore_EmailIncomingFolder]
GO
PRINT N'Rebuilding [dbo].[GenericFileStore]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_GenericFileStore]
(
[StoreID] [bigint] NOT NULL,
[FileFormat] [int] NOT NULL,
[FileSource] [int] NOT NULL,
[DiskFolder] [nvarchar] (355) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FtpAccountID] [bigint] NULL,
[FtpFolder] [nvarchar] (355) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
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
INSERT INTO [dbo].[tmp_rg_xx_GenericFileStore]([StoreID], [FileFormat], [FileSource], [DiskFolder], [FtpAccountID], [FtpFolder], [EmailAccountID], [EmailFolder], [EmailFolderValidityID], [EmailFolderLastMessageID], [EmailOnlyUnread], [NamePatternMatch], [NamePatternSkip], [SuccessAction], [SuccessMoveFolder], [ErrorAction], [ErrorMoveFolder], [XmlXsltFileName], [XmlXsltContent], [FlatImportMap]) SELECT [StoreID], [FileFormat], [FileSource], [DiskFolder], NULL, [FtpFolder], [EmailAccountID], [EmailFolder], [EmailFolderValidityID], [EmailFolderLastMessageID], [EmailOnlyUnread], [NamePatternMatch], [NamePatternSkip], [SuccessAction], [SuccessMoveFolder], [ErrorAction], [ErrorMoveFolder], [XmlXsltFileName], [XmlXsltContent], [FlatImportMap] FROM [dbo].[GenericFileStore]
GO
DROP TABLE [dbo].[GenericFileStore]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_GenericFileStore]', N'GenericFileStore'
GO
PRINT N'Creating primary key [PK_GenericFileStore] on [dbo].[GenericFileStore]'
GO
ALTER TABLE [dbo].[GenericFileStore] ADD CONSTRAINT [PK_GenericFileStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Creating [dbo].[FtpAccount]'
GO
CREATE TABLE [dbo].[FtpAccount]
(
[FtpAccountID] [bigint] NOT NULL IDENTITY(1071, 1000),
[Host] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Username] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Password] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Port] [int] NOT NULL,
[SecurityType] [int] NOT NULL,
[Passive] [bit] NOT NULL,
[InternalOwnerID] [bigint] NULL
)
GO
PRINT N'Creating primary key [PK_FtpAccount] on [dbo].[FtpAccount]'
GO
ALTER TABLE [dbo].[FtpAccount] ADD CONSTRAINT [PK_FtpAccount] PRIMARY KEY CLUSTERED  ([FtpAccountID])
GO
ALTER TABLE [dbo].[FtpAccount] ENABLE CHANGE_TRACKING
GO
PRINT N'Adding foreign keys to [dbo].[GenericFileStore]'
GO
ALTER TABLE [dbo].[GenericFileStore] ADD
CONSTRAINT [FK_GenericFileStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO

/* ChannelAdivsor */
PRINT N'Dropping foreign keys from [dbo].[ChannelAdvisorOrderItem]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrderItem] DROP
CONSTRAINT [FK_ChannelAdvisorOrderItem_OrderItem]
GO
PRINT N'Dropping constraints from [dbo].[ChannelAdvisorOrderItem]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrderItem] DROP CONSTRAINT [PK_ChannelAdvisorOrderItem]
GO
PRINT N'Rebuilding [dbo].[ChannelAdvisorOrderItem]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_ChannelAdvisorOrderItem]
(
[OrderItemID] [bigint] NOT NULL,
[SiteName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BuyerID] [nvarchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SalesSourceID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Classification] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DistributionCenter] [nvarchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[HarmonizedCode] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
-- manually edited to default Harmonnized code to blank
INSERT INTO [dbo].[tmp_rg_xx_ChannelAdvisorOrderItem]([OrderItemID], [SiteName], [BuyerID], [SalesSourceID], [Classification], [DistributionCenter], [HarmonizedCode]) SELECT [OrderItemID], [SiteName], [BuyerID], [SalesSourceID], [Classification], [DistributionCenter], '' FROM [dbo].[ChannelAdvisorOrderItem]
GO
DROP TABLE [dbo].[ChannelAdvisorOrderItem]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_ChannelAdvisorOrderItem]', N'ChannelAdvisorOrderItem'
GO
PRINT N'Creating primary key [PK_ChannelAdvisorOrderItem] on [dbo].[ChannelAdvisorOrderItem]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrderItem] ADD CONSTRAINT [PK_ChannelAdvisorOrderItem] PRIMARY KEY CLUSTERED  ([OrderItemID])
GO
PRINT N'Adding foreign keys to [dbo].[ChannelAdvisorOrderItem]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrderItem] ADD
CONSTRAINT [FK_ChannelAdvisorOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO

