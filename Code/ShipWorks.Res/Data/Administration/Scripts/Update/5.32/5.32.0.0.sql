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
