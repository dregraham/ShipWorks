-- ShipWorks V2 to V3 Database Migration Script, generated by CodeSmith
-- For table v2m_UpsShippers

-- operational variables
DECLARE 
    @workCounter int,
    @newUpsAccountKey bigint

-- source table variables
DECLARE
    -- @MasterDatabase will be generated at runtime
    -- @IsArchive will be generated at runtime    
    @sUpsShipperID int, 
    @sRowVersion timestamp, 
    @sShipperNumber nvarchar(10), 
    @sUserID nvarchar(25), 
    @sPassword nvarchar(25), 
    @sAccessKey nvarchar(50), 
    @sUsePickupLocation bit, 
    @sPrimaryCompanyOrName nvarchar(30), 
    @sPrimaryAttention nvarchar(30), 
    @sPrimaryAddress1 nvarchar(60), 
    @sPrimaryAddress2 nvarchar(60), 
    @sPrimaryAddress3 nvarchar(60), 
    @sPrimaryCity nvarchar(50), 
    @sPrimaryStateProvinceCode nvarchar(5), 
    @sPrimaryPostalCode nvarchar(10), 
    @sPrimaryCountryCode nvarchar(5), 
    @sPrimaryContactTitle nvarchar(25), 
    @sPrimaryContactEmail nvarchar(25), 
    @sPrimaryContactPhone nvarchar(25), 
    @sPrimaryContactFax nvarchar(25), 
    @sPickupCompanyOrName nvarchar(30), 
    @sPickupAttention nvarchar(30), 
    @sPickupAddress1 nvarchar(60), 
    @sPickupAddress2 nvarchar(60), 
    @sPickupAddress3 nvarchar(60), 
    @sPickupCity nvarchar(50), 
    @sPickupStateProvinceCode nvarchar(5), 
    @sPickupPostalCode nvarchar(10), 
    @sPickupCountryCode nvarchar(5), 
    @sPickupContactTitle nvarchar(25), 
    @sPickupContactEmail nvarchar(25), 
    @sPickupContactPhone nvarchar(25), 
    @sPickupContactFax nvarchar(25), 
    @sNegotiatedRates bit 

-- target table variables
DECLARE
    @tUpsAccountID bigint, 
    @tRowVersion timestamp, 
    @tDescription nvarchar(50), 
    @tAccountNumber nvarchar(10), 
    @tUserID nvarchar(25), 
    @tPassword nvarchar(25), 
    @tRateType int, 
    @tFirstName nvarchar(30), 
    @tMiddleName nvarchar(30), 
    @tLastName nvarchar(30), 
    @tCompany nvarchar(30), 
    @tStreet1 nvarchar(60), 
    @tStreet2 nvarchar(60), 
    @tStreet3 nvarchar(60), 
    @tCity nvarchar(50), 
    @tStateProvCode nvarchar(50), 
    @tPostalCode nvarchar(20), 
    @tCountryCode nvarchar(50), 
    @tPhone nvarchar(25), 
    @tEmail nvarchar(50), 
    @tWebsite nvarchar(50) 

-- Track Progress
SET @workCounter = 0

-- the cursor for cycling through the source table
DECLARE workCursor CURSOR FORWARD_ONLY FOR
SELECT 
	[UpsShipperID],
    [RowVersion],
    [ShipperNumber],
    [UserID],
    [Password],
    [AccessKey],
    [UsePickupLocation],
    [PrimaryCompanyOrName],
    [PrimaryAttention],
    [PrimaryAddress1],
    [PrimaryAddress2],
    [PrimaryAddress3],
    [PrimaryCity],
    [PrimaryStateProvinceCode],
    [PrimaryPostalCode],
    [PrimaryCountryCode],
    [PrimaryContactTitle],
    [PrimaryContactEmail],
    [PrimaryContactPhone],
    [PrimaryContactFax],
    [PickupCompanyOrName],
    [PickupAttention],
    [PickupAddress1],
    [PickupAddress2],
    [PickupAddress3],
    [PickupCity],
    [PickupStateProvinceCode],
    [PickupPostalCode],
    [PickupCountryCode],
    [PickupContactTitle],
    [PickupContactEmail],
    [PickupContactPhone],
    [PickupContactFax],
    [NegotiatedRates]
    FROM v2m_UpsShippers

-- open the source table cursor
OPEN workCursor

-- populate source table variables from the source cursor
FETCH NEXT FROM workCursor
INTO
    @sUpsShipperID,
    @sRowVersion,
    @sShipperNumber,
    @sUserID,
    @sPassword,
    @sAccessKey,
    @sUsePickupLocation,
    @sPrimaryCompanyOrName,
    @sPrimaryAttention,
    @sPrimaryAddress1,
    @sPrimaryAddress2,
    @sPrimaryAddress3,
    @sPrimaryCity,
    @sPrimaryStateProvinceCode,
    @sPrimaryPostalCode,
    @sPrimaryCountryCode,
    @sPrimaryContactTitle,
    @sPrimaryContactEmail,
    @sPrimaryContactPhone,
    @sPrimaryContactFax,
    @sPickupCompanyOrName,
    @sPickupAttention,
    @sPickupAddress1,
    @sPickupAddress2,
    @sPickupAddress3,
    @sPickupCity,
    @sPickupStateProvinceCode,
    @sPickupPostalCode,
    @sPickupCountryCode,
    @sPickupContactTitle,
    @sPickupContactEmail,
    @sPickupContactPhone,
    @sPickupContactFax,
    @sNegotiatedRates
