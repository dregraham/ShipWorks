DECLARE @col nvarchar(100)
DECLARE @tab nvarchar(100)
DECLARE @sql nvarchar(300)
DECLARE @obj int

DECLARE @recreateExisting bit;
DECLARE @dropAll bit;
DECLARE @printOnly bit;

SET @recreateExisting = 0;
SET @dropAll = 0;
SET @printOnly = 1;

DECLARE ProcCursor CURSOR FOR
select c.name as col, t.name as tab, t.object_id as obj
   from sys.columns c inner join sys.tables t on c.object_id = t.object_id
   where t.name IN ('Order', 'Customer')
         and c.is_identity = 0 
		 and c.name NOT IN (
			'RowVersion',
			'BillResidential',
			'ShipResidential',
			'OnlineCustomerID',
			'OnlineStatusCode')
OPEN ProcCursor;

FETCH NEXT FROM ProcCursor INTO @col, @tab, @obj
WHILE @@FETCH_STATUS = 0
BEGIN
	
	DECLARE @indexName nvarchar(100)
    SET @indexName = 'IX_Auto_' + @col;
    
    DECLARE @withDrop nvarchar(100)
    SET @withDrop = ' ';
    
    DECLARE @process bit;
    SET @process = 1

    IF (EXISTS(SELECT * FROM sys.indexes WHERE [name] = @indexName AND object_id = @obj))
    BEGIN
		
		IF (@dropAll = 0 AND @recreateExisting = 0)
		BEGIN
			SET @process = 0
		END
		
		IF (@recreateExisting = 1)
		BEGIN
			SET @withDrop = '  WITH DROP_EXISTING';
		END
		
    END ELSE BEGIN
    
		IF (@dropAll = 1)
		BEGIN
			SET @process = 0
		END
		
    END
    
    IF (@process = 1)
    BEGIN
    
		DECLARE @includes nvarchar(100)
		SET @includes = 'RowVersion';

		IF (@tab = 'Order')
		BEGIN
			IF (@col != 'StoreID')
				SET @includes = @includes + ', StoreID';
			IF (@col != 'IsManual')
				SET @includes = @includes + ', IsManual';
		END

		IF (@dropAll = 1)
		BEGIN
			SET @sql = 'DROP INDEX ' + @indexName + ' ON ' + @tab;
		END ELSE BEGIN
			SET @sql = 'CREATE NONCLUSTERED INDEX ' + @indexName + ' ON dbo.[' + @tab + '] (' + @col + ') include (' + @includes + ')' + @withDrop;
		END
		
		print @sql

		IF (@printOnly = 0)
		BEGIN
			exec sp_executesql @sql
		END

    END

	FETCH NEXT FROM ProcCursor INTO @col, @tab, @obj
END;

CLOSE ProcCursor;
DEALLOCATE ProcCursor;
