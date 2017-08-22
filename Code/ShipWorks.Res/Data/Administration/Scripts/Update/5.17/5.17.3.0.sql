SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Creating [dbo].[ShipmentReturnItem]'
GO
CREATE TABLE [dbo].[ShipmentReturnItem]
(
[ShipmentReturnItemID] [bigint] NOT NULL IDENTITY(1101, 1000),
[RowVersion] [timestamp] NOT NULL,
[ShipmentID] [bigint] NOT NULL,
[Name] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Quantity] [float] NOT NULL,
[Weight] [float] NOT NULL,
[Notes] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SKU] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Code] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_ShipmentReturnItem] on [dbo].[ShipmentReturnItem]'
GO
ALTER TABLE [dbo].[ShipmentReturnItem] ADD CONSTRAINT [PK_ShipmentReturnItem] PRIMARY KEY CLUSTERED  ([ShipmentReturnItemID])
GO
PRINT N'Adding foreign keys to [dbo].[ShipmentReturnItem]'
GO
ALTER TABLE [dbo].[ShipmentReturnItem] ADD CONSTRAINT [FK_ShipmentReturnItem_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO