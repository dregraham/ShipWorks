PRINT N'Altering [dbo].[Order]'
GO
ALTER TABLE [Order] ALTER COLUMN BillPhone NVARCHAR (35) NOT NULL;
GO
PRINT N'Altering [dbo].[Shipment]'
GO
ALTER TABLE Shipment ALTER COLUMN ShipPhone NVARCHAR (35) NOT NULL
GO