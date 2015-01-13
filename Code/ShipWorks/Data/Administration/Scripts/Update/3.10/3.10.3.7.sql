PRINT N'Altering [dbo].[FedExShipment]'
GO
ALTER TABLE [dbo].[FedExShipment] ALTER COLUMN [CustomsRecipientTIN] [varchar] (24) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
ALTER TABLE [dbo].[FedExShipment] ALTER COLUMN [ImporterTIN] [nvarchar] (24) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL