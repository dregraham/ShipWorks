
PRINT N'Creating index [IX_PrintResult_PrintDateRelatedObjectID] on [dbo].[PrintResult]'
GO
CREATE NONCLUSTERED INDEX [IX_PrintResult_PrintDateRelatedObjectID] ON [dbo].[PrintResult] ([PrintDate], [RelatedObjectID])
GO