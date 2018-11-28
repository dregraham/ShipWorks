PRINT N'Dropping index dbo.IX_SWDefault_ProductVariantAlias_IsDefaultSkuProductVariantID'
GO
IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_SWDefault_ProductVariantAlias_IsDefaultSkuProductVariantID')
  BEGIN
    DROP INDEX [IX_SWDefault_ProductVariantAlias_IsDefaultSkuProductVariantID] ON [DBO].[ProductVariantAlias];
  END
GO
PRINT N'Adding index dbo.IX_SWDefault_ProductVariantAlias_ProductVariantIDIsDefaultSku'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_ProductVariantAlias_ProductVariantIDIsDefaultSku] ON [dbo].[ProductVariantAlias]
(
	[ProductVariantID] ASC,
	[IsDefault] ASC
)
INCLUDE ([Sku])  ON [PRIMARY]
GO