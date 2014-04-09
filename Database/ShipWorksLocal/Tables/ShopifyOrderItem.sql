CREATE TABLE [dbo].[ShopifyOrderItem] (
    [OrderItemID]        BIGINT NOT NULL,
    [ShopifyOrderItemID] BIGINT NOT NULL,
    [ShopifyProductID]   BIGINT NOT NULL,
    CONSTRAINT [PK_ShopifyOrderItem] PRIMARY KEY CLUSTERED ([OrderItemID] ASC),
    CONSTRAINT [FK_ShopifyOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
);