WHILE @@FETCH_STATUS = 0
BEGIN
    SET @workCounter = @workCounter + 1


    SET @tDescription = @sPrimaryCompanyOrName + ' - ' + @sShipperNumber;
    SET @tAccountNumber = @sShipperNumber
    SET @tUserID = @sUserID
    SET @tPassword = @sPassword
    SET @tRateType = CASE 
						WHEN @sNegotiatedRates > 0 THEN 3 
						ELSE 0 
					 END-- negotiated or not
    SET @tCompany = @sPrimaryCompanyOrName
	EXEC dbo.v2m_ParseName @sPrimaryAttention, @tFirstName OUT, @tMiddleName OUT, @tLastName OUT
    SET @tStreet1 = @sPrimaryAddress1
    SET @tStreet2 = @sPrimaryAddress2
    SET @tStreet3 = @sPrimaryAddress3
    SET @tCity = @sPrimaryCity
    SET @tStateProvCode = @sPrimaryStateProvinceCode
    SET @tPostalCode = @sPrimaryPostalCode
    SET @tCountryCode = @sPrimaryCountryCode
    SET @tPhone = @sPrimaryContactPhone
    SET @tEmail = @sPrimaryContactEmail
    SET @tWebsite = ''

    INSERT INTO dbo.UpsAccount  (
	    [Description],
	    [AccountNumber],
	    [UserID],
	    [Password],
	    [RateType],
	    [FirstName],
	    [MiddleName],
	    [LastName],
	    [Company],
	    [Street1],
	    [Street2],
	    [Street3],
	    [City],
	    [StateProvCode],
	    [PostalCode],
	    [CountryCode],
	    [Phone],
	    [Email],
	    [Website]
    )
    VALUES
    (
	    @tDescription,
	    @tAccountNumber,
	    @tUserID,
	    @tPassword,
	    @tRateType,
	    @tFirstName,
	    @tMiddleName,
	    @tLastName,
	    @tCompany,
	    @tStreet1,
	    @tStreet2,
	    @tStreet3,
	    @tCity,
	    @tStateProvCode,
	    @tPostalCode,
	    @tCountryCode,
	    @tPhone,
	    @tEmail,
	    @tWebsite
    )             
    
    -- get the new key
	SET @newUpsAccountKey = @@IDENTITY
	
	-- record it
	EXEC dbo.v2m_RecordKey @sUpsShipperID, 13, @newUpsAccountKey

	-- Save off the AccessKey for later. In V3 its in the global ShippingSettings row (that doesn't exist yet), and not at the account level
	INSERT INTO dbo.v2m_UpsAccessKey (UpsAccessKey) VALUES(@sAccessKey);

	IF (@sUsePickupLocation = 1)
	BEGIN
		EXEC dbo.v2m_ParseName @sPickupAttention, @tFirstName OUT, @tMiddleName OUT, @tLastName OUT
		
		DECLARE @description nvarchar(50);
		SET @description = 'Alternate for UPS Account# ' + @tAccountNumber;

		DECLARE @alternateCount int;
		SELECT @alternateCount = COUNT(*) FROM dbo.ShippingOrigin WHERE UPPER([Description]) LIKE UPPER(@description) + '%';

		IF (@alternateCount > 0)
		BEGIN
			SET @description = @description + ' (' + CONVERT(nvarchar(10), @alternateCount + 1) + ')';
		END

		-- setup an origin based on the pickup location
	    INSERT INTO dbo.ShippingOrigin  (
		    [Description],
		    [FirstName],
		    [MiddleName],
		    [LastName],
		    [Company],
		    [Street1],
		    [Street2],
		    [Street3],
		    [City],
		    [StateProvCode],
		    [PostalCode],
		    [CountryCode],
		    [Phone],
		    [Fax],
		    [Email],
		    [Website]
	    )
	    VALUES
	    (
		    @description,
		    @tFirstName,
		    @tMiddleName,
		    @tLastName,
		    @sPickupCompanyOrName,
		    @sPickupAddress1,
		    @sPickupAddress2,
		    @sPickupAddress3,
		    @sPickupCity,
		    @sPickupStateProvinceCode,
		    @sPickupPostalCode,
		    @sPickupCountryCode,
		    @sPickupContactPhone,
		    @sPickupContactFax,
		    @sPickupContactEmail,
		    ''
	    )             
    END

	-- delete the old
	DELETE FROM dbo.v2m_UpsShippers WHERE UpsShipperID = @sUpsShipperID

-- fetch next row from source table
FETCH NEXT FROM workCursor
INTO
    @sUpsShipperID,
    @sRowVersion,
    @sShipperNumber,
    @sUserID,
    @sPassword,
    @sAccessKey,
    @sUsePickupLocation,
    @sPrimaryCompanyOrName,
    @sPrimaryAttention,
    @sPrimaryAddress1,
    @sPrimaryAddress2,
    @sPrimaryAddress3,
    @sPrimaryCity,
    @sPrimaryStateProvinceCode,
    @sPrimaryPostalCode,
    @sPrimaryCountryCode,
    @sPrimaryContactTitle,
    @sPrimaryContactEmail,
    @sPrimaryContactPhone,
    @sPrimaryContactFax,
    @sPickupCompanyOrName,
    @sPickupAttention,
    @sPickupAddress1,
    @sPickupAddress2,
    @sPickupAddress3,
    @sPickupCity,
    @sPickupStateProvinceCode,
    @sPickupPostalCode,
    @sPickupCountryCode,
    @sPickupContactTitle,
    @sPickupContactEmail,
    @sPickupContactPhone,
    @sPickupContactFax,
    @sNegotiatedRates
END
CLOSE workCursor
DEALLOCATE workCursor

-- data migration "protocol" demands we return the number of rows/work completed
SELECT @workCounter as WorkCompleted