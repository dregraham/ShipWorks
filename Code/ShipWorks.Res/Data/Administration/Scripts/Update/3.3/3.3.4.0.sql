SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Altering [dbo].[ThreeDCartStore]'
GO
ALTER TABLE [dbo].[ThreeDCartStore] ADD
[DownloadModifiedNumberOfDaysBack] [int] NOT NULL CONSTRAINT [DF_ThreeDCartStore_DownloadModifiedNumberOfDaysBack] DEFAULT ((7))
GO
PRINT N'Altering [dbo].[BigCommerceStore]'
GO
ALTER TABLE [dbo].[BigCommerceStore] ADD
[WeightUnitOfMeasure] [int] NOT NULL CONSTRAINT [DF_BigCommerceStore_WeightUnitOfMeasure] DEFAULT ((0)),
[DownloadModifiedNumberOfDaysBack] [int] NOT NULL CONSTRAINT [DF_BigCommerceStore_DownloadModifiedNumberOfDaysBack] DEFAULT ((7))
GO


ALTER TABLE [dbo].[ThreeDCartStore] DROP CONSTRAINT [DF_ThreeDCartStore_DownloadModifiedNumberOfDaysBack]
GO

ALTER TABLE [dbo].[BigCommerceStore] DROP CONSTRAINT [DF_BigCommerceStore_WeightUnitOfMeasure]
GO

ALTER TABLE [dbo].[BigCommerceStore] DROP CONSTRAINT [DF_BigCommerceStore_DownloadModifiedNumberOfDaysBack]
GO


ALTER TABLE [dbo].[EmailAccount] ALTER COLUMN [OutgoingPassword] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
GO

ALTER TABLE [dbo].[EmailAccount] ALTER COLUMN [IncomingPassword] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
GO