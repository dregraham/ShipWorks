CREATE TABLE [dbo].[YahooProduct] (
    [StoreID]        BIGINT         NOT NULL,
    [YahooProductID] NVARCHAR (255) NOT NULL,
    [Weight]         FLOAT (53)     NOT NULL,
    CONSTRAINT [PK_YahooProduct_1] PRIMARY KEY CLUSTERED ([StoreID] ASC, [YahooProductID] ASC),
    CONSTRAINT [FK_YahooProduct_YahooStore] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[YahooStore] ([StoreID]) ON DELETE CASCADE
);

