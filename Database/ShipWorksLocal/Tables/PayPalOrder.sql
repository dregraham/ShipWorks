CREATE TABLE [dbo].[PayPalOrder] (
    [OrderID]       BIGINT        NOT NULL,
    [TransactionID] NVARCHAR (50) NOT NULL,
    [AddressStatus] INT           NOT NULL,
    [PayPalFee]     MONEY         NOT NULL,
    [PaymentStatus] INT           NOT NULL,
    CONSTRAINT [PK_PayPalOrder] PRIMARY KEY CLUSTERED ([OrderID] ASC),
    CONSTRAINT [FK_PayPalOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
);


GO
CREATE TRIGGER [dbo].[FilterDirtyPayPalOrder]
    ON [dbo].[PayPalOrder]
    AFTER UPDATE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[FilterDirtyPayPalOrder]

