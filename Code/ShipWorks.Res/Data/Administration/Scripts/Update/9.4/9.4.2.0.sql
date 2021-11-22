PRINT N'Altering [dbo].[AsendiaShipment]'
GO
IF COL_LENGTH(N'[dbo].[AsendiaShipment]', N'CustomsRecipientTINType') IS NULL
ALTER TABLE [dbo].[AsendiaShipment] ADD [CustomsRecipientTINType] [int] NULL
GO
IF COL_LENGTH(N'[dbo].[AsendiaShipment]', N'CustomsRecipientTINType') IS NULL
ALTER TABLE [dbo].[AsendiaShipment] ADD [CustomsRecipientTINType] [nvarchar] (10) NULL
GO
IF COL_LENGTH(N'[dbo].[AsendiaShipment]', N'CustomsRecipientEntityType') IS NULL
ALTER TABLE [dbo].[AsendiaShipment] ADD [CustomsRecipientEntityType] [nvarchar] (24) NULL
GO
IF COL_LENGTH(N'[dbo].[AsendiaShipment]', N'CustomsRecipientIssuingAuthority') IS NULL
ALTER TABLE [dbo].[AsendiaShipment] ADD [CustomsRecipientIssuingAuthority] [nvarchar] (5) NULL

UPDATE asendiaShipment
SET asendiaShipment.[CustomsRecipientTINType] = 4 
FROM [AsendiaShipment] asendiaShipment
WHERE asendiaShipment.[CustomsRecipientTINType] IS NULL
GO

PRINT N'Altering [dbo].[AsendiaProfile]'
GO
IF COL_LENGTH(N'[dbo].[AsendiaProfile]', N'CustomsRecipientTIN') IS NULL
ALTER TABLE [dbo].[AsendiaProfile] ADD [CustomsRecipientTIN] [nvarchar] (24) NULL
GO
IF COL_LENGTH(N'[dbo].[AsendiaProfile]', N'CustomsRecipientTINType') IS NULL
ALTER TABLE [dbo].[AsendiaProfile] ADD [CustomsRecipientTINType] [nvarchar] (10) NULL
GO
IF COL_LENGTH(N'[dbo].[AsendiaProfile]', N'CustomsRecipientEntityType') IS NULL
ALTER TABLE [dbo].[AsendiaProfile] ADD [CustomsRecipientEntityType] [nvarchar] (24) NULL
GO
IF COL_LENGTH(N'[dbo].[AsendiaProfile]', N'CustomsRecipientIssuingAuthority') IS NULL
ALTER TABLE [dbo].[AsendiaProfile] ADD [CustomsRecipientIssuingAuthority] [nvarchar] (5) NULL
GO

UPDATE asendiaProfile
SET 
    asendiaProfile.[CustomsRecipientTINType] = 4 , 
    asendiaProfile.[CustomsRecipientTIN] = ''
FROM [AsendiaProfile] asendiaProfile
INNER JOIN ShippingProfile shipProfile
    ON asendiaProfile.ShippingProfileID = shipProfile.ShippingProfileID
WHERE 
    shipProfile.ShipmentTypePrimary = 1 
    AND asendiaProfile.[CustomsRecipientTINType] IS NULL 
    AND asendiaProfile.[CustomsRecipientTIN] IS NULL
GO