CREATE TABLE [dbo].[NeweggOrder] (
    [OrderID]       BIGINT NOT NULL,
    [InvoiceNumber] BIGINT NULL,
    [RefundAmount]  MONEY  NULL,
    [IsAutoVoid]    BIT    NULL,
    CONSTRAINT [PK_NeweggOrder] PRIMARY KEY CLUSTERED ([OrderID] ASC),
    CONSTRAINT [FK_NeweggOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
);


GO
CREATE TRIGGER [dbo].[FilterDirtyNeweggOrder]
    ON [dbo].[NeweggOrder]
    AFTER UPDATE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[FilterDirtyNeweggOrder]

