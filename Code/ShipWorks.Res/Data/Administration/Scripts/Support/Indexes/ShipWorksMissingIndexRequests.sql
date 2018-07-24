IF EXISTS(SELECT 1 FROM sys.views WHERE name='ShipWorksMissingIndexRequests' and type='v')
	DROP VIEW [dbo].[ShipWorksMissingIndexRequests]
GO

CREATE VIEW ShipWorksMissingIndexRequests WITH ENCRYPTION AS
	WITH MissingIndexRequests as
	(
		SELECT  mid.index_handle as [IndexHandle], OBJECT_NAME(mid.object_id) as [TableName], migs_adv.*
		FROM 
			(
				SELECT (user_seeks+user_scans) * avg_total_user_cost * (avg_user_impact * 0.01) AS IndexAdvantage, migs.group_handle AS [GroupHandle]
				FROM sys.dm_db_missing_index_group_stats migs
			) AS migs_adv,
			sys.dm_db_missing_index_groups mig,
			sys.dm_db_missing_index_details mid
		WHERE
			migs_adv.GroupHandle = mig.index_group_handle AND
			mig.index_handle = mid.index_handle
			and mid.database_id = db_id()
	),
	MissingIndexRequestColumns AS
	(
		SELECT IndexHandle, TableName, IndexAdvantage, GroupHandle, 
			   column_id AS [ColumnID], column_name AS [ColumnName], column_usage AS [ColumnUsage]
		FROM MissingIndexRequests mir
			cross apply
			(
				SELECT * 
				FROM sys.dm_db_missing_index_columns(mir.Indexhandle)
			) MissingIndexColumns
	)
	SELECT * FROM MissingIndexRequestColumns
GO