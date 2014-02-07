-- Update the SellerVantage ModuleUrl
update GenericModuleStore set ModuleUrl = 'http://app.sellervantage.com/shipworksv3/' where StoreID in (select StoreID from Store where TypeCode = 23)

SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Dropping foreign keys from [dbo].[WorldShipGoods]'
GO
ALTER TABLE [dbo].[WorldShipGoods] DROP CONSTRAINT [FK_WorldShipGoods_WorldShipShipment]
GO
PRINT N'Dropping constraints from [dbo].[WorldShipGoods]'
GO
ALTER TABLE [dbo].[WorldShipGoods] DROP CONSTRAINT [PK_WorldShipGoods]
GO
PRINT N'Rebuilding [dbo].[WorldShipGoods]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_WorldShipGoods]
(
[WorldShipGoodsID] [bigint] NOT NULL IDENTITY(1098, 1000),
[ShipmentID] [bigint] NOT NULL,
[ShipmentCustomsItemID] [bigint] NOT NULL,
[Description] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TariffCode] [varchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CountryOfOrigin] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Units] [int] NOT NULL,
[UnitOfMeasure] [varchar] (5) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[UnitPrice] [money] NOT NULL,
[Weight] [float] NOT NULL,
[InvoiceCurrencyCode] [varchar] (3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
INSERT INTO [dbo].[tmp_rg_xx_WorldShipGoods]([ShipmentID], [ShipmentCustomsItemID], [Description], [TariffCode], [CountryOfOrigin], [Units], [UnitOfMeasure], [UnitPrice], [Weight], [InvoiceCurrencyCode]) SELECT [ShipmentID], [ShipmentCustomsItemID], [Description], [TariffCode], [CountryOfOrigin], [Units], [UnitOfMeasure], [UnitPrice], [Weight], [InvoiceCurrencyCode] FROM [dbo].[WorldShipGoods]
GO
DROP TABLE [dbo].[WorldShipGoods]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_WorldShipGoods]', N'WorldShipGoods'
GO
PRINT N'Creating primary key [PK_WorldShipGoods_1] on [dbo].[WorldShipGoods]'
GO
ALTER TABLE [dbo].[WorldShipGoods] ADD CONSTRAINT [PK_WorldShipGoods] PRIMARY KEY CLUSTERED  ([WorldShipGoodsID])
GO
PRINT N'Adding foreign keys to [dbo].[WorldShipGoods]'
GO
ALTER TABLE [dbo].[WorldShipGoods] ADD CONSTRAINT [FK_WorldShipGoods_WorldShipShipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[WorldShipShipment] ([ShipmentID]) ON DELETE CASCADE
GO

