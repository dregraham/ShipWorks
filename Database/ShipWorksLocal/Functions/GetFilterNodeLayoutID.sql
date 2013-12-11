CREATE FUNCTION [dbo].[GetFilterNodeLayoutID]
(@filterNodeID BIGINT)
RETURNS BIGINT
AS
 EXTERNAL NAME [ShipWorks.SqlServer].[UserDefinedFunctions].[GetFilterNodeLayoutID]

