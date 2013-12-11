CREATE FUNCTION [dbo].[GetTemplateDescendantsOfFolder]
(@templateFolderID BIGINT)
RETURNS 
     TABLE (
        [TemplateID] BIGINT NULL)
AS
 EXTERNAL NAME [ShipWorks.SqlServer].[UserDefinedFunctions].[GetTemplateDescendantsOfFolder]

