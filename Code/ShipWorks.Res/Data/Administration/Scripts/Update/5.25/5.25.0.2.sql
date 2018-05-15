IF NOT EXISTS (SELECT 1 FROM [dbo].[Shortcut] WHERE Barcode = '-AP-')
	INSERT INTO Shortcut (ModifierKeys, VirtualKey, Barcode, [Action])
	VALUES (3, 65, '-AP-', 7)
GO
