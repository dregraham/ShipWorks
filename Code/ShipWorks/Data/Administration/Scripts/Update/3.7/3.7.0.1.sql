PRINT N'Altering [dbo].[WorldShipProcessed]'
GO

IF COL_LENGTH('WorldShipProcessed','ShipmentIdCalculated') IS NULL
	ALTER TABLE [dbo].[WorldShipProcessed] ADD
	[ShipmentIdCalculated] AS (case when isnumeric([ShipmentID]+'.e0')=(1) then CONVERT([bigint],[ShipmentID],(0))  end) PERSISTED
	GO
-- Add Best Rate Shipment Type to the activated and configured shipment types
update ShippingSettings 
	set Activated = 
		  CASE 
			 WHEN len(Activated) > 0 THEN Activated + ',14'
			 ELSE '14'
		  END,
	Configured = 
		  CASE 
			 WHEN len(Configured) > 0 THEN Configured + ',14'
			 ELSE '14'
		  END
GO

PRINT N'Creating [dbo].[BestRateShipment]'
GO
PRINT N'Creating [dbo].[BestRateShipment]'
CREATE TABLE [dbo].[BestRateShipment]
(
[ShipmentID] [bigint] NOT NULL,
[DimsProfileID] [bigint] NOT NULL,
[DimsLength] [float] NOT NULL,
[DimsWidth] [float] NOT NULL,
[DimsHeight] [float] NOT NULL,
[DimsWeight] [float] NOT NULL,
[DimsAddWeight] [bit] NOT NULL,
[ServiceLevel] [int] NOT NULL,
[InsuranceValue] [money] NOT NULL
)

GO

PRINT N'Creating primary key [PK_BestRateShipment] on [dbo].[BestRateShipment]'
GO
ALTER TABLE [dbo].[BestRateShipment] ADD CONSTRAINT [PK_BestRateShipment] PRIMARY KEY CLUSTERED  ([ShipmentID])
GO

PRINT N'Creating foriegn key constraint for [dbo].[BestRateShipment] and Shipment'
ALTER TABLE [dbo].[BestRateShipment]  WITH CHECK ADD  CONSTRAINT [FK_BestRateShipment_Shipment] FOREIGN KEY([ShipmentID])
REFERENCES [dbo].[Shipment] ([ShipmentID])
ON DELETE CASCADE
GO

