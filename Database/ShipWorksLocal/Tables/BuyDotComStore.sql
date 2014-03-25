CREATE TABLE [dbo].[BuyDotComStore] (
    [StoreID]     BIGINT        NOT NULL,
    [FtpUsername] NVARCHAR (50) NOT NULL,
    [FtpPassword] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_BuyComStore] PRIMARY KEY CLUSTERED ([StoreID] ASC),
    CONSTRAINT [FK_BuyComStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
);

