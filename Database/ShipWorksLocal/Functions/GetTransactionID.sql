CREATE FUNCTION [dbo].[GetTransactionID]
( )
RETURNS BIGINT
AS
 EXTERNAL NAME [ShipWorks.SqlServer].[UserDefinedFunctions].[GetTransactionID]

