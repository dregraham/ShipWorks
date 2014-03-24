CREATE TABLE [dbo].[OrderItem] (
    [OrderItemID] BIGINT         IDENTITY (1013, 1000) NOT NULL,
    [RowVersion]  ROWVERSION     NOT NULL,
    [OrderID]     BIGINT         NOT NULL,
    [Name]        NVARCHAR (300) NOT NULL,
    [Code]        NVARCHAR (300) NOT NULL,
    [SKU]         NVARCHAR (100) NOT NULL,
    [ISBN]        NVARCHAR (30)  NOT NULL,
    [UPC]         NVARCHAR (30)  NOT NULL,
    [Description] NVARCHAR (MAX) NOT NULL,
    [Location]    NVARCHAR (255) NOT NULL,
    [Image]       NVARCHAR (MAX) NOT NULL,
    [Thumbnail]   NVARCHAR (MAX) NOT NULL,
    [UnitPrice]   MONEY          NOT NULL,
    [UnitCost]    MONEY          NOT NULL,
    [Weight]      FLOAT (53)     NOT NULL,
    [Quantity]    FLOAT (53)     NOT NULL,
    [LocalStatus] NVARCHAR (255) NOT NULL,
    [IsManual]    BIT            NOT NULL,
    [TotalWeight] AS             ([Weight]*[Quantity]),
    CONSTRAINT [PK_OrderItem] PRIMARY KEY CLUSTERED ([OrderItemID] ASC),
    CONSTRAINT [FK_OrderItem_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
);


GO
ALTER TABLE [dbo].[OrderItem] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE NONCLUSTERED INDEX [IX_OrderItem_OrderID]
    ON [dbo].[OrderItem]([OrderID] ASC);


GO
CREATE TRIGGER [dbo].[FilterDirtyOrderItem]
    ON [dbo].[OrderItem]
    AFTER INSERT, DELETE, UPDATE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[FilterDirtyOrderItem]


GO
CREATE TRIGGER [dbo].[OrderItemAuditTrigger]
    ON [dbo].[OrderItem]
    AFTER INSERT, DELETE, UPDATE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[OrderItemAuditTrigger]


GO
CREATE TRIGGER [dbo].[OrderItemLabelTrigger]
    ON [dbo].[OrderItem]
    AFTER INSERT, DELETE, UPDATE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[OrderItemLabelTrigger]


GO
CREATE TRIGGER [dbo].[OrderItemRollupTrigger]
    ON [dbo].[OrderItem]
    AFTER INSERT, DELETE, UPDATE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[OrderItemRollupTrigger]


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderItem', @level2type = N'COLUMN', @level2name = N'UnitPrice';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderItem', @level2type = N'COLUMN', @level2name = N'UnitCost';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderItem', @level2type = N'COLUMN', @level2name = N'Weight';

