IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertLabelTestData]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[InsertLabelTestData]
GO
CREATE PROC InsertLabelTestData (@NumberToAdd INT) as

SET NOCOUNT ON

DECLARE @Message NVARCHAR(500)
DECLARE @ProcessingStart DATETIME = GETUTCDATE()
DECLARE @ProcessingEnd DATETIME
DECLARE @Count INT = 0
DECLARE @StartDate DATETIME = GETUTCDATE() - 28
DECLARE @EndDate DATETIME = GETUTCDATE()
DECLARE @NumberOfDays int = DATEDIFF(d, @StartDate, @enddate)
DECLARE @NumberPerDay INT = @NumberToAdd / @NumberOfDays

DECLARE @UpsAccountID BIGINT = 1056
DECLARE @FedExAccountID BIGINT = 1055
DECLARE @OnTrackAccountID BIGINT = 1090
DECLARE @iParcelAccountID BIGINT = 1091
DECLARE @OrderID bigint 
DECLARE @ShipmentID BIGINT
DECLARE @PackageID BIGINT
DECLARE @ObjectReferenceID BIGINT
DECLARE @HtmlPartResourceID BIGINT = NULL
DECLARE @PlainPartResourceID BIGINT = NULL
DECLARE @ResourceID BIGINT
DECLARE @UserID BIGINT = 1002
DECLARE @ComputerId BIGINT = 1001

--SELECT COUNT(*) AS 'Shipments' FROM dbo.Shipment
--SELECT COUNT(*) AS 'UpsPackages' FROM dbo.UpsPackage
--SELECT COUNT(*) AS 'FedExPackages' FROM dbo.FedExPackage
--SELECT COUNT(*) AS 'Resources' FROM dbo.Resource 
--SELECT COUNT(*) AS 'ObjectReferences' FROM dbo.ObjectReference
SELECT COUNT(*) FROM dbo.iParcelPackage

DECLARE @PercentComplete int = 0
DECLARE @PercentCompleteBreak INT = @NumberToAdd / 10

--In long running loops, Print doesn't send output to the client.  So using RaiseError with severity of no error and NoWait 
--so that we can always get timely status updates.
RAISERROR('Starting Label Test Data Inserts', 10, 1) WITH NOWAIT
        
SELECT @OrderID = OrderID FROM [ORDER] WHERE OrderID IN (SELECT TOP 1 OrderID FROM [Order])

