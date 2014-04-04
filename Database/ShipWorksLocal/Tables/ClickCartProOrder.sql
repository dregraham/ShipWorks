CREATE TABLE [dbo].[ClickCartProOrder] (
    [OrderID]             BIGINT       NOT NULL,
    [ClickCartProOrderID] VARCHAR (25) NOT NULL,
    CONSTRAINT [PK_ClickCartProOrder] PRIMARY KEY CLUSTERED ([OrderID] ASC),
    CONSTRAINT [FK_ClickCartProOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
);

