CREATE TABLE [dbo].[ChannelAdvisorOrderItem] (
    [OrderItemID]        BIGINT        NOT NULL,
    [MarketplaceName]    NVARCHAR (50) NOT NULL,
    [MarketplaceBuyerID] NVARCHAR (80) NOT NULL,
    [MarketplaceSalesID] NVARCHAR (50) NOT NULL,
    [Classification]     NVARCHAR (30) NOT NULL,
    [DistributionCenter] NVARCHAR (80) NOT NULL,
    [HarmonizedCode]     NVARCHAR (10) NOT NULL,
    [IsFBA]              BIT           NOT NULL,
    [MPN]                NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_ChannelAdvisorOrderItem] PRIMARY KEY CLUSTERED ([OrderItemID] ASC),
    CONSTRAINT [FK_ChannelAdvisorOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
);


