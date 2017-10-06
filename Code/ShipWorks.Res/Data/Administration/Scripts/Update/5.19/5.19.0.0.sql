PRINT N'Altering [dbo].[ShippingSettings]'
GO
ALTER TABLE dbo.ShippingSettings ADD
	UspsShippingDateCutoffEnabled bit NOT NULL CONSTRAINT DF_ShippingSettings_UspsShippingDateCutoffEnabled DEFAULT 0,
	UspsShippingDateCutoffTime time(0) NOT NULL CONSTRAINT DF_ShippingSettings_UspsShippingDateCutoffTime DEFAULT '17:00',
	[ShipmentDateCutoffJson] [nvarchar] (1000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_ShippingSettings_ShipmentDateCutoffJson] DEFAULT ('')
GO
PRINT N'Dropping constraints from [dbo].[ShippingSettings]'
GO
ALTER TABLE dbo.ShippingSettings DROP CONSTRAINT DF_ShippingSettings_UspsShippingDateCutoffEnabled
GO
ALTER TABLE dbo.ShippingSettings DROP CONSTRAINT DF_ShippingSettings_UspsShippingDateCutoffTime
GO
ALTER TABLE dbo.ShippingSettings DROP CONSTRAINT DF_ShippingSettings_ShipmentDateCutoffJson
GO