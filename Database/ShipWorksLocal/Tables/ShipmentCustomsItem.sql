CREATE TABLE [dbo].[ShipmentCustomsItem] (
    [ShipmentCustomsItemID] BIGINT         IDENTITY (1051, 1000) NOT NULL,
    [RowVersion]            ROWVERSION     NOT NULL,
    [ShipmentID]            BIGINT         NOT NULL,
    [Description]           NVARCHAR (150) NOT NULL,
    [Quantity]              FLOAT (53)     NOT NULL,
    [Weight]                FLOAT (53)     NOT NULL,
    [UnitValue]             MONEY          NOT NULL,
    [CountryOfOrigin]       NVARCHAR (50)  NOT NULL,
    [HarmonizedCode]        VARCHAR (14)   NOT NULL,
    [NumberOfPieces]        INT            NOT NULL,
    [UnitPriceAmount]       MONEY          NOT NULL,
    CONSTRAINT [PK_ShipmentCustomsItem] PRIMARY KEY CLUSTERED ([ShipmentCustomsItemID] ASC),
    CONSTRAINT [FK_ShipmentCustomsItem_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
);


GO
ALTER TABLE [dbo].[ShipmentCustomsItem] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);

