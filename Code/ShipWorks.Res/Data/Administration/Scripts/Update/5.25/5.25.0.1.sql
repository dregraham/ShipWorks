GO
IF NOT EXISTS (SELECT 1 FROM [dbo].[Shortcut] WHERE Barcode = '-TB-')
	INSERT INTO Shortcut (ModifierKeys, VirtualKey, Barcode, [Action])
	VALUES (null, null, '-TB-', 4)
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[Shortcut] WHERE Barcode = '-ES-')
	INSERT INTO Shortcut (ModifierKeys, VirtualKey, Barcode, [Action])
	VALUES (null, null, '-ES-', 5)
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[Shortcut] WHERE Barcode = '-CR-')
	INSERT INTO Shortcut (ModifierKeys, VirtualKey, Barcode, [Action])
	VALUES (null, null, '-CR-', 6)
GO