CREATE TABLE [dbo].[YahooOrder] (
    [OrderID]      BIGINT       NOT NULL,
    [YahooOrderID] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_YahooOrder] PRIMARY KEY CLUSTERED ([OrderID] ASC),
    CONSTRAINT [FK_YahooOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
);

