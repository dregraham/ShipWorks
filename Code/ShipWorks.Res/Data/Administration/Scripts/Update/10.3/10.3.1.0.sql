PRINT N'Altering [dbo].[PostalShipment]'
GO

IF COL_LENGTH(N'[dbo].[PostalShipment]', N'InternalTransactionNumber') IS NULL
BEGIN
    ALTER TABLE [dbo].[PostalShipment]
    ADD [InternalTransactionNumber] [nvarchar] (25) NULL
END
GO

PRINT N'Altering [dbo].[PostalProfile]'
GO

IF COL_LENGTH(N'[dbo].[PostalProfile]', N'InternalTransactionNumber') IS NULL
BEGIN
    ALTER TABLE [dbo].[PostalProfile]
    ADD [InternalTransactionNumber] [nvarchar] (25) NULL
END
GO