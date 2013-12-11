CREATE TABLE [dbo].[AmazonOrder] (
    [OrderID]            BIGINT       NOT NULL,
    [AmazonOrderID]      VARCHAR (32) NOT NULL,
    [AmazonCommission]   MONEY        NOT NULL,
    [FulfillmentChannel] INT          NOT NULL,
    CONSTRAINT [PK_AmazonOrder] PRIMARY KEY CLUSTERED ([OrderID] ASC),
    CONSTRAINT [FK_AmazonOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
);


GO
CREATE TRIGGER [dbo].[FilterDirtyAmazonOrder]
    ON [dbo].[AmazonOrder]
    AFTER UPDATE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[FilterDirtyAmazonOrder]

