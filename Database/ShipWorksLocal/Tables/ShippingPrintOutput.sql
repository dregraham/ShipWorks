CREATE TABLE [dbo].[ShippingPrintOutput] (
    [ShippingPrintOutputID] BIGINT        IDENTITY (1058, 1000) NOT NULL,
    [RowVersion]            ROWVERSION    NOT NULL,
    [ShipmentType]          INT           NOT NULL,
    [Name]                  NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_ShippingPrintOutput] PRIMARY KEY CLUSTERED ([ShippingPrintOutputID] ASC)
);


GO
ALTER TABLE [dbo].[ShippingPrintOutput] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);

