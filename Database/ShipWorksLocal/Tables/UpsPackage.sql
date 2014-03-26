CREATE TABLE [dbo].[UpsPackage] (
    [UpsPackageID]                     BIGINT        IDENTITY (1063, 1000) NOT NULL,
    [ShipmentID]                       BIGINT        NOT NULL,
    [PackagingType]                    INT           NOT NULL,
    [Weight]                           FLOAT (53)    NOT NULL,
    [DimsProfileID]                    BIGINT        NOT NULL,
    [DimsLength]                       FLOAT (53)    NOT NULL,
    [DimsWidth]                        FLOAT (53)    NOT NULL,
    [DimsHeight]                       FLOAT (53)    NOT NULL,
    [DimsWeight]                       FLOAT (53)    NOT NULL,
    [DimsAddWeight]                    BIT           NOT NULL,
    [Insurance]                        BIT           NOT NULL,
    [InsuranceValue]                   MONEY         NOT NULL,
    [InsurancePennyOne]                BIT           NOT NULL,
    [DeclaredValue]                    MONEY         NOT NULL,
    [TrackingNumber]                   NVARCHAR (50) NOT NULL,
    [UspsTrackingNumber]               NVARCHAR (50) NOT NULL,
    [AdditionalHandlingEnabled]        BIT           NOT NULL,
    [VerbalConfirmationEnabled]        BIT           NOT NULL,
    [VerbalConfirmationName]           NVARCHAR (35) NOT NULL,
    [VerbalConfirmationPhone]          NVARCHAR (15) NOT NULL,
    [VerbalConfirmationPhoneExtension] NVARCHAR (4)  NOT NULL,
    [DryIceEnabled]                    BIT           NOT NULL,
    [DryIceRegulationSet]              INT           NOT NULL,
    [DryIceWeight]                     FLOAT (53)    NOT NULL,
    [DryIceIsForMedicalUse]            BIT           NOT NULL,
    CONSTRAINT [PK_UpsPackage] PRIMARY KEY CLUSTERED ([UpsPackageID] ASC),
    CONSTRAINT [FK_UpsPackage_UpsShipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[UpsShipment] ([ShipmentID]) ON DELETE CASCADE
);


GO
CREATE TRIGGER [dbo].[UpsPackageDeleteTrigger]
    ON [dbo].[UpsPackage]
    AFTER DELETE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[UpsPackageDeleteTrigger]

