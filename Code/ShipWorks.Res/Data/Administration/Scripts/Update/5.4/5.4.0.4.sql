SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Dropping constraints from [dbo].[AmazonShipment]'
GO
ALTER TABLE [dbo].[AmazonShipment] DROP CONSTRAINT [DF_AmazonShipment_ShippingServiceOfferId]
GO
PRINT N'Altering [dbo].[AmazonShipment]'
GO
ALTER TABLE [dbo].[AmazonShipment] ALTER COLUMN [ShippingServiceOfferID] [nvarchar] (250) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
GO
PRINT N'Adding constraints to [dbo].[AmazonShipment]'
GO
ALTER TABLE [dbo].[AmazonShipment] ADD CONSTRAINT [DF_AmazonShipment_ShippingServiceOfferId] DEFAULT ('') FOR [ShippingServiceOfferID]
GO