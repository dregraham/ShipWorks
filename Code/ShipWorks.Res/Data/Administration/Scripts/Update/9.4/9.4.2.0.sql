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
ALTER TABLE [dbo].[AsendiaShipment] ADD [CustomsRecipientIssuingAuthority] [nvarchar] (2) NULL
GO

UPDATE asendiaShipment
SET asendiaShipment.[CustomsRecipientTinType] = 5,
    asendiaShipment.[CustomsRecipientEntityType] = 0, 
    asendiaShipment.[CustomsRecipientIssuingAuthority] = 'US', 
    asendiaShipment.[CustomsRecipientTin] = ''
FROM [AsendiaShipment] asendiaShipment
WHERE asendiaShipment.[CustomsRecipientTin] IS NULL
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
ALTER TABLE [dbo].[AsendiaProfile] ADD [CustomsRecipientIssuingAuthority] [nvarchar] (2) NULL
GO

UPDATE asendiaProfile
SET 
    asendiaProfile.[CustomsRecipientTinType] = 5,
    asendiaProfile.[CustomsRecipientEntityType] = 0, 
    asendiaProfile.[CustomsRecipientIssuingAuthority] = 'US' , 
    asendiaProfile.[CustomsRecipientTin] = ''
FROM [AsendiaProfile] asendiaProfile
INNER JOIN ShippingProfile shipProfile
    ON asendiaProfile.ShippingProfileID = shipProfile.ShippingProfileID
WHERE 
    shipProfile.ShipmentTypePrimary = 1 
    AND asendiaProfile.[CustomsRecipientTin] IS NULL
GO