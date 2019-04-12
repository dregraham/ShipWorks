--AmazonProfile
PRINT N'Renaming [dbo].[AmazonProfile]'
GO
EXEC sp_rename N'[dbo].[AmazonProfile]', N'AmazonSFPProfile', N'OBJECT'
GO
PRINT N'Renaming [PK_AmazonProfile] on [dbo].[AmazonSFPProfile]'
GO
exec sp_rename 'PK_AmazonProfile', 'PK_AmazonSFPProfile', 'object'
GO
PRINT N'Renaming [FK_AmazonProfile_ShippingProfile] on [dbo].[AmazonSFPProfile'
GO
exec sp_rename 'FK_AmazonProfile_ShippingProfile', 'FK_AmazonSFPProfile_ShippingProfile', 'object'
GO

--AmazonServiceType
PRINT N'Renaming [dbo].[AmazonServiceType]'
GO
EXEC sp_rename N'[dbo].[AmazonServiceType]', N'AmazonSFPServiceType', N'OBJECT'
GO
PRINT N'Renaming [dbo].[AmazonSFPServiceType].[AmazonServiceTypeID]'
EXEC sp_rename 'AmazonSFPServiceType.AmazonServiceTypeID', 'AmazonSFPServiceTypeID', 'COLUMN';
GO
PRINT N'Renaming [PK_AmazonServiceTypeID] on [dbo].[AmazonSFPServiceType]'
GO
exec sp_rename 'PK_AmazonServiceTypeID', 'PK_AmazonSFPServiceTypeID', 'object'
GO

IF EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[AmazonSFPServiceType]')
                                 AND name = N'IX_SWDefault_AmazonServiceType_ApiValue')
BEGIN
	PRINT N'Renaming [IX_SWDefault_AmazonServiceType_ApiValue] on [dbo].[AmazonSFPServiceType]'
	exec sp_rename 'AmazonSFPServiceType.IX_SWDefault_AmazonServiceType_ApiValue', 'IX_SWDefault_AmazonSFPServiceType_ApiValue'
END

-- AmazonShipment to AmazonSFPShipment
PRINT N'Renaming [dbo].[AmazonShipment]'
GO
EXEC sp_rename N'[dbo].[AmazonShipment]', N'AmazonSFPShipment', N'OBJECT'
GO
PRINT N'Renaming [PK_AmazonShipment] on [dbo].[AmazonSFPShipment]'
GO
exec sp_rename 'PK_AmazonShipment', 'PK_AmazonSFPShipment', 'object'
GO
PRINT N'Renaming [FK_AmazonShipment_Shipment] on [dbo].[AmazonSFPShipment]'
GO
exec sp_rename 'FK_AmazonShipment_Shipment', 'FK_AmazonSFPShipment_Shipment', 'object'
GO
PRINT N'Renaming default constraints on [dbo].[AmazonSFPShipment]'
GO
IF EXISTS (SELECT 1 FROM sys.default_constraints WHERE parent_object_id = OBJECT_ID(N'[dbo].[AmazonSFPShipment]')
                                 AND name = N'DF_AmazonShipment_CarrierName')
BEGIN
	exec sp_rename 'DF_AmazonShipment_CarrierName', 'DF_AmazonSFPShipment_CarrierName', 'object'
END

IF EXISTS (SELECT 1 FROM sys.default_constraints WHERE parent_object_id = OBJECT_ID(N'[dbo].[AmazonSFPShipment]')
                                 AND name = N'DF_AmazonShipment_DeliveryExperience')
BEGIN
	exec sp_rename 'DF_AmazonShipment_DeliveryExperience', 'DF_AmazonSFPShipment_DeliveryExperience', 'object'
END

IF EXISTS (SELECT 1 FROM sys.default_constraints WHERE parent_object_id = OBJECT_ID(N'[dbo].[AmazonSFPShipment]')
                                 AND name = N'DF_AmazonShipment_DimsAddWeight')
BEGIN
	exec sp_rename 'DF_AmazonShipment_DimsAddWeight', 'DF_AmazonSFPShipment_DimsAddWeight', 'object'
END

IF EXISTS (SELECT 1 FROM sys.default_constraints WHERE parent_object_id = OBJECT_ID(N'[dbo].[AmazonSFPShipment]')
                                 AND name = N'DF_AmazonShipment_DimsHeight')
