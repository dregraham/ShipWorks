PRINT N'Altering [dbo].[ShippingSettings]'
GO
ALTER TABLE [dbo].[ShippingSettings] ALTER COLUMN [BestRateExcludedShipmentTypes] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
GO
