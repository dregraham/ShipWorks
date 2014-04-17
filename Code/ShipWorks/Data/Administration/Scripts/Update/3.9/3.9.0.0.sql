SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Altering [dbo].[Shipment]'
GO
ALTER TABLE [dbo].[Shipment] ADD
[ShipSenseStatus] [int] NULL,
[ShipSenseChangeSets] [xml] NULL,
[ShipSenseEntry] varbinary(max) NULL
GO

PRINT N'Updating [dbo].[Shipment] defaults'
GO
UPDATE [Shipment] SET [ShipSenseStatus] = 0, [ShipSenseChangeSets] = '<ChangeSets/>', [ShipSenseEntry] = 0x00
GO

PRINT N'Altering [dbo].[Shipment] [ShipSenseStatus]'
GO
ALTER TABLE [dbo].[Shipment] ALTER COLUMN [ShipSenseStatus] [int] NOT NULL
GO

PRINT N'Altering [dbo].[Shipment] [ShipSenseChangeSets]'
GO
ALTER TABLE [dbo].[Shipment] ALTER COLUMN [ShipSenseChangeSets] [xml] NOT NULL
GO

PRINT N'Altering [dbo].[Shipment] [ShipSenseEntry]'
GO
ALTER TABLE [dbo].[Shipment] ALTER COLUMN [ShipSenseEntry] varbinary(max) NOT NULL
GO

PRINT N'Creating index [IX_Shipment_ShipmentID_HashKey] on [dbo].[Shipment]'
GO
CREATE NONCLUSTERED INDEX [IX_Shipment_OrderID_ShipSenseStatus] ON [dbo].[Shipment] 
(
	[OrderID] ASC,
	[Processed] ASC,
	[ShipSenseStatus] ASC
)
GO

PRINT N'Altering [dbo].[ShippingSettings]'
GO
ALTER TABLE [dbo].[ShippingSettings] ADD
[ShipSenseEnabled] [bit] NULL,
[ShipSenseUniquenessXml] [xml] NULL
GO


PRINT N'Update [dbo].[ShippingSettings] defaults'
GO
UPDATE [ShippingSettings] 
SET 
	[ShipSenseEnabled] = 1,
	[ShipSenseUniquenessXml] = '<ShipSenseUniqueness><ItemProperty><Name>SKU</Name><Name>Code</Name></ItemProperty><ItemAttribute /></ShipSenseUniqueness>'
GO


PRINT N'Altering [dbo].[ShippingSettings][ShipSenseEnabled]'
GO
ALTER TABLE [dbo].[ShippingSettings] 
	ALTER COLUMN [ShipSenseEnabled] [bit] NOT NULL
GO

PRINT N'Altering [dbo].[ShippingSettings][ShipSenseUniquenessXml]'
GO
ALTER TABLE [dbo].[ShippingSettings] 
	ALTER COLUMN [ShipSenseUniquenessXml] [xml] NOT NULL
GO

PRINT N'Creating [dbo].[ShipSenseKnowledgeBase]'
GO
CREATE TABLE [dbo].[ShipSenseKnowledgeBase]
(
[Hash] [nvarchar] (64) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Entry] [varbinary] (max) NOT NULL
)
GO
PRINT N'Creating primary key [PK_ShipSenseKnowledgeBase] on [dbo].[ShipSenseKnowledgeBase]'
GO
ALTER TABLE [dbo].[ShipSenseKnowledgeBase] ADD CONSTRAINT [PK_ShipSenseKnowledgeBase] PRIMARY KEY CLUSTERED  ([Hash])
GO


PRINT N'Altering [dbo].[Order]'
GO
-- Add Hash Key column to Order table
ALTER TABLE [dbo].[Order] ADD
	[ShipSenseHashKey] [nvarchar] (64) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
GO

PRINT N'Updating [dbo].[Order] defaults'
GO
update [dbo].[Order] set [ShipSenseHashKey] = ''
GO

PRINT N'Altering [dbo].[Order][ShipSenseHashKey]'
GO
ALTER TABLE [dbo].[Order]
	ALTER COLUMN [ShipSenseHashKey] [nvarchar] (64) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
GO



