SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
PRINT N'Dropping foreign keys from [dbo].[MagentoStore]'
GO
ALTER TABLE [dbo].[MagentoStore] DROP
CONSTRAINT [FK_MagentoStore_GenericStore]
GO
PRINT N'Dropping foreign keys from [dbo].[MivaStore]'
GO
ALTER TABLE [dbo].[MivaStore] DROP
CONSTRAINT [FK_MivaStore_GenericStore]
GO
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
[InsuranceAgreement] [bit] NOT NULL,
[FedExUsername] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[FedExPassword] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[FedExMaskAccount] [bit] NOT NULL,
[FedExLimitValue] [bit] NOT NULL,
[FedExThermal] [bit] NOT NULL,
[FedExThermalType] [int] NOT NULL,
[FedExThermalDocTab] [bit] NOT NULL,
[FedExThermalDocTabType] [int] NOT NULL,
[UpsAccessKey] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[UpsThermal] [bit] NOT NULL,
[UpsThermalType] [int] NOT NULL,
[EndiciaThermal] [bit] NOT NULL,
[EndiciaThermalType] [int] NOT NULL,
[EndiciaCustomsCertify] [bit] NOT NULL,
[EndiciaCustomsSigner] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[EndiciaThermalDocTab] [bit] NOT NULL,
[EndiciaThermalDocTabType] [int] NOT NULL CONSTRAINT [DF_ShippingSettings_EndiciaThermalDocTabType] DEFAULT ((0)),
[WorldShipLaunch] [bit] NOT NULL,
[StampsThermal] [bit] NOT NULL,
[StampsThermalType] [int] NOT NULL,
[Express1Thermal] [bit] NOT NULL,
[Express1ThermalType] [int] NOT NULL,
[Express1CustomsCertify] [bit] NOT NULL,
[Express1CustomsSigner] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Express1ThermalDocTab] [bit] NOT NULL,
[Express1ThermalDocTabType] [int] NOT NULL
)
GO
INSERT INTO [dbo].[tmp_rg_xx_ShippingSettings]([ShippingSettingsID], [Activated], [Configured], [Excluded], [DefaultType], [BlankPhoneOption], [BlankPhoneNumber], [InsuranceAgreement], [FedExUsername], [FedExPassword], [FedExMaskAccount], [FedExLimitValue], [FedExThermal], [FedExThermalType], [FedExThermalDocTab], [FedExThermalDocTabType], [UpsAccessKey], [UpsThermal], [UpsThermalType], [EndiciaThermal], [EndiciaThermalType], [EndiciaCustomsCertify], [EndiciaCustomsSigner], [EndiciaThermalDocTab], [EndiciaThermalDocTabType], [WorldShipLaunch], [StampsThermal], [StampsThermalType], [Express1Thermal], [Express1ThermalType], [Express1CustomsCertify], [Express1CustomsSigner], [Express1ThermalDocTab], [Express1ThermalDocTabType]) SELECT [ShippingSettingsID], [Activated], [Configured], [Excluded], [DefaultType], [BlankPhoneOption], [BlankPhoneNumber], [InsuranceAgreement], [FedExUsername], [FedExPassword], [FedExMaskAccount], [FedExLimitValue], [FedExThermal], [FedExThermalType], [FedExThermalDocTab], [FedExThermalDocTabType], [UpsAccessKey], [UpsThermal], [UpsThermalType], [EndiciaThermal], [EndiciaThermalType], [EndiciaCustomsCertify], [EndiciaCustomsSigner], 0, 0, [WorldShipLaunch], [StampsThermal], [StampsThermalType], [Express1Thermal], [Express1ThermalType], [Express1CustomsCertify], [Express1CustomsSigner], 0, 0 FROM [dbo].[ShippingSettings]
GO
DROP TABLE [dbo].[ShippingSettings]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_ShippingSettings]', N'ShippingSettings'
GO
PRINT N'Creating primary key [PK_ShippingSettings] on [dbo].[ShippingSettings]'
GO
ALTER TABLE [dbo].[ShippingSettings] ADD CONSTRAINT [PK_ShippingSettings] PRIMARY KEY CLUSTERED  ([ShippingSettingsID])
GO
PRINT N'Creating [dbo].[GenericFileStore]'
GO
CREATE TABLE [dbo].[GenericFileStore]
(
[StoreID] [bigint] NOT NULL,
[FileFormat] [int] NOT NULL,
[FileSource] [int] NOT NULL,
[DiskFolder] [nvarchar] (355) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FtpFolder] [nvarchar] (355) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FtpUsername] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FtpPassword] [nvarchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[EmailAccountID] [bigint] NULL,
[NamePatternMustMatch] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[NamePatternCantMatch] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[SuccessAction] [int] NOT NULL,
[SuccessMoveFolder] [nvarchar] (355) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ErrorAction] [int] NOT NULL,
[ErrorMoveFolder] [nvarchar] (355) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[XmlXsltFileName] [nvarchar] (355) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[XmlXsltContent] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[FlatImportMap] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_GenericFileStore] on [dbo].[GenericFileStore]'
GO
ALTER TABLE [dbo].[GenericFileStore] ADD CONSTRAINT [PK_GenericFileStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[GenericFileStore]'
GO
ALTER TABLE [dbo].[GenericFileStore] ADD
CONSTRAINT [FK_GenericFileStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
EXECUTE sp_rename N'dbo.GenericStore', N'GenericModuleStore', 'OBJECT' 
GO
PRINT N'Dropping foreign keys from [dbo].[GenericModuleStore]'
GO
ALTER TABLE [dbo].[GenericModuleStore] DROP
CONSTRAINT [FK_GenericStore_Store]
GO
PRINT N'Dropping constraints from [dbo].[GenericModuleStore]'
GO
ALTER TABLE [dbo].[GenericModuleStore] DROP CONSTRAINT [PK_GenericStore]
GO
PRINT N'Rebuilding [dbo].[GenericModuleStore]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_GenericModuleStore]
(
[StoreID] [bigint] NOT NULL,
[ModuleUsername] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ModulePassword] [nvarchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ModuleUrl] [nvarchar] (350) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ModuleVersion] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ModulePlatform] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ModuleDeveloper] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ModuleOnlineStoreCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ModuleStatusCodes] [xml] NOT NULL,
[ModuleDownloadPageSize] [int] NOT NULL,
[ModuleRequestTimeout] [int] NOT NULL,
[ModuleDownloadStrategy] [int] NOT NULL,
[ModuleOnlineStatusSupport] [int] NOT NULL,
[ModuleOnlineStatusDataType] [int] NOT NULL,
[ModuleOnlineCustomerSupport] [bit] NOT NULL,
[ModuleOnlineCustomerDataType] [int] NOT NULL,
[ModuleOnlineShipmentDetails] [bit] NOT NULL,
[ModuleHttpExpect100Continue] [bit] NOT NULL,
[ModuleResponseEncoding] [int] NOT NULL
)
GO
INSERT INTO [dbo].[tmp_rg_xx_GenericModuleStore]([StoreID], [ModuleUsername], [ModulePassword], [ModuleUrl], [ModuleVersion], [ModulePlatform], [ModuleDeveloper], [ModuleOnlineStoreCode], [ModuleStatusCodes], [ModuleDownloadPageSize], [ModuleRequestTimeout], [ModuleDownloadStrategy], [ModuleOnlineStatusSupport], [ModuleOnlineStatusDataType], [ModuleOnlineCustomerSupport], [ModuleOnlineCustomerDataType], [ModuleOnlineShipmentDetails], [ModuleHttpExpect100Continue], [ModuleResponseEncoding]) SELECT [StoreID], [Username], [Password], [ModuleUrl], [ModuleVersion], [ModulePlatform], [ModuleDeveloper], [OnlineStoreCode], [StatusCodes], [DownloadPageSize], [RequestTimeout], [DownloadStrategy], [OnlineStatusSupport], [OnlineStatusDataType], [OnlineCustomerSupport], [OnlineCustomerDataType], [OnlineShipmentDetails], [HttpExpect100Continue], [ResponseEncoding] FROM [dbo].[GenericModuleStore]
GO
DROP TABLE [dbo].[GenericModuleStore]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_GenericModuleStore]', N'GenericModuleStore'
GO
PRINT N'Creating primary key [PK_GenericModuleStore] on [dbo].[GenericModuleStore]'
GO
ALTER TABLE [dbo].[GenericModuleStore] ADD CONSTRAINT [PK_GenericModuleStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[GenericModuleStore]'
GO
ALTER TABLE [dbo].[GenericModuleStore] ADD
CONSTRAINT [FK_GenericModuleStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[MagentoStore]'
GO
ALTER TABLE [dbo].[MagentoStore] ADD
CONSTRAINT [FK_MagentoStore_GenericModuleStore] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[GenericModuleStore] ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[MivaStore]'
GO
ALTER TABLE [dbo].[MivaStore] ADD
CONSTRAINT [FK_MivaStore_GenericModuleStore] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[GenericModuleStore] ([StoreID])
GO
