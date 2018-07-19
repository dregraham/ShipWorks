IF EXISTS(SELECT 1 FROM sys.views WHERE name='ShipWorksDisabledDefaultIndexes' and type='v')
	DROP VIEW [dbo].[ShipWorksDisabledDefaultIndexes]
GO

CREATE VIEW ShipWorksDisabledDefaultIndexes WITH ENCRYPTION AS
    SELECT siu.tablename AS [TableName], siu.IndexName, c.name AS [ColumnName], siu.EnableIndex, 
	        ic.index_id AS [IndexID], ic.index_column_id AS [IndexColumnId], ic.is_included_column AS [IsIncluded]
	FROM ShipWorksIndexUsage siu, sys.index_columns ic, sys.columns c
	WHERE siu.object_id = ic.object_id 
		AND siu.index_id = ic.index_id 
		AND siu.object_id = c.object_id 
		AND c.column_id = ic.column_id 
		AND ic.object_id = c.object_id
	    AND siu.IsPrimaryKey = 0 
	    AND siu.IsEnabled = 0
GO