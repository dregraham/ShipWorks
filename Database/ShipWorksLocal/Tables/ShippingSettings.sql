﻿CREATE TABLE [dbo].[ShippingSettings] (
    [ShippingSettingsID]               BIT            NOT NULL,
    [Activated]                        VARCHAR (30)   NOT NULL,
    [Configured]                       VARCHAR (30)   NOT NULL,
    [Excluded]                         VARCHAR (30)   NOT NULL,
    [DefaultType]                      INT            NOT NULL,
    [BlankPhoneOption]                 INT            NOT NULL,
    [BlankPhoneNumber]                 NVARCHAR (16)  NOT NULL,
    [InsurancePolicy]                  NVARCHAR (40)  NOT NULL,
    [InsuranceLastAgreed]              DATETIME       NULL,
    [FedExUsername]                    NVARCHAR (50)  NULL,
    [FedExPassword]                    NVARCHAR (50)  NULL,
    [FedExMaskAccount]                 BIT            NOT NULL,
    [FedExThermal]                     BIT            NOT NULL,
    [FedExThermalType]                 INT            NOT NULL,
    [FedExThermalDocTab]               BIT            NOT NULL,
    [FedExThermalDocTabType]           INT            NOT NULL,
    [FedExInsuranceProvider]           INT            NOT NULL,
    [FedExInsurancePennyOne]           BIT            NOT NULL,
    [UpsAccessKey]                     NVARCHAR (50)  NULL,
    [UpsThermal]                       BIT            NOT NULL,
    [UpsThermalType]                   INT            NOT NULL,
    [UpsInsuranceProvider]             INT            NOT NULL,
    [UpsInsurancePennyOne]             BIT            NOT NULL,
    [EndiciaThermal]                   BIT            NOT NULL,
    [EndiciaThermalType]               INT            NOT NULL,
    [EndiciaCustomsCertify]            BIT            NOT NULL,
    [EndiciaCustomsSigner]             NVARCHAR (100) NOT NULL,
    [EndiciaThermalDocTab]             BIT            NOT NULL,
    [EndiciaThermalDocTabType]         INT            NOT NULL,
    [EndiciaAutomaticExpress1]         BIT            NOT NULL,
    [EndiciaAutomaticExpress1Account]  BIGINT         NOT NULL,
    [EndiciaInsuranceProvider]         INT            NOT NULL,
    [WorldShipLaunch]                  BIT            NOT NULL,
    [StampsThermal]                    BIT            NOT NULL,
    [StampsThermalType]                INT            NOT NULL,
    [StampsAutomaticExpress1]          BIT            NOT NULL,
    [StampsAutomaticExpress1Account]   BIGINT         NOT NULL,
    [Express1EndiciaThermal]           BIT            NOT NULL,
    [Express1EndiciaThermalType]       INT            NOT NULL,
    [Express1EndiciaCustomsCertify]    BIT            NOT NULL,
    [Express1EndiciaCustomsSigner]     NVARCHAR (100) NOT NULL,
    [Express1EndiciaThermalDocTab]     BIT            NOT NULL,
    [Express1EndiciaThermalDocTabType] INT            NOT NULL,
    [Express1EndiciaSingleSource]      BIT            NOT NULL,
    [EquaShipThermal]                  BIT            NOT NULL,
    [EquaShipThermalType]              INT            NOT NULL,
    [OnTracThermal]                    BIT            NOT NULL,
    [OnTracThermalType]                INT            NOT NULL,
    [OnTracInsuranceProvider]          INT            NOT NULL,
    [OnTracInsurancePennyOne]          BIT            NOT NULL,
    [iParcelThermal]                   BIT            NOT NULL,
    [iParcelThermalType]               INT            NOT NULL,
    [iParcelInsuranceProvider]         INT            NOT NULL,
    [iParcelInsurancePennyOne]         BIT            NOT NULL,
    [Express1StampsThermal]            BIT            NOT NULL,
    [Express1StampsThermalType]        INT            NOT NULL,
    [Express1StampsSingleSource]       BIT            NOT NULL,
    [UpsMailInnovationsEnabled]        BIT            NOT NULL,
    [WorldShipMailInnovationsEnabled]  BIT            NOT NULL,
    [BestRateExcludedShipmentTypes]    NVARCHAR (30)  NOT NULL,
    [ShipSenseEnabled]                 BIT            NOT NULL,
    [ShipSenseUniquenessXml]           XML            NOT NULL,
    [ShipSenseProcessedShipmentID]     BIGINT         NOT NULL,
    [ShipSenseEndShipmentID]           BIGINT         NOT NULL,
    CONSTRAINT [PK_ShippingSettings] PRIMARY KEY CLUSTERED ([ShippingSettingsID] ASC)
);

