PRINT N'Altering [dbo].[DhlExpressAccount]'
GO
IF COL_LENGTH(N'[dbo].[DhlExpressAccount]', N'UspsAccountId') IS NULL
BEGIN
    ALTER TABLE [dbo].[DhlExpressAccount]
        ADD [UspsAccountId] [bigint] NULL
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

IF COL_LENGTH(N'[dbo].[DhlExpressShipment]', N'ResidentialDelivery') IS NULL
BEGIN
    ALTER TABLE [dbo].[DhlExpressShipment]
        ADD [ResidentialDelivery] [bit] NOT NULL CONSTRAINT [DF_DhlExpressShipment_ResidentialDelivery] DEFAULT(0)
END
GO

PRINT N'Dropping constraints from [dbo].[DhlExpressShipment]'
GO
    IF EXISTS (SELECT 1 FROM sys.columns WHERE name = N'ResidentialDelivery' AND object_id = OBJECT_ID(N'[dbo].[DhlExpressShipment]', 'U') AND default_object_id = OBJECT_ID(N'[dbo].[DF_DhlExpressShipment_ResidentialDelivery]', 'D'))
        ALTER TABLE [dbo].[DhlExpressShipment] DROP CONSTRAINT [DF_DhlExpressShipment_ResidentialDelivery]
GO


IF COL_LENGTH(N'[dbo].[DhlExpressProfile]', N'ResidentialDelivery') IS NULL
BEGIN
    ALTER TABLE [dbo].[DhlExpressProfile]
        ADD [ResidentialDelivery] [bit] NULL
END
GO