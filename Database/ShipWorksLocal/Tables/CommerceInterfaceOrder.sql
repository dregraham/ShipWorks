CREATE TABLE [dbo].[CommerceInterfaceOrder] (
    [OrderID]                      BIGINT        NOT NULL,
    [CommerceInterfaceOrderNumber] NVARCHAR (60) NOT NULL,
    CONSTRAINT [PK_CommerceInterfaceOrder] PRIMARY KEY CLUSTERED ([OrderID] ASC),
    CONSTRAINT [FK_CommerceInterfaceOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
);

