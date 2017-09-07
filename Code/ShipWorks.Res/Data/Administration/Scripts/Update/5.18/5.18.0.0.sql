PRINT N'Dropping constraints from [dbo].[AmazonShipment]'
GO
ALTER TABLE [dbo].[AmazonShipment] DROP CONSTRAINT [DF_AmazonShipment_ShippingServiceOfferId]
GO
PRINT N'Altering [dbo].[AmazonShipment]'
GO
ALTER TABLE [dbo].[AmazonShipment] DROP
COLUMN [ShippingServiceOfferID]
GO
PRINT N'Altering [dbo].[AmazonProfile]'
GO
ALTER TABLE [dbo].[AmazonProfile] ADD
[ShippingServiceID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
GO
PRINT N'Setting default value of ShippingServiceID'
GO
UPDATE ap
SET ap.ShippingServiceID = ''
FROM AmazonProfile ap 
INNER JOIN ShippingProfile sp ON ap.ShippingProfileID = sp.ShippingProfileID
WHERE sp.ShipmentTypePrimary = 1
GO