SET XACT_ABORT ON;
BEGIN TRANSACTION;

-- rename the Express1 shipping settings to Express1Endicia
EXEC sp_rename N'[ShippingSettings].[Express1Thermal]', N'Express1EndiciaThermal';
EXEC sp_rename N'[ShippingSettings].[Express1ThermalType]', N'Express1EndiciaThermalType';
EXEC sp_rename N'[ShippingSettings].[Express1CustomsCertify]', N'Express1EndiciaCustomsCertify';
EXEC sp_rename N'[ShippingSettings].[Express1CustomsSigner]', N'Express1EndiciaCustomsSigner';
EXEC sp_rename N'[ShippingSettings].[Express1ThermalDocTab]', N'Express1EndiciaThermalDocTab';
EXEC sp_rename N'[ShippingSettings].[Express1ThermalDocTabType]', N'Express1EndiciaThermalDocTabType';
EXEC sp_rename N'[ShippingSettings].[Express1SingleSource]', N'Express1EndiciaSingleSource';

-- add the separate Express1Stamps settings
ALTER TABLE [ShippingSettings]
ADD
	[Express1StampsThermal] [bit] NOT NULL CONSTRAINT DF_Express1StampsThermal DEFAULT(0),
	[Express1StampsThermalType] [int] NOT NULL CONSTRAINT DF_Express1StampsThermalType DEFAULT(0),
	[Express1StampsCustomsCertify] [bit] NOT NULL CONSTRAINT DF_Express1StampsCustomsCertify DEFAULT(0),
	[Express1StampsCustomsSigner] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT DF_Express1StampsCustomsSigner DEFAULT(N''),
	[Express1StampsThermalDocTab] [bit] NOT NULL CONSTRAINT DF_Express1StampsThermalDocTab DEFAULT(0),
	[Express1StampsThermalDocTabType] [int] NOT NULL CONSTRAINT DF_Express1StampsThermalDocTabType DEFAULT(0),
	[Express1StampsSingleSource] [bit] NOT NULL CONSTRAINT DF_Express1StampsSingleSource DEFAULT(0);

ALTER TABLE [ShippingSettings]
DROP CONSTRAINT
	DF_Express1StampsThermal,
	DF_Express1StampsThermalType,
	DF_Express1StampsCustomsCertify,
	DF_Express1StampsCustomsSigner,
	DF_Express1StampsThermalDocTab,
	DF_Express1StampsThermalDocTabType,
	DF_Express1StampsSingleSource;

COMMIT TRANSACTION;
