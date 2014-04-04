CREATE TABLE [dbo].[AmazonOrderItem] (
    [OrderItemID]         BIGINT         NOT NULL,
    [AmazonOrderItemCode] BIGINT         NOT NULL,
    [ASIN]                NVARCHAR (255) NOT NULL,
    [ConditionNote]       NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_AmazonOrderItem] PRIMARY KEY CLUSTERED ([OrderItemID] ASC),
    CONSTRAINT [FK_AmazonOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
);

