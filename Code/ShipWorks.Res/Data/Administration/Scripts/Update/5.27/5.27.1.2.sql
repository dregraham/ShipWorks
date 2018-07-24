PRINT N'Altering [dbo].[AmazonProfile]'
GO
IF COL_LENGTH(N'[dbo].[AmazonProfile]', N'Reference1') IS NULL
	ALTER TABLE [dbo].[AmazonProfile] ADD [Reference1] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
GO

PRINT N'Updating primary Amazon Profile'
GO
UPDATE [dbo].[AmazonProfile]
SET 
	[Reference1] = 'Order {//Order/Number}'
WHERE ShippingProfileID in (SELECT ShippingProfileID FROM ShippingProfile WHERE ShipmentType = 16 AND ShipmentTypePrimary = 1)

