DECLARE
	@newProfileKey bigint,
	@upsShipperCount int,	
	@fedexShipperCount int,
	@endiciaShipperCount int,
	@frequentUpsAccountID bigint,
	@frequentFedexAccountID bigint,
	@frequentEndiciaAccountID bigint,
	@tempClientID int,
	@tempStoreID int

SELECT @upsShipperCount = COUNT(*) FROM UpsAccount;
SELECT @fedexShipperCount = COUNT(*) FROM FedexAccount;
SELECT @endiciaShipperCount = COUNT(*) FROM EndiciaAccount;

IF (@endiciaShipperCount > 0)
BEGIN

	-- Ensure variables are reset
	SELECT 
		@newProfileKey = NULL,
		@tempClientID = NULL,
		@tempStoreID = NULL

	SELECT @frequentEndiciaAccountID = EndiciaAccountId FROM
		(SELECT TOP 100 EndiciaAccountId
			FROM dbo.EndiciaShipment e, dbo.shipment s
			WHERE
				e.shipmentid = s.shipmentid
				AND s.processed = 1
				AND e.EndiciaAccountID > 0
			ORDER BY s.processeddate DESC) RecentShipments
	GROUP BY EndiciaAccountId
	ORDER BY COUNT(*) DESC

	IF (@frequentEndiciaAccountID IS NULL)
	BEGIN
		-- take the very last Endicia account in the system
		SELECT TOP 1 @frequentEndiciaAccountID = EndiciaAccountID FROM EndiciaAccount ORDER BY EndiciaAccountID DESC
	END

	-- now find the client to pull preferences from
	SELECT TOP(1) @tempClientID = ClientID, @tempStoreID = StoreID
	FROM v2m_EndiciaPreferences
	WHERE dbo.v2m_TranslateKey(DefaultShipperID, 12) = @frequentEndiciaAccountID

	-- no preferences row for the most frequently used recent shipper
	IF @tempClientID IS NULL
	BEGIN
			-- fallback to just picking the most rececent client/store/account preference that exists	
			SELECT TOP(1) @tempClientID = ClientID, @tempStoreID = StoreID, @frequentEndiciaAccountID = a.EndiciaAccountID
			FROM EndiciaAccount a, v2m_EndiciaPreferences p
			WHERE dbo.v2m_TranslateKey(p.DefaultShipperID, 12) = a.EndiciaAccountID
	END

	IF @tempClientID IS NOT NULL
	BEGIN
			-- source table variables
		DECLARE
		    @endPrefsClientID int, 
		    @endPrefsStoreID int, 
		    @endPrefsSetPackaging bit, 
		    @endPrefsSetService bit, 
		    @endPrefsSetConfirmation bit, 
		    @endPrefsSetWeight bit, 
		    @endPrefsSetDims bit, 
		    @endPrefsSetDate bit, 
		    @endPrefsSetInsurance bit, 
		    @endPrefsDefaultPackaging int, 
		    @endPrefsDefaultDomesticService int, 
		    @endPrefsDefaultInternationalService int, 
		    @endPrefsDefaultConfirmation int, 
		    @endPrefsDefaultDateAdvance int, 
		    @endPrefsDefaultInsuranceType int, 
		    @endPrefsDefaultLayouts nvarchar(max), 
		    @endPrefsDefaultStealthMode bit, 
		    @endPrefsDefaultOversize bit, 
		    @endPrefsDefaultNonMachinable bit, 
		    @endPrefsDefaultCustomsForm int, 
		    @endPrefsDefaultCustomsDescription varchar(200), 
		    @endPrefsDefaultCustomsContentType int, 
		    @endPrefsDefaultWidth float, 
		    @endPrefsDefaultLength float, 
		    @endPrefsDefaultDepth float, 
		    @endPrefsReferenceID varchar(200), 
		    @endPrefsRubberStamp1 varchar(200), 
		    @endPrefsRubberStamp2 varchar(200), 
		    @endPrefsRubberStamp3 varchar(200), 
		    @endPrefsRubberStamp4 varchar(200), 
		    @endPrefsTestMode bit, 
		    @endPrefsCloseOnComplete bit, 
		    @endPrefsAutoPrintCustoms bit, 
		    @endPrefsUnattendedPrinting bit, 
		    @endPrefsBlankRecipientPhone nvarchar(25), 
		    @endPrefsCustomsSigner nvarchar(100), 
		    @endPrefsCustomsCertify bit, 
		    @endPrefsDefaultShipperID int, 
		    @endPrefsUseShipperAddress bit, 
		    @endPrefsDefaultStandardPrinter nvarchar(350), 
		    @endPrefsDefaultIncludeThermal bit, 
		    @endPrefsDefaultThermalPrinter nvarchar(350) 
	    SELECT 
	        @endPrefsClientID = [ClientID],       
	        @endPrefsStoreID = [StoreID],       
	        @endPrefsSetPackaging = [SetPackaging],       
	        @endPrefsSetService = [SetService],       
	        @endPrefsSetConfirmation = [SetConfirmation],       
	        @endPrefsSetWeight = [SetWeight],       
	        @endPrefsSetDims = [SetDims],       
	        @endPrefsSetDate = [SetDate],       
	        @endPrefsSetInsurance = [SetInsurance],       
	        @endPrefsDefaultPackaging = [DefaultPackaging],       
	        @endPrefsDefaultDomesticService = [DefaultDomesticService],       
	        @endPrefsDefaultInternationalService = [DefaultInternationalService],       
	        @endPrefsDefaultConfirmation = [DefaultConfirmation],       
	        @endPrefsDefaultDateAdvance = [DefaultDateAdvance],       
	        @endPrefsDefaultInsuranceType = [DefaultInsuranceType],       
	        @endPrefsDefaultLayouts = [DefaultLayouts],       
	        @endPrefsDefaultStealthMode = [DefaultStealthMode],       
	        @endPrefsDefaultOversize = [DefaultOversize],       
	        @endPrefsDefaultNonMachinable = [DefaultNonMachinable],       
	        @endPrefsDefaultCustomsForm = [DefaultCustomsForm],       
	        @endPrefsDefaultCustomsDescription = [DefaultCustomsDescription],       
	        @endPrefsDefaultCustomsContentType = [DefaultCustomsContentType],       
	        @endPrefsDefaultWidth = [DefaultWidth],       
	        @endPrefsDefaultLength = [DefaultLength],       
	        @endPrefsDefaultDepth = [DefaultDepth],       
	        @endPrefsReferenceID = [ReferenceID],       
	        @endPrefsRubberStamp1 = [RubberStamp1],       
	        @endPrefsRubberStamp2 = [RubberStamp2],       
	        @endPrefsRubberStamp3 = [RubberStamp3],       
	        @endPrefsRubberStamp4 = [RubberStamp4],       
	        @endPrefsTestMode = [TestMode],       
	        @endPrefsCloseOnComplete = [CloseOnComplete],       
	        @endPrefsAutoPrintCustoms = [AutoPrintCustoms],       
	        @endPrefsUnattendedPrinting = [UnattendedPrinting],       
	        @endPrefsBlankRecipientPhone = [BlankRecipientPhone],       
	        @endPrefsCustomsSigner = [CustomsSigner],       
	        @endPrefsCustomsCertify = [CustomsCertify],       
	        @endPrefsDefaultShipperID = [DefaultShipperID],       
	        @endPrefsUseShipperAddress = [UseShipperAddress],       
	        @endPrefsDefaultStandardPrinter = [DefaultStandardPrinter],       
	        @endPrefsDefaultIncludeThermal = [DefaultIncludeThermal],       
	        @endPrefsDefaultThermalPrinter = [DefaultThermalPrinter]       
	    FROM dbo.[v2m_EndiciaPreferences]
	    WHERE [ClientID] = @tempClientID
			AND [StoreID] = @tempStoreID
			AND dbo.v2m_TranslateKey([DefaultShipperID], 12) = @frequentEndiciaAccountID

		-- create a shipping profile
		INSERT INTO dbo.ShippingProfile  
		(
			[Name],
			[ShipmentType],
			[ShipmentTypePrimary],
			[OriginID]
		)
		VALUES
		(
			'Defaults - USPS (Endicia)',
			2, 
			1, -- Primary
			0  -- Store
		)   

		SET @newProfileKey = @@IDENTITY

		-- create the Postal Profile
		INSERT INTO dbo.PostalProfile  (
	        [ShippingProfileID],
	        [Service],
	        [Confirmation],
	        [Weight],
	        [PackagingType],
	        [DimsProfileID],
	        [DimsLength],
	        [DimsWidth],
	        [DimsHeight],
	        [DimsWeight],
	        [DimsAddWeight],
	        [NonRectangular],
	        [NonMachinable],
	        [CustomsContentType],
	        [CustomsContentDescription],
	        [InsuranceType]
	    )
	    VALUES
	    (
	        @newProfileKey,
	        @endPrefsDefaultDomesticService,
	        @endPrefsDefaultConfirmation,
			0,	-- weight
	        dbo.v2m_TranslateUspsPackageTypeCode(@endPrefsDefaultPackaging),
			0, -- DimsProfileID
	        @endPrefsDefaultLength,
	        @endPrefsDefaultWidth,
	        @endPrefsDefaultDepth,
	        0,	-- DimsWeight
			1,	-- AddWeight
	        0,	-- NonRectangular
	        @endPrefsDefaultNonMachinable,
	        @endPrefsDefaultCustomsContentType,
	        @endPrefsDefaultCustomsDescription,
			CASE	-- Insurance
				WHEN @endPrefsDefaultInsuranceType = 4 THEN 1	-- Interapptive Insurance
				ELSE 0	-- none
			END
	    )

		-- endicia profile
	    INSERT INTO dbo.EndiciaProfile  
		(
			[ShippingProfileID],
	        [EndiciaAccountID],
	        [StealthPostage],
	        [NoPostage],
	        [ReferenceID],
	        [RubberStamp1],
	        [RubberStamp2],
	        [RubberStamp3]
	    )
	    VALUES
	    (
	        @newProfileKey,
	        @frequentEndiciaAccountID,
	        @endPrefsDefaultStealthMode,
	        0,	-- NoPostage
	        @endPrefsReferenceID,
	        @endPrefsRubberStamp1,
	        @endPrefsRubberStamp2,
	        @endPrefsRubberStamp3
	    )             

	END
