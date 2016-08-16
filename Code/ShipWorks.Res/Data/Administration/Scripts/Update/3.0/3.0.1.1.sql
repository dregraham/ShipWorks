SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
PRINT N'Dropping foreign keys from [dbo].[AmazonOrderItem]'
GO
ALTER TABLE [dbo].[AmazonOrderItem] DROP
CONSTRAINT [FK_AmazonOrderItem_OrderItem]
GO
PRINT N'Dropping foreign keys from [dbo].[ChannelAdvisorOrderItem]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrderItem] DROP
CONSTRAINT [FK_ChannelAdvisorOrderItem_OrderItem]
GO
PRINT N'Dropping foreign keys from [dbo].[EbayOrderItem]'
GO
ALTER TABLE [dbo].[EbayOrderItem] DROP
CONSTRAINT [FK_EbayOrderItem_OrderItem]
GO
PRINT N'Dropping foreign keys from [dbo].[InfopiaOrderItem]'
GO
ALTER TABLE [dbo].[InfopiaOrderItem] DROP
CONSTRAINT [FK_InfopiaOrderItem_OrderItem]
GO
PRINT N'Dropping foreign keys from [dbo].[OrderItemAttribute]'
GO
ALTER TABLE [dbo].[OrderItemAttribute] DROP
CONSTRAINT [FK_OrderItemAttribute_OrderItem]
GO
PRINT N'Dropping foreign keys from [dbo].[YahooOrderItem]'
GO
ALTER TABLE [dbo].[YahooOrderItem] DROP
CONSTRAINT [FK_YahooOrderItem_OrderItem]
GO
PRINT N'Dropping foreign keys from [dbo].[OrderItem]'
GO
ALTER TABLE [dbo].[OrderItem] DROP
CONSTRAINT [FK_OrderItem_Order]
GO
PRINT N'Dropping constraints from [dbo].[OrderItem]'
GO
ALTER TABLE [dbo].[OrderItem] DROP CONSTRAINT [PK_OrderItem]
GO
PRINT N'Dropping index [IX_Auto_BillCity] from [dbo].[Customer]'
GO
DROP INDEX [IX_Auto_BillCity] ON [dbo].[Customer]
GO
PRINT N'Dropping index [IX_Auto_BillFax] from [dbo].[Customer]'
GO
DROP INDEX [IX_Auto_BillFax] ON [dbo].[Customer]
GO
PRINT N'Dropping index [IX_Auto_BillFirstName] from [dbo].[Customer]'
GO
DROP INDEX [IX_Auto_BillFirstName] ON [dbo].[Customer]
GO
PRINT N'Dropping index [IX_Auto_BillMiddleName] from [dbo].[Customer]'
GO
DROP INDEX [IX_Auto_BillMiddleName] ON [dbo].[Customer]
GO
PRINT N'Dropping index [IX_Auto_BillPhone] from [dbo].[Customer]'
GO
DROP INDEX [IX_Auto_BillPhone] ON [dbo].[Customer]
GO
PRINT N'Dropping index [IX_Auto_BillStreet1] from [dbo].[Customer]'
GO
DROP INDEX [IX_Auto_BillStreet1] ON [dbo].[Customer]
GO
PRINT N'Dropping index [IX_Auto_BillStreet2] from [dbo].[Customer]'
GO
DROP INDEX [IX_Auto_BillStreet2] ON [dbo].[Customer]
GO
PRINT N'Dropping index [IX_Auto_BillStreet3] from [dbo].[Customer]'
GO
DROP INDEX [IX_Auto_BillStreet3] ON [dbo].[Customer]
GO
PRINT N'Dropping index [IX_Auto_BillWebsite] from [dbo].[Customer]'
GO
DROP INDEX [IX_Auto_BillWebsite] ON [dbo].[Customer]
GO
PRINT N'Dropping index [IX_Auto_ShipCity] from [dbo].[Customer]'
GO
DROP INDEX [IX_Auto_ShipCity] ON [dbo].[Customer]
GO
PRINT N'Dropping index [IX_Auto_ShipFax] from [dbo].[Customer]'
GO
DROP INDEX [IX_Auto_ShipFax] ON [dbo].[Customer]
GO
PRINT N'Dropping index [IX_Auto_ShipFirstName] from [dbo].[Customer]'
GO
DROP INDEX [IX_Auto_ShipFirstName] ON [dbo].[Customer]
GO
PRINT N'Dropping index [IX_Auto_ShipMiddleName] from [dbo].[Customer]'
GO
DROP INDEX [IX_Auto_ShipMiddleName] ON [dbo].[Customer]
GO
PRINT N'Dropping index [IX_Auto_ShipPhone] from [dbo].[Customer]'
GO
DROP INDEX [IX_Auto_ShipPhone] ON [dbo].[Customer]
GO
PRINT N'Dropping index [IX_Auto_ShipStreet1] from [dbo].[Customer]'
GO
DROP INDEX [IX_Auto_ShipStreet1] ON [dbo].[Customer]
GO
PRINT N'Dropping index [IX_Auto_ShipStreet2] from [dbo].[Customer]'
GO
DROP INDEX [IX_Auto_ShipStreet2] ON [dbo].[Customer]
GO
PRINT N'Dropping index [IX_Auto_ShipStreet3] from [dbo].[Customer]'
GO
DROP INDEX [IX_Auto_ShipStreet3] ON [dbo].[Customer]
GO
PRINT N'Dropping index [IX_Auto_ShipWebsite] from [dbo].[Customer]'
GO
DROP INDEX [IX_Auto_ShipWebsite] ON [dbo].[Customer]
GO
PRINT N'Dropping index [IX_Auto_BillCity] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_BillCity] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_BillFax] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_BillFax] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_BillFirstName] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_BillFirstName] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_BillMiddleName] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_BillMiddleName] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_BillPhone] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_BillPhone] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_BillStreet1] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_BillStreet1] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_BillStreet2] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_BillStreet2] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_BillStreet3] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_BillStreet3] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_BillWebsite] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_BillWebsite] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_IsManual] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_IsManual] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_RollupItemLocation] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_RollupItemLocation] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_RollupItemQuantity] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_RollupItemQuantity] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_RollupItemTotalWeight] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_RollupItemTotalWeight] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_ShipCity] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_ShipCity] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_ShipFax] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_ShipFax] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_ShipFirstName] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_ShipFirstName] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_ShipMiddleName] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_ShipMiddleName] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_ShipPhone] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_ShipPhone] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_ShipStreet1] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_ShipStreet1] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_ShipStreet2] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_ShipStreet2] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_ShipStreet3] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_ShipStreet3] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_Auto_ShipWebsite] from [dbo].[Order]'
GO
DROP INDEX [IX_Auto_ShipWebsite] ON [dbo].[Order]
GO
PRINT N'Dropping index [IX_OrderItem_OrderID] from [dbo].[OrderItem]'
GO
DROP INDEX [IX_OrderItem_OrderID] ON [dbo].[OrderItem]
GO
PRINT N'Rebuilding [dbo].[OrderItem]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_OrderItem]
(
[OrderItemID] [bigint] NOT NULL IDENTITY(1013, 1000),
[RowVersion] [timestamp] NOT NULL,
[OrderID] [bigint] NOT NULL,
[Name] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Code] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SKU] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ISBN] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[UPC] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Description] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Location] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Image] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Thumbnail] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[UnitPrice] [money] NOT NULL,
[UnitCost] [money] NOT NULL,
[Weight] [float] NOT NULL,
[Quantity] [float] NOT NULL,
[LocalStatus] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IsManual] [bit] NOT NULL,
[TotalWeight] AS ([Weight]*[Quantity])
)
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_OrderItem] ON
GO
INSERT INTO [dbo].[tmp_rg_xx_OrderItem]([OrderItemID], [OrderID], [Name], [Code], [SKU], [ISBN], [UPC], [Description], [Location], [Image], [Thumbnail], [UnitPrice], [UnitCost], [Weight], [Quantity], [LocalStatus], [IsManual]) SELECT [OrderItemID], [OrderID], [Name], [Code], [SKU], [ISBN], [UPC], [Description], [Location], [Image], [Thumbnail], [UnitPrice], [UnitCost], [Weight], [Quantity], [LocalStatus], [IsManual] FROM [dbo].[OrderItem]
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_OrderItem] OFF
GO
DECLARE @idVal INT
SELECT @idVal = IDENT_CURRENT(N'OrderItem')
DBCC CHECKIDENT(tmp_rg_xx_OrderItem, RESEED, @idVal)
GO
DROP TABLE [dbo].[OrderItem]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_OrderItem]', N'OrderItem'
GO
PRINT N'Creating primary key [PK_OrderItem] on [dbo].[OrderItem]'
GO
ALTER TABLE [dbo].[OrderItem] ADD CONSTRAINT [PK_OrderItem] PRIMARY KEY CLUSTERED  ([OrderItemID])
GO
PRINT N'Creating index [IX_OrderItem_OrderID] on [dbo].[OrderItem]'
GO
CREATE NONCLUSTERED INDEX [IX_OrderItem_OrderID] ON [dbo].[OrderItem] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[AmazonOrderItem]'
GO
ALTER TABLE [dbo].[AmazonOrderItem] ADD
CONSTRAINT [FK_AmazonOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO
PRINT N'Adding foreign keys to [dbo].[ChannelAdvisorOrderItem]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrderItem] ADD
CONSTRAINT [FK_ChannelAdvisorOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO
PRINT N'Adding foreign keys to [dbo].[EbayOrderItem]'
GO
ALTER TABLE [dbo].[EbayOrderItem] ADD
CONSTRAINT [FK_EbayOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO
PRINT N'Adding foreign keys to [dbo].[InfopiaOrderItem]'
GO
ALTER TABLE [dbo].[InfopiaOrderItem] ADD
CONSTRAINT [FK_InfopiaOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO
PRINT N'Adding foreign keys to [dbo].[OrderItemAttribute]'
GO
ALTER TABLE [dbo].[OrderItemAttribute] ADD
CONSTRAINT [FK_OrderItemAttribute_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO
PRINT N'Adding foreign keys to [dbo].[YahooOrderItem]'
GO
ALTER TABLE [dbo].[YahooOrderItem] ADD
CONSTRAINT [FK_YahooOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO
PRINT N'Adding foreign keys to [dbo].[OrderItem]'
GO
ALTER TABLE [dbo].[OrderItem] ADD
CONSTRAINT [FK_OrderItem_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Creating extended properties'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'2', 'SCHEMA', N'dbo', 'TABLE', N'OrderItem', 'COLUMN', N'UnitCost'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'2', 'SCHEMA', N'dbo', 'TABLE', N'OrderItem', 'COLUMN', N'UnitPrice'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'3', 'SCHEMA', N'dbo', 'TABLE', N'OrderItem', 'COLUMN', N'Weight'
GO
