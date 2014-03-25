CREATE TABLE [dbo].[OtherShipment] (
    [ShipmentID]     BIGINT        NOT NULL,
    [Carrier]        NVARCHAR (50) NOT NULL,
    [Service]        NVARCHAR (50) NOT NULL,
    [InsuranceValue] MONEY         NOT NULL,
    CONSTRAINT [PK_OtherShipment] PRIMARY KEY CLUSTERED ([ShipmentID] ASC),
    CONSTRAINT [FK_OtherShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
);


GO
CREATE TRIGGER [dbo].[OtherShipmentAuditTrigger]
    ON [dbo].[OtherShipment]
    AFTER INSERT, DELETE, UPDATE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[OtherShipmentAuditTrigger]


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OtherShipment', @level2type = N'COLUMN', @level2name = N'InsuranceValue';

