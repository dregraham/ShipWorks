SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Dropping foreign keys from [dbo].[ThreeDCartStore]'
GO
ALTER TABLE [dbo].[ThreeDCartStore] DROP CONSTRAINT [FK_ThreeDCartStore_Store]
GO
PRINT N'Dropping constraints from [dbo].[ThreeDCartStore]'
GO
ALTER TABLE [dbo].[ThreeDCartStore] DROP CONSTRAINT [PK_ThreeDCartStore]
GO
PRINT N'Creating [dbo].[ThreeDCartOrder]'
GO
CREATE TABLE [dbo].[ThreeDCartOrder]
(
[OrderID] [bigint] NOT NULL,
[ThreeDCartOrderID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_ThreeDCartOrder] on [dbo].[ThreeDCartOrder]'
GO
ALTER TABLE [dbo].[ThreeDCartOrder] ADD CONSTRAINT [PK_ThreeDCartOrder] PRIMARY KEY CLUSTERED  ([OrderID])
GO
PRINT N'Rebuilding [dbo].[ThreeDCartStore]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_ThreeDCartStore]
(
[StoreID] [bigint] NOT NULL,
[StoreUrl] [nvarchar] (110) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ApiUserKey] [nvarchar] (65) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TimeZoneID] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[StatusCodes] [xml] NULL,
[DownloadModifiedNumberOfDaysBack] [int] NOT NULL,
[RestUser] [bit] NOT NULL
)
GO
INSERT INTO [dbo].[tmp_rg_xx_ThreeDCartStore]([StoreID], [StoreUrl], [ApiUserKey], [TimeZoneID], [StatusCodes], [DownloadModifiedNumberOfDaysBack], [RestUser]) SELECT [StoreID], [StoreUrl], [ApiUserKey], [TimeZoneID], [StatusCodes], [DownloadModifiedNumberOfDaysBack], 0 FROM [dbo].[ThreeDCartStore]
GO
DROP TABLE [dbo].[ThreeDCartStore]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_ThreeDCartStore]', N'ThreeDCartStore'
GO
PRINT N'Creating primary key [PK_ThreeDCartStore] on [dbo].[ThreeDCartStore]'
GO
ALTER TABLE [dbo].[ThreeDCartStore] ADD CONSTRAINT [PK_ThreeDCartStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[ThreeDCartStore]'
GO
ALTER TABLE [dbo].[ThreeDCartStore] ADD CONSTRAINT [FK_ThreeDCartStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[ThreeDCartOrder]'
GO
ALTER TABLE [dbo].[ThreeDCartOrder] ADD CONSTRAINT [FK_ThreeDCartOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
