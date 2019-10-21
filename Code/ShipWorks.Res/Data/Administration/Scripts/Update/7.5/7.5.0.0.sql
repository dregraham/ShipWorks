PRINT N'Altering [dbo].[Shipment]'
GO
If(select COL_LENGTH('Shipment','CarrierAccountID')) IS NULL
ALTER TABLE UserSettings ADD CarrierAccountID BIGINT NULL
GO

