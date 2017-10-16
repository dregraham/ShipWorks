IF EXISTS (SELECT * FROM sys.procedures WHERE object_id = OBJECT_ID(N'[dbo].[TruncateNonEmptyTable_SupportsDeleteCascade]'))
BEGIN
	DROP PROCEDURE [dbo].[TruncateNonEmptyTable_SupportsDeleteCascade]
END
GO

CREATE PROCEDURE [dbo].[TruncateNonEmptyTable_SupportsDeleteCascade]
  @TableToTruncate                 VARCHAR(255)
AS 

BEGIN

SET NOCOUNT ON

-- GLOBAL VARIABLES
DECLARE @i int
DECLARE @Debug bit
DECLARE @Recycle bit
DECLARE @Verbose bit
DECLARE @TableName varchar(80)
DECLARE @ColumnName varchar(80)
DECLARE @ReferencedTableName varchar(80)
DECLARE @ReferencedColumnName varchar(80)
DECLARE @ConstraintName varchar(250)
DECLARE @DeleteAction varchar(20)

DECLARE @CreateStatement varchar(max)
DECLARE @DropStatement varchar(max)   
DECLARE @TruncateStatement varchar(max)
DECLARE @CreateStatementTemp varchar(max)
DECLARE @DropStatementTemp varchar(max)
DECLARE @TruncateStatementTemp varchar(max)
DECLARE @Statement varchar(max)

        -- 1 = Will not execute statements 
 SET @Debug = 0
        -- 0 = Will not create or truncate storage table
        -- 1 = Will create or truncate storage table
 SET @Recycle = 0
        -- 1 = Will print a message on every step
 set @Verbose = 1

 SET @i = 1
    SET @CreateStatement = 'ALTER TABLE [dbo].[<tablename>] ADD  CONSTRAINT [<constraintname>] FOREIGN KEY([<column>]) REFERENCES [dbo].[<reftable>] ([<refcolumn>]) <iscascade>'
    SET @DropStatement = 'ALTER TABLE [dbo].[<tablename>] DROP CONSTRAINT [<constraintname>]'
    SET @TruncateStatement = 'TRUNCATE TABLE [<tablename>]'

-- Drop Temporary tables

IF OBJECT_ID('tempdb..#FKs') IS NOT NULL
    DROP TABLE #FKs

-- GET FKs
SELECT ROW_NUMBER() OVER (ORDER BY OBJECT_NAME(fkc.parent_object_id), clm1.name) as ID,
       OBJECT_NAME(fkc.constraint_object_id) as ConstraintName,
       OBJECT_NAME(fkc.parent_object_id) as TableName,
       clm1.name as ColumnName, 
       OBJECT_NAME(fkc.referenced_object_id) as ReferencedTableName,
       clm2.name as ReferencedColumnName,
	   case	fk.delete_referential_action
			WHEN  1 then ' ON DELETE CASCADE'
			ELSE ''
		END DeleteAction
  INTO #FKs
  FROM sys.foreign_key_columns fkc join sys.foreign_keys fk on fk.object_id = fkc.constraint_object_id
       JOIN sys.columns clm1 
         ON fkc.parent_column_id = clm1.column_id 
            AND fkc.parent_object_id = clm1.object_id
       JOIN sys.columns clm2
         ON fkc.referenced_column_id = clm2.column_id 
            AND fkc.referenced_object_id= clm2.object_id
 --WHERE OBJECT_NAME(parent_object_id) not in ('//tables that you do not wont to be truncated')
 WHERE OBJECT_NAME(fkc.referenced_object_id) = @TableToTruncate
 ORDER BY OBJECT_NAME(fkc.parent_object_id)


-- Prepare Storage Table
IF Not EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Internal_FK_Definition_Storage')
   BEGIN
        IF @Verbose = 1
     PRINT '1. Creating Process Specific Tables...'

  -- CREATE STORAGE TABLE IF IT DOES NOT EXISTS
  CREATE TABLE [Internal_FK_Definition_Storage] 
  (
   ID int not null identity(1,1) primary key,
   FK_Name varchar(250) not null,
   FK_CreationStatement varchar(max) not null,
   FK_DestructionStatement varchar(max) not null,
   Table_TruncationStatement varchar(max) not null
  ) 
   END 
ELSE
   BEGIN
        IF @Recycle = 0
            BEGIN
                IF @Verbose = 1
       PRINT '1. Truncating Process Specific Tables...'

    -- TRUNCATE TABLE IF IT ALREADY EXISTS
    TRUNCATE TABLE [Internal_FK_Definition_Storage]    
      END
      ELSE
         PRINT '1. Process specific table will be recycled from previous execution...'
   END


