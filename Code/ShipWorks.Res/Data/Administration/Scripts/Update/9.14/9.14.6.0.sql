-- ShipEngine only supports a single SmartPostHub so we need a new field to hold that single value
IF COL_LENGTH(N'[dbo].[FedExAccount]', N'SmartPostHub') IS NULL
	ALTER TABLE [dbo].[FedExAccount] ADD [SmartPostHub] [int] NOT NULL DEFAULT (0)
GO