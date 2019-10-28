PRINT N'Altering [dbo].[Shipment]'
GO
If(select COL_LENGTH('Shipment','CarrierAccount')) IS NULL
ALTER TABLE UserSettings ADD CarrierAccount NVARCHAR(25) NULL
GO

