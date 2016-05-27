DECLARE @tableid int
DECLARE @tablename varchar(100)
DECLARE @sql nvarchar(500)

DECLARE TableCursor CURSOR FOR
SELECT object_id, name from sys.tables where name like 'v2m_%'
OPEN TableCursor;

FETCH NEXT FROM TableCursor INTO @tableid, @tablename
WHILE @@FETCH_STATUS = 0
BEGIN
    print 'dropping ' + @tablename
    
	set @sql = 'DROP TABLE ' + @tablename
	exec sp_executesql @sql

    FETCH NEXT FROM TableCursor INTO @tableid, @tablename
END;

CLOSE TableCursor;
DEALLOCATE TableCursor;
