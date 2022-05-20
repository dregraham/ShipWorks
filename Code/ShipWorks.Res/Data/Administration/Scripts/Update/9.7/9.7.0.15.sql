
PRINT N'Creating [dbo].[DhlEcommerceShipment]'
GO
IF OBJECT_ID(N'[dbo].[DhlEcommerceShipment]', 'U') IS NULL
    CREATE TABLE [dbo].[DhlEcommerceShipment]
    (
    [ShipmentID] [bigint] NOT NULL,
    [DhlEcommerceAccountID] [bigint] NOT NULL,
    [Service] [int] NOT NULL,
    [DeliveredDutyPaid] [bit] NOT NULL,
    [NonMachinable] [bit] NOT NULL,
    [SaturdayDelivery] [bit] NOT NULL,
    [RequestedLabelFormat] [int] NOT NULL,
    [Contents] [int] NOT NULL,
    [NonDelivery] [int] NOT NULL,
    [ShipEngineLabelID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [IntegratorTransactionID] [uniqueidentifier] NULL,
    [ResidentialDelivery] [bit] NOT NULL,
    [PackagingType] [int] NOT NULL,
    [DimsProfileID] [bigint] NOT NULL,
    [DimsLength] [float] NOT NULL,
    [DimsWidth] [float] NOT NULL,
    [DimsHeight] [float] NOT NULL,
    [DimsWeight] [float] NOT NULL,
    [DimsAddWeight] [bit] NOT NULL,
    [Reference1] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_DhlEcommerceShipment_Reference1] DEFAULT (''),
    [CustomsRecipientTin] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [CustomsTaxIdType] [int] NULL,
    [CustomsTinIssuingAuthority] [nvarchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [InsuranceValue] [money] NOT NULL CONSTRAINT [DF_DhlEcommerceShipment_InsuranceValue] DEFAULT ((0)),
    [InsurancePennyOne] [bit] NOT NULL CONSTRAINT [DF_DhlEcommerceShipment_InsurancePennyOne] DEFAULT ((0)),
    [AncillaryEndorsement] [int] NOT NULL CONSTRAINT [DF_DhlEcommerceShipment_AncillaryEndorsement] DEFAULT ((0))
    )
GO

PRINT N'Creating primary key [PK_DhlEcommerceShipment] on [dbo].[DhlEcommerceShipment]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'PK_DhlEcommerceShipment' AND object_id = OBJECT_ID(N'[dbo].[DhlEcommerceShipment]'))
    ALTER TABLE [dbo].[DhlEcommerceShipment] ADD CONSTRAINT [PK_DhlEcommerceShipment] PRIMARY KEY CLUSTERED  ([ShipmentID])
GO

PRINT N'Altering [dbo].[ProcessedShipmentsView]'
GO
IF OBJECT_ID(N'[dbo].[ProcessedShipmentsView]', 'V') IS NOT NULL
    EXEC sp_executesql N'ALTER view [dbo].[ProcessedShipmentsView] as
    WITH ProcessedShipments AS
             (
                 SELECT ShipmentID, ShipmentType, ShipDate, Insurance, InsuranceProvider, ProcessedDate, ProcessedUserID, ProcessedComputerID,
                        ProcessedWithUiMode, Voided, VoidedDate, VoidedUserID, VoidedComputerID, TotalWeight, TrackingNumber, ShipmentCost,
                        ShipSenseStatus, Shipment.ShipAddressValidationStatus, Shipment.ShipResidentialStatus, Shipment.ShipPOBox,
                        Shipment.ShipMilitaryAddress, Shipment.ShipUSTerritory, RequestedLabelFormat, ActualLabelFormat, ReturnShipment,
                        [Order].OrderID, [Order].OrderNumberComplete, [Order].CombineSplitStatus, [Order].Verified, TrackingStatus
                 FROM Shipment
                          INNER JOIN [Order] ON Shipment.OrderID = [Order].OrderID
                 WHERE Processed = 1
             ),
         RegularShipments AS
             (
                 SELECT s.ShipmentID, s.ShipmentType, s.ShipDate, s.Insurance, s.InsuranceProvider, s.ProcessedDate, s.ProcessedUserID,
                        s.ProcessedComputerID, s.ProcessedWithUiMode, s.Voided, s.VoidedDate, s.VoidedUserID, s.VoidedComputerID, s.TotalWeight,
                        s.TrackingNumber, s.ShipmentCost, s.ShipSenseStatus, s.ShipAddressValidationStatus, s.ShipResidentialStatus, s.ShipPOBox,
                        s.ShipMilitaryAddress, s.ShipUSTerritory, s.RequestedLabelFormat, s.ActualLabelFormat, s.ReturnShipment, s.OrderID, s.OrderNumberComplete,
                        s.CombineSplitStatus, CONVERT(NVARCHAR(50), carrierService.[Service]) AS [Service], Verified, TrackingStatus
                 FROM ProcessedShipments s
                          CROSS APPLY
                      (
                          SELECT
                              case
                                  when s.ShipmentType in (0, 1) THEN (SELECT c.[Service] FROM upsshipment c WHERE c.ShipmentID = s.ShipmentID)
                                  when s.ShipmentType IN (6   ) THEN (SELECT c.[Service] FROM FedExShipment c WHERE c.ShipmentID = s.ShipmentID)
                                  when s.ShipmentType IN (2, 4, 9, 13, 15) THEN (SELECT c.[Service] FROM PostalShipment c WHERE c.ShipmentID = s.ShipmentID)
                                  when s.ShipmentType IN (11  ) THEN (SELECT c.[Service] FROM OnTracShipment c WHERE c.ShipmentID = s.ShipmentID)
                                  when s.ShipmentType IN (12  ) THEN (SELECT c.[Service] FROM iParcelShipment c WHERE c.ShipmentID = s.ShipmentID)
                                  when s.ShipmentType IN (17  ) THEN (SELECT c.[Service] FROM DhlExpressShipment c WHERE c.ShipmentID = s.ShipmentID)
                                  when s.ShipmentType IN (20  ) THEN (SELECT c.[Service] FROM DhlEcommerceShipment c WHERE c.ShipmentID = s.ShipmentID)
                                  when s.ShipmentType IN (18  ) THEN (SELECT c.[Service] FROM AsendiaShipment c WHERE c.ShipmentID = s.ShipmentID)
                                  END AS [Service]
                      ) AS carrierService
                 WHERE s.ShipmentType NOT IN (5, 16)
             ),
         AmazonSFPShipments as
             (
                 SELECT s.ShipmentID, s.ShipmentType, s.ShipDate, s.Insurance, s.InsuranceProvider, s.ProcessedDate, s.ProcessedUserID,
                        s.ProcessedComputerID, s.ProcessedWithUiMode, s.Voided, s.VoidedDate, s.VoidedUserID, s.VoidedComputerID, s.TotalWeight,
                        s.TrackingNumber, s.ShipmentCost, s.ShipSenseStatus, s.ShipAddressValidationStatus, s.ShipResidentialStatus, s.ShipPOBox,
                        s.ShipMilitaryAddress, s.ShipUSTerritory, s.RequestedLabelFormat, s.ActualLabelFormat, s.ReturnShipment, s.OrderID, s.OrderNumberComplete,
                        s.CombineSplitStatus, c.ShippingServiceID, Verified, TrackingStatus
                 FROM AmazonSFPShipment c, ProcessedShipments s WHERE c.ShipmentID = s.ShipmentID  AND s.ShipmentType = 16
             ),
         OtherShipments as
             (
                 SELECT s.ShipmentID, s.ShipmentType, s.ShipDate, s.Insurance, s.InsuranceProvider, s.ProcessedDate, s.ProcessedUserID,
                        s.ProcessedComputerID, s.ProcessedWithUiMode, s.Voided, s.VoidedDate, s.VoidedUserID, s.VoidedComputerID, s.TotalWeight,
                        s.TrackingNumber, s.ShipmentCost, s.ShipSenseStatus, s.ShipAddressValidationStatus, s.ShipResidentialStatus, s.ShipPOBox,
                        s.ShipMilitaryAddress, s.ShipUSTerritory, s.RequestedLabelFormat, s.ActualLabelFormat, s.ReturnShipment, s.OrderID, s.OrderNumberComplete,
                        s.CombineSplitStatus, c.[Carrier] + '' '' + c.[Service] AS [Service], Verified, TrackingStatus
                 FROM OtherShipment c, ProcessedShipments s WHERE c.ShipmentID = s.ShipmentID AND s.ShipmentType = 5
             )
    SELECT * FROM RegularShipments
    UNION
    SELECT * FROM AmazonSFPShipments
    UNION
    SELECT * FROM OtherShipments'
GO

GO
PRINT N'Altering [dbo].[ShippingSettings]'
GO
IF COL_LENGTH(N'[dbo].[ShippingSettings]', N'DhlEcommerceInsuranceProvider') IS NULL
    ALTER TABLE [dbo].[ShippingSettings] ADD[DhlEcommerceInsuranceProvider] [int] NOT NULL CONSTRAINT [DF_ShippingSettings_DhlEcommerceInsuranceProvider] DEFAULT ((2))
IF COL_LENGTH(N'[dbo].[ShippingSettings]', N'DhlEcommerceInsurancePennyOne') IS NULL
    ALTER TABLE [dbo].[ShippingSettings] ADD[DhlEcommerceInsurancePennyOne] [bit] NOT NULL CONSTRAINT [DF_ShippingSettings_DhlEcommerceInsurancePennyOne] DEFAULT ((0))
IF COL_LENGTH(N'[dbo].[ShippingSettings]', N'DhlEcommerceInsuranceProvider') IS NULL AND COL_LENGTH(N'[dbo].[ShippingSettings]', N'DhlEcommerceInsurancePennyOne') IS NULL
    ALTER TABLE [dbo].[ShippingSettings] ADD
    [DhlEcommerceInsuranceProvider] [int] NOT NULL CONSTRAINT [DF_ShippingSettings_DhlEcommerceInsuranceProvider] DEFAULT ((2)),
    [DhlEcommerceInsurancePennyOne] [bit] NOT NULL CONSTRAINT [DF_ShippingSettings_DhlEcommerceInsurancePennyOne] DEFAULT ((0))
GO

PRINT N'Creating [dbo].[DhlEcommerceAccount]'
GO
IF OBJECT_ID(N'[dbo].[DhlEcommerceAccount]', 'U') IS NULL
    CREATE TABLE [dbo].[DhlEcommerceAccount]
    (
    [DhlEcommerceAccountID] [bigint] NOT NULL IDENTITY(1106, 1000),
    [RowVersion] [timestamp] NOT NULL,
    [ShipEngineCarrierId] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF__DhlEcomme__ShipE__2BC97F7C] DEFAULT (''),
    [ClientId] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF__DhlEcomme__Clien__2CBDA3B5] DEFAULT (''),
    [ApiSecret] [nvarchar] (400) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF__DhlEcomme__ApiSe__2DB1C7EE] DEFAULT (''),
    [PickupNumber] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF__DhlEcomme__Picku__2EA5EC27] DEFAULT (''),
    [DistributionCenter] [nvarchar] (12) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF__DhlEcomme__Distr__2F9A1060] DEFAULT (''),
    [SoldTo] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF__DhlEcomme__SoldT__308E3499] DEFAULT (''),
    [Description] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF__DhlEcomme__Descr__318258D2] DEFAULT (''),
    [FirstName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF__DhlEcomme__First__32767D0B] DEFAULT (''),
    [MiddleName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF__DhlEcomme__Middl__336AA144] DEFAULT (''),
    [LastName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF__DhlEcomme__LastN__345EC57D] DEFAULT (''),
    [Company] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF__DhlEcomme__Compa__3552E9B6] DEFAULT (''),
    [Street1] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF__DhlEcomme__Stree__36470DEF] DEFAULT (''),
    [Street2] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF__DhlEcomme__Stree__373B3228] DEFAULT (''),
    [Street3] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF__DhlEcomme__Stree__382F5661] DEFAULT (''),
    [City] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF__DhlEcommer__City__39237A9A] DEFAULT (''),
    [StateProvCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF__DhlEcomme__State__3A179ED3] DEFAULT (''),
    [PostalCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF__DhlEcomme__Posta__3B0BC30C] DEFAULT (''),
    [CountryCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF__DhlEcomme__Count__3BFFE745] DEFAULT (''),
    [Phone] [nvarchar] (26) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF__DhlEcomme__Phone__3CF40B7E] DEFAULT (''),
    [Email] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF__DhlEcomme__Email__3DE82FB7] DEFAULT (''),
    [CreatedDate] [datetime] NOT NULL
    )
