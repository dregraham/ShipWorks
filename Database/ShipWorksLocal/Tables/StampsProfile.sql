CREATE TABLE [dbo].[StampsProfile] (
    [ShippingProfileID]            BIGINT         NOT NULL,
    [StampsAccountID]              BIGINT         NULL,
    [HidePostage]                  BIT            NULL,
    [RequireFullAddressValidation] BIT            NULL,
    [Memo]                         NVARCHAR (200) NULL,
    CONSTRAINT [PK_StampsProfile] PRIMARY KEY CLUSTERED ([ShippingProfileID] ASC),
    CONSTRAINT [FK_StampsProfile_PostalProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[PostalProfile] ([ShippingProfileID]) ON DELETE CASCADE
);

