CREATE TABLE [dbo].[SearsOrderItem] (
    [OrderItemID]  BIGINT        NOT NULL,
    [LineNumber]   INT           NOT NULL,
    [ItemID]       VARCHAR (300) NOT NULL,
    [Commission]   MONEY         NOT NULL,
    [Shipping]     MONEY         NOT NULL,
    [OnlineStatus] VARCHAR (20)  NOT NULL,
    CONSTRAINT [PK_SearsOrderItem] PRIMARY KEY CLUSTERED ([OrderItemID] ASC),
    CONSTRAINT [FK_SearsOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
);

