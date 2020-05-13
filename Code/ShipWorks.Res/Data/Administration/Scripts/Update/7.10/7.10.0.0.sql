PRINT N'Altering [dbo].[ShippingSettings]'
GO
IF COL_LENGTH(N'[dbo].[ShippingSettings]', N'UpsAllowNoDims') IS NULL
ALTER TABLE [dbo].[ShippingSettings] ADD [UpsAllowNoDims] [bit] NOT NULL CONSTRAINT [DF_ShippingSettings_UpsAllowNoDims] DEFAULT ((0))
GO