-- UPS
RAISERROR('UPS:      0 Percent Complete', 10, 1) WITH NOWAIT
WHILE (@Count < @NumberToAdd)
	BEGIN
		SET @Count = @Count + 1
		
		IF (@PercentComplete < (@Count / @PercentCompleteBreak) % 10)
		BEGIN
			SET @PercentComplete = (@Count / @PercentCompleteBreak) % 10
			SET @Message = 'UPS:     ' + CAST(@PercentComplete * 10 AS NVARCHAR(10)) + ' Percent Complete.  ' + CAST(@Count AS NVARCHAR(10)) + ' Rows Inserted.'
			RAISERROR(@Message, 10, 1) WITH NOWAIT
		END
        
		-- Grab a UPS shipment
		INSERT INTO dbo.Shipment (OrderID, ShipmentType, ContentWeight, TotalWeight, Processed, ProcessedDate, ShipDate, ShipmentCost, Voided, VoidedDate, TrackingNumber, CustomsGenerated, CustomsValue, ThermalType, ShipFirstName, ShipMiddleName, ShipLastName, ShipCompany, ShipStreet1, ShipStreet2, ShipStreet3, ShipCity, ShipStateProvCode, ShipPostalCode, ShipCountryCode, ShipPhone, ShipEmail, ResidentialDetermination, ResidentialResult, OriginOriginID, OriginFirstName, OriginMiddleName, OriginLastName, OriginCompany, OriginStreet1, OriginStreet2, OriginStreet3, OriginCity, OriginStateProvCode, OriginPostalCode, OriginCountryCode, OriginPhone, OriginFax, OriginEmail, OriginWebsite, ReturnShipment, Insurance, InsuranceProvider, ShipNameParseStatus, ShipUnparsedName, OriginNameParseStatus, OriginUnparsedName )
		VALUES  (  @OrderID , -- OrderID - bigint
					0 , -- ShipmentType - int
					0.0 , -- ContentWeight - float,
					0.0 , -- TotalWeight - float
					1 , -- Processed - bit
					DATEADD(d,  @count % @NumberOfDays, @StartDate) , -- ProcessedDate - datetime
					'2013-07-25 13:24:39' , -- ShipDate - datetime
					0 , -- ShipmentCost - money
					0 , -- Voided - bit
					null , -- VoidedDate - datetime
					N'' , -- TrackingNumber - nvarchar(50)
					0 , -- CustomsGenerated - bit
					0 , -- CustomsValue - money
					0 , -- ThermalType - int
					N'' , -- ShipFirstName - nvarchar(30)
					N'' , -- ShipMiddleName - nvarchar(30)
					N'' , -- ShipLastName - nvarchar(30)
					N'' , -- ShipCompany - nvarchar(60)
					N'' , -- ShipStreet1 - nvarchar(60)
					N'' , -- ShipStreet2 - nvarchar(60)
					N'' , -- ShipStreet3 - nvarchar(60)
					N'' , -- ShipCity - nvarchar(50)
					N'' , -- ShipStateProvCode - nvarchar(50)
					N'' , -- ShipPostalCode - nvarchar(20)
					N'' , -- ShipCountryCode - nvarchar(50)
					N'' , -- ShipPhone - nvarchar(25)
					N'' , -- ShipEmail - nvarchar(100)
					0 , -- ResidentialDetermination - int
					1 , -- ResidentialResult - bit
					0 , -- OriginOriginID - bigint
					N'' , -- OriginFirstName - nvarchar(30)
					N'' , -- OriginMiddleName - nvarchar(30)
					N'' , -- OriginLastName - nvarchar(30)
					N'' , -- OriginCompany - nvarchar(60)
					N'' , -- OriginStreet1 - nvarchar(60)
					N'' , -- OriginStreet2 - nvarchar(60)
					N'' , -- OriginStreet3 - nvarchar(60)
					N'' , -- OriginCity - nvarchar(50)
					N'' , -- OriginStateProvCode - nvarchar(50)
					N'' , -- OriginPostalCode - nvarchar(20)
					N'' , -- OriginCountryCode - nvarchar(50)
					N'' , -- OriginPhone - nvarchar(25)
					N'' , -- OriginFax - nvarchar(35)
					N'' , -- OriginEmail - nvarchar(100)
					N'' , -- OriginWebsite - nvarchar(50)
					0 , -- ReturnShipment - bit
					0 , -- Insurance - bit
					0 , -- InsuranceProvider - int
					0 , -- ShipNameParseStatus - int
					N'' , -- ShipUnparsedName - nvarchar(100)
					0 , -- OriginNameParseStatus - int
					N''  -- OriginUnparsedName - nvarchar(100)
				)
		SELECT @ShipmentID = SCOPE_IDENTITY()

		-- Create a UpsShipment
		INSERT INTO dbo.UpsShipment (Shipmentid, UpsAccountID, Service, SaturdayDelivery, CodEnabled, CodAmount, CodPaymentType, DeliveryConfirmation, ReferenceNumber, ReferenceNumber2, PayorType, PayorAccount, PayorPostalCode, PayorCountryCode, EmailNotifySender, EmailNotifyRecipient, EmailNotifyOther, EmailNotifyOtherAddress, EmailNotifyFrom, EmailNotifySubject, EmailNotifyMessage, CustomsDocumentsOnly, CustomsDescription, CommercialInvoice, CommercialInvoiceTermsOfSale, CommercialInvoicePurpose, CommercialInvoiceComments, CommercialInvoiceFreight, CommercialInvoiceInsurance, CommercialInvoiceOther, WorldShipStatus, PublishedCharges, NegotiatedRate, ReturnService, ReturnUndeliverableEmail, ReturnContents, UspsTrackingNumber, Endorsement, Subclassification, PaperlessInternational, ShipperRelease, CarbonNeutral		        )
		VALUES  ( @ShipmentID , -- ShipmentID - bigint
		          @UpsAccountID , -- UpsAccountID - bigint
		          1, -- Service - int
		          0 , -- SaturdayDelivery - bit
		          0 , -- CodEnabled - bit
		          0 , -- CodAmount - money
		          0 , -- CodPaymentType - int
		          0 , -- DeliveryConfirmation - int
		          N'' , -- ReferenceNumber - nvarchar(300)
		          N'' , -- ReferenceNumber2 - nvarchar(300)
		          0 , -- PayorType - int
		          '' , -- PayorAccount - varchar(10)
		          N'' , -- PayorPostalCode - nvarchar(20)
		          N'' , -- PayorCountryCode - nvarchar(50)
		          0 , -- EmailNotifySender - int
		          0 , -- EmailNotifyRecipient - int
		          0 , -- EmailNotifyOther - int
		          N'' , -- EmailNotifyOtherAddress - nvarchar(100)
		          N'' , -- EmailNotifyFrom - nvarchar(100)
		          0 , -- EmailNotifySubject - int
		          N'' , -- EmailNotifyMessage - nvarchar(120)
		          0 , -- CustomsDocumentsOnly - bit
		          N'' , -- CustomsDescription - nvarchar(150)
		          0 , -- CommercialInvoice - bit
		          0 , -- CommercialInvoiceTermsOfSale - int
		          0 , -- CommercialInvoicePurpose - int
		          N'' , -- CommercialInvoiceComments - nvarchar(200)
		          0 , -- CommercialInvoiceFreight - money
		          0 , -- CommercialInvoiceInsurance - money
		          0 , -- CommercialInvoiceOther - money
		          0 , -- WorldShipStatus - int
		          0 , -- PublishedCharges - money
		          0 , -- NegotiatedRate - bit
		          0 , -- ReturnService - int
		          N'' , -- ReturnUndeliverableEmail - nvarchar(100)
		          N'' , -- ReturnContents - nvarchar(300)
		          N'' , -- UspsTrackingNumber - nvarchar(50)
		          0 , -- Endorsement - int
		          0 , -- Subclassification - int
		          0 , -- PaperlessInternational - bit
		          0 , -- ShipperRelease - bit
		          0  -- CarbonNeutral - bit
		        )

		-- Create the Package
		INSERT INTO dbo.UpsPackage (ShipmentID, PackagingType, Weight, DimsProfileID, DimsLength, DimsWidth, DimsHeight, DimsWeight, DimsAddWeight, Insurance, InsuranceValue, InsurancePennyOne, DeclaredValue, TrackingNumber, UspsTrackingNumber)
		VALUES  ( @ShipmentID , -- ShipmentID - bigint
		          0 , -- PackagingType - int
		          0.0 , -- Weight - float
		          0 , -- DimsProfileID - bigint
		          0.0 , -- DimsLength - float
		          0.0 , -- DimsWidth - float
		          0.0 , -- DimsHeight - float
		          0.0 , -- DimsWeight - float
		          0 , -- DimsAddWeight - bit
		          0 , -- Insurance - bit
		          0 , -- InsuranceValue - money
		          0 , -- InsurancePennyOne - bit
		          0 , -- DeclaredValue - money
		          N'' , -- TrackingNumber - nvarchar(50)
		          N''  -- UspsTrackingNumber - nvarchar(50)
		        )
		SET @PackageID = SCOPE_IDENTITY()

		-- Create a fake resource to use
		INSERT INTO dbo.Resource ( Data ,Checksum ,Compressed ,Filename)
		VALUES  ( NEWID(), -- Data - varbinary(max)
		          NEWID()  , -- Checksum - binary
		          0 , -- Compressed - bit
		          N'' + SUBSTRING(CAST(NEWID() AS NVARCHAR(50)), 0, 30)  -- Filename - nvarchar(30)
		        )
		SET @ResourceID = SCOPE_IDENTITY()

		INSERT INTO dbo.ObjectReference
		        ( ConsumerID , ReferenceKey ,ObjectID ,Reason
		        )
		VALUES  ( @PackageID , -- ConsumerID - bigint
		          N'' + SUBSTRING(CAST(NEWID() AS NVARCHAR(50)), 0, 30) , -- ReferenceKey - varchar(250)
		          @ResourceID , -- ObjectID - bigint
		          N''  -- Reason - nvarchar(250)
		        )
		SET @ObjectReferenceID = SCOPE_IDENTITY()

	END
SET @Message = 'UPS:    100 Percent Complete.  ' + CAST(@Count AS NVARCHAR(10)) + ' Rows Inserted.'
RAISERROR(@Message, 10, 1) WITH NOWAIT

