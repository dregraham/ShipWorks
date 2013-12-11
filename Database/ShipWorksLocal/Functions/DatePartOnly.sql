CREATE FUNCTION [dbo].[DatePartOnly]
(@dateTime DATETIME)
RETURNS DATETIME
AS
 EXTERNAL NAME [ShipWorks.SqlServer].[UserDefinedFunctions].[DatePartOnly]

