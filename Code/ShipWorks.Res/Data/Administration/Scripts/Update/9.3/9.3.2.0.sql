PRINT N'Altering [dbo].[Shipment]'
GO
IF COL_LENGTH(N'[dbo].[Shipment]', N'IossTaxId') IS NULL
ALTER TABLE [dbo].[Shipment] ADD [IossTaxId] [nvarchar] (25) NULL
GO