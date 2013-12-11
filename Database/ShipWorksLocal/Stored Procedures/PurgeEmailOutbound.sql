CREATE PROCEDURE [dbo].[PurgeEmailOutbound]
@olderThan DATETIME, @runUntil DATETIME
AS EXTERNAL NAME [ShipWorks.SqlServer].[StoredProcedures].[PurgeEmailOutbound]

