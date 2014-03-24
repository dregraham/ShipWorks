CREATE TABLE [dbo].[ThreeDCartOrderItem] (
    [OrderItemID]          BIGINT NOT NULL,
    [ThreeDCartShipmentID] BIGINT NOT NULL,
    CONSTRAINT [PK_ThreeDCartOrderItem] PRIMARY KEY CLUSTERED ([OrderItemID] ASC),
    CONSTRAINT [FK_ThreeDCartOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
);

