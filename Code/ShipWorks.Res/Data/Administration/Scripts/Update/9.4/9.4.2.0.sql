PRINT N'Altering [dbo].[AsendiaShipment]'
GO
IF COL_LENGTH(N'[dbo].[AsendiaShipment]', N'CustomsRecipientTinType') IS NULL
ALTER TABLE [dbo].[AsendiaShipment] ADD [CustomsRecipientTinType] [int] NULL
GO
IF COL_LENGTH(N'[dbo].[AsendiaShipment]', N'CustomsRecipientTin') IS NULL
ALTER TABLE [dbo].[AsendiaShipment] ADD [CustomsRecipientTin] [nvarchar] (24) NULL
GO
IF COL_LENGTH(N'[dbo].[AsendiaShipment]', N'CustomsRecipientEntityType') IS NULL
ALTER TABLE [dbo].[AsendiaShipment] ADD [CustomsRecipientEntityType] [int] NULL
GO
IF COL_LENGTH(N'[dbo].[AsendiaShipment]', N'CustomsRecipientIssuingAuthority') IS NULL
ALTER TABLE [dbo].[AsendiaShipment] ADD [CustomsRecipientIssuingAuthority] [int] NULL

UPDATE asendiaShipment
SET asendiaShipment.[CustomsRecipientTinType] = 4 
FROM [AsendiaShipment] asendiaShipment
WHERE asendiaShipment.[CustomsRecipientTinType] IS NULL
GO

PRINT N'Altering [dbo].[AsendiaProfile]'
GO
IF COL_LENGTH(N'[dbo].[AsendiaProfile]', N'CustomsRecipientTinType') IS NULL
ALTER TABLE [dbo].[AsendiaProfile] ADD [CustomsRecipientTINType] [int] NULL
GO
IF COL_LENGTH(N'[dbo].[AsendiaProfile]', N'CustomsRecipientTin') IS NULL
ALTER TABLE [dbo].[AsendiaProfile] ADD [CustomsRecipientTin] [nvarchar] (24) NULL
GO
IF COL_LENGTH(N'[dbo].[AsendiaProfile]', N'CustomsRecipientEntityType') IS NULL
ALTER TABLE [dbo].[AsendiaProfile] ADD [CustomsRecipientEntityType] [int] NULL
GO
IF COL_LENGTH(N'[dbo].[AsendiaProfile]', N'CustomsRecipientIssuingAuthority') IS NULL
ALTER TABLE [dbo].[AsendiaProfile] ADD [CustomsRecipientIssuingAuthority] [int] NULL
GO

UPDATE asendiaProfile
SET 
    asendiaProfile.[CustomsRecipientTinType] = 4 , 
    asendiaProfile.[CustomsRecipientTin] = ''
FROM [AsendiaProfile] asendiaProfile
INNER JOIN ShippingProfile shipProfile
    ON asendiaProfile.ShippingProfileID = shipProfile.ShippingProfileID
WHERE 
    shipProfile.ShipmentTypePrimary = 1 
    AND asendiaProfile.[CustomsRecipientTinType] IS NULL 
    AND asendiaProfile.[CustomsRecipientTin] IS NULL
GO