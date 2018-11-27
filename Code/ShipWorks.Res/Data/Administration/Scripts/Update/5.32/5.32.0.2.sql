IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ProductVariantAlias]') AND name = N'IX_SWDefault_ProductVariantAlias_IsDefaultSkuProductVariantID')
CREATE NONCLUSTERED INDEX [IX_SWDefault_ProductVariantAlias_IsDefaultSkuProductVariantID] ON [dbo].[ProductVariantAlias]
(
	[IsDefault] ASC
)
INCLUDE ([ProductVariantID], [Sku]) ON [PRIMARY]
GO
