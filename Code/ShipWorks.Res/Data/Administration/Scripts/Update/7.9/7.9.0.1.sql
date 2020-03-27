PRINT N'Adding columns to [dbo].[ProductVariant]'
GO

IF NOT EXISTS(SELECT * FROM sys.all_columns WHERE object_id = OBJECT_ID('ProductVariant') AND [name] = 'HubSequence')
BEGIN
    ALTER TABLE ProductVariant
        ADD HubSequence bigint NULL
END
GO

PRINT N'Creating index IX_ProductVariant_HubSequence'
GO
IF NOT EXISTS(SELECT * FROM sys.indexes WHERE object_id = object_id('[dbo].[ProductVariant]') AND NAME ='IX_ProductVariant_HubSequence')
	CREATE UNIQUE INDEX [IX_ProductVariant_HubSequence] 
		ON [dbo].[ProductVariant] ([HubSequence])
		WHERE HubSequence IS NOT NULL
GO