IF NOT EXISTS (SELECT 1 FROM [dbo].[Shortcut] WHERE Barcode = '-FO-')
	INSERT INTO Shortcut (ModifierKeys, VirtualKey, Barcode, [Action])
	VALUES (3, 70, '-FO-', 1)
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[Shortcut] WHERE Barcode = '-CL-')
	INSERT INTO Shortcut (ModifierKeys, VirtualKey, Barcode, [Action])
	VALUES (3, 67, '-CL-', 8)
GO