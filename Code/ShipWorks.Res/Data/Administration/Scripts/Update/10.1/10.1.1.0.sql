PRINT N'Altering [dbo].[FedExProfile]'
GO

IF COL_LENGTH(N'[dbo].[FedExProfile]', N'DeliveredDutyPaid') IS NULL
BEGIN
    ALTER TABLE [dbo].[FedExProfile]
    ADD [DeliveredDutyPaid] [bit] NULL
END
GO

PRINT N'Altering [dbo].[FedExShipment]'
GO

IF COL_LENGTH(N'[dbo].[FedExShipment]', N'DeliveredDutyPaid') IS NULL
BEGIN
    ALTER TABLE [dbo].[FedExShipment]
    ADD [DeliveredDutyPaid] [bit] NULL
END
GO