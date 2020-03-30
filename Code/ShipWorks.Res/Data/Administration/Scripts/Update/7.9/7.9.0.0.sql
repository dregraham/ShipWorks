PRINT N'Altering [dbo].[ShippingSettings]'
GO
ALTER TABLE [dbo].[ShippingSettings] ADD
[SingleScanConfirmationMode] [int] NOT NULL CONSTRAINT [DF_ShippingSettings_SingleScanConfirmationMode] DEFAULT ((0))
GO