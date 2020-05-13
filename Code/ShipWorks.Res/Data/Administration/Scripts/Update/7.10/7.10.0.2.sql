PRINT N'Dropping index [IX_SWDefault_ProductVariantAlias_Sku] from [dbo].[ProductVariantAlias]'
GO
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_SWDefault_ProductVariantAlias_Sku' AND object_id = OBJECT_ID(N'[dbo].[ProductVariantAlias]'))
DROP INDEX [IX_SWDefault_ProductVariantAlias_Sku] ON [dbo].[ProductVariantAlias]
GO

PRINT N'Dropping index [IX_SWDefault_ProductVariantAlias_ProductVariantIDIsDefaultSku] from [dbo].[ProductVariantAlias]'
GO
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_SWDefault_ProductVariantAlias_ProductVariantIDIsDefaultSku' AND object_id = OBJECT_ID(N'[dbo].[ProductVariantAlias]'))
DROP INDEX [IX_SWDefault_ProductVariantAlias_ProductVariantIDIsDefaultSku] ON [dbo].[ProductVariantAlias]
GO

PRINT N'Modifying ProductVariantAlias table'
GO
ALTER TABLE [dbo].[ProductVariantAlias] 
  ALTER COLUMN [Sku] NVARCHAR(300) COLLATE SQL_Latin1_General_CP1_CS_AS NOT NULL
GO

PRINT N'Creating index [IX_SWDefault_ProductVariantAlias_Sku] on [dbo].[ProductVariantAlias]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_SWDefault_ProductVariantAlias_Sku' AND object_id = OBJECT_ID(N'[dbo].[ProductVariantAlias]'))
CREATE UNIQUE NONCLUSTERED INDEX [IX_SWDefault_ProductVariantAlias_Sku] ON [dbo].[ProductVariantAlias] ([Sku]) INCLUDE ([ProductVariantID])
GO

PRINT N'Creating index [IX_SWDefault_ProductVariantAlias_ProductVariantIDIsDefaultSku] on [dbo].[ProductVariantAlias]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_SWDefault_ProductVariantAlias_ProductVariantIDIsDefaultSku' AND object_id = OBJECT_ID(N'[dbo].[ProductVariantAlias]'))
CREATE NONCLUSTERED INDEX [IX_SWDefault_ProductVariantAlias_ProductVariantIDIsDefaultSku] ON [dbo].[ProductVariantAlias]
(
	[ProductVariantID] ASC,
	[IsDefault] ASC
)
INCLUDE ([Sku])  ON [PRIMARY]
GO

