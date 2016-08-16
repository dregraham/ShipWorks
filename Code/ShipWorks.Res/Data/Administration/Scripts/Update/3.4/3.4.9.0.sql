BEGIN transaction
	exec sp_rename 'UpsShipment.[PaperlessInternational]', 'PaperlessAdditionalDocumentation'

	exec sp_rename 'UpsShipment.[CommercialInvoice]', 'CommercialPaperlessInvoice'

	exec sp_rename 'UpsProfile.[PaperlessInternational]', 'PaperlessAdditionalDocumentation'

	ALTER TABLE [dbo].[UpsProfile] ADD
	[CommercialPaperlessInvoice] [bit] NULL
	
	PRINT N'Updating primary UPS Profiles'
	GO
	UPDATE [dbo].UpsProfile
	SET 
		CommercialPaperlessInvoice = 0
	WHERE ShippingProfileID in (SELECT ShippingProfileID FROM ShippingProfile WHERE ShipmentType IN (0,1) AND ShipmentTypePrimary = 1)
	 
COMMIT