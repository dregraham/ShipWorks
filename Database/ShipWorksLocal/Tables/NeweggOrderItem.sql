CREATE TABLE [dbo].[NeweggOrderItem] (
    [OrderItemID]               BIGINT       NOT NULL,
    [SellerPartNumber]          VARCHAR (64) NULL,
    [NeweggItemNumber]          VARCHAR (64) NULL,
    [ManufacturerPartNumber]    VARCHAR (64) NULL,
    [ShippingStatusID]          INT          NULL,
    [ShippingStatusDescription] VARCHAR (32) NULL,
    [QuantityShipped]           INT          NULL,
    CONSTRAINT [PK_NeweggOrderItem] PRIMARY KEY CLUSTERED ([OrderItemID] ASC),
    CONSTRAINT [FK_NeweggOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
);