-- FedEx
SET @Count = 0
SET @PercentComplete = 0
RAISERROR('FedEx:    0 Percent Complete', 10, 1) WITH NOWAIT
WHILE (@Count < @NumberToAdd)
	BEGIN
		SET @Count = @Count + 1
		
		IF (@PercentComplete < (@Count / @PercentCompleteBreak) % 10)
		BEGIN
			SET @PercentComplete = (@Count / @PercentCompleteBreak) % 10
			SET @Message = 'FedEx:   ' + CAST(@PercentComplete * 10 AS NVARCHAR(10)) + ' Percent Complete.  ' + CAST(@Count AS NVARCHAR(10)) + ' Rows Inserted.'
			RAISERROR(@Message, 10, 1) WITH NOWAIT

		END
        
		-- Grab a shipment
		INSERT INTO dbo.Shipment (OrderID, ShipmentType, ContentWeight, TotalWeight, Processed, ProcessedDate, ShipDate, ShipmentCost, Voided, VoidedDate, TrackingNumber, CustomsGenerated, CustomsValue, ThermalType, ShipFirstName, ShipMiddleName, ShipLastName, ShipCompany, ShipStreet1, ShipStreet2, ShipStreet3, ShipCity, ShipStateProvCode, ShipPostalCode, ShipCountryCode, ShipPhone, ShipEmail, ResidentialDetermination, ResidentialResult, OriginOriginID, OriginFirstName, OriginMiddleName, OriginLastName, OriginCompany, OriginStreet1, OriginStreet2, OriginStreet3, OriginCity, OriginStateProvCode, OriginPostalCode, OriginCountryCode, OriginPhone, OriginFax, OriginEmail, OriginWebsite, ReturnShipment, Insurance, InsuranceProvider, ShipNameParseStatus, ShipUnparsedName, OriginNameParseStatus, OriginUnparsedName )
		VALUES  (   @OrderID , -- OrderID - bigint
					6 , -- ShipmentType - int
					0.0 , -- ContentWeight - float,
					0.0 , -- TotalWeight - float
					1 , -- Processed - bit
					DATEADD(d,  @count % @NumberOfDays, @StartDate) , -- ProcessedDate - datetime
					'2013-07-25 13:24:39' , -- ShipDate - datetime
					0 , -- ShipmentCost - money
					0 , -- Voided - bit
					null , -- VoidedDate - datetime
					N'' , -- TrackingNumber - nvarchar(50)
					0 , -- CustomsGenerated - bit
					0 , -- CustomsValue - money
					0 , -- ThermalType - int
					N'' , -- ShipFirstName - nvarchar(30)
					N'' , -- ShipMiddleName - nvarchar(30)
					N'' , -- ShipLastName - nvarchar(30)
					N'' , -- ShipCompany - nvarchar(60)
					N'' , -- ShipStreet1 - nvarchar(60)
					N'' , -- ShipStreet2 - nvarchar(60)
					N'' , -- ShipStreet3 - nvarchar(60)
					N'' , -- ShipCity - nvarchar(50)
					N'' , -- ShipStateProvCode - nvarchar(50)
					N'' , -- ShipPostalCode - nvarchar(20)
					N'' , -- ShipCountryCode - nvarchar(50)
					N'' , -- ShipPhone - nvarchar(25)
					N'' , -- ShipEmail - nvarchar(100)
					0 , -- ResidentialDetermination - int
					1 , -- ResidentialResult - bit
					0 , -- OriginOriginID - bigint
					N'' , -- OriginFirstName - nvarchar(30)
					N'' , -- OriginMiddleName - nvarchar(30)
					N'' , -- OriginLastName - nvarchar(30)
					N'' , -- OriginCompany - nvarchar(60)
					N'' , -- OriginStreet1 - nvarchar(60)
					N'' , -- OriginStreet2 - nvarchar(60)
					N'' , -- OriginStreet3 - nvarchar(60)
					N'' , -- OriginCity - nvarchar(50)
					N'' , -- OriginStateProvCode - nvarchar(50)
					N'' , -- OriginPostalCode - nvarchar(20)
					N'' , -- OriginCountryCode - nvarchar(50)
					N'' , -- OriginPhone - nvarchar(25)
					N'' , -- OriginFax - nvarchar(35)
					N'' , -- OriginEmail - nvarchar(100)
					N'' , -- OriginWebsite - nvarchar(50)
					0 , -- ReturnShipment - bit
					0 , -- Insurance - bit
					0 , -- InsuranceProvider - int
					0 , -- ShipNameParseStatus - int
					N'' , -- ShipUnparsedName - nvarchar(100)
					0 , -- OriginNameParseStatus - int
					N''  -- OriginUnparsedName - nvarchar(100)
				)
		SELECT @ShipmentID = SCOPE_IDENTITY()

		-- Create a FedExShipment
		INSERT INTO dbo.FedExShipment(ShipmentID, FedExAccountID, MasterFormID, Service, Signature, PackagingType, NonStandardContainer, ReferenceCustomer, ReferenceInvoice, ReferencePO, PayorTransportType, PayorTransportName, PayorTransportAccount, PayorDutiesType, PayorDutiesAccount, PayorDutiesName, PayorDutiesCountryCode, SaturdayDelivery, HomeDeliveryType, HomeDeliveryInstructions, HomeDeliveryDate, HomeDeliveryPhone, FreightInsidePickup, FreightInsideDelivery, FreightBookingNumber, FreightLoadAndCount, EmailNotifyBroker, EmailNotifySender, EmailNotifyRecipient, EmailNotifyOther, EmailNotifyOtherAddress, EmailNotifyMessage, CodEnabled, CodAmount, CodPaymentType, CodAddFreight, CodOriginID, CodFirstName, CodLastName, CodCompany, CodStreet1, CodStreet2, CodStreet3, CodCity, CodStateProvCode, CodPostalCode, CodCountryCode, CodPhone, CodTrackingNumber, CodTrackingFormID, CodTIN, CodChargeBasis, CodAccountNumber, BrokerEnabled, BrokerAccount, BrokerFirstName, BrokerLastName, BrokerCompany, BrokerStreet1, BrokerStreet2, BrokerStreet3, BrokerCity, BrokerStateProvCode, BrokerPostalCode, BrokerCountryCode, BrokerPhone, BrokerPhoneExtension, BrokerEmail, CustomsAdmissibilityPackaging, CustomsRecipientTIN, CustomsDocumentsOnly, CustomsDocumentsDescription, CustomsExportFilingOption, CustomsAESEEI, CustomsRecipientIdentificationType, CustomsRecipientIdentificationValue, CustomsOptionsType, CustomsOptionsDesription, CommercialInvoice, CommercialInvoiceTermsOfSale, CommercialInvoicePurpose, CommercialInvoiceComments, CommercialInvoiceFreight, CommercialInvoiceInsurance, CommercialInvoiceOther, CommercialInvoiceReference, ImporterOfRecord, ImporterAccount, ImporterTIN, ImporterFirstName, ImporterLastName, ImporterCompany, ImporterStreet1, ImporterStreet2, ImporterStreet3, ImporterCity, ImporterStateProvCode, ImporterPostalCode, ImporterCountryCode, ImporterPhone, SmartPostIndicia, SmartPostEndorsement, SmartPostConfirmation, SmartPostCustomerManifest, SmartPostHubID, DropoffType, OriginResidentialDetermination, FedExHoldAtLocationEnabled, HoldLocationId, HoldLocationType, HoldContactId, HoldPersonName, HoldTitle, HoldCompanyName, HoldPhoneNumber, HoldPhoneExtension, HoldPagerNumber, HoldFaxNumber, HoldEmailAddress, HoldStreet1, HoldStreet2, HoldStreet3, HoldCity, HoldStateOrProvinceCode, HoldPostalCode, HoldUrbanizationCode, HoldCountryCode, HoldResidential, CustomsNaftaEnabled, CustomsNaftaPreferenceType, CustomsNaftaDeterminationCode, CustomsNaftaProducerId, CustomsNaftaNetCostMethod, ReturnType, RmaNumber, RmaReason, ReturnSaturdayPickup, TrafficInArmsLicenseNumber, IntlExportDetailType, IntlExportDetailForeignTradeZoneCode, IntlExportDetailEntryNumber, IntlExportDetailLicenseOrPermitNumber, IntlExportDetailLicenseOrPermitExpirationDate, WeightUnitType, LinearUnitType)
		VALUES  ( @ShipmentID , -- ShipmentID - bigint
		          @FedExAccountID , -- FedExAccountID - bigint
		          '' , -- MasterFormID - varchar(4)
		          0 , -- Service - int
		          0 , -- Signature - int
		          0 , -- PackagingType - int
		          0 , -- NonStandardContainer - bit
		          N'' , -- ReferenceCustomer - nvarchar(300)
		          N'' , -- ReferenceInvoice - nvarchar(300)
		          N'' , -- ReferencePO - nvarchar(300)
		          0 , -- PayorTransportType - int
		          N'' , -- PayorTransportName - nvarchar(60)
		          '' , -- PayorTransportAccount - varchar(12)
		          0 , -- PayorDutiesType - int
		          '' , -- PayorDutiesAccount - varchar(12)
		          N'' , -- PayorDutiesName - nvarchar(60)
		          N'' , -- PayorDutiesCountryCode - nvarchar(50)
		          0 , -- SaturdayDelivery - bit
		          0 , -- HomeDeliveryType - int
		          '' , -- HomeDeliveryInstructions - varchar(74)
		          '2013-07-25 14:57:01' , -- HomeDeliveryDate - datetime
		          '' , -- HomeDeliveryPhone - varchar(24)
		          0 , -- FreightInsidePickup - bit
		          0 , -- FreightInsideDelivery - bit
		          '' , -- FreightBookingNumber - varchar(12)
		          0 , -- FreightLoadAndCount - int
		          0 , -- EmailNotifyBroker - int
		          0 , -- EmailNotifySender - int
		          0 , -- EmailNotifyRecipient - int
		          0 , -- EmailNotifyOther - int
		          N'' , -- EmailNotifyOtherAddress - nvarchar(100)
		          '' , -- EmailNotifyMessage - varchar(120)
		          0 , -- CodEnabled - bit
		          0 , -- CodAmount - money
		          0 , -- CodPaymentType - int
		          0 , -- CodAddFreight - bit
		          0 , -- CodOriginID - bigint
		          N'' , -- CodFirstName - nvarchar(30)
		          N'' , -- CodLastName - nvarchar(30)
		          N'' , -- CodCompany - nvarchar(35)
		          N'' , -- CodStreet1 - nvarchar(60)
		          N'' , -- CodStreet2 - nvarchar(60)
		          N'' , -- CodStreet3 - nvarchar(60)
		          N'' , -- CodCity - nvarchar(50)
		          N'' , -- CodStateProvCode - nvarchar(50)
		          N'' , -- CodPostalCode - nvarchar(20)
		          N'' , -- CodCountryCode - nvarchar(50)
		          N'' , -- CodPhone - nvarchar(25)
		          '' , -- CodTrackingNumber - varchar(50)
		          '' , -- CodTrackingFormID - varchar(4)
		          N'' , -- CodTIN - nvarchar(24)
		          0 , -- CodChargeBasis - int
		          N'' , -- CodAccountNumber - nvarchar(25)
		          0 , -- BrokerEnabled - bit
		          N'' , -- BrokerAccount - nvarchar(12)
		          N'' , -- BrokerFirstName - nvarchar(30)
		          N'' , -- BrokerLastName - nvarchar(30)
		          N'' , -- BrokerCompany - nvarchar(35)
		          N'' , -- BrokerStreet1 - nvarchar(60)
		          N'' , -- BrokerStreet2 - nvarchar(60)
		          N'' , -- BrokerStreet3 - nvarchar(60)
		          N'' , -- BrokerCity - nvarchar(50)
		          N'' , -- BrokerStateProvCode - nvarchar(50)
		          N'' , -- BrokerPostalCode - nvarchar(20)
		          N'' , -- BrokerCountryCode - nvarchar(50)
		          N'' , -- BrokerPhone - nvarchar(25)
		          N'' , -- BrokerPhoneExtension - nvarchar(8)
		          N'' , -- BrokerEmail - nvarchar(100)
		          0 , -- CustomsAdmissibilityPackaging - int
		          '' , -- CustomsRecipientTIN - varchar(15)
		          0 , -- CustomsDocumentsOnly - bit
		          N'' , -- CustomsDocumentsDescription - nvarchar(150)
		          0 , -- CustomsExportFilingOption - int
		          N'' , -- CustomsAESEEI - nvarchar(100)
		          0 , -- CustomsRecipientIdentificationType - int
		          N'' , -- CustomsRecipientIdentificationValue - nvarchar(50)
		          0 , -- CustomsOptionsType - int
		          N'' , -- CustomsOptionsDesription - nvarchar(32)
		          0 , -- CommercialInvoice - bit
		          0 , -- CommercialInvoiceTermsOfSale - int
		          0 , -- CommercialInvoicePurpose - int
		          N'' , -- CommercialInvoiceComments - nvarchar(200)
		          0 , -- CommercialInvoiceFreight - money
		          0 , -- CommercialInvoiceInsurance - money
		          0 , -- CommercialInvoiceOther - money
		          N'' , -- CommercialInvoiceReference - nvarchar(300)
		          0 , -- ImporterOfRecord - bit
		          N'' , -- ImporterAccount - nvarchar(12)
		          N'' , -- ImporterTIN - nvarchar(15)
		          N'' , -- ImporterFirstName - nvarchar(30)
		          N'' , -- ImporterLastName - nvarchar(30)
		          N'' , -- ImporterCompany - nvarchar(35)
		          N'' , -- ImporterStreet1 - nvarchar(60)
		          N'' , -- ImporterStreet2 - nvarchar(60)
		          N'' , -- ImporterStreet3 - nvarchar(60)
		          N'' , -- ImporterCity - nvarchar(50)
		          N'' , -- ImporterStateProvCode - nvarchar(50)
		          N'' , -- ImporterPostalCode - nvarchar(10)
		          N'' , -- ImporterCountryCode - nvarchar(50)
		          N'' , -- ImporterPhone - nvarchar(25)
		          0 , -- SmartPostIndicia - int
		          0 , -- SmartPostEndorsement - int
		          0 , -- SmartPostConfirmation - bit
		          N'' , -- SmartPostCustomerManifest - nvarchar(300)
		          '' , -- SmartPostHubID - varchar(10)
		          0 , -- DropoffType - int
		          0 , -- OriginResidentialDetermination - int
		          0 , -- FedExHoldAtLocationEnabled - bit
		          N'' , -- HoldLocationId - nvarchar(50)
		          0 , -- HoldLocationType - int
		          N'' , -- HoldContactId - nvarchar(50)
		          N'' , -- HoldPersonName - nvarchar(100)
		          N'' , -- HoldTitle - nvarchar(50)
		          N'' , -- HoldCompanyName - nvarchar(50)
		          N'' , -- HoldPhoneNumber - nvarchar(30)
		          N'' , -- HoldPhoneExtension - nvarchar(10)
		          N'' , -- HoldPagerNumber - nvarchar(30)
		          N'' , -- HoldFaxNumber - nvarchar(30)
		          N'' , -- HoldEmailAddress - nvarchar(100)
		          N'' , -- HoldStreet1 - nvarchar(250)
		          N'' , -- HoldStreet2 - nvarchar(250)
		          N'' , -- HoldStreet3 - nvarchar(250)
		          N'' , -- HoldCity - nvarchar(100)
		          N'' , -- HoldStateOrProvinceCode - nvarchar(50)
		          N'' , -- HoldPostalCode - nvarchar(20)
		          N'' , -- HoldUrbanizationCode - nvarchar(20)
		          N'' , -- HoldCountryCode - nvarchar(20)
		          0 , -- HoldResidential - bit
		          0 , -- CustomsNaftaEnabled - bit
		          0 , -- CustomsNaftaPreferenceType - int
		          0 , -- CustomsNaftaDeterminationCode - int
		          N'' , -- CustomsNaftaProducerId - nvarchar(20)
		          0 , -- CustomsNaftaNetCostMethod - int
		          0 , -- ReturnType - int
		          N'' , -- RmaNumber - nvarchar(30)
		          N'' , -- RmaReason - nvarchar(60)
		          0 , -- ReturnSaturdayPickup - bit
		          N'' , -- TrafficInArmsLicenseNumber - nvarchar(32)
		          0 , -- IntlExportDetailType - int
		          N'' , -- IntlExportDetailForeignTradeZoneCode - nvarchar(50)
		          N'' , -- IntlExportDetailEntryNumber - nvarchar(20)
		          N'' , -- IntlExportDetailLicenseOrPermitNumber - nvarchar(50)
		          '2013-07-25 14:57:01' , -- IntlExportDetailLicenseOrPermitExpirationDate - datetime
		          0 , -- WeightUnitType - int
		          0  -- LinearUnitType - int
		        )

		-- Create the Package
		INSERT INTO dbo.FedExPackage (ShipmentID, Weight, DimsProfileID, DimsLength, DimsWidth, DimsHeight, DimsWeight, DimsAddWeight, SkidPieces, Insurance, InsuranceValue, InsurancePennyOne, DeclaredValue, TrackingNumber, PriorityAlert, PriorityAlertEnhancementType, PriorityAlertDetailContent, DryIceWeight, ContainsAlcohol, DangerousGoodsEnabled, DangerousGoodsType, DangerousGoodsAccessibilityType, DangerousGoodsCargoAircraftOnly, DangerousGoodsEmergencyContactPhone, DangerousGoodsOfferor, DangerousGoodsPackagingCount, HazardousMaterialNumber, HazardousMaterialClass, HazardousMaterialProperName, HazardousMaterialPackingGroup, HazardousMaterialQuantityValue, HazardousMaterialQuanityUnits)
		VALUES  ( @ShipmentID , -- ShipmentID - bigint
		          0.0 , -- Weight - float
		          0 , -- DimsProfileID - bigint
		          0.0 , -- DimsLength - float
		          0.0 , -- DimsWidth - float
		          0.0 , -- DimsHeight - float
		          0.0 , -- DimsWeight - float
		          0 , -- DimsAddWeight - bit
		          0 , -- SkidPieces - int
		          0 , -- Insurance - bit
		          0 , -- InsuranceValue - money
		          0 , -- InsurancePennyOne - bit
		          0 , -- DeclaredValue - money
		          '' , -- TrackingNumber - varchar(50)
		          0 , -- PriorityAlert - bit
		          0 , -- PriorityAlertEnhancementType - int
		          N'' , -- PriorityAlertDetailContent - nvarchar(1024)
		          0.0 , -- DryIceWeight - float
		          0 , -- ContainsAlcohol - bit
		          0 , -- DangerousGoodsEnabled - bit
		          0 , -- DangerousGoodsType - int
		          0 , -- DangerousGoodsAccessibilityType - int
		          0 , -- DangerousGoodsCargoAircraftOnly - bit
		          N'' , -- DangerousGoodsEmergencyContactPhone - nvarchar(16)
		          N'' , -- DangerousGoodsOfferor - nvarchar(128)
		          0 , -- DangerousGoodsPackagingCount - int
		          N'' , -- HazardousMaterialNumber - nvarchar(16)
		          N'' , -- HazardousMaterialClass - nvarchar(8)
		          N'' , -- HazardousMaterialProperName - nvarchar(64)
		          0 , -- HazardousMaterialPackingGroup - int
		          0.0 , -- HazardousMaterialQuantityValue - float
		          0  -- HazardousMaterialQuanityUnits - int
		        )
		SET @PackageID = SCOPE_IDENTITY()

		-- Create a fake resource to use
		INSERT INTO dbo.Resource ( Data ,Checksum ,Compressed ,Filename)
		VALUES  ( NEWID(), -- Data - varbinary(max)
		          NEWID()  , -- Checksum - binary
		          0 , -- Compressed - bit
		          N'' + SUBSTRING(CAST(NEWID() AS NVARCHAR(50)), 0, 30)  -- Filename - nvarchar(30)
		        )
		SET @ResourceID = SCOPE_IDENTITY()

		INSERT INTO dbo.ObjectReference
		        ( ConsumerID , ReferenceKey ,ObjectID ,Reason
		        )
		VALUES  ( @PackageID , -- ConsumerID - bigint
		          N'' + SUBSTRING(CAST(NEWID() AS NVARCHAR(50)), 0, 30) , -- ReferenceKey - varchar(250)
		          @ResourceID , -- ObjectID - bigint
		          N''  -- Reason - nvarchar(250)
		        )
		SET @ObjectReferenceID = SCOPE_IDENTITY()

	END  
