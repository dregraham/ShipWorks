CREATE TABLE [dbo].[EquaShipShipment] (
    [ShipmentID]        BIGINT         NOT NULL,
    [EquaShipAccountID] BIGINT         NOT NULL,
    [Service]           INT            NOT NULL,
    [PackageType]       INT            NOT NULL,
    [ReferenceNumber]   NVARCHAR (50)  NOT NULL,
    [Description]       NVARCHAR (255) NOT NULL,
    [ShippingNotes]     NVARCHAR (255) NOT NULL,
    [DimsProfileID]     BIGINT         NOT NULL,
    [DimsLength]        FLOAT (53)     NOT NULL,
    [DimsHeight]        FLOAT (53)     NOT NULL,
    [DimsWidth]         FLOAT (53)     NOT NULL,
    [DimsWeight]        FLOAT (53)     NOT NULL,
    [DimsAddWeight]     BIT            NOT NULL,
    [InsuranceValue]    MONEY          NOT NULL,
    [DeclaredValue]     MONEY          NOT NULL,
    [EmailNotification] BIT            NOT NULL,
    [SaturdayDelivery]  BIT            NOT NULL,
    [Confirmation]      INT            NOT NULL,
    CONSTRAINT [PK_EquashipShipment] PRIMARY KEY CLUSTERED ([ShipmentID] ASC),
    CONSTRAINT [FK_EquashipShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
);


GO
CREATE TRIGGER [dbo].[EquashipShipmentAuditTrigger]
    ON [dbo].[EquaShipShipment]
    AFTER INSERT, DELETE, UPDATE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[EquashipShipmentAuditTrigger]


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'EquaShipShipment', @level2type = N'COLUMN', @level2name = N'EquaShipAccountID';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'118', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'EquaShipShipment', @level2type = N'COLUMN', @level2name = N'Service';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'119', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'EquaShipShipment', @level2type = N'COLUMN', @level2name = N'PackageType';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'EquaShipShipment', @level2type = N'COLUMN', @level2name = N'ReferenceNumber';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'EquaShipShipment', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'EquaShipShipment', @level2type = N'COLUMN', @level2name = N'ShippingNotes';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'EquaShipShipment', @level2type = N'COLUMN', @level2name = N'DimsProfileID';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'EquaShipShipment', @level2type = N'COLUMN', @level2name = N'DimsWeight';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'EquaShipShipment', @level2type = N'COLUMN', @level2name = N'InsuranceValue';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'EquaShipShipment', @level2type = N'COLUMN', @level2name = N'DeclaredValue';

