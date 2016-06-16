IF OBJECT_ID('dbo.v2m_MigrationKeys') IS NULL
CREATE TABLE [dbo].[v2m_MigrationKeys](
	[OriginalKey] [int] NOT NULL,
	[KeyTypeCode] [int] NOT NULL,
	[NewKey] [bigint] NOT NULL,
CONSTRAINT [PK_MigrationKeys] PRIMARY KEY CLUSTERED ( [OriginalKey], [KeyTypeCode] ASC ))
GO

IF OBJECT_ID(N'dbo.v2m_RecordKey') IS NOT NULL
	DROP PROCEDURE dbo.v2m_RecordKey
GO

CREATE PROCEDURE dbo.v2m_RecordKey (@oldKey int, @oldKeyType int, @newKey bigint)
AS 
BEGIN
	-- insert the old->new mapping in the main SW database
	INSERT INTO dbo.v2m_MigrationKeys
	VALUES 
	(
		@oldKey,
		@oldKeyType,
		@newKey
	)
	
END
GO

IF OBJECT_ID(N'dbo.v2m_RecordObjectLabel') IS NOT NULL
	DROP PROCEDURE dbo.v2m_RecordObjectLabel
GO

-- Records old and new database keys for upgraded records
CREATE PROCEDURE dbo.v2m_RecordObjectLabel (@tObjectID bigint, @tObjectType int, @tParentID bigint, @tLabel varchar(100))
AS 
BEGIN
    INSERT INTO {MASTERDATABASE}.dbo.ObjectLabel  (
        [ObjectID],        
        [ObjectType],
        [ParentID],
        [Label],
        [IsDeleted]
    )
    VALUES
    (
        @tObjectID,
        @tObjectType,
        @tParentID,
        @tLabel,
        0
    )             
END
GO

IF OBJECT_ID (N'dbo.v2m_GetOriginalArchiveKey') IS NOT NULL
	DROP FUNCTION dbo.v2m_GetOriginalArchiveKey
GO
-- Gets the original key value for an archived V2 entity
CREATE FUNCTION dbo.v2m_GetOriginalArchiveKey (@archiveKey int, @archiveKeyType int)
RETURNS int
WITH EXECUTE AS CALLER
AS
BEGIN
	DECLARE @translatedKey int
	
	SELECT @translatedKey = SourceKey
	FROM ArchiveLogs
	WHERE TargetKey = @archiveKey
	AND [Type] = @archiveKeyType
	
	RETURN @translatedKey
END
GO

IF OBJECT_ID (N'dbo.v2m_TranslateKey') IS NOT NULL
   DROP FUNCTION dbo.v2m_TranslateKey
GO
-- Gets the V3 migration key for an entity already upgraded to V3
CREATE FUNCTION dbo.v2m_TranslateKey (@oldKey int, @oldKeyType int)
RETURNS bigint
WITH EXECUTE AS CALLER
AS
BEGIN
     DECLARE 
		@translatedKey bigint
		
	 SELECT @translatedKey = NewKey
	 FROM dbo.v2m_MigrationKeys
	 WHERE OriginalKey = @oldKey
	 AND [KeyTypeCode] = @oldKeyType
			 
	 return @translatedKey
END
GO

IF OBJECT_ID (N'dbo.v2m_TranslateKeyGlobal') IS NOT NULL
   DROP FUNCTION dbo.v2m_TranslateKeyGlobal
GO
-- Gets the V3 migration key for an entity already upgraded to V3
CREATE FUNCTION dbo.v2m_TranslateKeyGlobal (@oldKey int, @oldKeyType int)
RETURNS bigint
WITH EXECUTE AS CALLER
AS
BEGIN
     DECLARE 
		@translatedKey bigint
		
	 SELECT @translatedKey = NewKey
	 FROM {MASTERDATABASE}.dbo.v2m_MigrationKeys
	 WHERE OriginalKey = @oldKey
	 AND [KeyTypeCode] = @oldKeyType
			 
	 return @translatedKey
END
GO

IF OBJECT_ID (N'dbo.v2m_ConvertToV3TimeZone') IS NOT NULL
	DROP FUNCTION dbo.v2m_ConvertToV3TimeZone
GO

