CREATE TABLE [dbo].[iParcelPackage] (
    [iParcelPackageID]  BIGINT         IDENTITY (1093, 1000) NOT NULL,
    [ShipmentID]        BIGINT         NOT NULL,
    [Weight]            FLOAT (53)     NOT NULL,
    [DimsProfileID]     BIGINT         NOT NULL,
    [DimsLength]        FLOAT (53)     NOT NULL,
    [DimsWidth]         FLOAT (53)     NOT NULL,
    [DimsHeight]        FLOAT (53)     NOT NULL,
    [DimsAddWeight]     BIT            NOT NULL,
    [DimsWeight]        FLOAT (53)     NOT NULL,
    [Insurance]         BIT            NOT NULL,
    [InsuranceValue]    MONEY          NOT NULL,
    [InsurancePennyOne] BIT            NOT NULL,
    [DeclaredValue]     MONEY          NOT NULL,
    [TrackingNumber]    VARCHAR (50)   NOT NULL,
    [ParcelNumber]      NVARCHAR (50)  NOT NULL,
    [SkuAndQuantities]  NVARCHAR (500) NOT NULL,
    CONSTRAINT [PK_iParcelPackage] PRIMARY KEY CLUSTERED ([iParcelPackageID] ASC),
    CONSTRAINT [FK_iParcelPackage_iParcelShipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[iParcelShipment] ([ShipmentID]) ON DELETE CASCADE
);

