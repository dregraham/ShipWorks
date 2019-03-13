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