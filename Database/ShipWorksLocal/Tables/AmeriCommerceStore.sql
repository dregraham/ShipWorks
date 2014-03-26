CREATE TABLE [dbo].[AmeriCommerceStore] (
    [StoreID]     BIGINT         NOT NULL,
    [Username]    NVARCHAR (70)  NOT NULL,
    [Password]    NVARCHAR (70)  NOT NULL,
    [StoreUrl]    NVARCHAR (350) NOT NULL,
    [StoreCode]   INT            NOT NULL,
    [StatusCodes] XML            NOT NULL,
    CONSTRAINT [PK_AmeriCommerceStore] PRIMARY KEY CLUSTERED ([StoreID] ASC),
    CONSTRAINT [FK_AmeriCommerceStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
);

