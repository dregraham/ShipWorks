PRINT N'ALTERING [dbo].[BestRateProfile]'
GO
IF COL_LENGTH(N'[dbo].[BestRateProfile]', N'InternalAllowedCarrierAccounts') IS NULL
ALTER TABLE [dbo].[BestRateProfile] ADD [InternalAllowedCarrierAccounts] [varchar] (MAX) NULL
GO
PRINT N'ALTERING [dbo].[BestRateShipment]'
GO
IF COL_LENGTH(N'[dbo].[BestRateShipment]', N'InternalAllowedCarrierAccounts') IS NULL
ALTER TABLE [dbo].[BestRateShipment] ADD [InternalAllowedCarrierAccounts] [varchar] (MAX) NULL
GO