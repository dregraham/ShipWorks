PRINT N'Altering [dbo].[Shipment]'
GO
IF COL_LENGTH(N'[dbo].[Shipment]', N'LoggedShippedToHub') IS NULL
    ALTER TABLE [dbo].[Shipment] ADD [LoggedShippedToHub] [bit] NULL
GO
IF COL_LENGTH(N'[dbo].[Shipment]', N'LoggedVoidToHub') IS NULL
    ALTER TABLE [dbo].[Shipment] ADD [LoggedVoidToHub] [bit] NULL
GO
