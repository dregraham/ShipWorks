CREATE TABLE [dbo].[BestRateShipment] (
    [ShipmentID]     BIGINT     NOT NULL,
    [DimsProfileID]  BIGINT     NOT NULL,
    [DimsLength]     FLOAT (53) NOT NULL,
    [DimsWidth]      FLOAT (53) NOT NULL,
    [DimsHeight]     FLOAT (53) NOT NULL,
    [DimsWeight]     FLOAT (53) NOT NULL,
    [DimsAddWeight]  BIT        NOT NULL,
    [ServiceLevel]   INT        NOT NULL,
    [InsuranceValue] MONEY      NOT NULL,
    CONSTRAINT [PK_BestRateShipment] PRIMARY KEY CLUSTERED ([ShipmentID] ASC),
    CONSTRAINT [FK_BestRateShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
);

