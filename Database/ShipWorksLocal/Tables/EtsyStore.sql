CREATE TABLE [dbo].[EtsyStore] (
    [StoreID]          BIGINT         NOT NULL,
    [EtsyShopID]       BIGINT         NOT NULL,
    [EtsyLogin]        NVARCHAR (255) NOT NULL,
    [EtsyStoreName]    NVARCHAR (255) NOT NULL,
    [OAuthToken]       NVARCHAR (50)  NOT NULL,
    [OAuthTokenSecret] NVARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_EtsyStore] PRIMARY KEY CLUSTERED ([StoreID] ASC),
    CONSTRAINT [FK_EtsyStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
);

