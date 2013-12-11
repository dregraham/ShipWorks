CREATE FUNCTION [dbo].[RegexMatch]
(@input NVARCHAR (4000), @regex NVARCHAR (4000))
RETURNS BIT
AS
 EXTERNAL NAME [ShipWorks.SqlServer].[UserDefinedFunctions].[RegexMatch]

