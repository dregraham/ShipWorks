PRINT N'Adding columns to [dbo].[ProductVariant]'
GO
IF NOT EXISTS(SELECT * FROM sys.all_columns WHERE object_id = OBJECT_ID('ProductVariant') AND [name] = 'HubProductId')
BEGIN
    ALTER TABLE ProductVariant
        ADD HubProductId uniqueidentifier NULL
END
GO

IF NOT EXISTS(SELECT * FROM sys.all_columns WHERE object_id = OBJECT_ID('ProductVariant') AND [name] = 'HubVersion')
BEGIN
    ALTER TABLE ProductVariant
        ADD HubVersion int NULL
END
GO

PRINT N'Creating index IX_ProductVariant_HubProductId'
GO
IF NOT EXISTS(SELECT * FROM sys.indexes WHERE object_id = object_id('[dbo].[ProductVariant]') AND NAME ='IX_ProductVariant_HubProductId')
	CREATE UNIQUE INDEX [IX_ProductVariant_HubProductId] 
		ON [dbo].[ProductVariant] ([HubProductId])
		WHERE HubProductId IS NOT NULL
GO