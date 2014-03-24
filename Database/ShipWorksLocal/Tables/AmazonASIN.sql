CREATE TABLE [dbo].[AmazonASIN] (
    [StoreID]    BIGINT        NOT NULL,
    [SKU]        VARCHAR (100) NOT NULL,
    [AmazonASIN] VARCHAR (32)  NOT NULL,
    CONSTRAINT [PK_AmazonASIN] PRIMARY KEY CLUSTERED ([StoreID] ASC, [SKU] ASC),
    CONSTRAINT [FK_AmazonASIN_AmazonStore] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[AmazonStore] ([StoreID]) ON DELETE CASCADE
);

