SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Dropping foreign keys from [dbo].[VolusionStore]'
GO
ALTER TABLE [dbo].[VolusionStore] DROP CONSTRAINT [FK_VolusionStore_Store]
GO
PRINT N'Dropping constraints from [dbo].[VolusionStore]'
GO
ALTER TABLE [dbo].[VolusionStore] DROP CONSTRAINT [PK_VolusionStore]
GO
PRINT N'Rebuilding [dbo].[VolusionStore]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_VolusionStore]
(
[StoreID] [bigint] NOT NULL,
[StoreUrl] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[WebUserName] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[WebPassword] [varchar] (70) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ApiPassword] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PaymentMethods] [xml] NOT NULL,
[ShipmentMethods] [xml] NOT NULL,
[DownloadOrderStatuses] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ServerTimeZone] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ServerTimeZoneDST] [bit] NOT NULL
)
GO
INSERT INTO [dbo].[tmp_rg_xx_VolusionStore]([StoreID], [StoreUrl], [WebUserName], [WebPassword], [ApiPassword], [PaymentMethods], [ShipmentMethods],[DownloadOrderStatuses] , [ServerTimeZone], [ServerTimeZoneDST]) 
SELECT [StoreID], [StoreUrl], [WebUserName], [WebPassword], [ApiPassword], [PaymentMethods], [ShipmentMethods], 'Ready to Ship', [ServerTimeZone], [ServerTimeZoneDST] 
FROM [dbo].[VolusionStore]
GO
DROP TABLE [dbo].[VolusionStore]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_VolusionStore]', N'VolusionStore'
GO
PRINT N'Creating primary key [PK_VolusionStore] on [dbo].[VolusionStore]'
GO
ALTER TABLE [dbo].[VolusionStore] ADD CONSTRAINT [PK_VolusionStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[VolusionStore]'
GO
ALTER TABLE [dbo].[VolusionStore] ADD CONSTRAINT [FK_VolusionStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
