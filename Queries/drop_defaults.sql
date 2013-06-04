DECLARE @col nvarchar(100)
DECLARE @tab nvarchar(100)
DECLARE @def nvarchar(200)
DECLARE @sql nvarchar(300)

DECLARE ProcCursor CURSOR FOR
SELECT t.name as tab, col.name as col, o.name as def
   FROM sys.tables t INNER JOIN sys.columns col ON t.object_id = col.object_id INNER JOIN sys.objects o ON col.default_object_id = o.object_id
   WHERE col.default_object_id > 0 AND t.name != 'ObjectDeletion'
OPEN ProcCursor;

FETCH NEXT FROM ProcCursor INTO @tab, @col, @def
WHILE @@FETCH_STATUS = 0
BEGIN
	
		SET @sql = 'ALTER TABLE dbo.[' + @tab + '] DROP CONSTRAINT ' + @def;
		print @sql

		--exec sp_executesql @sql

	FETCH NEXT FROM ProcCursor INTO @tab, @col, @def
END;

CLOSE ProcCursor;
DEALLOCATE ProcCursor;