BEGIN
	exec sp_rename 'DF_AmazonShipment_DimsHeight', 'DF_AmazonSFPShipment_DimsHeight', 'object'
END

IF EXISTS (SELECT 1 FROM sys.default_constraints WHERE parent_object_id = OBJECT_ID(N'[dbo].[AmazonSFPShipment]')
                                 AND name = N'DF_AmazonShipment_DimsLength')
BEGIN
	exec sp_rename 'DF_AmazonShipment_DimsLength', 'DF_AmazonSFPShipment_DimsLength', 'object'
END

IF EXISTS (SELECT 1 FROM sys.default_constraints WHERE parent_object_id = OBJECT_ID(N'[dbo].[AmazonSFPShipment]')
                                 AND name = N'DF_AmazonShipment_DimsProfileID')
BEGIN
	exec sp_rename 'DF_AmazonShipment_DimsProfileID', 'DF_AmazonSFPShipment_DimsProfileID', 'object'
END

IF EXISTS (SELECT 1 FROM sys.default_constraints WHERE parent_object_id = OBJECT_ID(N'[dbo].[AmazonSFPShipment]')
                                 AND name = N'DF_AmazonShipment_DimsWeight')
BEGIN
	exec sp_rename 'DF_AmazonShipment_DimsWeight', 'DF_AmazonSFPShipment_DimsWeight', 'object'
END

IF EXISTS (SELECT 1 FROM sys.default_constraints WHERE parent_object_id = OBJECT_ID(N'[dbo].[AmazonSFPShipment]')
                                 AND name = N'DF_AmazonShipment_DimsWidth')
BEGIN
	exec sp_rename 'DF_AmazonShipment_DimsWidth', 'DF_AmazonSFPShipment_DimsWidth', 'object'
END

IF EXISTS (SELECT 1 FROM sys.default_constraints WHERE parent_object_id = OBJECT_ID(N'[dbo].[AmazonSFPShipment]')
                                 AND name = N'DF_AmazonShipment_InsuranceValue')
BEGIN
	exec sp_rename 'DF_AmazonShipment_InsuranceValue', 'DF_AmazonSFPShipment_InsuranceValue', 'object'
END

IF EXISTS (SELECT 1 FROM sys.default_constraints WHERE parent_object_id = OBJECT_ID(N'[dbo].[AmazonSFPShipment]')
                                 AND name = N'DF_AmazonShipment_ShippingServiceId')
BEGIN
	exec sp_rename 'DF_AmazonShipment_ShippingServiceId', 'DF_AmazonSFPShipment_ShippingServiceId', 'object'
END

IF EXISTS (SELECT 1 FROM sys.default_constraints WHERE parent_object_id = OBJECT_ID(N'[dbo].[AmazonSFPShipment]')
                                 AND name = N'DF_AmazonShipment_ShippingServiceName')
BEGIN
	exec sp_rename 'DF_AmazonShipment_ShippingServiceName', 'DF_AmazonSFPShipment_ShippingServiceName', 'object'
END

IF EXISTS (SELECT 1 FROM sys.default_constraints WHERE parent_object_id = OBJECT_ID(N'[dbo].[AmazonSFPShipment]')
                                 AND name = N'DF_AmazonShipment_ShippingServiceOfferId')
BEGIN
	exec sp_rename 'DF_AmazonShipment_ShippingServiceOfferId', 'DF_AmazonSFPShipment_ShippingServiceOfferId', 'object'
END

IF EXISTS (SELECT 1 FROM sys.default_constraints WHERE parent_object_id = OBJECT_ID(N'[dbo].[AmazonSFPShipment]')
                                 AND name = N'DF_AmazonShipment_ReferenceNumber')
BEGIN
	exec sp_rename 'DF_AmazonShipment_ReferenceNumber', 'DF_AmazonSFPShipment_ReferenceNumber', 'object'
END

IF EXISTS (SELECT 1 FROM sys.default_constraints WHERE parent_object_id = OBJECT_ID(N'[dbo].[AmazonSFPShipment]')
                                 AND name = N'DF_AmazonShipment_RequestedLabelFormat')
