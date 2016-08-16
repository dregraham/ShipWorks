IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Note]') AND name = N'IX_OrderNote_ObjectID')
DROP INDEX [IX_OrderNote_ObjectID] ON [dbo].[Note] WITH ( ONLINE = OFF )
GO

CREATE NONCLUSTERED INDEX [IX_OrderNote_ObjectID] ON [dbo].[Note] ([ObjectID]) INCLUDE ([Edited])
GO
