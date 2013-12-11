CREATE PROCEDURE [dbo].[PurgePrintResult]
@olderThan DATETIME, @runUntil DATETIME
AS EXTERNAL NAME [ShipWorks.SqlServer].[StoredProcedures].[PurgePrintResult]