BEGIN
	exec sp_rename 'DF_AmazonShipment_RequestedLabelFormat', 'DF_AmazonSFPShipment_RequestedLabelFormat', 'object'
END

GO

PRINT N'Creating [dbo].[AmazonSWAAccount]'
GO
CREATE TABLE [dbo].[AmazonSWAAccount]
(
[AmazonSWAAccountID] [bigint] NOT NULL IDENTITY(1106, 1000),
[RowVersion] [timestamp] NOT NULL,
[ShipEngineCarrierId] [nvarchar] (12) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Description] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FirstName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MiddleName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[LastName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Company] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Street1] [nvarchar] (43) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[City] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[StateProvCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PostalCode] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CountryCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Email] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Phone] [nvarchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_AmazonSWAAccount] on [dbo].[AmazonSWAAccount]'
GO
ALTER TABLE [dbo].[AmazonSWAAccount] ADD CONSTRAINT [PK_AmazonSWAAccount] PRIMARY KEY CLUSTERED  ([AmazonSWAAccountID])
GO
ALTER TABLE [dbo].[AmazonSWAAccount] ENABLE CHANGE_TRACKING
GO
PRINT N'Creating [dbo].[AmazonSWAShipment]'
GO
CREATE TABLE [dbo].[AmazonSWAShipment](
	[ShipmentID] [bigint] NOT NULL,
	[AmazonSWAAccountID] [bigint] NOT NULL,
	[Service] [int] NOT NULL,
	[RequestedLabelFormat] [int] NOT NULL,
	[ShipEngineLabelID] [nvarchar] (12) NOT NULL,
	[DimsProfileID] [bigint] NOT NULL,
	[DimsLength] [float] NOT NULL,
	[DimsWidth] [float] NOT NULL,
	[DimsHeight] [float] NOT NULL,
	[DimsAddWeight] [bit] NOT NULL,
	[DimsWeight] [float] NOT NULL,
	[InsuranceValue] [money] NOT NULL,
	[Insurance] [bit] NOT NULL
)
GO
PRINT N'Creating primary key [PK_AmazonSWAShipment] on [dbo].[AmazonSWAShipment]'
GO
ALTER TABLE [dbo].[AmazonSWAShipment] ADD CONSTRAINT [PK_AmazonSWAShipment] PRIMARY KEY CLUSTERED  ([ShipmentID])
GO
PRINT N'Adding foreign keys to [dbo].[AmazonSWAShipment]'
GO
ALTER TABLE [dbo].[AmazonSWAShipment] ADD CONSTRAINT [FK_AmazonSWAShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Creating index [IX_SWDefault_AmazonSWAShipment_Service] on [dbo].[AmazonSWAShipment]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_AmazonSWAShipment_Service] ON [dbo].[AmazonSWAShipment] ([Service])
GO
EXEC sys.sp_addextendedproperty @name=N'AuditFormat', @value=N'4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AmazonSWAShipment', @level2type=N'COLUMN',@level2name=N'AmazonSWAAccountID'
GO
EXEC sys.sp_addextendedproperty @name=N'AuditFormat', @value=N'130' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AmazonSWAShipment', @level2type=N'COLUMN',@level2name=N'Service'
GO
PRINT N'Creating [dbo].[AmazonSWAProfile]'
GO
CREATE TABLE [dbo].[AmazonSWAProfile](
	[ShippingProfileID] [bigint] NOT NULL,
	[AmazonSWAAccountID] [bigint] NULL,
	[Service] [int] NULL
)
GO
PRINT N'Creating primary key [PK_AmazonSWAProfile] on [dbo].[AmazonSWAProfile]'
GO
ALTER TABLE [dbo].[AmazonSWAProfile] ADD CONSTRAINT [PK_AmazonSWAProfile] PRIMARY KEY CLUSTERED  ([ShippingProfileID])
GO
PRINT N'Adding foreign keys to [dbo].[AmazonSWAProfile]'
GO
ALTER TABLE [dbo].[AmazonSWAProfile] ADD CONSTRAINT [FK_AmazonSWAProfile_ShippingProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[ShippingProfile] ([ShippingProfileID]) ON DELETE CASCADE
GO

UPDATE [dbo].[Store] SET Edition=''
GO