IF @Recycle = 0
   BEGIN

  IF @Verbose = 1
     PRINT '2. Backing up Foreign Key Definitions...'

  -- Fetch and persist FKs             
  WHILE (@i <= (SELECT MAX(ID) FROM #FKs))
   BEGIN
    SET @ConstraintName = (SELECT ConstraintName FROM #FKs WHERE ID = @i)
    SET @TableName = (SELECT TableName FROM #FKs WHERE ID = @i)
    SET @ColumnName = (SELECT ColumnName FROM #FKs WHERE ID = @i)
    SET @ReferencedTableName = (SELECT ReferencedTableName FROM #FKs WHERE ID = @i)
    SET @ReferencedColumnName = (SELECT ReferencedColumnName FROM #FKs WHERE ID = @i)
    SET @ReferencedColumnName = (SELECT ReferencedColumnName FROM #FKs WHERE ID = @i)
    SET @DeleteAction = (SELECT DeleteAction FROM #FKs WHERE ID = @i)

    SET @DropStatementTemp = REPLACE(REPLACE(@DropStatement,'<tablename>',@TableName),'<constraintname>',@ConstraintName)
    SET @CreateStatementTemp = REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@CreateStatement,'<tablename>',@TableName),'<column>',@ColumnName),'<constraintname>',@ConstraintName),'<reftable>',@ReferencedTableName),'<refcolumn>',@ReferencedColumnName),'<iscascade>',@DeleteAction)
    SET @TruncateStatementTemp = REPLACE(@TruncateStatement,'<tablename>',@TableName) 

	

    INSERT INTO [Internal_FK_Definition_Storage]
                        SELECT @ConstraintName, @CreateStatementTemp, @DropStatementTemp, @TruncateStatementTemp

    SET @i = @i + 1

    IF @Verbose = 1
       PRINT '  > Backing up [' + @ConstraintName + '] from [' + @TableName + ']'

    END   
    END   
    ELSE 
       PRINT '2. Backup up was recycled from previous execution...'

       IF @Verbose = 1
     PRINT '3. Dropping Foreign Keys...'

    -- DROP FOREING KEYS
    SET @i = 1
    WHILE (@i <= (SELECT MAX(ID) FROM [Internal_FK_Definition_Storage]))
          BEGIN
             SET @ConstraintName = (SELECT FK_Name FROM [Internal_FK_Definition_Storage] WHERE ID = @i)
    SET @Statement = (SELECT FK_DestructionStatement FROM [Internal_FK_Definition_Storage] WITH (NOLOCK) WHERE ID = @i)

    IF @Debug = 1 
       PRINT @Statement
    ELSE
       EXEC(@Statement)

    SET @i = @i + 1


    IF @Verbose = 1
       PRINT '  > Dropping [' + @ConstraintName + ']'

             END     


    IF @Verbose = 1
       PRINT '4. Truncating Tables...'

    -- TRUNCATE TABLES
-- SzP: commented out as the tables to be truncated might also contain tables that has foreign keys
-- to resolve this the stored procedure should be called recursively, but I dont have the time to do it...          
 /*
    SET @i = 1
    WHILE (@i <= (SELECT MAX(ID) FROM [Internal_FK_Definition_Storage]))
          BEGIN

    SET @Statement = (SELECT Table_TruncationStatement FROM [Internal_FK_Definition_Storage] WHERE ID = @i)

    IF @Debug = 1 
       PRINT @Statement
    ELSE
       EXEC(@Statement)

    SET @i = @i + 1

    IF @Verbose = 1
       PRINT '  > ' + @Statement
          END
*/          


    IF @Verbose = 1
       PRINT '  > TRUNCATE TABLE [' + @TableToTruncate + ']'

    IF @Debug = 1 
        PRINT 'TRUNCATE TABLE [' + @TableToTruncate + ']'
    ELSE
        EXEC('TRUNCATE TABLE [' + @TableToTruncate + ']')


    IF @Verbose = 1
       PRINT '5. Re-creating Foreign Keys...'

    -- CREATE FOREING KEYS
    SET @i = 1
    WHILE (@i <= (SELECT MAX(ID) FROM [Internal_FK_Definition_Storage]))
          BEGIN
             SET @ConstraintName = (SELECT FK_Name FROM [Internal_FK_Definition_Storage] WHERE ID = @i)
    SET @Statement = (SELECT FK_CreationStatement FROM [Internal_FK_Definition_Storage] WHERE ID = @i)

    IF @Debug = 1 
       PRINT @Statement
    ELSE
       EXEC(@Statement)

    SET @i = @i + 1


    IF @Verbose = 1
       PRINT '  > Re-creating [' + @ConstraintName + ']'

          END

    IF @Verbose = 1
       PRINT '6. Process Completed'


END
