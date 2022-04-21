PRINT N'ALTERING [dbo].[DhlEcommerceShipment]'
GO
IF COL_LENGTH(N'[dbo].[DhlEcommerceShipment]', N'Insurance') IS NULL
	ALTER TABLE [dbo].[DhlEcommerceShipment] ADD [Insurance] [bit] NOT NULL CONSTRAINT [DF_DhlEcommerceShipment_Insurance] DEFAULT ((0))
GO
IF COL_LENGTH(N'[dbo].[DhlEcommerceShipment]', N'InsuranceValue') IS NULL
	ALTER TABLE [dbo].[DhlEcommerceShipment] ADD [InsuranceValue] [money] NOT NULL CONSTRAINT [DF_DhlEcommerceShipment_InsuranceValue] DEFAULT ((0)) 
GO
IF COL_LENGTH(N'[dbo].[DhlEcommerceShipment]', N'InsurancePennyOne') IS NULL
	ALTER TABLE [dbo].[DhlEcommerceShipment] ADD [InsurancePennyOne] [bit] NOT NULL CONSTRAINT [DF_DhlEcommerceShipment_InsurancePennyOne] DEFAULT ((0)) 
GO
