SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Dropping foreign keys from [dbo].[GenericModuleStore]'
GO
ALTER TABLE [dbo].[GenericModuleStore] DROP CONSTRAINT [FK_GenericModuleStore_Store]
GO
PRINT N'Dropping foreign keys from [dbo].[MagentoStore]'
GO
ALTER TABLE [dbo].[MagentoStore] DROP CONSTRAINT [FK_MagentoStore_GenericModuleStore]
GO
PRINT N'Dropping foreign keys from [dbo].[MivaStore]'
GO
ALTER TABLE [dbo].[MivaStore] DROP CONSTRAINT [FK_MivaStore_GenericModuleStore]
GO
PRINT N'Dropping constraints from [dbo].[GenericModuleStore]'
GO
ALTER TABLE [dbo].[GenericModuleStore] DROP CONSTRAINT [PK_GenericModuleStore]
GO
PRINT N'Rebuilding [dbo].[GenericModuleStore]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_GenericModuleStore]
(
[StoreID] [bigint] NOT NULL,
[ModuleUsername] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ModulePassword] [nvarchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ModuleUrl] [nvarchar] (350) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ModuleVersion] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ModulePlatform] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ModuleDeveloper] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ModuleOnlineStoreCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ModuleStatusCodes] [xml] NOT NULL,
[ModuleDownloadPageSize] [int] NOT NULL,
[ModuleRequestTimeout] [int] NOT NULL,
[ModuleDownloadStrategy] [int] NOT NULL,
[ModuleOnlineStatusSupport] [int] NOT NULL,
[ModuleOnlineStatusDataType] [int] NOT NULL,
[ModuleOnlineCustomerSupport] [bit] NOT NULL,
[ModuleOnlineCustomerDataType] [int] NOT NULL,
[ModuleOnlineShipmentDetails] [bit] NOT NULL,
[ModuleHttpExpect100Continue] [bit] NOT NULL,
[ModuleResponseEncoding] [int] NOT NULL,
[SchemaVersion] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
INSERT INTO [dbo].[tmp_rg_xx_GenericModuleStore]([StoreID], [ModuleUsername], [ModulePassword], [ModuleUrl], [ModuleVersion], [ModulePlatform], [ModuleDeveloper], [ModuleOnlineStoreCode], [ModuleStatusCodes], [ModuleDownloadPageSize], [ModuleRequestTimeout], [ModuleDownloadStrategy], [ModuleOnlineStatusSupport], [ModuleOnlineStatusDataType], [ModuleOnlineCustomerSupport], [ModuleOnlineCustomerDataType], [ModuleOnlineShipmentDetails], [ModuleHttpExpect100Continue], [ModuleResponseEncoding], [SchemaVersion]) SELECT [StoreID], [ModuleUsername], [ModulePassword], [ModuleUrl], [ModuleVersion], [ModulePlatform], [ModuleDeveloper], [ModuleOnlineStoreCode], [ModuleStatusCodes], [ModuleDownloadPageSize], [ModuleRequestTimeout], [ModuleDownloadStrategy], [ModuleOnlineStatusSupport], [ModuleOnlineStatusDataType], [ModuleOnlineCustomerSupport], [ModuleOnlineCustomerDataType], [ModuleOnlineShipmentDetails], [ModuleHttpExpect100Continue], [ModuleResponseEncoding], '1.0.0.0' FROM [dbo].[GenericModuleStore]
GO
DROP TABLE [dbo].[GenericModuleStore]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_GenericModuleStore]', N'GenericModuleStore'
GO
PRINT N'Creating primary key [PK_GenericModuleStore] on [dbo].[GenericModuleStore]'
GO
ALTER TABLE [dbo].[GenericModuleStore] ADD CONSTRAINT [PK_GenericModuleStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[GenericModuleStore]'
GO
ALTER TABLE [dbo].[GenericModuleStore] ADD CONSTRAINT [FK_GenericModuleStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[MagentoStore]'
GO
ALTER TABLE [dbo].[MagentoStore] ADD CONSTRAINT [FK_MagentoStore_GenericModuleStore] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[GenericModuleStore] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[MivaStore]'
GO
ALTER TABLE [dbo].[MivaStore] ADD CONSTRAINT [FK_MivaStore_GenericModuleStore] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[GenericModuleStore] ([StoreID])
GO