CREATE PROCEDURE [dbo].[PurgeAudit]
@olderThan DATETIME, @runUntil DATETIME
AS EXTERNAL NAME [ShipWorks.SqlServer].[StoredProcedures].[PurgeAudit]

