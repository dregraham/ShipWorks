CREATE FUNCTION [dbo].[GetFilterNodeLevels]
(@filterLayoutID BIGINT)
RETURNS INT
AS
 EXTERNAL NAME [ShipWorks.SqlServer].[UserDefinedFunctions].[GetFilterNodeLevels]

