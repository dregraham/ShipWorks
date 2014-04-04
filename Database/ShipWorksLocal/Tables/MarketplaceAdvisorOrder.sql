CREATE TABLE [dbo].[MarketplaceAdvisorOrder] (
    [OrderID]           BIGINT        NOT NULL,
    [BuyerNumber]       BIGINT        NOT NULL,
    [SellerOrderNumber] BIGINT        NOT NULL,
    [InvoiceNumber]     NVARCHAR (50) NOT NULL,
    [ParcelID]          BIGINT        NOT NULL,
    CONSTRAINT [PK_MarketworksOrder] PRIMARY KEY CLUSTERED ([OrderID] ASC),
    CONSTRAINT [FK_MarketworksOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
);


GO
CREATE TRIGGER [dbo].[FilterDirtyMarketplaceAdvisorOrder]
    ON [dbo].[MarketplaceAdvisorOrder]
    AFTER UPDATE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[FilterDirtyMarketplaceAdvisorOrder]

