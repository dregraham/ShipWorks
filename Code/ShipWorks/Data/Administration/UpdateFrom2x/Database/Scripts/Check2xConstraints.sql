-- Runs DBCC CHECKCONSTRAINTS on every table in the database to ensure everything is OK before we start migrating data
DECLARE @tablename varchar(100)

DECLARE TableCursor CURSOR FOR
SELECT name from sys.tables where [type] ='U'
OPEN TableCursor;

FETCH NEXT FROM TableCursor INTO @tablename
WHILE @@FETCH_STATUS = 0
BEGIN

	print 'Checking constraints on ' + @tablename
	DBCC CHECKCONSTRAINTS (@tablename) WITH ALL_ERRORMSGS
	
	FETCH NEXT FROM TableCursor INTO @tablename
END;

CLOSE TableCursor;
DEALLOCATE TableCursor;

