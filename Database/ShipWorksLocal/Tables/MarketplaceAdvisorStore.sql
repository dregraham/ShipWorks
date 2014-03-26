CREATE TABLE [dbo].[MarketplaceAdvisorStore] (
    [StoreID]       BIGINT        NOT NULL,
    [Username]      NVARCHAR (50) NOT NULL,
    [Password]      NVARCHAR (50) NOT NULL,
    [AccountType]   INT           NOT NULL,
    [DownloadFlags] INT           NOT NULL,
    CONSTRAINT [PK_MarketworksStore] PRIMARY KEY CLUSTERED ([StoreID] ASC),
    CONSTRAINT [FK_MarketworksStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
);

