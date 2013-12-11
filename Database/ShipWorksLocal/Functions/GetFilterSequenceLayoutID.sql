CREATE FUNCTION [dbo].[GetFilterSequenceLayoutID]
(@filterSequenceID BIGINT)
RETURNS BIGINT
AS
 EXTERNAL NAME [ShipWorks.SqlServer].[UserDefinedFunctions].[GetFilterSequenceLayoutID]

