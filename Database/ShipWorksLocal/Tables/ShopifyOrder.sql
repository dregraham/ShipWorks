CREATE TABLE [dbo].[ShopifyOrder] (
    [OrderID]               BIGINT NOT NULL,
    [ShopifyOrderID]        BIGINT NOT NULL,
    [FulfillmentStatusCode] INT    NOT NULL,
    [PaymentStatusCode]     INT    NOT NULL,
    CONSTRAINT [PK_ShopifyOrder] PRIMARY KEY CLUSTERED ([OrderID] ASC),
    CONSTRAINT [FK_ShopifyOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
);


GO
CREATE TRIGGER [dbo].[FilterDirtyShopifyOrder]
    ON [dbo].[ShopifyOrder]
    AFTER UPDATE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[FilterDirtyShopifyOrder]


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'122', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShopifyOrder', @level2type = N'COLUMN', @level2name = N'FulfillmentStatusCode';


GO
EXECUTE sp_addextendedproperty @name = N'AuditName', @value = N'Fulfillment Status', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShopifyOrder', @level2type = N'COLUMN', @level2name = N'FulfillmentStatusCode';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'121', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShopifyOrder', @level2type = N'COLUMN', @level2name = N'PaymentStatusCode';


GO
EXECUTE sp_addextendedproperty @name = N'AuditName', @value = N'Fulfillment Status', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShopifyOrder', @level2type = N'COLUMN', @level2name = N'PaymentStatusCode';