END

IF (@upsShipperCount > 0)
BEGIN

		-- Ensure variables are reset
		SELECT 
			@newProfileKey = NULL,
			@tempClientID = NULL,
			@tempStoreID = NULL

		SELECT @frequentUpsAccountID = UpsAccountId FROM
			(SELECT TOP 100 UpsAccountId
				FROM dbo.upsshipment u, dbo.shipment s
				WHERE 
				u.shipmentid = s.shipmentid 
				AND s.processed = 1 
				AND u.UpsAccountId > 0   -- v2 shipper accounts that were deleted, would have been turned into 0s here
				ORDER BY s.processeddate DESC) RecentShipments
		GROUP BY UpsAccountId
		ORDER BY COUNT(*) desc

		IF (@frequentUpsAccountID IS NULL)
		BEGIN
			-- just take the very last Ups Account in the system
			SELECT TOP 1 @frequentUpsAccountID = UpsAccountID FROM UpsAccount ORDER BY UpsAccountID DESC
		END

		SELECT TOP(1) @tempClientID = ClientID, @tempStoreID = StoreID
		FROM v2m_UpsPreferences
		WHERE dbo.v2m_TranslateKey(DefaultShipperID, 13) = @frequentUpsAccountID

		-- no preferences row for the most frequently used recent shippers
		IF @tempClientID IS NULL
		BEGIN
			-- fallback to just picking the most rececent client/store/account preference that exists	
			SELECT TOP(1) @tempClientID = ClientID, @tempStoreID = StoreID, @frequentUpsAccountID = a.UpsAccountID
			FROM UpsAccount a, v2m_UpsPreferences p
			WHERE dbo.v2m_TranslateKey(p.DefaultShipperID, 13) = a.UpsAccountID
		END

		-- make sure there was a preferences row 
		IF @tempClientID IS NOT NULL
		BEGIN
			-- there was a Preferences row setup for this shipper.  Get the V2 preferences
			DECLARE
				@upsPrefsClientID int, 
				@upsPrefsStoreID int, 
				@upsPrefsDefaultShipperID int, 
				@upsPrefsPickupTypeCode nvarchar(10), 
				@upsPrefsDomesticServiceCode nvarchar(10), 
				@upsPrefsInternationalServiceCode nvarchar(10), 
				@upsPrefsPackagingTypeCode nvarchar(10), 
				@upsPrefsPackageLength int, 
				@upsPrefsPackageWidth int, 
				@upsPrefsPackageHeight int, 
				@upsPrefsPackageReferenceNumber nvarchar(100), 
				@upsPrefsAdditionalHandling bit, 
				@upsPrefsDeliveryConfirmation bit, 
				@upsPrefsDeliveryConfirmationType nvarchar(15), 
				@upsPrefsInsurance bit, 
				@upsPrefsInsuranceUPS bit, 
				@upsPrefsCOD bit, 
				@upsPrefsCODFundsCode nvarchar(10), 
				@upsPrefsCODAmount money, 
				@upsPrefsShipmentReferenceNumber nvarchar(100), 
				@upsPrefsEmailFromName nvarchar(50), 
				@upsPrefsEmailSubjectCode nvarchar(10), 
				@upsPrefsEmailFailedAddress nvarchar(50), 
				@upsPrefsEmailMemo nvarchar(120), 
				@upsPrefsAddShipToEmail bit, 
				@upsPrefsAddBillToEmail bit, 
				@upsPrefsAddArbitraryEmail bit, 
				@upsPrefsAddArbitraryEmailAddress nvarchar(50), 
				@upsPrefsShipNotify bit, 
				@upsPrefsDeliveryNotify bit, 
				@upsPrefsExceptionNotify bit, 
				@upsPrefsLabelTypeCode nvarchar(10), 
				@upsPrefsDefaultTemplate nvarchar(50), 
				@upsPrefsThermalPrinterName nvarchar(350), 
				@upsPrefsCommercialInvoiceCopies int, 
				@upsPrefsCommercialInvoiceTemplate nvarchar(50), 
				@upsPrefsBlankRecipientPhone nvarchar(25), 
				@upsPrefsDefaultToCommercial bit 

			SELECT 
				@upsPrefsClientID = [ClientID],       
				@upsPrefsStoreID = [StoreID],       
				@upsPrefsDefaultShipperID = [DefaultShipperID],       
				@upsPrefsPickupTypeCode = [PickupTypeCode],       
				@upsPrefsDomesticServiceCode = [DomesticServiceCode],       
				@upsPrefsInternationalServiceCode = [InternationalServiceCode],       
				@upsPrefsPackagingTypeCode = [PackagingTypeCode],       
				@upsPrefsPackageLength = [PackageLength],       
				@upsPrefsPackageWidth = [PackageWidth],       
				@upsPrefsPackageHeight = [PackageHeight],       
				@upsPrefsPackageReferenceNumber = [PackageReferenceNumber],       
				@upsPrefsAdditionalHandling = [AdditionalHandling],       
				@upsPrefsDeliveryConfirmation = [DeliveryConfirmation],       
				@upsPrefsDeliveryConfirmationType = [DeliveryConfirmationType],       
				@upsPrefsInsurance = [Insurance],       
				@upsPrefsInsuranceUPS = [InsuranceUPS],       
				@upsPrefsCOD = [COD],       
				@upsPrefsCODFundsCode = [CODFundsCode],       
				@upsPrefsCODAmount = [CODAmount],       
				@upsPrefsShipmentReferenceNumber = [ShipmentReferenceNumber],       
				@upsPrefsEmailFromName = [EmailFromName],       
				@upsPrefsEmailSubjectCode = [EmailSubjectCode],       
				@upsPrefsEmailFailedAddress = [EmailFailedAddress],       
				@upsPrefsEmailMemo = [EmailMemo],       
				@upsPrefsAddShipToEmail = [AddShipToEmail],       
				@upsPrefsAddBillToEmail = [AddBillToEmail],       
				@upsPrefsAddArbitraryEmail = [AddArbitraryEmail],       
				@upsPrefsAddArbitraryEmailAddress = [AddArbitraryEmailAddress],       
				@upsPrefsShipNotify = [ShipNotify],       
				@upsPrefsDeliveryNotify = [DeliveryNotify],       
				@upsPrefsExceptionNotify = [ExceptionNotify],       
				@upsPrefsLabelTypeCode = [LabelTypeCode],       
				@upsPrefsDefaultTemplate = [DefaultTemplate],       
				@upsPrefsThermalPrinterName = [ThermalPrinterName],       
				@upsPrefsCommercialInvoiceCopies = [CommercialInvoiceCopies],       
				@upsPrefsCommercialInvoiceTemplate = [CommercialInvoiceTemplate],       
				@upsPrefsBlankRecipientPhone = [BlankRecipientPhone],       
				@upsPrefsDefaultToCommercial = [DefaultToCommercial]       
			FROM dbo.[v2m_UpsPreferences]
			WHERE [ClientID] = @tempClientID
			AND [StoreID] = @tempStoreID
			AND dbo.v2m_TranslateKey([DefaultShipperID], 13) = @frequentUpsAccountID

			-- create a shipping profile
			INSERT INTO dbo.ShippingProfile  
			(
				[Name],
				[ShipmentType],
				[ShipmentTypePrimary],
				[OriginID]
			)
			VALUES
			(
				'Defaults - UPS',
				0, -- onlinetools = 1, worldship = 0 
				1, -- Primary
				2  -- account
			)             

			SET @newProfileKey = @@IDENTITY
			
			-- create a Ups Profile.  Maybe create 2, one demestic and one international
			INSERT INTO dbo.UpsProfile  (
				[ShippingProfileID],
				[UpsAccountID],
				[Service],
				[SaturdayDelivery],
				[ResidentialDetermination],
				[DeliveryConfirmation],
				[ReferenceNumber],
				[InsuranceType],
				[PayorType],
				[PayorAccount],
				[PayorPostalCode],
				[PayorCountryCode],
				[EmailNotifySender],
				[EmailNotifyRecipient],
				[EmailNotifyOther],
				[EmailNotifyOtherAddress],
				[EmailNotifyFrom],
				[EmailNotifySubject],
				[EmailNotifyMessage]
			)
			VALUES
			(
				@newProfileKey,
				@frequentUpsAccountID,
				dbo.v2m_TranslateUpsServiceCode(@upsPrefsDomesticServiceCode), -- service
				0, -- saturday delivery, not in v2
				CASE -- ResidentialDetermination
					WHEN @upsPrefsDefaultToCommercial = 1 THEN 2 -- commercial always
					ELSE 0	-- CommercialIfCompany, the default way of doing things in V2				
				END,
   				CASE -- Delivery Confirmation
					WHEN @upsPrefsDeliveryConfirmation = 0 THEN 0			-- None
					WHEN @upsPrefsDeliveryConfirmationType = '0' THEN 1	-- No Signature
					WHEN @upsPrefsDeliveryConfirmationType = '1' THEN 2	-- Required
					WHEN @upsPrefsDeliveryConfirmationType = '2' THEN 3	-- Adult
					ELSE 0
				END,
				@upsPrefsShipmentReferenceNumber, -- reference 
				CASE -- Insurance Type
					WHEN @upsPrefsInsurance = 0 THEN 0	-- no insurance
					WHEN @upsPrefsInsurance = 1 AND @upsPrefsInsuranceUPS = 0 THEN 1 -- ShipWorks insurance
					WHEN @upsPrefsInsurance = 1 AND @upsPrefsInsuranceUPS = 1 THEN 2 -- UPS insurance
					ELSE 0
				END,
				0, -- payor type = UpsPayorType.Sender, the defualt in V2
				'', -- payor account
				'', -- postal code
				'', -- payor country code
				0, -- email notify sender, skipping because v2 didn't do this
				(@upsPrefsAddShipToEmail | @upsPrefsAddBillToEmail) * (@upsPrefsShipNotify * 1 + @upsPrefsDeliveryNotify * 4 + @upsPrefsExceptionNotify * 2), -- email notify recipient
				(@upsPrefsAddArbitraryEmail) * (@upsPrefsShipNotify * 1 + @upsPrefsDeliveryNotify * 4 + @upsPrefsExceptionNotify * 2), -- email notify other
				@upsPrefsAddArbitraryEmailAddress, -- email notifiy other address
				@upsPrefsEmailFromName, -- email notify from
				CASE -- email notify subject
					WHEN @upsPrefsEmailSubjectCode = '' THEN 0
					WHEN @upsPrefsEmailSubjectCode = '01' THEN 1
					ELSE 0
				END,
				@upsPrefsEmailMemo -- email notify message
			)             

		END