SET @Message = 'FedEx:  100 Percent Complete.  ' + CAST(@Count AS NVARCHAR(10)) + ' Rows Inserted.'
RAISERROR(@Message, 10, 1) WITH NOWAIT

-- iParcel
SET @Count = 0
SET @PercentComplete = 0
WHILE (@Count < @NumberToAdd)
	BEGIN
		SET @Count = @Count + 1
		
		IF (@PercentComplete < (@Count / @PercentCompleteBreak) % 10)
		--PRINT @PercentComplete
		BEGIN
			SET @PercentComplete = (@Count / @PercentCompleteBreak) % 10
			SET @Message = 'iParcel: ' + CAST(@PercentComplete * 10 AS NVARCHAR(10)) + ' Percent Complete.  ' + CAST(@Count AS NVARCHAR(10)) + ' Rows Inserted.'
			RAISERROR(@Message, 10, 1) WITH NOWAIT
		END
        
		-- Grab a iParcel shipment
		INSERT INTO dbo.Shipment (OrderID, ShipmentType, ContentWeight, TotalWeight, Processed, ProcessedDate, ShipDate, ShipmentCost, Voided, VoidedDate, TrackingNumber, CustomsGenerated, CustomsValue, ThermalType, ShipFirstName, ShipMiddleName, ShipLastName, ShipCompany, ShipStreet1, ShipStreet2, ShipStreet3, ShipCity, ShipStateProvCode, ShipPostalCode, ShipCountryCode, ShipPhone, ShipEmail, ResidentialDetermination, ResidentialResult, OriginOriginID, OriginFirstName, OriginMiddleName, OriginLastName, OriginCompany, OriginStreet1, OriginStreet2, OriginStreet3, OriginCity, OriginStateProvCode, OriginPostalCode, OriginCountryCode, OriginPhone, OriginFax, OriginEmail, OriginWebsite, ReturnShipment, Insurance, InsuranceProvider, ShipNameParseStatus, ShipUnparsedName, OriginNameParseStatus, OriginUnparsedName )
		VALUES  (  @OrderID , -- OrderID - bigint
					12 , -- ShipmentType - int
					0.0 , -- ContentWeight - float,
					0.0 , -- TotalWeight - float
					1 , -- Processed - bit
					DATEADD(d,  @count % @NumberOfDays, @StartDate) , -- ProcessedDate - datetime
					'2013-07-25 13:24:39' , -- ShipDate - datetime
					0 , -- ShipmentCost - money
					0 , -- Voided - bit
					null , -- VoidedDate - datetime
					N'' , -- TrackingNumber - nvarchar(50)
					0 , -- CustomsGenerated - bit
					0 , -- CustomsValue - money
					0 , -- ThermalType - int
					N'' , -- ShipFirstName - nvarchar(30)
					N'' , -- ShipMiddleName - nvarchar(30)
					N'' , -- ShipLastName - nvarchar(30)
					N'' , -- ShipCompany - nvarchar(60)
					N'' , -- ShipStreet1 - nvarchar(60)
					N'' , -- ShipStreet2 - nvarchar(60)
					N'' , -- ShipStreet3 - nvarchar(60)
					N'' , -- ShipCity - nvarchar(50)
					N'' , -- ShipStateProvCode - nvarchar(50)
					N'' , -- ShipPostalCode - nvarchar(20)
					N'' , -- ShipCountryCode - nvarchar(50)
					N'' , -- ShipPhone - nvarchar(25)
					N'' , -- ShipEmail - nvarchar(100)
					0 , -- ResidentialDetermination - int
					1 , -- ResidentialResult - bit
					0 , -- OriginOriginID - bigint
					N'' , -- OriginFirstName - nvarchar(30)
					N'' , -- OriginMiddleName - nvarchar(30)
					N'' , -- OriginLastName - nvarchar(30)
					N'' , -- OriginCompany - nvarchar(60)
					N'' , -- OriginStreet1 - nvarchar(60)
					N'' , -- OriginStreet2 - nvarchar(60)
					N'' , -- OriginStreet3 - nvarchar(60)
					N'' , -- OriginCity - nvarchar(50)
					N'' , -- OriginStateProvCode - nvarchar(50)
					N'' , -- OriginPostalCode - nvarchar(20)
					N'' , -- OriginCountryCode - nvarchar(50)
					N'' , -- OriginPhone - nvarchar(25)
					N'' , -- OriginFax - nvarchar(35)
					N'' , -- OriginEmail - nvarchar(100)
					N'' , -- OriginWebsite - nvarchar(50)
					0 , -- ReturnShipment - bit
					0 , -- Insurance - bit
					0 , -- InsuranceProvider - int
					0 , -- ShipNameParseStatus - int
					N'' , -- ShipUnparsedName - nvarchar(100)
					0 , -- OriginNameParseStatus - int
					N''  -- OriginUnparsedName - nvarchar(100)
				)
		SELECT @ShipmentID = SCOPE_IDENTITY()

		-- Create a iParcelShipment
		INSERT INTO dbo.iParcelShipment (ShipmentID, iParcelAccountID, Service, Reference, TrackByEmail, TrackBySMS, IsDeliveryDutyPaid  )
		VALUES  ( @ShipmentID , -- ShipmentID - bigint
		          @iParcelAccountID , -- iParcelAccountID - bigint
		          0 , -- Service - int
		          N'' , -- Reference - nvarchar(300)
		          0 , -- TrackByEmail - bit
		          0 , -- TrackBySMS - bit
		          0  -- IsDeliveryDutyPaid - bit
		        )

		-- Create the Package
		INSERT INTO dbo.iParcelPackage(ShipmentID, Weight, DimsProfileID, DimsLength, DimsWidth, DimsHeight, DimsAddWeight, DimsWeight, Insurance, InsuranceValue, InsurancePennyOne, DeclaredValue, TrackingNumber, ParcelNumber, SkuAndQuantities )
		VALUES  ( @ShipmentID , -- ShipmentID - bigint
		          0.0 , -- Weight - float
		          0 , -- DimsProfileID - bigint
		          0.0 , -- DimsLength - float
		          0.0 , -- DimsWidth - float
		          0.0 , -- DimsHeight - float
		          0 , -- DimsAddWeight - bit
		          0.0 , -- DimsWeight - float
		          0 , -- Insurance - bit
		          0 , -- InsuranceValue - money
		          0 , -- InsurancePennyOne - bit
		          0 , -- DeclaredValue - money
		          '' , -- TrackingNumber - varchar(50)
		          N'' , -- ParcelNumber - nvarchar(50)
		          N''  -- SkuAndQuantities - nvarchar(500)
		        )
		SET @PackageID = SCOPE_IDENTITY()

		-- Create a fake resource to use
		INSERT INTO dbo.Resource ( Data ,Checksum ,Compressed ,Filename)
		VALUES  ( NEWID(), -- Data - varbinary(max)
		          NEWID()  , -- Checksum - binary
		          0 , -- Compressed - bit
		          N'' + SUBSTRING(CAST(NEWID() AS NVARCHAR(50)), 0, 30)  -- Filename - nvarchar(30)
		        )
		SET @ResourceID = SCOPE_IDENTITY()

		INSERT INTO dbo.ObjectReference
		        ( ConsumerID , ReferenceKey ,ObjectID ,Reason
		        )
		VALUES  ( @PackageID , -- ConsumerID - bigint
		          N'' + SUBSTRING(CAST(NEWID() AS NVARCHAR(50)), 0, 30) , -- ReferenceKey - varchar(250)
		          @ResourceID , -- ObjectID - bigint
		          N''  -- Reason - nvarchar(250)
		        )
		SET @ObjectReferenceID = SCOPE_IDENTITY()

	END
