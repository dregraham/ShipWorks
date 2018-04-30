
;WITH TablesToBlockInsertsUpdatesDeletes as
(
  SELECT name AS 'TableName',
       HasCascades = 
        (
          select count(object_id)
          from sys.foreign_keys
          where t.name = object_name(parent_object_id)
            AND delete_referential_action_desc = 'CASCADE'
        ) 
  FROM sys.tables t
	WHERE [name] IN (%readonlyTableNames%)
)
SELECT 
  CASE HasCascades
    WHEN 0 THEN 
      '
		IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'''+  TableName +'_EnforceReadonly''))
		EXEC dbo.sp_executesql @statement = N''
			CREATE TRIGGER ' +  TableName + '_EnforceReadonly ON  [dbo].[' +  TableName + '] INSTEAD OF INSERT,DELETE,UPDATE AS 
			BEGIN
			  RAISERROR(''''This ShipWorks database is in read only mode.'''', 16, 254) WITH NOWAIT
			END
      '''
    ELSE 
      '
      IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N''' +  TableName + '_EnforceReadonly''))
		EXEC dbo.sp_executesql @statement = N''
			CREATE TRIGGER ' +  TableName + '_EnforceReadonly ON  [dbo].[' +  TableName + '] AFTER INSERT,DELETE,UPDATE AS 
			BEGIN
			  RAISERROR(''''This ShipWorks database is in read only mode.'''', 16, 254) WITH NOWAIT
			END
      '''
  END

 AS 'CreateTriggers',
  '
      IF EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N''' +  TableName + '_EnforceReadonly''))
		DROP TRIGGER [dbo].[' +  TableName + '_EnforceReadonly]
  ' 
  AS 'DropTriggers'
FROM TablesToBlockInsertsUpdatesDeletes
ORDER BY TableName

