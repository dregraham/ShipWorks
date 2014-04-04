CREATE TABLE [dbo].[UpsProfilePackage] (
    [UpsProfilePackageID]              BIGINT        IDENTITY (1064, 1000) NOT NULL,
    [ShippingProfileID]                BIGINT        NOT NULL,
    [PackagingType]                    INT           NULL,
    [Weight]                           FLOAT (53)    NULL,
    [DimsProfileID]                    BIGINT        NULL,
    [DimsLength]                       FLOAT (53)    NULL,
    [DimsWidth]                        FLOAT (53)    NULL,
    [DimsHeight]                       FLOAT (53)    NULL,
    [DimsWeight]                       FLOAT (53)    NULL,
    [DimsAddWeight]                    BIT           NULL,
    [AdditionalHandlingEnabled]        BIT           NULL,
    [VerbalConfirmationEnabled]        BIT           NULL,
    [VerbalConfirmationName]           NVARCHAR (35) NULL,
    [VerbalConfirmationPhone]          NVARCHAR (15) NULL,
    [VerbalConfirmationPhoneExtension] NVARCHAR (4)  NULL,
    [DryIceEnabled]                    BIT           NULL,
    [DryIceRegulationSet]              INT           NULL,
    [DryIceWeight]                     FLOAT (53)    NULL,
    [DryIceIsForMedicalUse]            BIT           NULL,
    CONSTRAINT [PK_UpsProfilePackage] PRIMARY KEY CLUSTERED ([UpsProfilePackageID] ASC),
    CONSTRAINT [FK_UpsProfilePackage_UpsProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[UpsProfile] ([ShippingProfileID]) ON DELETE CASCADE
);

