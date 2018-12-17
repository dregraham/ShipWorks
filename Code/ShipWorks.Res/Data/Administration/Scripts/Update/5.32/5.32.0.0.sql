
PRINT N'Altering InsurancePolicy'
GO

IF COL_LENGTH('InsurancePolicy', 'InsureShipPolicyID') IS NULL
BEGIN
	ALTER TABLE [dbo].[InsurancePolicy] ADD [InsureShipPolicyID] [BIGINT] NULL
END
GO

IF COL_LENGTH('InsurancePolicy', 'DateOfIssue') IS NULL
BEGIN
	ALTER TABLE [dbo].[InsurancePolicy] ADD [DateOfIssue] [DATETIME] NULL
END
GO

PRINT N'Altering Store'
GO

IF COL_LENGTH('Store', 'InsureShipClientID') IS NULL
BEGIN
	ALTER TABLE [dbo].[Store] ADD [InsureShipClientID] [BIGINT] NULL
END
GO

IF COL_LENGTH('Store', 'InsureShipApiKey') IS NULL
BEGIN
	ALTER TABLE [dbo].[Store] ADD [InsureShipApiKey] NVARCHAR(255) NULL
END
GO
