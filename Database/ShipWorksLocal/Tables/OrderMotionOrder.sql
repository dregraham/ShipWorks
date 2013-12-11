CREATE TABLE [dbo].[OrderMotionOrder] (
    [OrderID]                  BIGINT        NOT NULL,
    [OrderMotionShipmentID]    INT           NOT NULL,
    [OrderMotionPromotion]     NVARCHAR (50) NOT NULL,
    [OrderMotionInvoiceNumber] NVARCHAR (64) NOT NULL,
    CONSTRAINT [PK_OrderMotionOrder] PRIMARY KEY CLUSTERED ([OrderID] ASC),
    CONSTRAINT [FK_OrderMotionOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
);


GO
CREATE TRIGGER [dbo].[FilterDirtyOrderMotionOrder]
    ON [dbo].[OrderMotionOrder]
    AFTER UPDATE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[FilterDirtyOrderMotionOrder]

