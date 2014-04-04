CREATE TABLE [dbo].[iParcelProfile] (
    [ShippingProfileID]  BIGINT         NOT NULL,
    [iParcelAccountID]   BIGINT         NULL,
    [Service]            INT            NULL,
    [Reference]          NVARCHAR (300) NULL,
    [TrackByEmail]       BIT            NULL,
    [TrackBySMS]         BIT            NULL,
    [IsDeliveryDutyPaid] BIT            NULL,
    [SkuAndQuantities]   NVARCHAR (500) NULL,
    CONSTRAINT [PK_iParcelProfile] PRIMARY KEY CLUSTERED ([ShippingProfileID] ASC),
    CONSTRAINT [FK_iParcelProfile_ShippingProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[ShippingProfile] ([ShippingProfileID]) ON DELETE CASCADE
);