SET @Message = 'iParcel:100 Percent Complete.  ' + CAST(@Count AS NVARCHAR(10)) + ' Rows Inserted.'
RAISERROR(@Message, 10, 1) WITH NOWAIT

  

-- OnTrack
SET @Count = 0
SET @PercentComplete = 0
WHILE (@Count < @NumberToAdd)
	BEGIN
		SET @Count = @Count + 1
		
		IF (@PercentComplete < (@Count / @PercentCompleteBreak) % 10)
		BEGIN
			SET @PercentComplete = (@Count / @PercentCompleteBreak) % 10
			SET @Message = 'OnTrack: ' + CAST(@PercentComplete * 10 AS NVARCHAR(10)) + ' Percent Complete.  ' + CAST(@Count AS NVARCHAR(10)) + ' Rows Inserted.'
			RAISERROR(@Message, 10, 1) WITH NOWAIT
		END
        
		-- Grab a shipment
		INSERT INTO dbo.Shipment (OrderID, ShipmentType, ContentWeight, TotalWeight, Processed, ProcessedDate, ShipDate, ShipmentCost, Voided, VoidedDate, TrackingNumber, CustomsGenerated, CustomsValue, ThermalType, ShipFirstName, ShipMiddleName, ShipLastName, ShipCompany, ShipStreet1, ShipStreet2, ShipStreet3, ShipCity, ShipStateProvCode, ShipPostalCode, ShipCountryCode, ShipPhone, ShipEmail, ResidentialDetermination, ResidentialResult, OriginOriginID, OriginFirstName, OriginMiddleName, OriginLastName, OriginCompany, OriginStreet1, OriginStreet2, OriginStreet3, OriginCity, OriginStateProvCode, OriginPostalCode, OriginCountryCode, OriginPhone, OriginFax, OriginEmail, OriginWebsite, ReturnShipment, Insurance, InsuranceProvider, ShipNameParseStatus, ShipUnparsedName, OriginNameParseStatus, OriginUnparsedName )
		VALUES  (   @OrderID , -- OrderID - bigint
					11 , -- ShipmentType - int
					0.0 , -- ContentWeight - float,
					0.0 , -- TotalWeight - float
					1 , -- Processed - bit
					DATEADD(d,  @count % @NumberOfDays, @StartDate) , -- ProcessedDate - datetime
					'2013-07-25 13:24:39' , -- ShipDate - datetime
					0 , -- ShipmentCost - money
					0 , -- Voided - bit
					null , -- VoidedDate - datetime
					N'' , -- TrackingNumber - nvarchar(50)
					0 , -- CustomsGenerated - bit
					0 , -- CustomsValue - money
					0 , -- ThermalType - int
					N'' , -- ShipFirstName - nvarchar(30)
					N'' , -- ShipMiddleName - nvarchar(30)
					N'' , -- ShipLastName - nvarchar(30)
					N'' , -- ShipCompany - nvarchar(60)
					N'' , -- ShipStreet1 - nvarchar(60)
					N'' , -- ShipStreet2 - nvarchar(60)
					N'' , -- ShipStreet3 - nvarchar(60)
					N'' , -- ShipCity - nvarchar(50)
					N'' , -- ShipStateProvCode - nvarchar(50)
					N'' , -- ShipPostalCode - nvarchar(20)
					N'' , -- ShipCountryCode - nvarchar(50)
					N'' , -- ShipPhone - nvarchar(25)
					N'' , -- ShipEmail - nvarchar(100)
					0 , -- ResidentialDetermination - int
					1 , -- ResidentialResult - bit
					0 , -- OriginOriginID - bigint
					N'' , -- OriginFirstName - nvarchar(30)
					N'' , -- OriginMiddleName - nvarchar(30)
					N'' , -- OriginLastName - nvarchar(30)
					N'' , -- OriginCompany - nvarchar(60)
					N'' , -- OriginStreet1 - nvarchar(60)
					N'' , -- OriginStreet2 - nvarchar(60)
					N'' , -- OriginStreet3 - nvarchar(60)
					N'' , -- OriginCity - nvarchar(50)
					N'' , -- OriginStateProvCode - nvarchar(50)
					N'' , -- OriginPostalCode - nvarchar(20)
					N'' , -- OriginCountryCode - nvarchar(50)
					N'' , -- OriginPhone - nvarchar(25)
					N'' , -- OriginFax - nvarchar(35)
					N'' , -- OriginEmail - nvarchar(100)
					N'' , -- OriginWebsite - nvarchar(50)
					0 , -- ReturnShipment - bit
					0 , -- Insurance - bit
					0 , -- InsuranceProvider - int
					0 , -- ShipNameParseStatus - int
					N'' , -- ShipUnparsedName - nvarchar(100)
					0 , -- OriginNameParseStatus - int
					N''  -- OriginUnparsedName - nvarchar(100)
				)
		SELECT @ShipmentID = SCOPE_IDENTITY()

		-- Create a OnTracShipment
		INSERT INTO dbo.OnTracShipment(ShipmentID, OnTracAccountID, Service, IsCod, CodType, CodAmount, SaturdayDelivery, SignatureRequired, PackagingType, Instructions, DimsProfileID, DimsLength, DimsWidth, DimsHeight, DimsWeight, DimsAddWeight, Reference1, Reference2, InsuranceValue, InsurancePennyOne, DeclaredValue)
		VALUES  ( @ShipmentID , -- ShipmentID - bigint
		          @OnTrackAccountID , -- OnTracAccountID - bigint
		          0 , -- Service - int
		          0 , -- IsCod - bit
		          0 , -- CodType - int
		          0 , -- CodAmount - money
		          0 , -- SaturdayDelivery - bit
		          0 , -- SignatureRequired - bit
		          0 , -- PackagingType - int
		          N'' , -- Instructions - nvarchar(300)
		          0 , -- DimsProfileID - bigint
		          0.0 , -- DimsLength - float
		          0.0 , -- DimsWidth - float
		          0.0 , -- DimsHeight - float
		          0.0 , -- DimsWeight - float
		          0 , -- DimsAddWeight - bit
		          N'' , -- Reference1 - nvarchar(300)
		          0 , -- Reference2 - nvarchar(300)
		          0 , -- InsuranceValue - money
		          0 , -- InsurancePennyOne - bit
		          0  -- DeclaredValue - money
		        )

		-- Create a fake resource to use
		INSERT INTO dbo.Resource ( Data ,Checksum ,Compressed ,Filename)
		VALUES  ( NEWID(), -- Data - varbinary(max)
		          NEWID()  , -- Checksum - binary
		          0 , -- Compressed - bit
		          N'' + SUBSTRING(CAST(NEWID() AS NVARCHAR(50)), 0, 30)  -- Filename - nvarchar(30)
		        )
		SET @ResourceID = SCOPE_IDENTITY()

		INSERT INTO dbo.ObjectReference
		        ( ConsumerID , ReferenceKey ,ObjectID ,Reason
		        )
		VALUES  ( @ShipmentID , -- ConsumerID - bigint
		          N'' + SUBSTRING(CAST(NEWID() AS NVARCHAR(50)), 0, 30) , -- ReferenceKey - varchar(250)
		          @ResourceID , -- ObjectID - bigint
		          N''  -- Reason - nvarchar(250)
		        )
		SET @ObjectReferenceID = SCOPE_IDENTITY()

	END  
