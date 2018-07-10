IF EXISTS(SELECT 1 FROM sys.views WHERE name='ShipWorksIndexUsage' and type='v')
	DROP VIEW IF EXISTS [dbo].[ShipWorksIndexUsage]
GO

CREATE VIEW ShipWorksIndexUsage WITH ENCRYPTION AS
	WITH IndexUsage AS
	(
		SELECT t.name AS TableName, i.name AS IndexName, i.is_primary_key as IsPrimaryKey, i.is_unique as IsUnique, i.index_id, i.object_id,
			CASE i.is_disabled
				WHEN 1 THEN 0
				ELSE 1
			END as 'IsEnabled',
			MAX(CASE LEN(COALESCE(constraints.CONSTRAINT_NAME, ''))
				WHEN 0 THEN 0
				ELSE 1
			END) as 'HasConstraintColumn',
			sum(ius.user_seeks) + sum(ius.user_scans) + sum(ius.user_lookups) as 'UserQueries',
			max(lastQuery.QueryDate) as 'MaxLastQueryDate'
		FROM sys.dm_db_index_usage_stats ius
			JOIN sys.tables t ON t.object_id = ius.object_id
			JOIN sys.indexes i ON   i.object_id = ius.object_id AND i.index_id = ius.index_id 
			JOIN Sys.Index_Columns ic On i.Object_Id = ic.Object_Id And i.Index_Id = ic.Index_Id
			JOIN Sys.Columns c on c.Object_Id = ic.Object_Id And c.Column_Id = ic.Column_Id
			JOIN Sys.Schemas s On t.ScheMa_Id = s.Schema_Id
			CROSS APPLY
			(
				select ius.last_user_seek as 'QueryDate'
				union  
				select ius.last_user_lookup
				union 
				select ius.last_user_scan
			) as lastQuery
			outer APPLY
			(
				select A.CONSTRAINT_NAME
				from INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS b 
				JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE a ON a.CONSTRAINT_CATALOG = b.CONSTRAINT_CATALOG AND a.CONSTRAINT_NAME = b.CONSTRAINT_NAME 
				WHERE A.TABLE_NAME = t.name and A.COLUMN_NAME = c.name and ic.is_included_column = 0
			) as constraints
		WHERE ius.database_id = db_id() 
		group by t.name, i.name, I.is_disabled, i.is_primary_key, i.is_unique,  i.index_id, i.object_id
	)
	select *, 
		CASE IsPrimaryKey
			WHEN 0 THEN 
				'IF EXISTS(SELECT * FROM ' + QUOTENAME(TableName) + ') BEGIN RAISERROR (''Disabling index ' + QUOTENAME(TableName) + ''', 0, 1) WITH NOWAIT;  ALTER INDEX ' + QUOTENAME(IndexName) + ' ON ' + QUOTENAME(TableName) + ' DISABLE END'
			ELSE '' 
		END as 'DisableIndex',
		'IF EXISTS(SELECT * FROM ' + QUOTENAME(TableName) + ')  ALTER INDEX ' + QUOTENAME(IndexName) + ' ON ' + QUOTENAME(TableName) + ' REBUILD' AS 'EnableIndex'
	from IndexUsage
go