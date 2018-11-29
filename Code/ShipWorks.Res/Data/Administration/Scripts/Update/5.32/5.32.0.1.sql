PRINT N'Altering [dbo].[ProductVariantAlias]'
GO
IF COL_LENGTH(N'[dbo].[ProductVariantAlias]', N'IsDefault') IS NULL
ALTER TABLE [dbo].[ProductVariantAlias] ADD [IsDefault] [bit] NOT NULL CONSTRAINT [DF_ProductVariantAlias_IsDefault] DEFAULT ((0))
GO
