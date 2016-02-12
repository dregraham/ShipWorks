PRINT N'Creating [dbo].[EbayCombinedOrderRelation]'
GO
CREATE TABLE [dbo].[EbayCombinedOrderRelation]
(
[EbayCombinedOrderRelationID] [bigint] NOT NULL IDENTITY(1099, 1000),
[OrderID] [bigint] NOT NULL,
[EbayOrderID] [bigint] NOT NULL,
[StoreID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_EbayCombinedOrderRelation] on [dbo].[EbayCombinedOrderRelation]'
GO
ALTER TABLE [dbo].[EbayCombinedOrderRelation] ADD CONSTRAINT [PK_EbayCombinedOrderRelation] PRIMARY KEY CLUSTERED  ([EbayCombinedOrderRelationID])
GO
PRINT N'Creating index [IX_EbayCombinedOrderRelation] on [dbo].[EbayCombinedOrderRelation]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_EbayCombinedOrderRelation] ON [dbo].[EbayCombinedOrderRelation] ([EbayOrderID])
GO
PRINT N'Adding foreign keys to [dbo].[EbayCombinedOrderRelation]'
GO
ALTER TABLE [dbo].[EbayCombinedOrderRelation] ADD CONSTRAINT [FK_EbayCombinedOrderRelation_EbayOrder] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[EbayOrder] ([OrderID]) ON DELETE CASCADE
ALTER TABLE [dbo].[EbayCombinedOrderRelation] ADD CONSTRAINT [FK_EbayCombinedOrderRelation_EbayStore] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[EbayStore] ([StoreID]) ON DELETE CASCADE
GO