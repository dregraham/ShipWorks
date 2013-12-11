CREATE FUNCTION [dbo].[GetEntityTableNumber]
(@entityID BIGINT)
RETURNS INT
AS
 EXTERNAL NAME [ShipWorks.SqlServer].[UserDefinedFunctions].[GetEntityTableNumber]

