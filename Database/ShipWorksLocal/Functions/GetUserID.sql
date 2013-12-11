CREATE FUNCTION [dbo].[GetUserID]
( )
RETURNS BIGINT
AS
 EXTERNAL NAME [ShipWorks.SqlServer].[UserDefinedFunctions].[GetUserID]

