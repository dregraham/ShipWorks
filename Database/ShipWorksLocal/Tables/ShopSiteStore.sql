CREATE TABLE [dbo].[ShopSiteStore] (
    [StoreID]          BIGINT         NOT NULL,
    [Username]         NVARCHAR (50)  NOT NULL,
    [Password]         NVARCHAR (50)  NOT NULL,
    [CgiUrl]           NVARCHAR (350) NOT NULL,
    [RequireSSL]       BIT            NOT NULL,
    [DownloadPageSize] INT            NOT NULL,
    [RequestTimeout]   INT            NOT NULL,
    CONSTRAINT [PK_StoreShopSite] PRIMARY KEY CLUSTERED ([StoreID] ASC),
    CONSTRAINT [FK_StoreShopSite_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
);