CREATE FUNCTION dbo.v2m_ConvertToV3TimeZone (@v2TimeZoneID int)
RETURNS varchar(30)
WITH EXECUTE AS CALLER
AS 
BEGIN
	RETURN CASE
		WHEN @v2TimeZoneID = 0 THEN 'Dateline Standard Time'
		WHEN @v2TimeZoneID = 1 THEN 'Samoa Standard Time'
		WHEN @v2TimeZoneID = 2 THEN 'Hawaiian Standard Time'
		WHEN @v2TimeZoneID = 3 THEN 'Alaskan Standard Time'
		WHEN @v2TimeZoneID = 4 THEN 'Pacific Standard Time'
		WHEN @v2TimeZoneID = 10 THEN 'Mountain Standard Time'
		WHEN @v2TimeZoneID = 13 THEN 'Mountain Standard Time (Mexico)'
		WHEN @v2TimeZoneID = 15 THEN 'US Mountain Standard Time'
		WHEN @v2TimeZoneID = 20 THEN 'Central Standard Time'
		WHEN @v2TimeZoneID = 25 THEN 'Canada Central Standard Time'
		WHEN @v2TimeZoneID = 30 THEN 'Central Standard Time (Mexico)'
		WHEN @v2TimeZoneID = 33 THEN 'Central America Standard Time'
		WHEN @v2TimeZoneID = 35 THEN 'Eastern Standard Time'
		WHEN @v2TimeZoneID = 40 THEN 'US Eastern Standard Time'
		WHEN @v2TimeZoneID = 45 THEN 'SA Pacific Standard Time'
		WHEN @v2TimeZoneID = 50 THEN 'Atlantic Standard Time'
		WHEN @v2TimeZoneID = 55 THEN 'Venezuela Standard Time'
		WHEN @v2TimeZoneID = 56 THEN 'Pacific SA Standard Time'
		WHEN @v2TimeZoneID = 65 THEN 'E. South America Standard Time'
		WHEN @v2TimeZoneID = 70 THEN 'Argentina Standard Time'
		WHEN @v2TimeZoneID = 73 THEN 'Greenland Standard Time'
		WHEN @v2TimeZoneID = 75 THEN 'Mid-Atlantic Standard Time'
		WHEN @v2TimeZoneID = 80 THEN 'Azores Standard Time'
		WHEN @v2TimeZoneID = 83 THEN 'Cape Verde Standard Time'
		WHEN @v2TimeZoneID = 85 THEN 'GMT Standard Time'
		WHEN @v2TimeZoneID = 90 THEN 'Morocco Standard Time'
		WHEN @v2TimeZoneID = 95 THEN 'Central Europe Standard Time'
		WHEN @v2TimeZoneID = 100 THEN 'Central European Standard Time'
		WHEN @v2TimeZoneID = 105 THEN 'Romance Standard Time'
		WHEN @v2TimeZoneID = 110 THEN 'W. Europe Standard Time'
		WHEN @v2TimeZoneID = 113 THEN 'W. Central Africa Standard Time'
		WHEN @v2TimeZoneID = 115 THEN 'GTB Standard Time'
		WHEN @v2TimeZoneID = 120 THEN 'Egypt Standard Time'
		WHEN @v2TimeZoneID = 125 THEN 'FLE Standard Time'
		WHEN @v2TimeZoneID = 130 THEN 'GTB Standard Time'
		WHEN @v2TimeZoneID = 135 THEN 'Israel Standard Time'
		WHEN @v2TimeZoneID = 140 THEN 'South Africa Standard Time'
		WHEN @v2TimeZoneID = 145 THEN 'Russian Standard Time'
		WHEN @v2TimeZoneID = 150 THEN 'Arab Standard Time'
		WHEN @v2TimeZoneID = 155 THEN 'E. Africa Standard Time'
		WHEN @v2TimeZoneID = 158 THEN 'Arabic Standard Time '
		WHEN @v2TimeZoneID = 165 THEN 'Arabian Standard Time'
		WHEN @v2TimeZoneID = 170 THEN 'Caucasus Standard Time'
		WHEN @v2TimeZoneID = 180 THEN 'Ekaterinburg Standard Time'
		WHEN @v2TimeZoneID = 185 THEN 'Pakistan Standard Time'
		WHEN @v2TimeZoneID = 195 THEN 'Central Asia Standard Time'
		WHEN @v2TimeZoneID = 200 THEN 'Sri Lanka Standard Time'
		WHEN @v2TimeZoneID = 201 THEN 'N. Central Asia Standard Time'
		WHEN @v2TimeZoneID = 205 THEN 'SE Asia Standard Time'
		WHEN @v2TimeZoneID = 207 THEN 'North Asia Standard Time'
		WHEN @v2TimeZoneID = 210 THEN 'China Standard Time'
		WHEN @v2TimeZoneID = 215 THEN 'Singapore Standard Time'
		WHEN @v2TimeZoneID = 220 THEN 'Taipei Standard Time'
		WHEN @v2TimeZoneID = 225 THEN 'W. Australia Standard Time'
		WHEN @v2TimeZoneID = 227 THEN 'Ulaanbaatar Standard Time'
		WHEN @v2TimeZoneID = 230 THEN 'Korea Standard Time'
		WHEN @v2TimeZoneID = 235 THEN 'Tokyo Standard Time'
		WHEN @v2TimeZoneID = 240 THEN 'Yakutsk Standard Time'
		WHEN @v2TimeZoneID = 255 THEN 'AUS Eastern Standard Time'
		WHEN @v2TimeZoneID = 260 THEN 'E. Australia Standard Time'
		WHEN @v2TimeZoneID = 265 THEN 'Tasmania Standard Time'
		WHEN @v2TimeZoneID = 270 THEN 'Vladivostok Standard Time'
		WHEN @v2TimeZoneID = 275 THEN 'West Pacific Standard Time'
		WHEN @v2TimeZoneID = 280 THEN 'Central Pacific Standard Time'
		WHEN @v2TimeZoneID = 285 THEN 'Fiji Standard Time'
		WHEN @v2TimeZoneID = 290 THEN 'New Zealand Standard Time'
		ELSE 'GMT Standard Time'
	END
