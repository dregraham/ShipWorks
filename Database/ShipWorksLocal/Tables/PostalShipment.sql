CREATE TABLE [dbo].[PostalShipment] (
    [ShipmentID]                BIGINT        NOT NULL,
    [Service]                   INT           NOT NULL,
    [Confirmation]              INT           NOT NULL,
    [PackagingType]             INT           NOT NULL,
    [DimsProfileID]             BIGINT        NOT NULL,
    [DimsLength]                FLOAT (53)    NOT NULL,
    [DimsWidth]                 FLOAT (53)    NOT NULL,
    [DimsHeight]                FLOAT (53)    NOT NULL,
    [DimsWeight]                FLOAT (53)    NOT NULL,
    [DimsAddWeight]             BIT           NOT NULL,
    [NonRectangular]            BIT           NOT NULL,
    [NonMachinable]             BIT           NOT NULL,
    [CustomsContentType]        INT           NOT NULL,
    [CustomsContentDescription] NVARCHAR (50) NOT NULL,
    [InsuranceValue]            MONEY         NOT NULL,
    [ExpressSignatureWaiver]    BIT           NOT NULL,
    [SortType]                  INT           NOT NULL,
    [EntryFacility]             INT           NOT NULL,
    CONSTRAINT [PK_PostalShipment] PRIMARY KEY CLUSTERED ([ShipmentID] ASC),
    CONSTRAINT [FK_PostalShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
);


GO
CREATE TRIGGER [dbo].[FilterDirtyPostalShipment]
    ON [dbo].[PostalShipment]
    AFTER UPDATE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[FilterDirtyPostalShipment]


GO
CREATE TRIGGER [dbo].[PostalShipmentAuditTrigger]
    ON [dbo].[PostalShipment]
    AFTER INSERT, DELETE, UPDATE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[PostalShipmentAuditTrigger]


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'104', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PostalShipment', @level2type = N'COLUMN', @level2name = N'Service';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'105', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PostalShipment', @level2type = N'COLUMN', @level2name = N'Confirmation';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'106', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PostalShipment', @level2type = N'COLUMN', @level2name = N'PackagingType';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PostalShipment', @level2type = N'COLUMN', @level2name = N'DimsProfileID';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PostalShipment', @level2type = N'COLUMN', @level2name = N'DimsWeight';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PostalShipment', @level2type = N'COLUMN', @level2name = N'CustomsContentType';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PostalShipment', @level2type = N'COLUMN', @level2name = N'CustomsContentDescription';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PostalShipment', @level2type = N'COLUMN', @level2name = N'InsuranceValue';

