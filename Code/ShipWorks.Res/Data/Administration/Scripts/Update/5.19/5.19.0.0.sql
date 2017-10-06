PRINT N'Altering [dbo].[ShippingSettings]'
GO
ALTER TABLE dbo.ShippingSettings ADD
	[ShipmentDateCutoffJson] [nvarchar] (1000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_ShippingSettings_ShipmentDateCutoffJson] DEFAULT ('')
GO
PRINT N'Dropping constraints from [dbo].[ShippingSettings]'
GO
ALTER TABLE dbo.ShippingSettings DROP CONSTRAINT DF_ShippingSettings_ShipmentDateCutoffJson
GO