CREATE TABLE [dbo].[BestRateProfile]
(
[ShippingProfileID] [bigint] NOT NULL,
[DimsProfileID] [bigint] NULL,
[DimsLength] [float] NULL,
[DimsWidth] [float] NULL,
[DimsHeight] [float] NULL,
[DimsWeight] [float] NULL,
[DimsAddWeight] [bit] NULL,
[Weight] [float] NULL,
[ServiceLevel] [int] NULL
)
GO
PRINT N'Creating primary key [PK_BestRateProfile] on [dbo].[BestRateProfile]'
GO
ALTER TABLE [dbo].[BestRateProfile] ADD CONSTRAINT [PK_BestRateProfile] PRIMARY KEY CLUSTERED  ([ShippingProfileID])
GO
-- Create default best rate profile
INSERT INTO [dbo].[ShippingProfile] ([Name], [ShipmentType], [ShipmentTypePrimary], [OriginID], [Insurance], [InsuranceInitialValueSource], [InsuranceInitialValueAmount], [ReturnShipment])
VALUES ('Defaults - Best rate', 14, 1, 0, 0, 0, 0.00, 0)
GO
PRINT N'Adding foreign keys to [dbo].[BestRateProfile]'
GO
ALTER TABLE [dbo].[BestRateProfile] ADD CONSTRAINT [FK_BestRateProfile_ShippingProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[ShippingProfile] ([ShippingProfileID]) ON DELETE CASCADE
GO

INSERT INTO [dbo].[BestRateProfile] ([ShippingProfileID], [DimsProfileID], [DimsLength], [DimsWidth], [DimsHeight], [DimsWeight], [DimsAddWeight], [Weight], [ServiceLevel])
SELECT TOP 1 ShippingProfileID, 0, 0, 0, 0, 0, 0, 0, 0  FROM ShippingProfile WHERE ShipmentType = 14
GO


-- Script for adding BestRateExcludedShipmentTypes column
PRINT N'Dropping constraints from [dbo].[ShippingSettings]'
GO
ALTER TABLE [dbo].[ShippingSettings] DROP CONSTRAINT [PK_ShippingSettings]
GO
PRINT N'Rebuilding [dbo].[ShippingSettings]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_ShippingSettings]
(
[ShippingSettingsID] [bit] NOT NULL,
[Activated] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Configured] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Excluded] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DefaultType] [int] NOT NULL,
[BlankPhoneOption] [int] NOT NULL,
[BlankPhoneNumber] [nvarchar] (16) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[InsurancePolicy] [nvarchar] (40) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[InsuranceLastAgreed] [datetime] NULL,
[FedExUsername] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[FedExPassword] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[FedExMaskAccount] [bit] NOT NULL,
[FedExThermal] [bit] NOT NULL,
[FedExThermalType] [int] NOT NULL,
[FedExThermalDocTab] [bit] NOT NULL,
[FedExThermalDocTabType] [int] NOT NULL,
[FedExInsuranceProvider] [int] NOT NULL,
[FedExInsurancePennyOne] [bit] NOT NULL,
[UpsAccessKey] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[UpsThermal] [bit] NOT NULL,
[UpsThermalType] [int] NOT NULL,
[UpsInsuranceProvider] [int] NOT NULL,
[UpsInsurancePennyOne] [bit] NOT NULL,
[EndiciaThermal] [bit] NOT NULL,
[EndiciaThermalType] [int] NOT NULL,
[EndiciaCustomsCertify] [bit] NOT NULL,
[EndiciaCustomsSigner] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[EndiciaThermalDocTab] [bit] NOT NULL,
[EndiciaThermalDocTabType] [int] NOT NULL,
[EndiciaAutomaticExpress1] [bit] NOT NULL,
[EndiciaAutomaticExpress1Account] [bigint] NOT NULL,
[EndiciaInsuranceProvider] [int] NOT NULL,
[WorldShipLaunch] [bit] NOT NULL,
[StampsThermal] [bit] NOT NULL,
[StampsThermalType] [int] NOT NULL,
[StampsAutomaticExpress1] [bit] NOT NULL,
[StampsAutomaticExpress1Account] [bigint] NOT NULL,
[Express1EndiciaThermal] [bit] NOT NULL,
[Express1EndiciaThermalType] [int] NOT NULL,
[Express1EndiciaCustomsCertify] [bit] NOT NULL,
[Express1EndiciaCustomsSigner] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Express1EndiciaThermalDocTab] [bit] NOT NULL,
[Express1EndiciaThermalDocTabType] [int] NOT NULL,
[Express1EndiciaSingleSource] [bit] NOT NULL,
[EquaShipThermal] [bit] NOT NULL,
[EquaShipThermalType] [int] NOT NULL,
[OnTracThermal] [bit] NOT NULL,
[OnTracThermalType] [int] NOT NULL,
[OnTracInsuranceProvider] [int] NOT NULL,
[OnTracInsurancePennyOne] [bit] NOT NULL,
[iParcelThermal] [bit] NOT NULL,
[iParcelThermalType] [int] NOT NULL,
[iParcelInsuranceProvider] [int] NOT NULL,
[iParcelInsurancePennyOne] [bit] NOT NULL,
[Express1StampsThermal] [bit] NOT NULL,
[Express1StampsThermalType] [int] NOT NULL,
[Express1StampsSingleSource] [bit] NOT NULL,
[UpsMailInnovationsEnabled] [bit] NOT NULL,
[WorldShipMailInnovationsEnabled] [bit] NOT NULL,
[BestRateExcludedShipmentTypes] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
INSERT INTO [dbo].[tmp_rg_xx_ShippingSettings]([ShippingSettingsID], [Activated], [Configured], [Excluded], [DefaultType], [BlankPhoneOption], [BlankPhoneNumber], [InsurancePolicy], [InsuranceLastAgreed], [FedExUsername], [FedExPassword], [FedExMaskAccount], [FedExThermal], [FedExThermalType], [FedExThermalDocTab], [FedExThermalDocTabType], [FedExInsuranceProvider], [FedExInsurancePennyOne], [UpsAccessKey], [UpsThermal], [UpsThermalType], [UpsInsuranceProvider], [UpsInsurancePennyOne], [EndiciaThermal], [EndiciaThermalType], [EndiciaCustomsCertify], [EndiciaCustomsSigner], [EndiciaThermalDocTab], [EndiciaThermalDocTabType], [EndiciaAutomaticExpress1], [EndiciaAutomaticExpress1Account], [EndiciaInsuranceProvider], [WorldShipLaunch], [StampsThermal], [StampsThermalType], [StampsAutomaticExpress1], [StampsAutomaticExpress1Account], [Express1EndiciaThermal], [Express1EndiciaThermalType], [Express1EndiciaCustomsCertify], [Express1EndiciaCustomsSigner], [Express1EndiciaThermalDocTab], [Express1EndiciaThermalDocTabType], [Express1EndiciaSingleSource], [EquaShipThermal], [EquaShipThermalType], [OnTracThermal], [OnTracThermalType], [OnTracInsuranceProvider], [OnTracInsurancePennyOne], [iParcelThermal], [iParcelThermalType], [iParcelInsuranceProvider], [iParcelInsurancePennyOne], [Express1StampsThermal], [Express1StampsThermalType], [Express1StampsSingleSource], [UpsMailInnovationsEnabled], [WorldShipMailInnovationsEnabled], [BestRateExcludedShipmentTypes]) SELECT [ShippingSettingsID], [Activated], [Configured], [Excluded], [DefaultType], [BlankPhoneOption], [BlankPhoneNumber], [InsurancePolicy], [InsuranceLastAgreed], [FedExUsername], [FedExPassword], [FedExMaskAccount], [FedExThermal], [FedExThermalType], [FedExThermalDocTab], [FedExThermalDocTabType], [FedExInsuranceProvider], [FedExInsurancePennyOne], [UpsAccessKey], [UpsThermal], [UpsThermalType], [UpsInsuranceProvider], [UpsInsurancePennyOne], [EndiciaThermal], [EndiciaThermalType], [EndiciaCustomsCertify], [EndiciaCustomsSigner], [EndiciaThermalDocTab], [EndiciaThermalDocTabType], [EndiciaAutomaticExpress1], [EndiciaAutomaticExpress1Account], [EndiciaInsuranceProvider], [WorldShipLaunch], [StampsThermal], [StampsThermalType], [StampsAutomaticExpress1], [StampsAutomaticExpress1Account], [Express1EndiciaThermal], [Express1EndiciaThermalType], [Express1EndiciaCustomsCertify], [Express1EndiciaCustomsSigner], [Express1EndiciaThermalDocTab], [Express1EndiciaThermalDocTabType], [Express1EndiciaSingleSource], [EquaShipThermal], [EquaShipThermalType], [OnTracThermal], [OnTracThermalType], [OnTracInsuranceProvider], [OnTracInsurancePennyOne], [iParcelThermal], [iParcelThermalType], [iParcelInsuranceProvider], [iParcelInsurancePennyOne], [Express1StampsThermal], [Express1StampsThermalType], [Express1StampsSingleSource], [UpsMailInnovationsEnabled], [WorldShipMailInnovationsEnabled], '' FROM [dbo].[ShippingSettings]
GO
DROP TABLE [dbo].[ShippingSettings]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_ShippingSettings]', N'ShippingSettings'
GO
PRINT N'Creating primary key [PK_ShippingSettings] on [dbo].[ShippingSettings]'
GO
ALTER TABLE [dbo].[ShippingSettings] ADD CONSTRAINT [PK_ShippingSettings] PRIMARY KEY CLUSTERED  ([ShippingSettingsID])
GO

-- We're adding a column rather than recreating the table and copying the data because the shipment table could be very large for some customers
ALTER TABLE [dbo].[Shipment] ADD [BestRateEvents] [tinyint] NULL
GO
UPDATE Shipment SET BestRateEvents = 0
GO
ALTER TABLE [dbo].[Shipment] ALTER COLUMN [BestRateEvents] [tinyint] NOT NULL
GO

UPDATE ShippingSettings
SET	FedExUsername = '07AQFKOy51LbLAhK',
	FedExPassword = '3pi4NjRiialJkTS24bZcZqg/NgAfdfWewwpatUMzqcs='
	WHERE ISNULL(FedExUsername,'') != ''
GO