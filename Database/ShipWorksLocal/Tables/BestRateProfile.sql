CREATE TABLE [dbo].[BestRateProfile] (
    [ShippingProfileID] BIGINT     NOT NULL,
    [DimsProfileID]     BIGINT     NULL,
    [DimsLength]        FLOAT (53) NULL,
    [DimsWidth]         FLOAT (53) NULL,
    [DimsHeight]        FLOAT (53) NULL,
    [DimsWeight]        FLOAT (53) NULL,
    [DimsAddWeight]     BIT        NULL,
    [Weight]            FLOAT (53) NULL,
    [ServiceLevel]      INT        NULL,
    CONSTRAINT [PK_BestRateProfile] PRIMARY KEY CLUSTERED ([ShippingProfileID] ASC),
    CONSTRAINT [FK_BestRateProfile_ShippingProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[ShippingProfile] ([ShippingProfileID]) ON DELETE CASCADE
);

