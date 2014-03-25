CREATE TABLE [dbo].[SearsOrder] (
    [OrderID]          BIGINT       NOT NULL,
    [PoNumber]         VARCHAR (30) NOT NULL,
    [PoNumberWithDate] VARCHAR (30) NOT NULL,
    [LocationID]       INT          NOT NULL,
    [Commission]       MONEY        NOT NULL,
    [CustomerPickup]   BIT          NOT NULL,
    CONSTRAINT [PK_SearsOrder] PRIMARY KEY CLUSTERED ([OrderID] ASC),
    CONSTRAINT [FK_SearsOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
);


GO
CREATE TRIGGER [dbo].[FilterDirtySearsOrder]
    ON [dbo].[SearsOrder]
    AFTER UPDATE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[FilterDirtySearsOrder]

