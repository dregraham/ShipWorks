CREATE TABLE [dbo].[iParcelProfilePackage] (
    [iParcelProfilePackageID] BIGINT     IDENTITY (1094, 1000) NOT NULL,
    [ShippingProfileID]       BIGINT     NOT NULL,
    [Weight]                  FLOAT (53) NULL,
    [DimsProfileID]           BIGINT     NULL,
    [DimsLength]              FLOAT (53) NULL,
    [DimsWidth]               FLOAT (53) NULL,
    [DimsHeight]              FLOAT (53) NULL,
    [DimsWeight]              FLOAT (53) NULL,
    [DimsAddWeight]           BIT        NULL,
    CONSTRAINT [PK_iParcelPackageProfile] PRIMARY KEY CLUSTERED ([iParcelProfilePackageID] ASC),
    CONSTRAINT [FK_iParcelPackageProfile_iParcelProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[iParcelProfile] ([ShippingProfileID]) ON DELETE CASCADE
);

