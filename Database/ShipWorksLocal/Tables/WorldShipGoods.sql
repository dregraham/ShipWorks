CREATE TABLE [dbo].[WorldShipGoods] (
    [WorldShipGoodsID]      BIGINT         IDENTITY (1098, 1000) NOT NULL,
    [ShipmentID]            BIGINT         NOT NULL,
    [ShipmentCustomsItemID] BIGINT         NOT NULL,
    [Description]           NVARCHAR (150) NOT NULL,
    [TariffCode]            VARCHAR (15)   NOT NULL,
    [CountryOfOrigin]       VARCHAR (50)   NOT NULL,
    [Units]                 INT            NOT NULL,
    [UnitOfMeasure]         VARCHAR (5)    NOT NULL,
    [UnitPrice]             MONEY          NOT NULL,
    [Weight]                FLOAT (53)     NOT NULL,
    [InvoiceCurrencyCode]   VARCHAR (3)    NULL,
    CONSTRAINT [PK_WorldShipGoods] PRIMARY KEY CLUSTERED ([WorldShipGoodsID] ASC),
    CONSTRAINT [FK_WorldShipGoods_WorldShipShipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[WorldShipShipment] ([ShipmentID]) ON DELETE CASCADE
);

