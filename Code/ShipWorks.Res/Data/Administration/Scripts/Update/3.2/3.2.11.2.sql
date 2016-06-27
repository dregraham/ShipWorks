SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Altering [dbo].[WorldShipPackage]'
GO
ALTER TABLE [dbo].[WorldShipPackage] ADD
[DeclaredValueAmount] [float] NULL,
[DeclaredValueOption] [nchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CN22GoodsType] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CN22Description] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PostalSubClass] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
GO
PRINT N'Altering [dbo].[WorldShipShipment]'
GO
ALTER TABLE [dbo].[WorldShipShipment] ALTER COLUMN [InvoiceCurrencyCode] [varchar] (3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
GO
PRINT N'Altering [dbo].[WorldShipGoods]'
GO
ALTER TABLE [dbo].[WorldShipGoods] ADD
[InvoiceCurrencyCode] [varchar] (3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
GO
PRINT N'Altering [dbo].[WorldShipProcessed]'
GO
ALTER TABLE [dbo].[WorldShipProcessed] ADD
[ServiceType] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PackageType] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[UpsPackageID] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DeclaredValueAmount] [float] NULL,
[DeclaredValueOption] [nchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[WorldShipShipmentID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[VoidIndicator] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[NumberOfPackages] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[LeadTrackingNumber] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
GO
ALTER TABLE [dbo].[WorldShipProcessed] ALTER COLUMN [ShipmentID] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
GO
