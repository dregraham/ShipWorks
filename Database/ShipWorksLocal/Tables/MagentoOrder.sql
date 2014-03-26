CREATE TABLE [dbo].[MagentoOrder] (
    [OrderID]        BIGINT NOT NULL,
    [MagentoOrderID] BIGINT NOT NULL,
    CONSTRAINT [PK_MagentoOrder] PRIMARY KEY CLUSTERED ([OrderID] ASC),
    CONSTRAINT [FK_MagentoOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
);

