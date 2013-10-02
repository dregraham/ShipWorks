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
