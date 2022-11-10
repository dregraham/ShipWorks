PRINT N'Altering [dbo].[ShipmentCustomsItem]'
GO
IF COL_LENGTH(N'[dbo].[ShipmentCustomsItem]', N'SKU') IS NULL
ALTER TABLE [dbo].[ShipmentCustomsItem] ADD[SKU] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_ShipmentCustomsItem_SKU] DEFAULT ('')
GO
PRINT N'Dropping constraints from [dbo].[ShipmentCustomsItem]'
GO
IF EXISTS (SELECT 1 FROM sys.columns WHERE name = N'SKU' AND object_id = OBJECT_ID(N'[dbo].[ShipmentCustomsItem]', 'U') AND default_object_id = OBJECT_ID(N'[dbo].[DF_ShipmentCustomsItem_SKU]', 'D'))
ALTER TABLE [dbo].[ShipmentCustomsItem] DROP CONSTRAINT [DF_ShipmentCustomsItem_SKU]
GO
PRINT N'Altering [dbo].[AsendiaAccount]'
GO
ALTER TABLE [dbo].[AsendiaAccount] ALTER COLUMN [AccountNumber] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
GO
