-- Drop OtherShipment.RequestedLabelFormat
IF EXISTS(SELECT * FROM sys.columns 
        WHERE [name] = N'RequestedLabelFormat' AND [object_id] = OBJECT_ID(N'OtherShipment'))
BEGIN
	ALTER TABLE [dbo].[OtherShipment] DROP COLUMN [RequestedLabelFormat]
END
GO

-- Drop WorldShipShipment.RequestedLabelFormat
IF EXISTS(SELECT * FROM sys.columns 
        WHERE [name] = N'RequestedLabelFormat' AND [object_id] = OBJECT_ID(N'WorldShipShipment'))
BEGIN
	ALTER TABLE [dbo].[WorldShipShipment] DROP COLUMN [RequestedLabelFormat]
END
GO