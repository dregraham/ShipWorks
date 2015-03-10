CREATE TABLE [dbo].[AmazonStore] (
    [StoreID]               BIGINT           NOT NULL,
    [AmazonApi]             INT              NOT NULL,
    [AmazonApiRegion]       CHAR (2)         NOT NULL,
    [SellerCentralUsername] NVARCHAR (50)    NOT NULL,
    [SellerCentralPassword] NVARCHAR (50)    NOT NULL,
    [MerchantName]          VARCHAR (64)     NOT NULL,
    [MerchantToken]         VARCHAR (32)     NOT NULL,
    [AccessKeyID]           VARCHAR (32)     NOT NULL,
    [Cookie]                TEXT             NOT NULL,
    [CookieExpires]         DATETIME         NOT NULL,
    [CookieWaitUntil]       DATETIME         NOT NULL,
    [Certificate]           VARBINARY (2048) NULL,
    [WeightDownloads]       TEXT             NOT NULL,
    [MerchantID]            NVARCHAR (50)    NOT NULL,
    [MarketplaceID]         NVARCHAR (50)    NOT NULL,
    [ExcludeFBA]            BIT              NOT NULL,
    [DomainName]            NVARCHAR (50)    NOT NULL,
    CONSTRAINT [PK_AmazonStore] PRIMARY KEY CLUSTERED ([StoreID] ASC),
    CONSTRAINT [FK_AmazonStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
);

