PRINT N'Altering [dbo].[DhlExpressAccount]'
GO
IF COL_LENGTH(N'[dbo].[DhlExpressAccount]', N'UspsAccountId') IS NULL
BEGIN
    ALTER TABLE [dbo].[DhlExpressAccount]
        ADD [UspsAccountId] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
END
GO
ALTER TABLE [dbo].[DhlExpressAccount]
    ALTER COLUMN [ShipEngineCarrierId] [nvarchar] (50) NULL
GO

PRINT N'Altering [dbo].[DhlExpressShipment]'
GO
ALTER TABLE [dbo].[DhlExpressShipment]
    ALTER COLUMN [ShipEngineLabelID] [nvarchar] (50) NULL
GO

IF COL_LENGTH(N'[dbo].[DhlExpressShipment]', N'IntegratorTransactionID') IS NULL
BEGIN
    ALTER TABLE [dbo].[DhlExpressShipment]
        ADD [IntegratorTransactionID] [uniqueidentifier] NULL
END
GO

IF COL_LENGTH(N'[dbo].[DhlExpressShipment]', N'StampsTransactionID') IS NULL
BEGIN
    ALTER TABLE [dbo].[DhlExpressShipment]
        ADD [StampsTransactionID] [uniqueidentifier] NULL
END
GO
