DECLARE @ExecStatement NVARCHAR(4000), @SQLString NVARCHAR(4000)
SET @ExecStatement = 'EXEC sp_executesql @SQLString'
SET @SQLString = N'CREATE PROCEDURE [GetDatabaseGuid] WITH ENCRYPTION AS SELECT ''' + CONVERT(varchar(255),NEWID()) + ''''
EXEC sp_executesql  @ExecStatement, N'@SQLString NVARCHAR(4000)', @SQLString=@SQLString