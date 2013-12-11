CREATE TABLE [dbo].[SearsStore] (
    [StoreID]  BIGINT        NOT NULL,
    [Email]    NVARCHAR (75) NOT NULL,
    [Password] NVARCHAR (75) NOT NULL,
    CONSTRAINT [PK_SearsStore] PRIMARY KEY CLUSTERED ([StoreID] ASC),
    CONSTRAINT [FK_SearsStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
);

