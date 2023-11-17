PRINT N'Altering [dbo].[FedExProfile]'
GO

IF COL_LENGTH(N'[dbo].[FedExProfile]', N'CustomsTinIssuingAuthority') IS NULL
BEGIN
    ALTER TABLE [dbo].[FedExProfile]
    ADD [CustomsTinIssuingAuthority] [nvarchar] (2) NULL
END
GO

PRINT N'Altering [dbo].[FedExShipment]'
GO

IF COL_LENGTH(N'[dbo].[FedExShipment]', N'CustomsTinIssuingAuthority') IS NULL
BEGIN
    ALTER TABLE [dbo].[FedExShipment]
    ADD [CustomsTinIssuingAuthority] [nvarchar] (2) NULL
END