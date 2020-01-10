PRINT N'Adding Shortcut'
GO
IF NOT EXISTS(SELECT * FROM Shortcut WHERE Barcode = '-NP-')
BEGIN
	INSERT INTO Shortcut (ModifierKeys, VirtualKey, Barcode, [Action])
	VALUES 
	(null, null, '-NP-',10),
	(null, null, '-NS-', 11) 
END;
GO