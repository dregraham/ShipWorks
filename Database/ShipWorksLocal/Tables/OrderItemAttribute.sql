CREATE TABLE [dbo].[OrderItemAttribute] (
    [OrderItemAttributeID] BIGINT         IDENTITY (1020, 1000) NOT NULL,
    [RowVersion]           ROWVERSION     NOT NULL,
    [OrderItemID]          BIGINT         NOT NULL,
    [Name]                 NVARCHAR (300) NOT NULL,
    [Description]          NVARCHAR (MAX) NOT NULL,
    [UnitPrice]            MONEY          NOT NULL,
    [IsManual]             BIT            NOT NULL,
    CONSTRAINT [PK_OrderItemAttribute] PRIMARY KEY CLUSTERED ([OrderItemAttributeID] ASC),
    CONSTRAINT [FK_OrderItemAttribute_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
);


GO
ALTER TABLE [dbo].[OrderItemAttribute] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE NONCLUSTERED INDEX [IX_OrderItemAttribute_OrderItemID]
    ON [dbo].[OrderItemAttribute]([OrderItemID] ASC);


GO
CREATE TRIGGER [dbo].[OrderItemAttributeAuditTrigger]
    ON [dbo].[OrderItemAttribute]
    AFTER INSERT, DELETE, UPDATE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[OrderItemAttributeAuditTrigger]


GO
CREATE TRIGGER [dbo].[OrderItemAttributeLabelTrigger]
    ON [dbo].[OrderItemAttribute]
    AFTER INSERT, DELETE, UPDATE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[OrderItemAttributeLabelTrigger]


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderItemAttribute', @level2type = N'COLUMN', @level2name = N'UnitPrice';

