CREATE FUNCTION [dbo].[BitwiseAnd]
(@data VARBINARY (8000), @test VARBINARY (8000))
RETURNS VARBINARY (8000)
AS
 EXTERNAL NAME [ShipWorks.SqlServer].[UserDefinedFunctions].[BitwiseAnd]