END
GO

IF OBJECT_ID(N'dbo.v2m_CreateNote') IS NOT NULL
	DROP PROCEDURE dbo.v2m_CreateNote
GO	

-- Creates a note in the master SW database during the migration process
CREATE PROCEDURE dbo.v2m_CreateNote (@ObjectID bigint, @Edited datetime, @Text nvarchar(max), @Source int, @Visibility int)
AS 
BEGIN

	DECLARE @newNoteKey bigint,
			@editDateString varchar(100)

	INSERT INTO {MASTERDATABASE}.dbo.Note  (
	    [ObjectID],
	    [UserID],
	    [Edited],
	    [Text],
	    [Source],
	    [Visibility]
    )
    VALUES
    (
	    @ObjectID,
	    1027309002,
	    @Edited,
	    @Text,
	    @Source,
	    @Visibility
    )

	SET @newNoteKey = @@IDENTITY

	SET @editDateString = convert(varchar, @Edited, 101)
	
	-- record object label	
	EXEC dbo.v2m_RecordObjectLabel @newNoteKey, 44, @ObjectID, @editDateString
END
GO

IF OBJECT_ID(N'dbo.v2m_TranslateUpsServiceCode') IS NOT NULL
	DROP FUNCTION dbo.v2m_TranslateUpsServiceCode
GO

-- Gets the original key value for an archived V2 entity
CREATE FUNCTION dbo.v2m_TranslateUpsServiceCode(@v2ServiceCode varchar(10))
RETURNS int
WITH EXECUTE AS CALLER
AS
BEGIN
	DECLARE @translatedCode int

	SET @translatedCode = 
	CASE 
		WHEN @v2ServiceCode = '01' THEN 4	-- Next Day Air
		WHEN @v2ServiceCode = '02' THEN 2	-- Second Day Air
		WHEN @v2ServiceCode = '03' THEN 0	-- Ground
		WHEN @v2ServiceCode = '07' THEN 7   -- Worldwide Express
		WHEN @v2ServiceCode = '08' THEN 9   -- Worldwide Expedited
		WHEN @v2ServiceCode = '11' THEN 11	-- Standard
		WHEN @v2ServiceCode = '12' THEN 1   -- 3 Day Select		
		WHEN @v2ServiceCode = '13' THEN 5   -- Next Day Air Saver
		WHEN @v2ServiceCode = '14' THEN 6	-- Next Day Air Early AM
		WHEN @v2ServiceCode = '54' THEN 8	-- Worldwide Express Plus
		WHEN @v2ServiceCode = '59' THEN 3	-- Second Day Air AM
		WHEN @v2ServiceCode = '65' THEN 10	-- Worldwide saver
		ELSE 11
	END

	RETURN @translatedCode
END
GO

IF OBJECT_ID(N'dbo.v2m_TranslateUpsPackageTypeCode') IS NOT NULL
	DROP FUNCTION dbo.v2m_TranslateUpsPackageTypeCode
