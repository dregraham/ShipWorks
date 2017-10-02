PRINT N'Altering [dbo].[ShippingSettings]'
GO
ALTER TABLE dbo.ShippingSettings ADD
	UspsShippingDateCutoffEnabled bit NOT NULL CONSTRAINT DF_ShippingSettings_UspsShippingDateCutoffEnabled DEFAULT 0,
	UspsShippingDateCutoffTime time(0) NOT NULL CONSTRAINT DF_ShippingSettings_UspsShippingDateCutoffTime DEFAULT '17:00'
GO
PRINT N'Dropping constraints from [dbo].[ShippingSettings]'
GO
ALTER TABLE dbo.ShippingSettings DROP CONSTRAINT DF_ShippingSettings_UspsShippingDateCutoffEnabled
GO
ALTER TABLE dbo.ShippingSettings DROP CONSTRAINT DF_ShippingSettings_UspsShippingDateCutoffTime
GO