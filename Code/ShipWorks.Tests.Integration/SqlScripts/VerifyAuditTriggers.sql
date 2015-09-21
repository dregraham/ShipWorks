set nocount on

declare @results int = 0
declare @maxAuditId bigint

select @maxAuditId = max(AuditID) from [Audit]

declare @tableName nvarchar(200)
declare @triggerName nvarchar(200)
declare @columnName nvarchar(200)
declare @identityColumnName nvarchar(200)

	DECLARE testCursor CURSOR FOR 
		select distinct '[' + object_name(t.parent_id) + ']' as TableName, t.name as 'TriggerName', c.name as ColumnName, cIdentity.name as 'IdenitityColumn'
		from sys.triggers t, sys.columns c, sys.columns cIdentity
		where t.name like '%audit%' 
		  and object_name(t.parent_id) = object_name(c.object_id)
		  and c.column_id = (select top 1 c2.column_id from sys.columns c2 where object_name(c2.object_id) = object_name(t.parent_id) and c2.column_id > 2 and (c2.user_type_id = 104 or c2.user_type_id = 127 or c2.user_type_id = 231) order by c2.user_type_id asc)
		  and cIdentity.name = (SELECT column_name FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE OBJECTPROPERTY(OBJECT_ID(constraint_name), 'IsPrimaryKey') = 1 AND table_name = object_name(t.parent_id))
		order by t.name
		OPEN testCursor
		FETCH NEXT FROM testCursor INTO @tableName, @triggerName, @columnName, @identityColumnName

		WHILE @@FETCH_STATUS = 0
		BEGIN

			select @maxAuditId = max(AuditID) from [Audit]

			declare @sql nvarchar(4000) = 'update '+ @tablename + ' set '+ @columnName + ' = '+ @columnName + ' where '+ @identityColumnName + ' in (select max('+ @identityColumnName + ') from '+ @tablename + ')'
			exec ( @sql )

			select @results = count(*) from [Audit] where AuditID > @maxAuditId

			if @results <> 1
			begin
				print @results
				declare @msg nvarchar(255) = 'FAILED: ' + @tablename + '.'+ @triggerName + ' audit trigger failed'
				RAISERROR(@msg, 10, 1, '')
			end

			FETCH NEXT FROM testCursor INTO @tableName, @triggerName, @columnName, @identityColumnName
			END

		CLOSE testCursor
		DEALLOCATE testCursor




/*


SELECT column_name FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE OBJECTPROPERTY(OBJECT_ID(constraint_name), 'IsPrimaryKey') = 1 AND table_name = 'uspsshipment'


select distinct '[' + object_name(t.parent_id) + ']' as TableName, t.name as 'TriggerName', c.name as ColumnName, cIdentity.name as 'IdenitityColumn'
from sys.triggers t, sys.columns c, sys.columns cIdentity
where t.name like '%audit%' 
  and object_name(t.parent_id) = object_name(c.object_id)
  and c.column_id = (select top 1 c2.column_id from sys.columns c2 where object_name(c2.object_id) = object_name(t.parent_id) and c2.column_id > 2 and (c2.user_type_id = 104 or c2.user_type_id = 127 or c2.user_type_id = 231) order by c2.user_type_id asc)
  and cIdentity.name = (SELECT column_name FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE OBJECTPROPERTY(OBJECT_ID(constraint_name), 'IsPrimaryKey') = 1 AND table_name = object_name(t.parent_id))
order by t.name

select * from sys.tables where object_name(object_id) = 'uspsshipment'

select object_name(parent_id) from sys.triggers where name like '%audit%' order by name

select * from sys.columns c where object_name(c.object_id) = 'uspsshipment' and c.column_id > 2 and c.user_type_id = 231

select max(auditid) from [audit]

update [EbayOrder] set CombinedLocally = CombinedLocally where OrderID in (select max(OrderID) from [EbayOrder])

select max(auditid) from [audit]


declare @tableName nvarchar(200) = 'Customer'
declare @triggerName nvarchar(200) = 'CustomerAuditTrigger'
declare @columnName nvarchar(200) = 'BillFirstName'

begin tran
		declare @sql nvarchar(4000) = 'update ['+ @tablename + '] set '+ @columnName + ' = ''0'' where '+ @tablename + 'ID in (select max('+ @tablename + 'ID) from '+ @tablename + ')'
		exec ( @sql )

rollback


*/
