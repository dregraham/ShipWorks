CREATE TABLE [dbo].[BigCommerceOrderItem] (
    [OrderItemID]    BIGINT         NOT NULL,
    [OrderAddressID] BIGINT         NOT NULL,
    [OrderProductID] BIGINT         NOT NULL,
    [IsDigitalItem]  BIT            CONSTRAINT [DF_BigCommerceOrderItem_IsDigitalItem] DEFAULT ((0)) NOT NULL,
    [EventDate]      DATETIME       NULL,
    [EventName]      NVARCHAR (255) NULL,
    CONSTRAINT [PK_BigCommerceOrderItem] PRIMARY KEY CLUSTERED ([OrderItemID] ASC),
    CONSTRAINT [FK_BigCommerceOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
);

