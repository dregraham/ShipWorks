CREATE TABLE [dbo].[NetworkSolutionsOrder] (
    [OrderID]                 BIGINT NOT NULL,
    [NetworkSolutionsOrderID] BIGINT NOT NULL,
    CONSTRAINT [PK_NetworkSolutionsOrder] PRIMARY KEY CLUSTERED ([OrderID] ASC),
    CONSTRAINT [FK_NetworkSolutionsOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
);

