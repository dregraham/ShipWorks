PRINT N'Altering [dbo].[WorldShipProcessed]'
GO
ALTER TABLE [dbo].[WorldShipProcessed] ADD
[ShipmentIdCalculated] AS (case when isnumeric([ShipmentID]+'.e0')=(1) then CONVERT([bigint],[ShipmentID],(0))  end) PERSISTED
GO