SET @Message = 'OnTrack:100 Percent Complete.  ' + CAST(@Count AS NVARCHAR(10)) + ' Rows Inserted.'
RAISERROR(@Message, 10, 1) WITH NOWAIT
  

-- Postal
SET @Count = 0
SET @PercentComplete = 0
WHILE (@Count < @NumberToAdd)
	BEGIN
		SET @Count = @Count + 1
		
		IF (@PercentComplete < (@Count / @PercentCompleteBreak) % 10)
		BEGIN
			SET @PercentComplete = (@Count / @PercentCompleteBreak) % 10
			SET @Message = 'Postal:  ' + CAST(@PercentComplete * 10 AS NVARCHAR(10)) + ' Percent Complete.  ' + CAST(@Count AS NVARCHAR(10)) + ' Rows Inserted.'
			RAISERROR(@Message, 10, 1) WITH NOWAIT
		END
        
		-- Grab a shipment
		INSERT INTO dbo.Shipment (OrderID, ShipmentType, ContentWeight, TotalWeight, Processed, ProcessedDate, ShipDate, ShipmentCost, Voided, VoidedDate, TrackingNumber, CustomsGenerated, CustomsValue, ThermalType, ShipFirstName, ShipMiddleName, ShipLastName, ShipCompany, ShipStreet1, ShipStreet2, ShipStreet3, ShipCity, ShipStateProvCode, ShipPostalCode, ShipCountryCode, ShipPhone, ShipEmail, ResidentialDetermination, ResidentialResult, OriginOriginID, OriginFirstName, OriginMiddleName, OriginLastName, OriginCompany, OriginStreet1, OriginStreet2, OriginStreet3, OriginCity, OriginStateProvCode, OriginPostalCode, OriginCountryCode, OriginPhone, OriginFax, OriginEmail, OriginWebsite, ReturnShipment, Insurance, InsuranceProvider, ShipNameParseStatus, ShipUnparsedName, OriginNameParseStatus, OriginUnparsedName )
		VALUES  (   @OrderID , -- OrderID - bigint
					CASE 
						WHEN @Count % 2 = 0 THEN 2
						WHEN @Count % 3 = 0 THEN 3
						WHEN @Count % 4 = 0 THEN 4
						WHEN @Count % 9 = 0 THEN 9
						ELSE 2
					END , -- ShipmentType - int
					0.0 , -- ContentWeight - float,
					0.0 , -- TotalWeight - float
					1 , -- Processed - bit
					DATEADD(d,  @count % @NumberOfDays, @StartDate) , -- ProcessedDate - datetime
					'2013-07-25 13:24:39' , -- ShipDate - datetime
					0 , -- ShipmentCost - money
					0 , -- Voided - bit
					null , -- VoidedDate - datetime
					N'' , -- TrackingNumber - nvarchar(50)
					0 , -- CustomsGenerated - bit
					0 , -- CustomsValue - money
					0 , -- ThermalType - int
					N'' , -- ShipFirstName - nvarchar(30)
					N'' , -- ShipMiddleName - nvarchar(30)
					N'' , -- ShipLastName - nvarchar(30)
					N'' , -- ShipCompany - nvarchar(60)
					N'' , -- ShipStreet1 - nvarchar(60)
					N'' , -- ShipStreet2 - nvarchar(60)
					N'' , -- ShipStreet3 - nvarchar(60)
					N'' , -- ShipCity - nvarchar(50)
					N'' , -- ShipStateProvCode - nvarchar(50)
					N'' , -- ShipPostalCode - nvarchar(20)
					N'' , -- ShipCountryCode - nvarchar(50)
					N'' , -- ShipPhone - nvarchar(25)
					N'' , -- ShipEmail - nvarchar(100)
					0 , -- ResidentialDetermination - int
					1 , -- ResidentialResult - bit
					0 , -- OriginOriginID - bigint
					N'' , -- OriginFirstName - nvarchar(30)
					N'' , -- OriginMiddleName - nvarchar(30)
					N'' , -- OriginLastName - nvarchar(30)
					N'' , -- OriginCompany - nvarchar(60)
					N'' , -- OriginStreet1 - nvarchar(60)
					N'' , -- OriginStreet2 - nvarchar(60)
					N'' , -- OriginStreet3 - nvarchar(60)
					N'' , -- OriginCity - nvarchar(50)
					N'' , -- OriginStateProvCode - nvarchar(50)
					N'' , -- OriginPostalCode - nvarchar(20)
					N'' , -- OriginCountryCode - nvarchar(50)
					N'' , -- OriginPhone - nvarchar(25)
					N'' , -- OriginFax - nvarchar(35)
					N'' , -- OriginEmail - nvarchar(100)
					N'' , -- OriginWebsite - nvarchar(50)
					0 , -- ReturnShipment - bit
					0 , -- Insurance - bit
					0 , -- InsuranceProvider - int
					0 , -- ShipNameParseStatus - int
					N'' , -- ShipUnparsedName - nvarchar(100)
					0 , -- OriginNameParseStatus - int
					N''  -- OriginUnparsedName - nvarchar(100)
				)
		SELECT @ShipmentID = SCOPE_IDENTITY()

		-- Create a PostalShipment
		INSERT INTO dbo.PostalShipment (ShipmentID, Service, Confirmation, PackagingType, DimsProfileID, DimsLength, DimsWidth, DimsHeight, DimsWeight, DimsAddWeight, NonRectangular, NonMachinable, CustomsContentType, CustomsContentDescription, InsuranceValue, ExpressSignatureWaiver, SortType, EntryFacility  )
		VALUES  ( @ShipmentID , -- ShipmentID - bigint
		          0 , -- Service - int
		          0 , -- Confirmation - int
		          0 , -- PackagingType - int
		          0 , -- DimsProfileID - bigint
		          0.0 , -- DimsLength - float
		          0.0 , -- DimsWidth - float
		          0.0 , -- DimsHeight - float
		          0.0 , -- DimsWeight - float
		          0 , -- DimsAddWeight - bit
		          0 , -- NonRectangular - bit
		          0 , -- NonMachinable - bit
		          0 , -- CustomsContentType - int
		          N'' , -- CustomsContentDescription - nvarchar(50)
		          0 , -- InsuranceValue - money
		          0 , -- ExpressSignatureWaiver - bit
		          0 , -- SortType - int
		          0  -- EntryFacility - int
		        )

		-- Create a fake resource to use
		INSERT INTO dbo.Resource ( Data ,Checksum ,Compressed ,Filename)
		VALUES  ( NEWID(), -- Data - varbinary(max)
		          NEWID()  , -- Checksum - binary
		          0 , -- Compressed - bit
		          N'' + SUBSTRING(CAST(NEWID() AS NVARCHAR(50)), 0, 30)  -- Filename - nvarchar(30)
		        )
		SET @ResourceID = SCOPE_IDENTITY()

		INSERT INTO dbo.ObjectReference
		        ( ConsumerID , ReferenceKey ,ObjectID ,Reason
		        )
		VALUES  ( @ShipmentID , -- ConsumerID - bigint
		          N'' + SUBSTRING(CAST(NEWID() AS NVARCHAR(50)), 0, 30) , -- ReferenceKey - varchar(250)
		          @ResourceID , -- ObjectID - bigint
		          N''  -- Reason - nvarchar(250)
		        )
		SET @ObjectReferenceID = SCOPE_IDENTITY()

	END  
