PRINT N'Creating [dbo].[Product]'
GO
IF OBJECT_ID(N'[dbo].[Product]', 'U') IS NULL
CREATE TABLE [dbo].[Product]
(
[ProductID] [bigint] NOT NULL IDENTITY(1, 1),
[CreatedDate] [datetime] NOT NULL,
[Name] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IsActive] [bit] NOT NULL,
[IsBundle] [bit] NOT NULL
)
GO
PRINT N'Creating primary key [PK_Product] on [dbo].[Product]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'PK_Product' AND object_id = OBJECT_ID(N'[dbo].[Product]'))
ALTER TABLE [dbo].[Product] ADD CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED  ([ProductID])
GO
PRINT N'Creating [dbo].[ProductBundle]'
GO
IF OBJECT_ID(N'[dbo].[ProductBundle]', 'U') IS NULL
CREATE TABLE [dbo].[ProductBundle]
(
[ProductID] [bigint] NOT NULL,
[ChildProductVariantID] [bigint] NOT NULL,
[Quantity] [int] NOT NULL
)
GO
PRINT N'Creating primary key [PK_ProductBundle] on [dbo].[ProductBundle]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'PK_ProductBundle' AND object_id = OBJECT_ID(N'[dbo].[ProductBundle]'))
ALTER TABLE [dbo].[ProductBundle] ADD CONSTRAINT [PK_ProductBundle] PRIMARY KEY CLUSTERED  ([ProductID], [ChildProductVariantID])
GO
PRINT N'Creating [dbo].[ProductVariant]'
GO
IF OBJECT_ID(N'[dbo].[ProductVariant]', 'U') IS NULL
CREATE TABLE [dbo].[ProductVariant]
(
[ProductVariantID] [bigint] NOT NULL IDENTITY(1, 1),
[ProductID] [bigint] NOT NULL,
[CreatedDate] [datetime] NOT NULL,
[Name] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IsActive] [bit] NOT NULL,
[UPC] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ASIN] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ISBN] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Weight] [decimal] (29, 9) NULL,
[Length] [decimal] (10, 2) NULL,
[Width] [decimal] (10, 2) NULL,
[Height] [decimal] (10, 2) NULL,
[ImageUrl] [nvarchar] (500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[BinLocation] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HarmonizedCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DeclaredValue] [money] NULL,
[CountryOfOrigin] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
PRINT N'Creating primary key [PK_ProductVariant] on [dbo].[ProductVariant]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'PK_ProductVariant' AND object_id = OBJECT_ID(N'[dbo].[ProductVariant]'))
ALTER TABLE [dbo].[ProductVariant] ADD CONSTRAINT [PK_ProductVariant] PRIMARY KEY CLUSTERED  ([ProductVariantID])
GO
PRINT N'Creating [dbo].[ProductVariantAlias]'
GO
IF OBJECT_ID(N'[dbo].[ProductVariantAlias]', 'U') IS NULL
CREATE TABLE [dbo].[ProductVariantAlias]
(
[ProductVariantAliasID] [bigint] NOT NULL IDENTITY(1, 1),
[ProductVariantID] [bigint] NOT NULL,
[AliasName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Sku] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_ProductVariantAlias] on [dbo].[ProductVariantAlias]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'PK_ProductVariantAlias' AND object_id = OBJECT_ID(N'[dbo].[ProductVariantAlias]'))
ALTER TABLE [dbo].[ProductVariantAlias] ADD CONSTRAINT [PK_ProductVariantAlias] PRIMARY KEY CLUSTERED  ([ProductVariantAliasID])
GO
PRINT N'Creating index [IX_SWDefault_ProductVariantAlias_Sku] on [dbo].[ProductVariantAlias]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_SWDefault_ProductVariantAlias_Sku' AND object_id = OBJECT_ID(N'[dbo].[ProductVariantAlias]'))
CREATE NONCLUSTERED INDEX [IX_SWDefault_ProductVariantAlias_Sku] ON [dbo].[ProductVariantAlias] ([Sku]) INCLUDE ([ProductVariantID])
GO
PRINT N'Creating [dbo].[ProductVariantAttribute]'
GO
IF OBJECT_ID(N'[dbo].[ProductVariantAttribute]', 'U') IS NULL
CREATE TABLE [dbo].[ProductVariantAttribute]
(
[ProductVariantAttributeID] [bigint] NOT NULL IDENTITY(1, 1),
[ProductVariantID] [bigint] NOT NULL,
[AttributeName] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AttributeValue] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_ProductVariantAttribute] on [dbo].[ProductVariantAttribute]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'PK_ProductVariantAttribute' AND object_id = OBJECT_ID(N'[dbo].[ProductVariantAttribute]'))
ALTER TABLE [dbo].[ProductVariantAttribute] ADD CONSTRAINT [PK_ProductVariantAttribute] PRIMARY KEY CLUSTERED  ([ProductVariantAttributeID])
GO
PRINT N'Creating index [IX_SWDefault_ProductVariantAttribute_ProductVariantID] on [dbo].[ProductVariantAttribute]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_SWDefault_ProductVariantAttribute_ProductVariantID' AND object_id = OBJECT_ID(N'[dbo].[ProductVariantAttribute]'))
CREATE NONCLUSTERED INDEX [IX_SWDefault_ProductVariantAttribute_ProductVariantID] ON [dbo].[ProductVariantAttribute] ([ProductVariantID])
GO
PRINT N'Adding foreign keys to [dbo].[ProductBundle]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ProductBundle_Product]', 'F') AND parent_object_id = OBJECT_ID(N'[dbo].[ProductBundle]', 'U'))
ALTER TABLE [dbo].[ProductBundle] ADD CONSTRAINT [FK_ProductBundle_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ProductID])
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ProductBundle_ProductVariant]', 'F') AND parent_object_id = OBJECT_ID(N'[dbo].[ProductBundle]', 'U'))
ALTER TABLE [dbo].[ProductBundle] ADD CONSTRAINT [FK_ProductBundle_ProductVariant] FOREIGN KEY ([ChildProductVariantID]) REFERENCES [dbo].[ProductVariant] ([ProductVariantID])
GO
PRINT N'Adding foreign keys to [dbo].[ProductVariantAlias]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ProductVariantAlias_ProductVariant]', 'F') AND parent_object_id = OBJECT_ID(N'[dbo].[ProductVariantAlias]', 'U'))
ALTER TABLE [dbo].[ProductVariantAlias] ADD CONSTRAINT [FK_ProductVariantAlias_ProductVariant] FOREIGN KEY ([ProductVariantID]) REFERENCES [dbo].[ProductVariant] ([ProductVariantID])
GO
PRINT N'Adding foreign keys to [dbo].[ProductVariantAttribute]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ProductVariantAttribute_ProductVariant]', 'F') AND parent_object_id = OBJECT_ID(N'[dbo].[ProductVariantAttribute]', 'U'))
ALTER TABLE [dbo].[ProductVariantAttribute] ADD CONSTRAINT [FK_ProductVariantAttribute_ProductVariant] FOREIGN KEY ([ProductVariantID]) REFERENCES [dbo].[ProductVariant] ([ProductVariantID])
GO
PRINT N'Adding foreign keys to [dbo].[ProductVariant]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ProductVariant_Product]', 'F') AND parent_object_id = OBJECT_ID(N'[dbo].[ProductVariant]', 'U'))
ALTER TABLE [dbo].[ProductVariant] ADD CONSTRAINT [FK_ProductVariant_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ProductID])
GO