GO

PRINT N'Creating primary key [PK_PostalDhlEcommerceAccount] on [dbo].[DhlEcommerceAccount]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'PK_PostalDhlEcommerceAccount' AND object_id = OBJECT_ID(N'[dbo].[DhlEcommerceAccount]'))
    ALTER TABLE [dbo].[DhlEcommerceAccount] ADD CONSTRAINT [PK_PostalDhlEcommerceAccount] PRIMARY KEY CLUSTERED  ([DhlEcommerceAccountID])
GO

IF NOT EXISTS (SELECT 1 FROM sys.change_tracking_tables WHERE object_id = OBJECT_ID(N'[dbo].[DhlEcommerceAccount]', 'U'))
    ALTER TABLE [dbo].[DhlEcommerceAccount] ENABLE CHANGE_TRACKING
GO

PRINT N'Altering [dbo].[DhlEcommerceAccount]'
GO

PRINT N'Creating [dbo].[DhlEcommerceProfile]'
GO
IF OBJECT_ID(N'[dbo].[DhlEcommerceProfile]', 'U') IS NULL
    CREATE TABLE [dbo].[DhlEcommerceProfile]
    (
    [ShippingProfileID] [bigint] NOT NULL,
    [DhlEcommerceAccountID] [bigint] NULL,
    [Service] [int] NULL,
    [DeliveryDutyPaid] [bit] NULL,
    [NonMachinable] [bit] NULL,
    [SaturdayDelivery] [bit] NULL,
    [Contents] [int] NULL,
    [NonDelivery] [int] NULL,
    [ResidentialDelivery] [bit] NULL,
    [CustomsRecipientTin] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [CustomsTaxIdType] [int] NULL,
    [CustomsTinIssuingAuthority] [nvarchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [PackagingType] [int] NULL,
    [Reference1] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [AncillaryEndorsement] [int] NULL
    )
GO

PRINT N'Creating primary key [PK_DhlEcommerceProfile] on [dbo].[DhlEcommerceProfile]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'PK_DhlEcommerceProfile' AND object_id = OBJECT_ID(N'[dbo].[DhlEcommerceProfile]'))
    ALTER TABLE [dbo].[DhlEcommerceProfile] ADD CONSTRAINT [PK_DhlEcommerceProfile] PRIMARY KEY CLUSTERED  ([ShippingProfileID])
GO

PRINT N'Creating [dbo].[ShipEngineManifest]'
GO
IF OBJECT_ID(N'[dbo].[ShipEngineManifest]', 'U') IS NULL
    CREATE TABLE [dbo].[ShipEngineManifest]
    (
    [ShipEngineManifestID] [bigint] NOT NULL IDENTITY(1107, 1000),
    [CarrierAccountID] [bigint] NOT NULL,
    [ShipmentTypeCode] [int] NOT NULL,
    [ManifestID] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [FormID] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [CreatedAt] [datetime] NOT NULL,
    [ShipDate] [datetime] NOT NULL,
    [ShipmentCount] [int] NOT NULL,
    [PlatformWarehouseID] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [SubmissionID] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [CarrierID] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [ManifestUrl] [varchar] (2048) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
    )
GO

PRINT N'Creating primary key [PK_ShipEngineManifest] on [dbo].[ShipEngineManifest]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'PK_ShipEngineManifest' AND object_id = OBJECT_ID(N'[dbo].[ShipEngineManifest]'))
    ALTER TABLE [dbo].[ShipEngineManifest] ADD CONSTRAINT [PK_ShipEngineManifest] PRIMARY KEY CLUSTERED  ([ShipEngineManifestID])
GO

PRINT N'Adding foreign keys to [dbo].[DhlEcommerceProfile]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DhlEcommerceProfile_ShippingProfile]', 'F') AND parent_object_id = OBJECT_ID(N'[dbo].[DhlEcommerceProfile]', 'U'))
    ALTER TABLE [dbo].[DhlEcommerceProfile] ADD CONSTRAINT [FK_DhlEcommerceProfile_ShippingProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[ShippingProfile] ([ShippingProfileID]) ON DELETE CASCADE
GO

PRINT N'Adding foreign keys to [dbo].[DhlEcommerceShipment]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DhlEcommerceShipment_Shipment]', 'F') AND parent_object_id = OBJECT_ID(N'[dbo].[DhlEcommerceShipment]', 'U'))
    ALTER TABLE [dbo].[DhlEcommerceShipment] ADD CONSTRAINT [FK_DhlEcommerceShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO

PRINT N'Creating extended properties'
GO
IF NOT EXISTS (SELECT 1 FROM fn_listextendedproperty(N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'DhlEcommerceShipment', 'COLUMN', N'DeliveredDutyPaid'))
    EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'DhlEcommerceShipment', 'COLUMN', N'DeliveredDutyPaid'
GO

IF NOT EXISTS (SELECT 1 FROM fn_listextendedproperty(N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'DhlEcommerceShipment', 'COLUMN', N'DhlEcommerceAccountID'))
    EXEC sp_addextendedproperty N'AuditFormat', N'4', 'SCHEMA', N'dbo', 'TABLE', N'DhlEcommerceShipment', 'COLUMN', N'DhlEcommerceAccountID'
GO

IF NOT EXISTS (SELECT 1 FROM fn_listextendedproperty(N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'DhlEcommerceShipment', 'COLUMN', N'NonMachinable'))
    EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'DhlEcommerceShipment', 'COLUMN', N'NonMachinable'
GO

IF NOT EXISTS (SELECT 1 FROM fn_listextendedproperty(N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'DhlEcommerceShipment', 'COLUMN', N'SaturdayDelivery'))
    EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'DhlEcommerceShipment', 'COLUMN', N'SaturdayDelivery'
GO
 
IF NOT EXISTS (SELECT 1 FROM fn_listextendedproperty(N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'DhlEcommerceShipment', 'COLUMN', N'Service'))
    EXEC sp_addextendedproperty N'AuditFormat', N'130', 'SCHEMA', N'dbo', 'TABLE', N'DhlEcommerceShipment', 'COLUMN', N'Service'
GO
