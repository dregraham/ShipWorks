SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
PRINT N'Dropping foreign keys from [dbo].[BuyDotComOrderItem]'
GO
ALTER TABLE [dbo].[BuyDotComOrderItem] DROP
CONSTRAINT [FK_BuyDotComOrderItem_OrderItem]
GO
PRINT N'Dropping constraints from [dbo].[BuyDotComOrderItem]'
GO
ALTER TABLE [dbo].[BuyDotComOrderItem] DROP CONSTRAINT [PK_BuyDotComOrderItem]
GO
PRINT N'Rebuilding [dbo].[BuyDotComOrderItem]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_BuyDotComOrderItem]
(
[OrderItemID] [bigint] NOT NULL,
[ReceiptItemID] [bigint] NOT NULL,
[ListingID] [int] NOT NULL,
[Shipping] [money] NOT NULL,
[Tax] [money] NOT NULL,
[Commission] [money] NOT NULL,
[ItemFee] [money] NOT NULL
)
GO
INSERT INTO [dbo].[tmp_rg_xx_BuyDotComOrderItem]([OrderItemID], [ReceiptItemID], [ListingID], [Shipping], [Tax], [Commission], [ItemFee]) SELECT [OrderItemID], 0, [ListingID], [Shipping], [Tax], [Commission], [ItemFee] FROM [dbo].[BuyDotComOrderItem]
GO
DROP TABLE [dbo].[BuyDotComOrderItem]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_BuyDotComOrderItem]', N'BuyDotComOrderItem'
GO
PRINT N'Creating primary key [PK_BuyDotComOrderItem] on [dbo].[BuyDotComOrderItem]'
GO
ALTER TABLE [dbo].[BuyDotComOrderItem] ADD CONSTRAINT [PK_BuyDotComOrderItem] PRIMARY KEY CLUSTERED  ([OrderItemID])
GO
PRINT N'Adding foreign keys to [dbo].[BuyDotComOrderItem]'
GO
ALTER TABLE [dbo].[BuyDotComOrderItem] ADD
CONSTRAINT [FK_BuyDotComOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO
