CREATE FUNCTION [dbo].[DateToUniversalTime]
(@dateTime DATETIME)
RETURNS DATETIME
AS
 EXTERNAL NAME [ShipWorks.SqlServer].[UserDefinedFunctions].[DateToUniversalTime]

