-- Add the new ScanFormBatchID column to the stamps and endicia shipment tables
ALTER TABLE dbo.StampsShipment ADD
	ScanFormBatchID bigint NULL
GO
ALTER TABLE dbo.StampsShipment SET (LOCK_ESCALATION = TABLE)
GO

ALTER TABLE dbo.EndiciaShipment ADD
	ScanFormBatchID bigint NULL
GO
ALTER TABLE dbo.EndiciaShipment SET (LOCK_ESCALATION = TABLE)
GO

-- Back-fill the batch ID for any existing shipment/scan form data
UPDATE stampsShipment
SET stampsShipment.ScanFormBatchID = StampsScanForm.ScanFormBatchID
FROM StampsShipment
INNER JOIN StampsScanForm
on stampsShipment.ScanFormID = StampsScanForm.StampsScanFormID

UPDATE endiciaShipment
SET endiciaShipment.ScanFormBatchID = EndiciaScanForm.ScanFormBatchID
FROM endiciaShipment
INNER JOIN EndiciaScanForm
on endiciaShipment.ScanFormID = EndiciaScanForm.EndiciaScanFormID

-- Now that the batch ID has been back-filled we can drop the scan
-- form ID column on the endicia and stamps shipment tables
ALTER TABLE dbo.EndiciaShipment
	DROP COLUMN ScanFormID
GO
ALTER TABLE dbo.EndiciaShipment SET (LOCK_ESCALATION = TABLE)
GO

ALTER TABLE dbo.StampsShipment
	DROP COLUMN ScanFormID
GO
ALTER TABLE dbo.StampsShipment SET (LOCK_ESCALATION = TABLE)
GO


-- Create the FK relation from the shipping tables to scan form batch
PRINT N'Adding foreign keys to [dbo].[EndiciaShipment]'
GO
ALTER TABLE [dbo].[EndiciaShipment] ADD CONSTRAINT [FK_EndiciaShipment_ScanFormBatch] FOREIGN KEY ([ScanFormBatchID]) REFERENCES [dbo].[ScanFormBatch] ([ScanFormBatchID])
GO
PRINT N'Adding foreign keys to [dbo].[StampsShipment]'
GO
ALTER TABLE [dbo].[StampsShipment] ADD CONSTRAINT [FK_StampsShipment_ScanFormBatch] FOREIGN KEY ([ScanFormBatchID]) REFERENCES [dbo].[ScanFormBatch] ([ScanFormBatchID])
GO

-- Drop the shipment count column on the individual scan form
-- tables since they will no longer be accurate due to the
-- behavior of the Express1 for Stamps.com API 
PRINT N'Altering [dbo].[EndiciaScanForm]'
GO
ALTER TABLE [dbo].[EndiciaScanForm] DROP
COLUMN [ShipmentCount]
GO
PRINT N'Altering [dbo].[StampsScanForm]'
GO
ALTER TABLE [dbo].[StampsScanForm] DROP
COLUMN [ShipmentCount]
GO