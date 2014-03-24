CREATE TABLE [dbo].[YahooStore] (
    [StoreID]                BIGINT        NOT NULL,
    [YahooEmailAccountID]    BIGINT        NOT NULL,
    [TrackingUpdatePassword] VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_YahooStore] PRIMARY KEY CLUSTERED ([StoreID] ASC),
    CONSTRAINT [FK_YahooStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
);

