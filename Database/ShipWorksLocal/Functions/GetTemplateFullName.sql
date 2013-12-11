CREATE FUNCTION [dbo].[GetTemplateFullName]
(@templateID BIGINT)
RETURNS NVARCHAR (4000)
AS
 EXTERNAL NAME [ShipWorks.SqlServer].[UserDefinedFunctions].[GetTemplateFullName]

