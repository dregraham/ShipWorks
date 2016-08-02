SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Altering [dbo].[ShippingSettings]'
GO
ALTER TABLE [dbo].[ShippingSettings] ADD
[ShipmentEditLimit] [int] NOT NULL CONSTRAINT [DF_ShippingSettings_ShipmentEditLimit] DEFAULT ((100000))
GO
PRINT N'Dropping constraints from [dbo].[[ShippingSettings]]'
GO
ALTER TABLE [dbo].[ShippingSettings] DROP CONSTRAINT [DF_ShippingSettings_ShipmentEditLimit]
GO