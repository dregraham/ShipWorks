PRINT N'Renaming Indexes'
GO
/* Rename indexes that don't match any of the regular formats */
IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_EmailOutbound_EmailOutboundIDRelationTypeObjectID' AND object_id = OBJECT_ID('[dbo].[EmailOutboundRelation]'))
	EXEC sp_rename N'dbo.EmailOutboundRelation.IX_EmailOutbound_EmailOutboundIDRelationTypeObjectID', N'IX_EmailOutboundRelation_EmailOutboundIDRelationTypeObjectID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_EmailOutbound_ObjectIDRelationTypeEmailOutboundID' AND object_id = OBJECT_ID('[dbo].[EmailOutboundRelation]'))
	EXEC sp_rename N'dbo.EmailOutboundRelation.IX_EmailOutbound_ObjectIDRelationTypeEmailOutboundID', N'IX_EmailOutboundRelation_ObjectIDRelationTypeEmailOutboundID', N'INDEX';
	
IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_EmailOutbound_RelationTypeObject' AND object_id = OBJECT_ID('[dbo].[EmailOutboundRelation]'))
	EXEC sp_rename N'dbo.EmailOutboundRelation.IX_EmailOutbound_RelationTypeObject', N'IX_EmailOutboundRelation_RelationTypeObject', N'INDEX';
	
IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_FilterChild_ParentFilterID' AND object_id = OBJECT_ID('[dbo].[FilterSequence]'))
	EXEC sp_rename N'dbo.FilterSequence.IX_FilterChild_ParentFilterID', N'IX_FilterSequence_FilterChild_ParentFilterID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_OnlineCustomerID' AND object_id = OBJECT_ID('[dbo].[Order]'))
	EXEC sp_rename N'dbo.Order.IX_OnlineCustomerID', N'IX_Order_OnlineCustomerID', N'INDEX';
	
IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_OnlineLastModified_StoreID_IsManual' AND object_id = OBJECT_ID('[dbo].[Order]'))
	EXEC sp_rename N'dbo.Order.IX_OnlineLastModified_StoreID_IsManual', N'IX_Order_OnlineLastModified_StoreID_IsManual', N'INDEX';
	
IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='UK_Computer_Identifier' AND object_id = OBJECT_ID('[dbo].[Computer]'))
	EXEC sp_rename N'dbo.Computer.UK_Computer_Identifier', N'IX_Computer_Identifier', N'INDEX';

GO

PRINT N'Renaming Indexes'
GO

DECLARE @sql NVARCHAR(max) 

;WITH IndexNaming as
(
	SELECT 
		 TableName = t.name,
		 IndexName = ind.name,
	 
		 REPLACE(
			 REPLACE(
				REPLACE(ind.name, 'IX_Auto_', 'IX_'),
				'SW_', 'IX_'),
			 'IX_', 'IX_SWDefault_')
		 AS 'NewName'
	FROM 
		 sys.indexes ind 
	INNER JOIN 
		 sys.index_columns ic ON  ind.object_id = ic.object_id and ind.index_id = ic.index_id 
	INNER JOIN 
		 sys.columns col ON ic.object_id = col.object_id and ic.column_id = col.column_id 
	INNER JOIN 
		 sys.tables t ON ind.object_id = t.object_id 
	WHERE 
		 ind.is_primary_key = 0 
		 AND t.is_ms_shipped = 0 
),
IndexRenaming as
(
	select distinct *, 'EXEC sp_rename N''dbo.' + TableName + '.' + IndexName + ''', N''' + NewName + ''', N''INDEX'';' as 'RenameSql'
	from IndexNaming 
	WHERE NewName not like 'IDX_Scheduling_%'
	  AND IndexName not like 'IX_SWDefault_%'
)
select @sql = COALESCE(@sql + char(13), '') + RenameSql
from IndexRenaming

exec sp_executesql @sql
GO