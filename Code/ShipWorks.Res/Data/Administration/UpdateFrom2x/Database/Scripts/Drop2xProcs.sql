DECLARE @name varchar(100)
DECLARE @sql nvarchar(500)

DECLARE ProcCursor CURSOR FOR
SELECT name from sys.procedures k where is_ms_shipped = 0 and name <> 'GetSchemaVersion'
OPEN ProcCursor;

FETCH NEXT FROM ProcCursor INTO @name
WHILE @@FETCH_STATUS = 0
BEGIN
	print 'dropping ' + @name

	SET @sql = 'DROP PROCEDURE ' + @name
	exec sp_executesql @sql

	FETCH NEXT FROM ProcCursor INTO @name
END;

CLOSE ProcCursor;
DEALLOCATE ProcCursor;

