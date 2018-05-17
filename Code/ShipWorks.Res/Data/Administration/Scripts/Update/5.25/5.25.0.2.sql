IF NOT EXISTS (SELECT 1 FROM [dbo].[Shortcut] WHERE Barcode = '-PL-')
	INSERT INTO Shortcut (ModifierKeys, VirtualKey, Barcode, [Action])
	VALUES (0, 121, '-PL-', 3)
ELSE
	INSERT INTO Shortcut (ModifierKeys, VirtualKey, Barcode, [Action])
	VALUES (0, 121, '', 3)
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

IF NOT EXISTS (SELECT 1 FROM [dbo].[Shortcut] WHERE Barcode = '-AP-')
	INSERT INTO Shortcut (ModifierKeys, VirtualKey, Barcode, [Action])
	VALUES (3, 65, '-AP-', 7)
ELSE
	INSERT INTO Shortcut (ModifierKeys, VirtualKey, Barcode, [Action])
	VALUES (3, 65, '' , 7)
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[Shortcut] WHERE Barcode = '-FO-')
	INSERT INTO Shortcut (ModifierKeys, VirtualKey, Barcode, [Action])
	VALUES (3, 70, '-FO-', 1)
ELSE
	INSERT INTO Shortcut (ModifierKeys, VirtualKey, Barcode, [Action])
	VALUES (3, 70, '' , 1)
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[Shortcut] WHERE Barcode = '-CL-')
	INSERT INTO Shortcut (ModifierKeys, VirtualKey, Barcode, [Action])
	VALUES (3, 67, '-CL-', 8)
ELSE
	INSERT INTO Shortcut (ModifierKeys, VirtualKey, Barcode, [Action])
	VALUES (3, 67, '' , 8)
GO