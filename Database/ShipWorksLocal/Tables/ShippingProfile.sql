CREATE TABLE [dbo].[ShippingProfile] (
    [ShippingProfileID]           BIGINT        IDENTITY (1053, 1000) NOT NULL,
    [RowVersion]                  ROWVERSION    NOT NULL,
    [Name]                        NVARCHAR (50) NOT NULL,
    [ShipmentType]                INT           NOT NULL,
    [ShipmentTypePrimary]         BIT           NOT NULL,
    [OriginID]                    BIGINT        NULL,
    [Insurance]                   BIT           NULL,
    [InsuranceInitialValueSource] INT           NULL,
    [InsuranceInitialValueAmount] MONEY         NULL,
    [ReturnShipment]              BIT           NULL,
    CONSTRAINT [PK_ShippingProfile] PRIMARY KEY CLUSTERED ([ShippingProfileID] ASC)
);


GO
ALTER TABLE [dbo].[ShippingProfile] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);

