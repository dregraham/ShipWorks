CREATE TABLE [dbo].[ProStoresOrder] (
    [OrderID]            BIGINT       NOT NULL,
    [ConfirmationNumber] VARCHAR (12) NOT NULL,
    [AuthorizedDate]     DATETIME     NULL,
    [AuthorizedBy]       VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_ProStoresOrder] PRIMARY KEY CLUSTERED ([OrderID] ASC),
    CONSTRAINT [FK_ProStoresOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
);


GO
CREATE TRIGGER [dbo].[FilterDirtyProStoresOrder]
    ON [dbo].[ProStoresOrder]
    AFTER UPDATE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[FilterDirtyProStoresOrder]

