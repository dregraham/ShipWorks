SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Altering [dbo].[Shipment]'
GO
ALTER TABLE [dbo].[Shipment] ADD
[ShipSenseStatus] [int] NULL,
[ShipSenseChangeSets] [xml] NULL,
[ShipSenseHashKey] varchar(64) NULL,
[ShipSenseEntry] varbinary(max) NULL
GO

UPDATE [Shipment] SET [ShipSenseStatus] = 0, [ShipSenseChangeSets] = '<ChangeSets/>', [ShipSenseHashKey] = '', [ShipSenseEntry] = 0x00
GO

ALTER TABLE [dbo].[Shipment] ALTER COLUMN [ShipSenseStatus] [int] NOT NULL
GO

ALTER TABLE [dbo].[Shipment] ALTER COLUMN [ShipSenseChangeSets] [xml] NOT NULL
GO

ALTER TABLE [dbo].[Shipment] ALTER COLUMN [ShipSenseHashKey] [varchar(64)] NOT NULL
GO

ALTER TABLE [dbo].[Shipment] ALTER COLUMN [ShipSenseEntry] [varbinary(max)] NOT NULL
GO


PRINT N'Altering [dbo].[ShippingSettings]'
GO
ALTER TABLE [dbo].[ShippingSettings] ADD
[ShipSenseEnabled] [bit] NULL,
[ShipSenseUniquenessXml] [xml] NULL
GO

UPDATE [ShippingSettings] 
SET 
	[ShipSenseEnabled] = 1,
	[ShipSenseUniquenessXml] = '<ShipSenseUniqueness><ItemProperty><Name>SKU</Name><Name>Code</Name></ItemProperty><ItemAttribute /></ShipSenseUniqueness>'
GO

ALTER TABLE [dbo].[ShippingSettings] 
	ALTER COLUMN [ShipSenseEnabled] [bit] NOT NULL
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