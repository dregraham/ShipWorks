select  SKU.Sku as [SKU], 
			rtrim(ltrim(substring(aliases.AliasSkus, 1, len(aliases.AliasSkus) - 1))) as [Alias SKUs],
			rtrim(ltrim(substring(Bundles.[BundleSkus], 1, len(Bundles.[BundleSkus]) - 1))) as  [Bundled SKUs],
			p.Name as [Name],
			pv.[UPC] AS [UPC],
			pv.[ASIN] AS [ASIN],
			pv.[ISBN] AS [ISBN],
			CONVERT(NVARCHAR(50), pv.[Weight]) AS [Weight],
			CONVERT(NVARCHAR(50), pv.[Length]) AS [Length],
			CONVERT(NVARCHAR(50), pv.[Width]) AS [Width],
			CONVERT(NVARCHAR(50), pv.[Height]) AS [Height],
			pv.[ImageURL] AS [Image URL],
			pv.[BinLocation] AS [Warehouse-Bin Location],
			CONVERT(NVARCHAR(50), pv.[DeclaredValue]) AS [Declared Value],
			pv.[CountryOfOrigin] AS [Country of Origin],
			pv.[HarmonizedCode] AS [Harmonized Code],
			pv.[IsActive] AS [Active]
	from Product p, ProductVariant pv
	cross apply 
	(
		select ISNULL(
			(select REPLACE(REPLACE(a.Sku, '|', '\|'), ':', '\:') + '|' AS [text()]
			from ProductVariantAlias a
			where a.ProductVariantID = pv.ProductVariantID
			  and a.IsDefault = 0
			FOR XML PATH ('')  ), ' x')
		  as [AliasSkus]
	) as aliases
	cross apply
	(
		select top 1 REPLACE(REPLACE(a.Sku, '|', '\|'), ':', '\:') as [Sku]
		from ProductVariantAlias a
			where a.ProductVariantID = pv.ProductVariantID
			  and a.IsDefault = 1
	) as [SKU]
	cross apply 
	(
		select ISNULL(
			(
			select REPLACE(REPLACE(aBundle.Sku, '|', '\|'), ':', '\:') + ':' + CONVERT(nvarchar(20), pbBundle.Quantity) + '|' AS [text()]
			from ProductVariant pvBundle, ProductBundle pbBundle, ProductVariantAlias aBundle
			where aBundle.ProductVariantID = pbBundle.ChildProductVariantID
			  and aBundle.IsDefault = 1
			  and pbBundle.ProductID = pvBundle.ProductID
			  and pbBundle.ProductID = pv.ProductID
			FOR XML PATH ('')  ), ' x')
		  as [BundleSkus]
	) as Bundles
	where p.ProductID = pv.ProductID