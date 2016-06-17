DECLARE @triggername varchar(100)
DECLARE @sql nvarchar(500)

DECLARE TriggerCursor CURSOR FOR
	SELECT tr.name FROM sys.triggers tr, sys.tables tb
	WHERE tr.parent_id = tb.object_id
	AND (tb.name like 'v2m_%'  OR tb.name = 'Users')

OPEN TriggerCursor;

FETCH NEXT FROM TriggerCursor INTO @triggername
WHILE @@FETCH_STATUS = 0
BEGIN
    print 'dropping ' + @triggername
    
	set @sql = 'DROP TRIGGER ' + @triggername
	exec sp_executesql @sql

    FETCH NEXT FROM TriggerCursor INTO @triggername
END;

CLOSE TriggerCursor
DEALLOCATE TriggerCursor