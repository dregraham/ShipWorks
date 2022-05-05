PRINT N'ALTERING [dbo].[ShippingSettings]'
GO

IF COL_LENGTH(N'[dbo].[ShippingSettings]', N'DhlEcommerceInsuranceProvider') IS NULL
	ALTER TABLE [dbo].[ShippingSettings] ADD [DhlEcommerceInsuranceProvider] [int] NOT NULL CONSTRAINT [DF_ShippingSettings_DhlEcommerceInsuranceProvider] DEFAULT ((2))
GO

IF COL_LENGTH(N'[dbo].[ShippingSettings]', N'DhlEcommerceInsurancePennyOne') IS NULL
	ALTER TABLE [dbo].[ShippingSettings] ADD [DhlEcommerceInsurancePennyOne] [bit] NOT NULL CONSTRAINT [DF_ShippingSettings_DhlEcommerceInsurancePennyOne] DEFAULT ((0))
GO