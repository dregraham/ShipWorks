SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Altering [dbo].[FedExShipment]'
GO
ALTER TABLE [dbo].[FedExShipment] ADD
[ReturnsClearance] [bit] NOT NULL CONSTRAINT [DF_FedExShipment_ReturnsClearance] DEFAULT ((0))
GO
PRINT N'Altering [dbo].[FedExProfile]'
GO
ALTER TABLE [dbo].[FedExProfile] ADD
[ReturnsClearance] [bit]
GO
UPDATE TABLE fp
