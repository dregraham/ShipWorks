PRINT N'Altering [dbo].[Configuration]'
GO
IF COL_LENGTH(N'[dbo].[Configuration]', N'DefaultPickListTemplateID') IS NULL
ALTER TABLE [dbo].[Configuration] ADD [DefaultPickListTemplateID] [bigint] NULL
GO

PRINT N'Adding Shortcut'
GO
INSERT INTO Shortcut (ModifierKeys, VirtualKey, Barcode, [Action])
VALUES (3, 80, '' , 9)
GO