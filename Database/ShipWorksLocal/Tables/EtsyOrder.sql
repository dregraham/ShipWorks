CREATE TABLE [dbo].[EtsyOrder] (
    [OrderID]    BIGINT NOT NULL,
    [WasPaid]    BIT    NOT NULL,
    [WasShipped] BIT    NOT NULL,
    CONSTRAINT [PK_EtsyOrder] PRIMARY KEY CLUSTERED ([OrderID] ASC),
    CONSTRAINT [FK_EtsyOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
);


GO
CREATE TRIGGER [dbo].[FilterDirtyEtsyOrder]
    ON [dbo].[EtsyOrder]
    AFTER UPDATE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[FilterDirtyEtsyOrder]


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'121', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'EtsyOrder', @level2type = N'COLUMN', @level2name = N'WasPaid';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'120', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'EtsyOrder', @level2type = N'COLUMN', @level2name = N'WasShipped';

