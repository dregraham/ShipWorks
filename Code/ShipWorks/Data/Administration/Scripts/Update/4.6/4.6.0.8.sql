/*

PRINT N'Altering [dbo].[AmazonShipment]'
GO
ALTER TABLE [AmazonShipment]
DROP COLUMN AmazonAccountID

ALTER TABLE [AmazonShipment]
DROP COLUMN DateMustArriveBy

ALTER TABLE [AmazonShipment]
DROP CONSTRAINT DF_AmazonShipment_SendDateMustArriveBy

ALTER TABLE [AmazonShipment]
DROP COLUMN SendDateMustArriveBy

ALTER TABLE [AmazonShipment]
DROP CONSTRAINT DF_AmazonShipment_CarrierWillPickUp

ALTER TABLE [AmazonShipment]
DROP COLUMN CarrierWillPickup
GO

*/