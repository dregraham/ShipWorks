
PRINT N'Altering InsurancePolicy'
GO

IF COL_LENGTH('InsurancePolicy', 'InsureShipPolicyID') IS NULL
BEGIN
	ALTER TABLE [dbo].[InsurancePolicy] ADD [InsureShipPolicyID] [bigint] NULL
END
GO

IF COL_LENGTH('InsurancePolicy', 'DateOfIssue') IS NULL
BEGIN
	ALTER TABLE [dbo].[InsurancePolicy] ADD [DateOfIssue] [datetime] NULL
END
GO

PRINT N'Altering Store'
GO

IF COL_LENGTH('Store', 'InsureShipClientID') IS NULL
BEGIN
	ALTER TABLE [dbo].[Store] ADD [InsureShipClientID] [bigint] NULL
END
GO

IF COL_LENGTH('Store', 'InsureShipApiKey') IS NULL
BEGIN
	ALTER TABLE [dbo].[Store] ADD [InsureShipApiKey] [nvarchar] (255) NULL
END
GO
