UPDATE UpsShipment 
SET CustomsRecipientTIN='', CustomsRecipientTINType=0 
where CustomsRecipientTIN IS NULL
GO

Update PostalShipment 
SET CustomsRecipientTin=''
where CustomsRecipientTin IS NULL
GO

UPDATE DhlExpressProfile
SET DhlExpressProfile.CustomsTaxIdType = 5,
    DhlExpressProfile.CustomsRecipientTin = '',
    DhlExpressProfile.CustomsTinIssuingAuthority = 'US'
FROM DhlExpressProfile
         INNER JOIN ShippingProfile shipProfile
                    ON DhlExpressProfile.ShippingProfileID = shipProfile.ShippingProfileID
WHERE shipProfile.ShipmentTypePrimary = 1
  AND DhlExpressProfile.CustomsRecipientTin IS NULL
GO