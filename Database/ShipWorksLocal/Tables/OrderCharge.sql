CREATE TABLE [dbo].[OrderCharge] (
    [OrderChargeID] BIGINT         IDENTITY (1021, 1000) NOT NULL,
    [RowVersion]    ROWVERSION     NOT NULL,
    [OrderID]       BIGINT         NOT NULL,
    [Type]          NVARCHAR (50)  NOT NULL,
    [Description]   NVARCHAR (255) NOT NULL,
    [Amount]        MONEY          NOT NULL,
    CONSTRAINT [PK_OrderCharge] PRIMARY KEY CLUSTERED ([OrderChargeID] ASC),
    CONSTRAINT [FK_OrderCharge_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
);


GO
ALTER TABLE [dbo].[OrderCharge] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE NONCLUSTERED INDEX [IX_OrderCharge_OrderID]
    ON [dbo].[OrderCharge]([OrderID] ASC);


GO
CREATE TRIGGER [dbo].[FilterDirtyOrderCharge]
    ON [dbo].[OrderCharge]
    AFTER INSERT, DELETE, UPDATE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[FilterDirtyOrderCharge]


GO
CREATE TRIGGER [dbo].[OrderChargeAuditTrigger]
    ON [dbo].[OrderCharge]
    AFTER INSERT, DELETE, UPDATE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[OrderChargeAuditTrigger]


GO
CREATE TRIGGER [dbo].[OrderChargeLabelTrigger]
    ON [dbo].[OrderCharge]
    AFTER INSERT, DELETE, UPDATE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[OrderChargeLabelTrigger]


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderCharge', @level2type = N'COLUMN', @level2name = N'Amount';

