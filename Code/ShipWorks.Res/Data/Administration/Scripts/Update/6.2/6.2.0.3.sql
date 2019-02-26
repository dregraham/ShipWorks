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



