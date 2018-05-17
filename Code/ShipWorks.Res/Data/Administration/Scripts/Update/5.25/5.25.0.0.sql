IF NOT EXISTS (SELECT 1 FROM [dbo].[Shortcut] WHERE Barcode = '-PL-')
	INSERT INTO Shortcut
	(ModifierKeys, VirtualKey, Barcode, [Action])
	VALUES
	(0, 121, '-PL-', 3)
ELSE
	INSERT INTO Shortcut
	(ModifierKeys, VirtualKey, Barcode, [Action])
	VALUES
	(0, 121, '', 3)
GO