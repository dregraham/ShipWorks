PRINT N'Altering [dbo].[InsurancePolicy]'
GO
IF COL_LENGTH(N'[dbo].[InsurancePolicy]', N'InsureShipStatus') IS NULL
    ALTER TABLE [dbo].InsurancePolicy ADD [InsureShipStatus] [nvarchar] (50) NULL
GO
