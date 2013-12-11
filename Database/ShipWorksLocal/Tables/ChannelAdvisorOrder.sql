CREATE TABLE [dbo].[ChannelAdvisorOrder] (
    [OrderID]               BIGINT          NOT NULL,
    [CustomOrderIdentifier] NVARCHAR (50)   NOT NULL,
    [ResellerID]            NVARCHAR (80)   NOT NULL,
    [OnlineShippingStatus]  INT             NOT NULL,
    [OnlineCheckoutStatus]  INT             NOT NULL,
    [OnlinePaymentStatus]   INT             NOT NULL,
    [FlagStyle]             NVARCHAR (32)   NOT NULL,
    [FlagDescription]       NVARCHAR (80)   NOT NULL,
    [FlagType]              INT             NOT NULL,
    [MarketplaceNames]      NVARCHAR (1024) NOT NULL,
    CONSTRAINT [PK_ChannelAdvisorOrder] PRIMARY KEY CLUSTERED ([OrderID] ASC),
    CONSTRAINT [FK_ChannelAdvisorOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
);


GO
CREATE TRIGGER [dbo].[FilterDirtyChannelAdvisorOrder]
    ON [dbo].[ChannelAdvisorOrder]
    AFTER UPDATE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[FilterDirtyChannelAdvisorOrder]

