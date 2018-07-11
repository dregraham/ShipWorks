IF EXISTS(SELECT 1 FROM sys.views WHERE name='ShipWorksIndexUsage' and type='v')
	DROP VIEW [dbo].[ShipWorksIndexUsage]
GO

CREATE VIEW ShipWorksIndexUsage WITH ENCRYPTION AS
	WITH IndexUsage as
	(
			SELECT t.name AS TableName, i.name AS IndexName, i.is_primary_key as IsPrimaryKey, i.is_unique as IsUnique, i.index_id, i.object_id,
				CASE i.is_disabled
					WHEN 1 THEN 0
					ELSE 1
				END as 'IsEnabled',
				MAX(CASE LEN(COALESCE(constraints.CONSTRAINT_NAME, ''))
					WHEN 0 THEN 0
					ELSE 1
				END) as 'HasConstraintColumn'
			FROM
				sys.tables t 
				JOIN sys.indexes i ON   i.object_id = t.object_id 
				JOIN Sys.Index_Columns ic ON i.Object_Id = ic.Object_Id And i.Index_Id = ic.Index_Id
				JOIN Sys.Columns c ON c.Object_Id = ic.Object_Id And c.Column_Id = ic.Column_Id
				JOIN Sys.Schemas s ON t.ScheMa_Id = s.Schema_Id
				outer APPLY
				(
					SELECT A.CONSTRAINT_NAME
					FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS b 
					JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE a ON a.CONSTRAINT_CATALOG = b.CONSTRAINT_CATALOG AND a.CONSTRAINT_NAME = b.CONSTRAINT_NAME 
					WHERE A.TABLE_NAME = t.name and A.COLUMN_NAME = c.name and ic.is_included_column = 0
				) AS constraints
			GROUP BY t.name, i.name, I.is_disabled, i.is_primary_key, i.is_unique,  i.index_id, i.object_id
	),
IndexUsageFull as
(
	SELECT t.TableName, t.IndexName, t.IsPrimaryKey, t.IsUnique, t.index_id, t.object_id, t.IsEnabled, t.HasConstraintColumn, 
		   coalesce(ius.UserQueries, 0) as 'UserQueries', max(ius.MaxLastQueryDate) as 'MaxLastQueryDate'
	FROM IndexUsage t 
		 outer APPLY 
			 (
				SELECT sum(coalesce(ius.user_seeks, 0)) + sum(coalesce(ius.user_scans, 0)) + sum(coalesce(ius.user_lookups, 0)) as 'UserQueries',
				ius.database_id, max(coalesce(lastQuery.QueryDate, 0)) as 'MaxLastQueryDate'
				FROM sys.dm_db_index_usage_stats ius 
				outer APPLY
						(
							SELECT ius.last_user_seek as 'QueryDate'
							UNION  
							SELECT ius.last_user_lookup
							UNION 
							SELECT ius.last_user_scan
						) AS lastQuery
				WHERE t.object_id = object_id and t.index_id = index_id and ius.database_id = db_id()
				GROUP BY ius.database_id, lastQuery.QueryDate
					) ius 
	GROUP BY TableName, IndexName, IsPrimaryKey, IsUnique, t.index_id, t.object_id, t.IsEnabled, t.HasConstraintColumn, ius.UserQueries
)
	select *, 
		CASE IsPrimaryKey
			WHEN 0 THEN 
				'IF EXISTS(SELECT * FROM ' + QUOTENAME(TableName) + ') BEGIN RAISERROR (''Disabling index ' + QUOTENAME(TableName) + ''', 0, 1) WITH NOWAIT;  ALTER INDEX ' + QUOTENAME(IndexName) + ' ON ' + QUOTENAME(TableName) + ' DISABLE END'
			ELSE '' 
		END as 'DisableIndex',
		'IF EXISTS(SELECT * FROM ' + QUOTENAME(TableName) + ')  ALTER INDEX ' + QUOTENAME(IndexName) + ' ON ' + QUOTENAME(TableName) + ' REBUILD' AS 'EnableIndex'
	from IndexUsageFull
go