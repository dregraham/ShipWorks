update DhlExpressShipment
set CustomsRecipientTin='', CustomsTaxIdType=5, CustomsTinIssuingAuthority='US'
Where CustomsTinIssuingAuthority is null
GO