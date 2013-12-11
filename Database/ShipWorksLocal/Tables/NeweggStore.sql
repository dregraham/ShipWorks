CREATE TABLE [dbo].[NeweggStore] (
    [StoreID]                  BIGINT       NOT NULL,
    [SellerID]                 VARCHAR (10) NOT NULL,
    [SecretKey]                VARCHAR (50) NOT NULL,
    [ExcludeFulfilledByNewegg] BIT          NOT NULL,
    CONSTRAINT [PK_NeweggStore] PRIMARY KEY CLUSTERED ([StoreID] ASC),
    CONSTRAINT [FK_NeweggStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
);

