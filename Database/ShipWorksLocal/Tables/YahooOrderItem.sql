CREATE TABLE [dbo].[YahooOrderItem] (
    [OrderItemID]    BIGINT         NOT NULL,
    [YahooProductID] NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_YahooOrderItem] PRIMARY KEY CLUSTERED ([OrderItemID] ASC),
    CONSTRAINT [FK_YahooOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
);

