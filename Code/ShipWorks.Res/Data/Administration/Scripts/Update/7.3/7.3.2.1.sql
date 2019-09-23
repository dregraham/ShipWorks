PRINT N'Altering [dbo].[Order]'
GO
ALTER TABLE [Order] ALTER COLUMN BillPhone NVARCHAR (35) NOT Null;
GO
PRINT N'Altering [dbo].[Shipment]'
GO
ALTER TABLE Shipment ALTER COLUMN ShipPhone NVARCHAR (35) not null
GO