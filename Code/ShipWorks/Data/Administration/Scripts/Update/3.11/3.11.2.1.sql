DECLARE @StampsShipmentTypeCode INT = 3
DECLARE @UspsShipmentTypeCode INT = 15
DECLARE @IsUspsConfigured BIT

-- Check whether Usps is configured
DECLARE @UspsShipmentTypeCodeString VARCHAR(2)
SELECT @UspsShipmentTypeCodeString = CAST(@UspsShipmentTypeCode AS VARCHAR(2))

IF EXISTS(SELECT *
	FROM ShippingSettings 
	WHERE Configured LIKE '%,' + @UspsShipmentTypeCodeString + ',%'
		OR Configured LIKE '%,' + @UspsShipmentTypeCodeString
		OR Configured LIKE @UspsShipmentTypeCodeString + ',%'
		OR Configured = @UspsShipmentTypeCodeString)
BEGIN
	SET @IsUspsConfigured = 1
END

-- Update the default shipment type when Stamps.com is the default. If Usps is configured, set it to Usps else set it to none
UPDATE ShippingSettings
	SET DefaultType = CASE WHEN @IsUspsConfigured = 1 THEN @UspsShipmentTypeCode ELSE 0 END
	WHERE DefaultType = @StampsShipmentTypeCode

UPDATE ScanFormBatch 
	SET ShipmentType = @UspsShipmentTypeCode 
	WHERE ShipmentType = @StampsShipmentTypeCode

UPDATE Shipment 
	SET ShipmentType = @UspsShipmentTypeCode 
	WHERE ShipmentType = @StampsShipmentTypeCode

UPDATE ShippingProviderRule
	SET ShipmentType = @UspsShipmentTypeCode 
	WHERE ShipmentType = @StampsShipmentTypeCode

-- Update any filter reference to Stamps.com with Usps. We can't use the variables since a string literal is required
UPDATE Filter
	SET [Definition].modify('replace value of (//Item[@identifier="Shipment.ShipmentType"]/Value[@value="3"]/@value)[1] with "15"')
	WHERE [Definition] IS NOT NULL

UPDATE [Action]
	SET TriggerSettings.modify('replace value of (/Settings/ShipmentType[@value="3"]/@value)[1] with "15"')
	WHERE TriggerSettings IS NOT NULL

IF @IsUspsConfigured = 0
BEGIN
	-- Update Stamps.com rules and profiles to Usps, since Usps isn't configured
	UPDATE ShippingDefaultsRule
		SET ShipmentType = @UspsShipmentTypeCode 
		WHERE ShipmentType = @StampsShipmentTypeCode
		
	UPDATE ShippingPrintOutput
		SET ShipmentType = @UspsShipmentTypeCode 
		WHERE ShipmentType = @StampsShipmentTypeCode
		
	DELETE FROM ShippingProfile
		WHERE ShipmentType = @UspsShipmentTypeCode

	UPDATE ShippingProfile
		SET ShipmentType = @UspsShipmentTypeCode 
		WHERE ShipmentType = @StampsShipmentTypeCode
END
ELSE
BEGIN
	-- Since Usps IS configured, remove all the Stamps.com rules and profiles
	DELETE FROM ShippingDefaultsRule
		WHERE ShipmentType = @StampsShipmentTypeCode

	DELETE FROM ShippingPrintOutput
		WHERE ShipmentType = @StampsShipmentTypeCode

	DELETE FROM ShippingProfile
		WHERE ShipmentType = @StampsShipmentTypeCode
END
