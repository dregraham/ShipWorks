CREATE PROCEDURE [dbo].[PurgeAbandonedResources]
@olderThan DATETIME, @runUntil DATETIME
AS EXTERNAL NAME [ShipWorks.SqlServer].[StoredProcedures].[PurgeAbandonedResources]

