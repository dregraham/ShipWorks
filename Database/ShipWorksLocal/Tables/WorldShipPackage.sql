﻿CREATE TABLE [dbo].[WorldShipPackage] (
    [UpsPackageID]                  BIGINT         NOT NULL,
    [ShipmentID]                    BIGINT         NOT NULL,
    [PackageType]                   VARCHAR (35)   NOT NULL,
    [Weight]                        FLOAT (53)     NOT NULL,
    [ReferenceNumber]               NVARCHAR (35)  NOT NULL,
    [ReferenceNumber2]              NVARCHAR (35)  NOT NULL,
    [CodOption]                     CHAR (1)       NOT NULL,
    [CodAmount]                     MONEY          NOT NULL,
    [CodCashOnly]                   CHAR (1)       NOT NULL,
    [DeliveryConfirmation]          CHAR (1)       NOT NULL,
    [DeliveryConfirmationSignature] CHAR (1)       NOT NULL,
    [DeliveryConfirmationAdult]     CHAR (1)       NOT NULL,
    [Length]                        INT            CONSTRAINT [DF_WorldShipPackage_Length] DEFAULT ('') NOT NULL,
    [Width]                         INT            NOT NULL,
    [Height]                        INT            NOT NULL,
    [DeclaredValueAmount]           FLOAT (53)     NULL,
    [DeclaredValueOption]           NCHAR (2)      NULL,
    [CN22GoodsType]                 NVARCHAR (50)  NULL,
    [CN22Description]               NVARCHAR (100) NULL,
    [PostalSubClass]                NVARCHAR (50)  NULL,
    [MIDeliveryConfirmation]        CHAR (1)       NULL,
    [QvnOption]                     CHAR (1)       NULL,
    [QvnFrom]                       NVARCHAR (35)  NULL,
    [QvnSubjectLine]                NVARCHAR (18)  NULL,
    [QvnMemo]                       NVARCHAR (150) NULL,
    [Qvn1ShipNotify]                CHAR (1)       NULL,
    [Qvn1ContactName]               NVARCHAR (35)  NULL,
    [Qvn1Email]                     NVARCHAR (100) NULL,
    [Qvn2ShipNotify]                CHAR (1)       NULL,
    [Qvn2ContactName]               NVARCHAR (35)  NULL,
    [Qvn2Email]                     NVARCHAR (100) NULL,
    [Qvn3ShipNotify]                CHAR (1)       NULL,
    [Qvn3ContactName]               NVARCHAR (35)  NULL,
    [Qvn3Email]                     NVARCHAR (100) NULL,
    [ShipperRelease]                CHAR (1)       NULL,
    [AdditionalHandlingEnabled]     CHAR (1)       NULL,
    [VerbalConfirmationOption]      CHAR (1)       NULL,
    [VerbalConfirmationContactName] NVARCHAR (35)  NULL,
    [VerbalConfirmationTelephone]   NVARCHAR (15)  NULL,
    [DryIceRegulationSet]           NVARCHAR (5)   NULL,
    [DryIceWeight]                  FLOAT (53)     NULL,
    [DryIceMedicalPurpose]          CHAR (1)       NULL,
    [DryIceOption]                  CHAR (1)       NULL,
    [DryIceWeightUnitOfMeasure]     NVARCHAR (10)  NULL,
    CONSTRAINT [PK_WorldShipPackage] PRIMARY KEY CLUSTERED ([UpsPackageID] ASC),
    CONSTRAINT [FK_WorldShipPackage_WorldShipShipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[WorldShipShipment] ([ShipmentID]) ON DELETE CASCADE
);

