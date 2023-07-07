PRINT N'Altering [dbo].[FedExShipment]'
GO

IF COL_LENGTH(N'[dbo].[FedExShipment]', N'DeliveredDutyPaid') IS NULL
BEGIN
    ALTER TABLE [dbo].[FedExShipment]
    ADD [DeliveredDutyPaid] [bit] NULL
END
GO