
PRINT N'Creating index [IX_FilterNode_FilterSequenceID_FilterNodeContentID_Purpose] on [dbo].[FilterNode]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_FilterNode_FilterSequenceID_FilterNodeContentID_Purpose' AND object_id = OBJECT_ID(N'[dbo].[FilterNode]'))
CREATE NONCLUSTERED INDEX [IX_FilterNode_FilterSequenceID_FilterNodeContentID_Purpose] ON [dbo].[FilterNode] ([FilterSequenceID], [FilterNodeContentID], [Purpose]) INCLUDE ([FilterNodeID])
GO

PRINT N'Creating index [IX_FilterSequence_FilterID] on [dbo].[FilterSequence]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_FilterSequence_FilterID' AND object_id = OBJECT_ID(N'[dbo].[FilterSequence]'))
CREATE NONCLUSTERED INDEX [IX_FilterSequence_FilterID] ON [dbo].[FilterSequence] ([FilterID]) INCLUDE ([FilterSequenceID])
GO

PRINT N'Creating index [IX_Filter_IsFolder] on [dbo].[Filter]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Filter_IsFolder' AND object_id = OBJECT_ID(N'[dbo].[Filter]'))
CREATE NONCLUSTERED INDEX [IX_Filter_IsFolder] ON [dbo].[Filter] ([IsFolder]) INCLUDE ([FilterID])
GO


