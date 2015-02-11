
IF EXISTS (SELECT name FROM sysindexes WHERE name = 'IX_Store_OrderNumberComplete_IsManual') 
BEGIN 
	DROP INDEX [Order].[IX_Store_OrderNumberComplete_IsManual]
END
GO