GO

CREATE FUNCTION dbo.v2m_TranslateUpsPackageTypeCode(@v2PackageTypeCode varchar(10))
RETURNS int
WITH EXECUTE AS CALLER
AS
BEGIN
	DECLARE @translatedCode int
	
	SET @translatedCode = 
	CASE
		WHEN @v2PackageTypeCode = '01' THEN 1 -- UPS Letter
		WHEN @v2PackageTypeCode = '02' THEN 0 -- Custom
		WHEN @v2PackageTypeCode = '03' THEN 2 -- UPS Tube
		WHEN @v2PackageTypeCode = '04' THEN 3 -- UPS Pak
		WHEN @v2PackageTypeCode = '21' THEN 4 -- UPS Express Box (calling it small in 3)
		WHEN @v2PackageTypeCode = '24' THEN 7 -- 25KG Box
		WHEN @v2PackageTypeCode = '25' THEN 8 -- 10KG Box
		ELSE 0										-- call everything else Custom
	END

	 RETURN @translatedCode
END
GO

IF OBJECT_ID(N'dbo.v2m_ParseName') IS NOT NULL
	DROP PROCEDURE dbo.v2m_ParseName
GO

CREATE PROCEDURE v2m_ParseName( @fullname varchar(200), @firstName varchar(100) OUTPUT, 
							@middleName varchar(100) OUTPUT, @lastName varchar(100) OUTPUT)

AS
BEGIN

DECLARE @index int	

	SET @firstName = ''
	SET @middleName = ''
	SET @lastName = ''

	-- We assume as space separates parts.  If a name ends in a space it goofs it up
	SET @fullname = LTRIM(RTRIM(@fullname));

	SET @index = CHARINDEX(' ', @fullname)	
	IF @index > 0
	BEGIN
		SET @firstName = SUBSTRING(@fullname, 1, @index - 1)
		SET @lastName = LTRIM(RTRIM(SUBSTRING(@fullname, @index + 1, LEN(@fullname) - @index)))

		SET @index = CHARINDEX(' ', @lastName)
		if (@index > 0)
		BEGIN
			SET @middleName = SUBSTRING(@lastName, 1, @index - 1)
			SET @lastName = LTRIM(RTRIM(SUBSTRING(@lastName, @index + 1, LEN(@lastName) - @index)))
		END
	END
	ELSE
	BEGIN
		SET @firstName = @fullname;
		SET @lastName = ''
		SET @middleName = ''
	END
	
	RETURN 1
END


IF OBJECT_ID(N'dbo.v2m_TranslateUspsPackageTypeCode') IS NOT NULL
	DROP FUNCTION dbo.v2m_TranslateUspsPackageTypeCode
GO

CREATE FUNCTION dbo.v2m_TranslateUspsPackageTypeCode(@v2PackageTypeCode int)
RETURNS int
WITH EXECUTE AS CALLER
AS
BEGIN
	RETURN
		 CASE
			WHEN @v2PackageTypeCode = 0 THEN 0
			WHEN @v2PackageTypeCode = 1 THEN 0	-- Postcard -> Package
			WHEN @v2PackageTypeCode = 2 THEN 1  -- envelope -> envelope
			WHEN @v2PackageTypeCode = 3 THEN 0	-- Flat -> package
			WHEN @v2PackageTypeCode = 4 THEN 0	-- Rect Parcel 
			WHEN @v2PackageTypeCode = 5 THEN 0	-- NoRect Parcel 
			WHEN @v2PackageTypeCode = 6 THEN 3  -- FlatRateEnvelope -> FlatRateEnvelope
			WHEN @v2PackageTypeCode = 7 THEN 4  -- FlatRateBox -> FlatRateBox
			WHEN @v2PackageTypeCode = 8 THEN 4  -- FlatRateLargeBox -> FlatRateBox
			WHEN @v2PackageTypeCode = 9 THEN 4  -- FlatRateSmallBox -> FlatRateBox
			WHEN @v2PackageTypeCode = 10 THEN 3  -- FlatRatePaddedEnvelope -> FlatRateEnvelope
			WHEN @v2PackageTypeCode = 11 THEN 8  -- RegionalRateBoxA -> RateRegionalBoxA
			WHEN @v2PackageTypeCode = 12 THEN 9  -- RegionalRateBoxB -> RateRegionalBoxB
			ELSE 0
	    END
END
GO
