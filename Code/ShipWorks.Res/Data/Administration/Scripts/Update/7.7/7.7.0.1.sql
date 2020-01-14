PRINT N'Adding Shortcut'
GO
IF NOT EXISTS(SELECT * FROM Shortcut WHERE Barcode = '-UV-')
BEGIN
	INSERT INTO Shortcut (ModifierKeys, VirtualKey, Barcode, [Action])
	VALUES 
	(null, null, '-UV-', 12);
END;
GO