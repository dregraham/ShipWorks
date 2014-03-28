SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Altering [dbo].[Shipment]'
GO
ALTER TABLE [dbo].[Shipment] ADD
[ShipSenseStatus] [int] NULL,
[ShipSenseChangeSets] [xml] NULL
GO

update [Shipment] set [ShipSenseStatus] = 0, [ShipSenseChangeSets] = '<ChangeSets/>'
GO

ALTER TABLE [dbo].[Shipment] ALTER COLUMN [ShipSenseStatus] [int] NOT NULL
GO

ALTER TABLE [dbo].[Shipment] ALTER COLUMN [ShipSenseChangeSets] [xml] NOT NULL
GO


PRINT N'Altering [dbo].[ShippingSettings]'
GO
ALTER TABLE [dbo].[ShippingSettings] ADD
[ShipSenseEnabled] [bit] NULL
GO

update [ShippingSettings] set [ShipSenseEnabled] = 1
GO

ALTER TABLE [dbo].[ShippingSettings] ALTER COLUMN [ShipSenseEnabled] [bit] NOT NULL
GO


PRINT N'Creating [dbo].[ShipSenseKnowledgeBase]'
GO
CREATE TABLE [dbo].[ShipSenseKnowledgeBase]
(
[Hash] [nvarchar] (64) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Entry] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_ShipSenseKnowledgeBase] on [dbo].[ShipSenseKnowledgeBase]'
GO
ALTER TABLE [dbo].[ShipSenseKnowledgeBase] ADD CONSTRAINT [PK_ShipSenseKnowledgeBase] PRIMARY KEY CLUSTERED  ([Hash])
GO