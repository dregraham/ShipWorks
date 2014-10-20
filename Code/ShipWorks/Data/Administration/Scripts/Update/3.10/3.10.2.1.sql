/*
Shipment Type Codes

Endicia			= 2,
Express1Endicia = 9,

Stamps			= 3,
Express1Stamps	= 13,

USPS            = 15,

PostalWebTools	= 4,

OnTrac = 11,
i-parcel = 12
*/
-- Determine if Web Tools is configured since it doesn't have accounts
declare @WebToolsConfigured bit
;with ConfiguredTypeCodes(ShippingSettingsID, ShipmentTypeCode, Data) as (
		select ShippingSettingsID, 
			cast(LEFT(Configured, CHARINDEX(',',Configured+',')-1) as nvarchar(50)), 
			STUFF(Configured, 1, CHARINDEX(',',Configured+','), '')
		from ShippingSettings
		union all
		select ShippingSettingsID, 
			cast(LEFT(Data    , CHARINDEX(',',Data + ','  )-1) as nvarchar(50)), 
			STUFF(Data,     1, CHARINDEX(',', Data + ','),  '')
		from ConfiguredTypeCodes
		where Data > ''
)
select @WebToolsConfigured = count(*)
from ConfiguredTypeCodes
where ShipmentTypeCode = 4

-- Now check the Excluded types
declare @ExcludedList nvarchar(20)
set @ExcludedList = ''

-- Get a list of currently excluded types so that we can keep them excluded
;with ExcludedTypeCodes(ShippingSettingsID, ShipmentTypeCode, Data) as (
	select ShippingSettingsID, 
		cast(LEFT(Excluded, CHARINDEX(',',Excluded+',')-1) as nvarchar(50)), 
		STUFF(Excluded, 1, CHARINDEX(',',Excluded+','), '')
	from ShippingSettings
	union all
	select ShippingSettingsID, 
		cast(LEFT(Data    , CHARINDEX(',',Data + ','  )-1) as nvarchar(50)), 
		STUFF(Data,     1, CHARINDEX(',', Data + ','),  '')
	from ExcludedTypeCodes
	where Data > ''
),
-- Now get a list of postal types that do not have accounts so that we can exclude them
ShipmentTypes (ShipmentTypeCode) as (
	select 2 where not exists (select 1 from EndiciaAccount where EndiciaReseller = 0)
	union
	select 9 where not exists (select 1 from EndiciaAccount where EndiciaReseller = 1)
	union 
	select 3 where not exists (select 1 from StampsAccount where [StampsReseller] = 0)
	union 
	select 13 where not exists (select 1 from StampsAccount where [StampsReseller] = 1)
	union 
	select 11 where not exists (select 1 from OnTracAccount)
	union 
	select 12 where not exists (select 1 from iParcelAccount)
), 
-- Now union the:
	-- Currently excluded types
	-- Ones that don't have accounts
	-- And WebTools if it isn't configured
NewExcludedShipmentTypeCodes (ShipmentTypeCode) as (
	select ShipmentTypeCode from ExcludedTypeCodes where ShipmentTypeCode <> ''
	union
	select ShipmentTypeCode from ShipmentTypes where ShipmentTypeCode <> ''
	union 
	select 4 where @WebToolsConfigured = 0
)

-- Create the comma separated list of excluded types
SELECT @ExcludedList = COALESCE(@ExcludedList + ',', '') + CAST(ShipmentTypeCode AS nvarchar(2)) 
FROM NewExcludedShipmentTypeCodes 

-- Remove the first comma
set @ExcludedList = SUBSTRING(@ExcludedList, 2, 50)

-- Update the excluded list to what we've discovered
update ShippingSettings set Excluded = @ExcludedList
