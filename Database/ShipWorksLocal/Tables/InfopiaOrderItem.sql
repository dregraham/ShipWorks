CREATE TABLE [dbo].[InfopiaOrderItem] (
    [OrderItemID]       BIGINT        NOT NULL,
    [Marketplace]       NVARCHAR (50) NOT NULL,
    [MarketplaceItemID] NVARCHAR (20) NOT NULL,
    [BuyerID]           NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_InfopiaOrderItem] PRIMARY KEY CLUSTERED ([OrderItemID] ASC),
    CONSTRAINT [FK_InfopiaOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
);

