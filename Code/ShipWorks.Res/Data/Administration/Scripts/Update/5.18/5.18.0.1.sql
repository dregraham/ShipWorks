
PRINT N'Altering [dbo].[GrouponOrder]'
GO
ALTER TABLE [dbo].[GrouponOrder] ADD
[ParentOrderID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_GrouponOrder_ParentOrderID] DEFAULT ('')
GO
PRINT N'Creating index [IX_GrouponOrder_ParentOrderID] on [dbo].[GrouponOrder]'
GO
CREATE NONCLUSTERED INDEX [IX_GrouponOrder_ParentOrderID] ON [dbo].[GrouponOrder] ([ParentOrderID])
GO

PRINT N'Altering [dbo].[GrouponOrderSearch]'
GO
ALTER TABLE [dbo].[GrouponOrderSearch] ADD
[ParentOrderID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_GrouponOrderSearch_ParentOrderID] DEFAULT ('')
GO
PRINT N'Creating index [IX_GrouponOrderSearch_ParentOrderID] on [dbo].[GrouponOrderSearch]'
GO
CREATE NONCLUSTERED INDEX [IX_GrouponOrderSearch_ParentOrderID] ON [dbo].[GrouponOrderSearch] ([ParentOrderID]) INCLUDE ([OrderID])
GO



