CREATE TABLE [dbo].[InfopiaStore] (
    [StoreID]  BIGINT        NOT NULL,
    [ApiToken] VARCHAR (128) NOT NULL,
    CONSTRAINT [PK_InfopiaStore] PRIMARY KEY CLUSTERED ([StoreID] ASC),
    CONSTRAINT [FK_InfopiaStore_InfopiaStore] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
);

