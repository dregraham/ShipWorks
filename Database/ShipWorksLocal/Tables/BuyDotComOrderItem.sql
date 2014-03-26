CREATE TABLE [dbo].[BuyDotComOrderItem] (
    [OrderItemID]   BIGINT NOT NULL,
    [ReceiptItemID] BIGINT NOT NULL,
    [ListingID]     INT    NOT NULL,
    [Shipping]      MONEY  NOT NULL,
    [Tax]           MONEY  NOT NULL,
    [Commission]    MONEY  NOT NULL,
    [ItemFee]       MONEY  NOT NULL,
    CONSTRAINT [PK_BuyDotComOrderItem] PRIMARY KEY CLUSTERED ([OrderItemID] ASC),
    CONSTRAINT [FK_BuyDotComOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
);


GO
CREATE TRIGGER [dbo].[FilterDirtyBuyDotComOrderItem]
    ON [dbo].[BuyDotComOrderItem]
    AFTER UPDATE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[FilterDirtyBuyDotComOrderItem]