SET @Message = 'Postal: 100 Percent Complete.  ' + CAST(@Count AS NVARCHAR(10)) + ' Rows Inserted.'
RAISERROR(@Message, 10, 1) WITH NOWAIT
  

SET @Message = 'Done. ' + CAST(5 * @Count AS NVARCHAR(10)) + ' Rows Inserted.'
RAISERROR(@Message, 10, 1) WITH NOWAIT
SET @ProcessingEnd = GETUTCDATE()

SELECT DATEDIFF(second, @ProcessingStart, @ProcessingEnd) AS 'Time To Process'

SELECT ProcessedDate, COUNT(ProcessedDate) 
FROM dbo.Shipment 
WHERE ShipmentID IN (SELECT TOP (5 * @NumberToAdd) ShipmentID FROM Shipment ORDER BY ShipmentID DESC)
GROUP BY ProcessedDate 
ORDER BY ProcessedDate

SELECT COUNT(*) AS 'Shipments' FROM dbo.Shipment
SELECT COUNT(*) AS 'UpsPackages' FROM dbo.UpsPackage
SELECT COUNT(*) AS 'FedExPackages' FROM dbo.FedExPackage
SELECT COUNT(*) AS 'iParcelPackages' FROM dbo.iParcelPackage
SELECT COUNT(*) AS 'Resources' FROM dbo.Resource 
SELECT COUNT(*) AS 'ObjectReferences' FROM dbo.ObjectReference

go

