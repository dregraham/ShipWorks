
PRINT N'Creating index [IX_PrintResult_PrintDateRelatedObjectID] on [dbo].[PrintResult]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_PrintResult_PrintDateRelatedObjectID' AND object_id = OBJECT_ID(N'[dbo].[PrintResult]'))
	CREATE NONCLUSTERED INDEX [IX_PrintResult_PrintDateRelatedObjectID] ON [dbo].[PrintResult] ([PrintDate], [RelatedObjectID])
GO