PRINT N'Altering [dbo].[ShippingSettings]'
GO
ALTER TABLE [dbo].[ShippingSettings] ADD
[ShipmentsLoaderEnsureFiltersLoadedTimeout] [int] NOT NULL CONSTRAINT [DF_ShippingSettings_ShipmentsLoaderEnsureFiltersLoadedTimeout] DEFAULT ((0))
GO
