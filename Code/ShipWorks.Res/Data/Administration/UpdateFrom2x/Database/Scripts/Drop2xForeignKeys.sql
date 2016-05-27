DECLARE @tablename varchar(100)
DECLARE @keyname varchar(100)
DECLARE @sql nvarchar(500)

DECLARE KeyCursor CURSOR FOR
SELECT k.name as keyname, t.name as tablename
	from sys.foreign_keys k, sys.tables t
	where k.parent_object_id = t.object_id
OPEN KeyCursor;

FETCH NEXT FROM KeyCursor INTO @keyname, @tablename
WHILE @@FETCH_STATUS = 0
BEGIN
	print 'dropping ' + @tablename + '.' + @keyname

	SET @sql = 'ALTER TABLE [' + @tablename + '] DROP CONSTRAINT [' + @keyname + ']'
	exec sp_executesql @sql

	FETCH NEXT FROM KeyCursor INTO @keyname, @tablename
END;

CLOSE KeyCursor;
DEALLOCATE KeyCursor;