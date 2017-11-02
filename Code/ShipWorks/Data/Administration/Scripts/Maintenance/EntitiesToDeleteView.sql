/***********

This view determines the entity id's that should be deleted.

Before using this, verify that no tables are returned that should NOT be truncated.

************/

IF EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[EntitiesToDeleteView]'))
BEGIN
	DROP VIEW [dbo].[EntitiesToDeleteView]
END
GO

create view EntitiesToDeleteView as

	with AllEntities as
	(
		SELECT t.name, CONVERT(bigint, ic.seed_value) % 1000 AS 'EntitySeed'
		FROM   sys.tables AS t LEFT OUTER JOIN sys.identity_columns AS ic ON t.object_id = ic.object_id
		WHERE  ic.seed_value > 0
	),
	EntitiesToKeep as
	(
		-- To Keep Entity Seeds
		select *
		from AllEntities t
		where 
				 t.name in ('Computer', 'Action', 'ActionTask', 'Permission', 'Store', 'Filter', 'FilterNode', 'FilterSequence', 'FilterLayout', 'FilterNodeContent', 'StatusPreset')
			  or t.name in ('Resource', 'LabelSheet', 'ObjectReference', 'FilterNodeColumnSettings', 'EmailAccount', 'VersionSignoff', 'ServiceStatus', 'FilterLayout', 'FilterNodeContent', 'StatusPreset')
			  or t.name in ('FilterNodeContentDetail', 'ObjectLabel', 'Search', 'SystemData', 'ActionFilterTrigger', 'AmazonASIN')
			  or t.name in ('UpsLetterRate', 'UpsLocalRatingDeliveryAreaSurcharge', 'UpsLocalRatingZone', 'UpsLocalRatingZoneFile', 'UpsPackageRate', 'UpsPricePerPound', 'UpsRateSurcharge', 'UpsRateTable')
			  or t.name like 'GridColumn%'
			  or t.name like 'Template%'
			  or t.name like 'Server%'
			  or t.name like 'User%'
			  or t.name like '%Profile%'
			  or t.name like '%Origin%'
			  or t.name like '%Account%'
			  or t.name like '%Rule%'
			  or t.name like 'Shipping%'
			  or t.name like '%ScanForm%'
			  or t.name like '%Configuration%'
			  or t.name like '%Store'
			  or t.name like 'ExcludedPackageType'
			  or t.name like 'ExcludedServiceType'
			  or t.name like 'Scheduling%'
	),
	EntitiesToDelete as
	(
		select *
		from AllEntities where name not in
		(select name from EntitiesToKeep)
	)
	select distinct name, EntitySeed from EntitiesToDelete
