SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Adding [dbo].[ProductListView]'
GO
IF EXISTS(SELECT * FROM sys.views WHERE [name] = 'ProductListView')
    DROP VIEW ProductListView
GO

CREATE VIEW ProductListView AS
SELECT ProductVariantID, DefaultSku.Sku AS SKU, Name, Length, Width, Height, Weight, BinLocation, ImageUrl, IsActive
    FROM ProductVariant
    CROSS APPLY (
        SELECT TOP 1 Sku
            FROM ProductVariantAlias
            WHERE ProductVariantID = ProductVariant.ProductVariantID
            ORDER BY IsDefault DESC
    ) AS DefaultSku
