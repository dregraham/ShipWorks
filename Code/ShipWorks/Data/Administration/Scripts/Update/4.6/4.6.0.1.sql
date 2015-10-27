SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Disabling Amazon shipping carrier in [dbo].[ShippingSettings] for existing customers'
GO
UPDATE [ShippingSettings] set [Excluded] = [Excluded] + ',16'
GO
