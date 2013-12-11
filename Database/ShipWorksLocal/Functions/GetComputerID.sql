CREATE FUNCTION [dbo].[GetComputerID]
( )
RETURNS BIGINT
AS
 EXTERNAL NAME [ShipWorks.SqlServer].[UserDefinedFunctions].[GetComputerID]

