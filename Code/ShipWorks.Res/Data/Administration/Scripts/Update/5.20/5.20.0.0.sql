PRINT N'Altering [dbo].[WalmartOrderSearch]'
GO
ALTER TABLE [WalmartOrderSearch]
ADD CustomerOrderID varchar(50) NOT NULL
CONSTRAINT DF_CustomerOrderID DEFAULT ''
GO
PRINT N'Dropping Constraint on [dbo].[WalmartOrderSearch]'
ALTER TABLE [WalmartOrderSearch]
DROP CONSTRAINT DF_CustomerOrderID
GO
PRINT N'Creating index [IX_WalmartOrderSearch_CustomerOrderID] on [dbo].[WalmartOrderSearch]'
GO
CREATE NONCLUSTERED INDEX [IX_WalmartOrderSearch_CustomerOrderID] ON [dbo].[WalmartOrderSearch] ([CustomerOrderID]) INCLUDE ([OrderID])
GO