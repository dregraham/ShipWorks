
PRINT N'Creating index [IX_FilterNode_FilterSequenceID_FilterNodeContentID_Purpose] on [dbo].[FilterNode]'
GO
CREATE NONCLUSTERED INDEX [IX_FilterNode_FilterSequenceID_FilterNodeContentID_Purpose] ON [dbo].[FilterNode] ([FilterSequenceID], [FilterNodeContentID], [Purpose]) INCLUDE ([FilterNodeID])
GO

PRINT N'Creating index [IX_FilterSequence_FilterID] on [dbo].[FilterSequence]'
GO
CREATE NONCLUSTERED INDEX [IX_FilterSequence_FilterID] ON [dbo].[FilterSequence] ([FilterID]) INCLUDE ([FilterSequenceID])
GO

PRINT N'Creating index [IX_Filter_IsFolder] on [dbo].[Filter]'
GO
CREATE NONCLUSTERED INDEX [IX_Filter_IsFolder] ON [dbo].[Filter] ([IsFolder]) INCLUDE ([FilterID])
GO


