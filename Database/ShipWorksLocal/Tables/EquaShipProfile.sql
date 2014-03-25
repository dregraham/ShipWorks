CREATE TABLE [dbo].[EquaShipProfile] (
    [ShippingProfileID] BIGINT         NOT NULL,
    [EquaShipAccountID] BIGINT         NULL,
    [Service]           INT            NULL,
    [PackageType]       INT            NULL,
    [ReferenceNumber]   NVARCHAR (300) NULL,
    [Description]       NVARCHAR (300) NULL,
    [ShippingNotes]     NVARCHAR (300) NULL,
    [Weight]            FLOAT (53)     NULL,
    [DimsProfileID]     BIGINT         NULL,
    [DimsLength]        FLOAT (53)     NULL,
    [DimsHeight]        FLOAT (53)     NULL,
    [DimsWidth]         FLOAT (53)     NULL,
    [DimsWeight]        FLOAT (53)     NULL,
    [DimsAddWeight]     BIT            NULL,
    [DeclaredValue]     MONEY          NULL,
    [EmailNotification] BIT            NULL,
    [SaturdayDelivery]  BIT            NULL,
    [Confirmation]      INT            NULL,
    CONSTRAINT [PK_EquashipProfile] PRIMARY KEY CLUSTERED ([ShippingProfileID] ASC),
    CONSTRAINT [FK_EquashipProfile_ShippingProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[ShippingProfile] ([ShippingProfileID]) ON DELETE CASCADE
);

