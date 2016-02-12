DECLARE 
	@command nvarchar(512);

DECLARE workCursor CURSOR FORWARD_ONLY FOR
	SELECT 'drop index [' + o.name + '].' + i.name FROM sys.indexes i left join sys.objects o
	ON i.object_id = o.object_id
	WHERE
	o.name not like 'v2m_%'
	and o.type = 'U'
	and i.type <> 0
	and i.is_unique_constraint = 0
	and i.is_primary_key = 0
	
OPEN workCursor

FETCH NEXT FROM workCursor
	INTO @command
	
WHILE @@FETCH_STATUS = 0
BEGIN
	-- execute the drop
	EXEC(@command)

	FETCH NEXT FROM workCursor
		INTO @command
END

CLOSE workCursor
DEALLOCATE workCursor