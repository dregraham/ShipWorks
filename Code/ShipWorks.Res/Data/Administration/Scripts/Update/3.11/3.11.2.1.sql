DECLARE @StampsShipmentTypeCode INT = 3
DECLARE @UspsShipmentTypeCode INT = 15
DECLARE @IsUspsConfigured BIT = 0
DECLARE @IsStampsConfigured BIT = 0
DECLARE @IsStampsAndUspsExcluded BIT = 0
DECLARE @Excluded NVARCHAR(45) = ''

-- Check whether Usps is configured
DECLARE @UspsShipmentTypeCodeString VARCHAR(2)
SELECT @UspsShipmentTypeCodeString = CAST(@UspsShipmentTypeCode AS VARCHAR(2))

IF EXISTS(SELECT *
	FROM ShippingSettings 
	WHERE ',' + Configured + ',' LIKE '%,' + @UspsShipmentTypeCodeString + ',%')
BEGIN
	SET @IsUspsConfigured = 1
END

IF EXISTS(SELECT * 
	FROM ShippingSettings
	WHERE ',' + Excluded + ',' LIKE '%,' + @UspsShipmentTypeCodeString + ',%' AND
		  ',' + Excluded + ',' LIKE '%,' + CAST(@StampsShipmentTypeCode AS varchar(2)) + ',%')
BEGIN
	SET @IsStampsAndUspsExcluded = 1
END

-- Check whether Usps is configured
DECLARE @StampsShipmentTypeCodeString VARCHAR(2)
SELECT @StampsShipmentTypeCodeString = CAST(@StampsShipmentTypeCode AS VARCHAR(2))

IF EXISTS(SELECT *
	FROM ShippingSettings 
	WHERE ',' + Configured + ',' LIKE '%,' + @StampsShipmentTypeCodeString + ',%')
BEGIN
	SET @IsStampsConfigured = 1
END


-- Update the default shipment type when Stamps.com is the default. If Usps is configured, set it to Usps else set it to none
UPDATE ShippingSettings
	SET DefaultType = CASE WHEN DefaultType = @StampsShipmentTypeCode THEN @UspsShipmentTypeCode ELSE DefaultType END,
		Configured = Configured + CASE WHEN @IsStampsConfigured = 1 AND @IsUspsConfigured = 0 
										THEN ',' + @UspsShipmentTypeCodeString
										ELSE ''
									END
IF(@IsStampsAndUspsExcluded = 0)
BEGIN
	SELECT @Excluded = ',' + Excluded + ',' FROM ShippingSettings
	SELECT @Excluded = REPLACE(@Excluded, ',' + @UspsShipmentTypeCodeString + ',', ',')
	SELECT @Excluded = SUBSTRING(@Excluded, 2, LEN(@Excluded) - 2)
	Update ShippingSettings SET Excluded = @Excluded
END

UPDATE ScanFormBatch
	SET ShipmentType = @UspsShipmentTypeCode
	WHERE ShipmentType = @StampsShipmentTypeCode

UPDATE Shipment
	SET ShipmentType = @UspsShipmentTypeCode
	WHERE ShipmentType = @StampsShipmentTypeCode

UPDATE ShippingProviderRule
	SET ShipmentType = @UspsShipmentTypeCode
	WHERE ShipmentType = @StampsShipmentTypeCode

UPDATE UspsAccount
	SET UspsReseller = 0
	WHERE UspsReseller = 2

UPDATE AuditChangeDetail
	SET VariantOld = CASE WHEN VariantOld = @StampsShipmentTypeCode THEN @UspsShipmentTypeCode ELSE VariantOld END,
		VariantNew = CASE WHEN VariantNew = @StampsShipmentTypeCode THEN @UspsShipmentTypeCode ELSE VariantNew END
	WHERE DisplayFormat = 103

-- Update any filter reference to Stamps.com with Usps. We can't use the variables since a string literal is required.
-- We also have to loop because you cannot update multiple xml nodes in a single call.
WHILE EXISTS(SELECT *
				FROM Filter
				WHERE Definition IS NOT NULL 
					AND Definition.value('(//Item[@identifier="Shipment.ShipmentType"]/Value[@value="3"]/@value)[1]', 'int') IS NOT NULL)
BEGIN
	UPDATE Filter
		SET Definition.modify('replace value of (//Item[@identifier="Shipment.ShipmentType"]/Value[@value="3"]/@value)[1] with "15"')
		WHERE Definition IS NOT NULL
END

-- We don't need to loop on the Action table because it's a flat group of settings
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

	UPDATE [Action]
		SET InternalOwner = REPLACE(InternalOwner, 'Ship3-', 'Ship15-')
		WHERE InternalOwner LIKE 'Ship3-%'
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

	-- The USPS shipment type was already setup, so there will be duplicates that can 
	-- be deleted from the action table otherwise we'll get duplicate print jobs, emails, etc.
	DELETE FROM [ActionFilterTrigger]
		WHERE ActionId IN (SELECT ActionID FROM [Action] WHERE InternalOwner LIKE 'Ship3-%')

	DELETE FROM [ActionTask]
		WHERE ActionId IN (SELECT ActionID FROM [Action] WHERE InternalOwner LIKE 'Ship3-%')

	DELETE FROM [Action]
		WHERE InternalOwner LIKE 'Ship3-%'
	
END
