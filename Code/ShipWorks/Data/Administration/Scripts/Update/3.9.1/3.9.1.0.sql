

if exists(select index_id from sys.indexes where name = 'IX_GridLayoutColumn' and object_id = OBJECT_ID('GridColumnPosition'))
BEGIN
	EXEC sp_rename N'dbo.[GridColumnPosition].[IX_GridLayoutColumn]', N'IX_GridColumnPosition_GridColumnLayoutIdColumn', N'INDEX';
END
GO

if exists(select index_id from sys.indexes where name = 'PK_GridColumnLayout' and object_id = OBJECT_ID('GridColumnPosition'))
BEGIN
	EXEC sp_rename N'dbo.[GridColumnPosition].[PK_GridColumnLayout]', N'PK_GridColumnPosition', N'INDEX';
END
GO

if exists(select index_id from sys.indexes where name = 'PK_GridLayout' and object_id = OBJECT_ID('GridColumnLayout'))
BEGIN
	EXEC sp_rename N'dbo.GridColumnLayout.PK_GridLayout', N'PK_GridColumnLayout', N'INDEX';
END
GO