END

IF (@fedexShipperCount > 0)
BEGIN
		-- Ensure variables are reset
		SELECT 
			@newProfileKey = NULL,
			@tempClientID = NULL,
			@tempStoreID = NULL

		SELECT @frequentFedexAccountID = FedexAccountId FROM
			(SELECT TOP 100 FedexAccountId
				FROM dbo.fedexshipment u, dbo.shipment s
				WHERE 
				u.shipmentid = s.shipmentid 
				AND s.processed = 1 
				AND u.FedexAccountId > 0   -- v2 shipper accounts that were deleted, would have been turned into 0s here
				ORDER BY s.processeddate DESC) RecentShipments
		GROUP BY FedexAccountID
		ORDER BY COUNT(*) desc
	
		IF (@frequentFedexAccountID IS NULL)
		BEGIN
			-- just take the very last Fedex Account in the system
			SELECT TOP 1 @frequentFedexAccountID = FedexAccountID FROM FedexAccount ORDER BY FedexAccountID DESC
		END

		SELECT TOP(1) @tempClientID = ClientID, @tempStoreID = StoreID
		FROM v2m_FedexPreferences
		WHERE dbo.v2m_TranslateKey(DefaultShipperID, 18) = @frequentFedexAccountID

		-- no preferences row for the most frequently used recent shippers
		IF @tempClientID IS NULL
		BEGIN
			-- fallback to just picking the most rececent client/store/account preference that exists	
			SELECT TOP(1) @tempClientID = ClientID, @tempStoreID = StoreID, @frequentFedexAccountID = a.FedexAccountID
			FROM FedexAccount a, v2m_FedexPreferences p
			WHERE dbo.v2m_TranslateKey(p.DefaultShipperID, 18) = a.FedexAccountID
		END

		IF @tempClientID IS NOT NULL
		BEGIN
			-- there was a Preferences row setup for this shipper.  Get the v2 preferences
			-- source table variables
			DECLARE
				@fedexPrefsClientID int, 
				@fedexPrefsStoreID int, 
				@fedexPrefsDefaultShipperID int, 
				@fedexPrefsDefaultDomesticService smallint, 
				@fedexPrefsDefaultInternationalService smallint, 
				@fedexPrefsDefaultTemplate nvarchar(50), 
				@fedexPrefsEplPrinterName nvarchar(350), 
				@fedexPrefsZplPrinterName nvarchar(350), 
				@fedexPrefsLabelType smallint, 
				@fedexPrefsThermalLabelDocTabType smallint, 
				@fedexPrefsReferenceNumber nvarchar(200), 
				@fedexPrefsPackagingType smallint, 
				@fedexPrefsPackagingLength int, 
				@fedexPrefsPackagingWidth int, 
				@fedexPrefsPackagingHeight int, 
				@fedexPrefsSignatureOption int, 
				@fedexPrefsPayorType smallint, 
				@fedexPrefsPayorAccountNumber nvarchar(12), 
				@fedexPrefsDutiesPayorType smallint, 
				@fedexPrefsDutiesPayorAccountNumber nvarchar(12), 
				@fedexPrefsShipAlertRecipientAddressUseBill bit, 
				@fedexPrefsShipAlertRecipientShip bit, 
				@fedexPrefsShipAlertRecipientDelivery bit, 
				@fedexPrefsShipAlertSenderShip bit, 
				@fedexPrefsShipAlertSenderDelivery bit, 
				@fedexPrefsShipAlertOther1Address nvarchar(120), 
				@fedexPrefsShipAlertOther1Ship bit, 
				@fedexPrefsShipAlertOther1Delivery bit, 
				@fedexPrefsShipAlertExpressMessage nvarchar(75), 
				@fedexPrefsShipAlertGroundEnable bit, 
				@fedexPrefsShipAlertGroundAddressUseBill bit, 
				@fedexPrefsShipAlertGroundMessage nvarchar(75), 
				@fedexPrefsCodEnable bit, 
				@fedexPrefsCodType int, 
				@fedexPrefsCodAddFreight bit, 
				@fedexPrefsCodUseShipperAddress bit, 
				@fedexPrefsCodReturnContactName nvarchar(35), 
				@fedexPrefsCodReturnCompany nvarchar(35), 
				@fedexPrefsCodReturnAddress1 nvarchar(35), 
				@fedexPrefsCodReturnAddress2 nvarchar(35), 
				@fedexPrefsCodReturnCity nvarchar(35), 
				@fedexPrefsCodReturnStateProvinceCode nvarchar(2), 
				@fedexPrefsCodReturnPostalCode nvarchar(16), 
				@fedexPrefsCodReturnPhone nvarchar(16), 
				@fedexPrefsBrokerEnable bit, 
				@fedexPrefsBrokerAccount nvarchar(12), 
				@fedexPrefsBrokerContactName nvarchar(35), 
				@fedexPrefsBrokerCompany nvarchar(35), 
				@fedexPrefsBrokerAddress1 nvarchar(35), 
				@fedexPrefsBrokerAddress2 nvarchar(35), 
				@fedexPrefsBrokerCity nvarchar(35), 
				@fedexPrefsBrokerStateProvinceCode nvarchar(2), 
				@fedexPrefsBrokerPostalCode nvarchar(16), 
				@fedexPrefsBrokerPhone nvarchar(16), 
				@fedexPrefsMaskAccountNumber bit, 
				@fedexPrefsBlankRecipientPhone nvarchar(25), 
				@fedexPrefsLimitDeclaredValue bit, 
				@fedexPrefsDefaultToCommercial bit, 
				@fedexPrefsCommercialInvoiceCopies int, 
				@fedexPrefsCommercialInvoiceTemplate nvarchar(50), 
				@fedexPrefsCodTemplate nvarchar(50) 

			SELECT 
				@fedexPrefsClientID = [ClientID],       
				@fedexPrefsStoreID = [StoreID],       
				@fedexPrefsDefaultShipperID = [DefaultShipperID],       
				@fedexPrefsDefaultDomesticService = [DefaultDomesticService],       
				@fedexPrefsDefaultInternationalService = [DefaultInternationalService],       
				@fedexPrefsDefaultTemplate = [DefaultTemplate],       
				@fedexPrefsEplPrinterName = [EplPrinterName],       
				@fedexPrefsZplPrinterName = [ZplPrinterName],       
				@fedexPrefsLabelType = [LabelType],       
				@fedexPrefsThermalLabelDocTabType = [ThermalLabelDocTabType],       
				@fedexPrefsReferenceNumber = [ReferenceNumber],       
				@fedexPrefsPackagingType = [PackagingType],       
				@fedexPrefsPackagingLength = [PackagingLength],       
				@fedexPrefsPackagingWidth = [PackagingWidth],       
				@fedexPrefsPackagingHeight = [PackagingHeight],       
				@fedexPrefsSignatureOption = [SignatureOption],       
				@fedexPrefsPayorType = [PayorType],       
				@fedexPrefsPayorAccountNumber = [PayorAccountNumber],       
				@fedexPrefsDutiesPayorType = [DutiesPayorType],       
				@fedexPrefsDutiesPayorAccountNumber = [DutiesPayorAccountNumber],       
				@fedexPrefsShipAlertRecipientAddressUseBill = [ShipAlertRecipientAddressUseBill],       
				@fedexPrefsShipAlertRecipientShip = [ShipAlertRecipientShip],       
				@fedexPrefsShipAlertRecipientDelivery = [ShipAlertRecipientDelivery],       
				@fedexPrefsShipAlertSenderShip = [ShipAlertSenderShip],       
				@fedexPrefsShipAlertSenderDelivery = [ShipAlertSenderDelivery],       
				@fedexPrefsShipAlertOther1Address = [ShipAlertOther1Address],       
				@fedexPrefsShipAlertOther1Ship = [ShipAlertOther1Ship],       
				@fedexPrefsShipAlertOther1Delivery = [ShipAlertOther1Delivery],       
				@fedexPrefsShipAlertExpressMessage = [ShipAlertExpressMessage],       
				@fedexPrefsShipAlertGroundEnable = [ShipAlertGroundEnable],       
				@fedexPrefsShipAlertGroundAddressUseBill = [ShipAlertGroundAddressUseBill],       
				@fedexPrefsShipAlertGroundMessage = [ShipAlertGroundMessage],       
				@fedexPrefsCodEnable = [CodEnable],       
				@fedexPrefsCodType = [CodType],       
				@fedexPrefsCodAddFreight = [CodAddFreight],       
				@fedexPrefsCodUseShipperAddress = [CodUseShipperAddress],       
				@fedexPrefsCodReturnContactName = [CodReturnContactName],       
				@fedexPrefsCodReturnCompany = [CodReturnCompany],       
				@fedexPrefsCodReturnAddress1 = [CodReturnAddress1],       
				@fedexPrefsCodReturnAddress2 = [CodReturnAddress2],       
				@fedexPrefsCodReturnCity = [CodReturnCity],       
				@fedexPrefsCodReturnStateProvinceCode = [CodReturnStateProvinceCode],       
				@fedexPrefsCodReturnPostalCode = [CodReturnPostalCode],       
				@fedexPrefsCodReturnPhone = [CodReturnPhone],       
				@fedexPrefsBrokerEnable = [BrokerEnable],       
				@fedexPrefsBrokerAccount = [BrokerAccount],       
				@fedexPrefsBrokerContactName = [BrokerContactName],       
				@fedexPrefsBrokerCompany = [BrokerCompany],       
				@fedexPrefsBrokerAddress1 = [BrokerAddress1],       
				@fedexPrefsBrokerAddress2 = [BrokerAddress2],       
				@fedexPrefsBrokerCity = [BrokerCity],       
				@fedexPrefsBrokerStateProvinceCode = [BrokerStateProvinceCode],       
				@fedexPrefsBrokerPostalCode = [BrokerPostalCode],       
				@fedexPrefsBrokerPhone = [BrokerPhone],       
				@fedexPrefsMaskAccountNumber = [MaskAccountNumber],       
				@fedexPrefsBlankRecipientPhone = [BlankRecipientPhone],       
				@fedexPrefsLimitDeclaredValue = [LimitDeclaredValue],       
				@fedexPrefsDefaultToCommercial = [DefaultToCommercial],       
				@fedexPrefsCommercialInvoiceCopies = [CommercialInvoiceCopies],       
				@fedexPrefsCommercialInvoiceTemplate = [CommercialInvoiceTemplate],       
				@fedexPrefsCodTemplate = [CodTemplate]       
			FROM dbo.[v2m_FedexPreferences]
			WHERE [ClientID] = @tempClientID
			AND [StoreID] = @tempStoreID
			AND dbo.v2m_TranslateKey([DefaultShipperID], 18) = @frequentFedexAccountID

			-- Create a shipping profile
			INSERT INTO dbo.ShippingProfile  
			(
				[Name],
				[ShipmentType],
				[ShipmentTypePrimary],
				[OriginID]
			)
			VALUES
			(
				'Defaults - FedEx',
				6, -- Fedex
				1, -- Primary
				2  -- Account
			)  

			SET @newProfileKey = @@IDENTITY

			-- Create a Fedex Profile
			INSERT INTO dbo.FedExProfile  (
				[ShippingProfileID],
				[FedExAccountID],
				[Service],
				[Signature],
				[PackagingType],
				[NonStandardContainer],
				[ReferenceCustomer],
				[ReferenceInvoice],
				[ReferencePO],
				[PayorTransportType],
				[PayorTransportAccount],
				[PayorDutiesType],
				[PayorDutiesAccount],
				[SaturdayDelivery],
				[EmailNotifySender],
				[EmailNotifyRecipient],
				[EmailNotifyOther],
				[EmailNotifyOtherAddress],
				[EmailNotifyMessage],
				[ResidentialDetermination],
				[InsuranceType],
				[SmartPostIndicia],
				[SmartPostEndorsement],
				[SmartPostConfirmation],
				[SmartPostCustomerManifest],
				[SmartPostHubID]
			)
			VALUES
			(
				@newProfileKey,
				@frequentFedexAccountID,
				@fedexPrefsDefaultDomesticService,
				@fedexPrefsSignatureOption,
				@fedexPrefsPackagingType,
				0,	-- Nonstandard Container
				@fedexPrefsReferenceNumber,
				'', -- ReferenceInvoice
				'', -- ReferencePO
				@fedexPrefsPayorType,
				@fedexPrefsPayorAccountNumber,
				@fedexPrefsDutiesPayorType,
				@fedexPrefsDutiesPayorAccountNumber,
				0,	-- Saturday Delivery
				0,	-- EmailNotifysender
				0,	-- EmailNotifyRecipient
				0,	-- EmailNotifyOther
				'',	-- EmailNotifyOtherAddress
				'', -- EmailNotifyMessage
				CASE -- ResidentialDetermination
					WHEN @fedexPrefsDefaultToCommercial = 1 THEN 2 -- commercial always
					ELSE 0	-- CommercialIfCompany, the default way of doing things in V2				
				END,
				0, -- Insurance Type.  No default in v2.
				0, -- SmartPostIndicia
				0, -- SmartPostEndorsement
				0, -- SmartPostConfirmation
				'', -- SmartHostCustomerManifest
				''	-- SmartHosthubID
			)             

		END
END