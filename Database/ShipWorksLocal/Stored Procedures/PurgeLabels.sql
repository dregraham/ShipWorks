CREATE PROCEDURE [dbo].[PurgeLabels]
@olderThan DATETIME, @runUntil DATETIME
AS EXTERNAL NAME [ShipWorks.SqlServer].[StoredProcedures].[PurgeLabels]

