CREATE TABLE [dbo].[iParcelShipment] (
    [ShipmentID]         BIGINT         NOT NULL,
    [iParcelAccountID]   BIGINT         NOT NULL,
    [Service]            INT            NOT NULL,
    [Reference]          NVARCHAR (300) NOT NULL,
    [TrackByEmail]       BIT            NOT NULL,
    [TrackBySMS]         BIT            NOT NULL,
    [IsDeliveryDutyPaid] BIT            NOT NULL,
    CONSTRAINT [PK_iParcelShipment] PRIMARY KEY CLUSTERED ([ShipmentID] ASC),
    CONSTRAINT [FK_iParcelShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
);


GO
CREATE TRIGGER [dbo].[iParcelShipmentAuditTrigger]
    ON [dbo].[iParcelShipment]
    AFTER INSERT, DELETE, UPDATE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[iParcelShipmentAuditTrigger]


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'iParcelShipment', @level2type = N'COLUMN', @level2name = N'iParcelAccountID';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'127', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'iParcelShipment', @level2type = N'COLUMN', @level2name = N'Service';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'iParcelShipment', @level2type = N'COLUMN', @level2name = N'TrackBySMS';

