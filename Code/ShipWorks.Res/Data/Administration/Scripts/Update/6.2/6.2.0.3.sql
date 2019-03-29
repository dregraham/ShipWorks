PRINT N'Altering [dbo].[OrderItem]'
GO
	UPDATE [dbo].[OrderItem] SET [HarmonizedCode] = '' WHERE [HarmonizedCode] IS NULL
GO
	ALTER TABLE [dbo].[OrderItem] ALTER COLUMN [HarmonizedCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
GO

PRINT N'Altering [dbo].[ShipmentCustomsItem]'
GO
	UPDATE [dbo].[ShipmentCustomsItem] SET [HarmonizedCode] = '' WHERE [HarmonizedCode] IS NULL
GO
	ALTER TABLE [dbo].[ShipmentCustomsItem] ALTER COLUMN [HarmonizedCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
GO

PRINT N'Adding constraints to [dbo].[BigCommerceOrderItem]'
GO
	IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE name = N'IsDigitalItem' AND object_id = OBJECT_ID(N'[dbo].[BigCommerceOrderItem]', 'U') AND default_object_id = OBJECT_ID(N'[dbo].[DF_BigCommerceOrderItem_IsDigitalItem]', 'D'))
		ALTER TABLE [dbo].[BigCommerceOrderItem] ADD CONSTRAINT [DF_BigCommerceOrderItem_IsDigitalItem] DEFAULT ((0)) FOR [IsDigitalItem]
GO

PRINT N'Creating extended properties'
GO
	IF NOT EXISTS (SELECT 1 FROM fn_listextendedproperty(N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'ThreeDCartStore', 'COLUMN', N'ApiUserKey'))
		EXEC sp_addextendedproperty N'AuditFormat', N'0', 'SCHEMA', N'dbo', 'TABLE', N'ThreeDCartStore', 'COLUMN', N'ApiUserKey'
GO
	IF NOT EXISTS (SELECT 1 FROM fn_listextendedproperty(N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'ThreeDCartStore', 'COLUMN', N'ApiUserKey'))
		EXEC sp_addextendedproperty N'AuditName', N'User Key', 'SCHEMA', N'dbo', 'TABLE', N'ThreeDCartStore', 'COLUMN', N'ApiUserKey'
GO
	IF NOT EXISTS (SELECT 1 FROM fn_listextendedproperty(N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'ThreeDCartStore', 'COLUMN', N'StoreUrl'))
		EXEC sp_addextendedproperty N'AuditFormat', N'0', 'SCHEMA', N'dbo', 'TABLE', N'ThreeDCartStore', 'COLUMN', N'StoreUrl'
GO
	IF NOT EXISTS (SELECT 1 FROM fn_listextendedproperty(N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'ThreeDCartStore', 'COLUMN', N'StoreUrl'))
		EXEC sp_addextendedproperty N'AuditName', N'Store URL', 'SCHEMA', N'dbo', 'TABLE', N'ThreeDCartStore', 'COLUMN', N'StoreUrl'
GO

DECLARE @sql nvarchar(500) = ''
SELECT @sql = [NAME] 
	FROM SYS.objects 
	WHERE parent_object_id = OBJECT_ID(N'[dbo].[FilterNodeContentDirty]', 'U') and TYPE='PK'

IF LEN(@sql) > 0
BEGIN
	PRINT N'Dropping constraints from [dbo].[FilterNodeContentDirty]'
	SET @sql = 'ALTER TABLE [dbo].[FilterNodeContentDirty] DROP CONSTRAINT [' + @sql + ']'
	select @sql
	EXEC (@sql)
END
GO

DECLARE @sql nvarchar(500) = ''
SELECT @sql = [NAME] 
	FROM SYS.objects 
	WHERE parent_object_id = OBJECT_ID(N'[dbo].[iParcelPackage]', 'U') and TYPE='PK'

IF LEN(@sql) > 0
BEGIN
	PRINT N'Dropping constraints from [dbo].[iParcelPackage]'
	SET @sql = 'ALTER TABLE [dbo].[iParcelPackage] DROP CONSTRAINT [' + @sql + ']'
	select @sql
	EXEC (@sql)
END
GO

DECLARE @sql nvarchar(500) = ''
SELECT @sql = OBJECT_NAME(default_object_id) FROM sys.columns 
	 WHERE name = N'ReuseControlConnectionSession' 
	 AND object_id = OBJECT_ID(N'[dbo].[FtpAccount]', 'U')

IF LEN(@sql) > 0
BEGIN
	PRINT N'Dropping constraints from [dbo].[FtpAccount]'
	SET @sql = 'ALTER TABLE [dbo].[FtpAccount] DROP CONSTRAINT [' + @sql + ']'
	select @sql
	EXEC (@sql)
END
GO

DECLARE @sql nvarchar(500) = ''
SELECT @sql = OBJECT_NAME(default_object_id) FROM sys.columns 
	 WHERE name = N'Url' 
	 AND object_id = OBJECT_ID(N'[dbo].[YahooOrderItem]', 'U')

IF LEN(@sql) > 0
BEGIN
	PRINT N'Dropping constraints from [dbo].[YahooOrderItem]'
	SET @sql = 'ALTER TABLE [dbo].[YahooOrderItem] DROP CONSTRAINT [' + @sql + ']'
	select @sql
	EXEC (@sql)
END
GO

DECLARE @sql nvarchar(500) = ''
SELECT @sql = OBJECT_NAME(default_object_id) FROM sys.columns 
	 WHERE name = N'YahooStoreID' 
	 AND object_id = OBJECT_ID(N'[dbo].[YahooStore]', 'U')

IF LEN(@sql) > 0
BEGIN
	PRINT N'Dropping constraints from [dbo].[YahooStore]'
	SET @sql = 'ALTER TABLE [dbo].[YahooStore] DROP CONSTRAINT [' + @sql + ']'
	select @sql
	EXEC (@sql)
END
GO

DECLARE @sql nvarchar(500) = ''
SELECT @sql = OBJECT_NAME(default_object_id) FROM sys.columns 
	 WHERE name = N'AccessToken' 
	 AND object_id = OBJECT_ID(N'[dbo].[YahooStore]', 'U')

IF LEN(@sql) > 0
BEGIN
	PRINT N'Dropping constraints from [dbo].[YahooStore]'
	SET @sql = 'ALTER TABLE [dbo].[YahooStore] DROP CONSTRAINT [' + @sql + ']'
	select @sql
	EXEC (@sql)
END
GO

PRINT N'Dropping index [IX_MagentoOrderID] from [dbo].[MagentoOrder]'
GO
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_MagentoOrderID' AND object_id = OBJECT_ID(N'[dbo].[MagentoOrder]'))
DROP INDEX [IX_MagentoOrderID] ON [dbo].[MagentoOrder]
GO
PRINT N'Creating primary key [PK_FilterNodeContentDirty] on [dbo].[FilterNodeContentDirty]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'PK_FilterNodeContentDirty' AND object_id = OBJECT_ID(N'[dbo].[FilterNodeContentDirty]'))
ALTER TABLE [dbo].[FilterNodeContentDirty] ADD CONSTRAINT [PK_FilterNodeContentDirty] PRIMARY KEY CLUSTERED  ([FilterNodeContentDirtyID])
GO
PRINT N'Creating primary key [PK_QuickFilterNodeUpdateCheckpoint] on [dbo].[QuickFilterNodeUpdateCheckpoint]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'PK_QuickFilterNodeUpdateCheckpoint' AND object_id = OBJECT_ID(N'[dbo].[QuickFilterNodeUpdateCheckpoint]'))
ALTER TABLE [dbo].[QuickFilterNodeUpdateCheckpoint] ADD CONSTRAINT [PK_QuickFilterNodeUpdateCheckpoint] PRIMARY KEY CLUSTERED  ([CheckpointID])
GO
PRINT N'Creating primary key [PK_ShippingSettings] on [dbo].[ShippingSettings]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'PK_ShippingSettings' AND object_id = OBJECT_ID(N'[dbo].[ShippingSettings]'))
ALTER TABLE [dbo].[ShippingSettings] ADD CONSTRAINT [PK_ShippingSettings] PRIMARY KEY CLUSTERED  ([ShippingSettingsID])
GO
PRINT N'Creating primary key [PK_iParcelPackage] on [dbo].[iParcelPackage]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'PK_iParcelPackage' AND object_id = OBJECT_ID(N'[dbo].[iParcelPackage]'))
ALTER TABLE [dbo].[iParcelPackage] ADD CONSTRAINT [PK_iParcelPackage] PRIMARY KEY CLUSTERED  ([iParcelPackageID])
GO
PRINT N'Creating index [IX_SWDefault_MagentoOrder_MagentoOrderID] on [dbo].[MagentoOrder]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_SWDefault_MagentoOrder_MagentoOrderID' AND object_id = OBJECT_ID(N'[dbo].[MagentoOrder]'))
CREATE NONCLUSTERED INDEX [IX_SWDefault_MagentoOrder_MagentoOrderID] ON [dbo].[MagentoOrder] ([MagentoOrderID])
GO
PRINT N'Creating index [IX_SWDefault_iParcelPackage_ShipmentID] on [dbo].[iParcelPackage]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_SWDefault_iParcelPackage_ShipmentID' AND object_id = OBJECT_ID(N'[dbo].[iParcelPackage]'))
CREATE NONCLUSTERED INDEX [IX_SWDefault_iParcelPackage_ShipmentID] ON [dbo].[iParcelPackage] ([ShipmentID])
GO
PRINT N'Adding constraints to [dbo].[FtpAccount]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE name = N'ReuseControlConnectionSession' AND object_id = OBJECT_ID(N'[dbo].[FtpAccount]', 'U') AND default_object_id = OBJECT_ID(N'[dbo].[DF_FtpAccount_ReuseControlConnectionSession]', 'D'))
ALTER TABLE [dbo].[FtpAccount] ADD CONSTRAINT [DF_FtpAccount_ReuseControlConnectionSession] DEFAULT ((0)) FOR [ReuseControlConnectionSession]
GO
PRINT N'Adding constraints to [dbo].[YahooOrderItem]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE name = N'Url' AND object_id = OBJECT_ID(N'[dbo].[YahooOrderItem]', 'U') AND default_object_id = OBJECT_ID(N'[dbo].[DF_YahooOrderItem_Url]', 'D'))
ALTER TABLE [dbo].[YahooOrderItem] ADD CONSTRAINT [DF_YahooOrderItem_Url] DEFAULT ('') FOR [Url]
GO
PRINT N'Adding constraints to [dbo].[YahooStore]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE name = N'YahooStoreID' AND object_id = OBJECT_ID(N'[dbo].[YahooStore]', 'U') AND default_object_id = OBJECT_ID(N'[dbo].[DF_YahooStore_YahooStoreID]', 'D'))
ALTER TABLE [dbo].[YahooStore] ADD CONSTRAINT [DF_YahooStore_YahooStoreID] DEFAULT ('') FOR [YahooStoreID]
GO
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE name = N'AccessToken' AND object_id = OBJECT_ID(N'[dbo].[YahooStore]', 'U') AND default_object_id = OBJECT_ID(N'[dbo].[DF_YahooStore_AccessToken]', 'D'))
ALTER TABLE [dbo].[YahooStore] ADD CONSTRAINT [DF_YahooStore_AccessToken] DEFAULT ('') FOR [AccessToken]
GO


