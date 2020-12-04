PRINT N'Altering [dbo].[ShippingSettings]'
GO
IF COL_LENGTH(N'[dbo].[ShippingSettings]', N'ShipEngineAccountID') IS NULL
ALTER TABLE [dbo].[ShippingSettings] ADD
    [ShipEngineAccountID] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_ShippingSettings_ShipEngineAccountID] DEFAULT ('')
GO
PRINT N'Dropping constraints from [dbo].[ShippingSettings]'
GO
IF EXISTS (SELECT 1 FROM sys.columns WHERE name = N'ShipEngineAccountID' AND object_id = OBJECT_ID(N'[dbo].[ShippingSettings]', 'U') AND default_object_id = OBJECT_ID(N'[dbo].[DF_ShippingSettings_ShipEngineAccountID]', 'D'))
ALTER TABLE [dbo].[ShippingSettings] DROP CONSTRAINT [DF_ShippingSettings_ShipEngineAccountID]
GO