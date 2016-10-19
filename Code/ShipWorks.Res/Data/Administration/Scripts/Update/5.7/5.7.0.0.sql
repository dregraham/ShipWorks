SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Altering [dbo].[FedExPackage]'
GO
ALTER TABLE [dbo].[FedExPackage] ADD
[SignatoryContactName] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[SignatoryTitle] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[SignatoryPlace] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
GO
PRINT N'Altering [dbo].[FedExProfilePackage]'
GO
ALTER TABLE [dbo].[FedExProfilePackage] ADD
[SignatoryContactName] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[SignatoryTitle] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[SignatoryPlace] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
GO
PRINT N'Altering [dbo].[FedExShipment]'
GO
ALTER TABLE [dbo].[FedExShipment] ADD
[ThirdPartyConsignee] [bit] NOT NULL CONSTRAINT [DF_FedExShipment_ThirdPartyConsignee] DEFAULT ((0))
GO
PRINT N'Dropping constraints from [dbo].[FedExShipment]'
GO
ALTER TABLE [dbo].[FedExShipment] DROP CONSTRAINT [DF_FedExShipment_ThirdPartyConsignee]
GO
PRINT N'Altering [dbo].[FedExProfile]'
GO
ALTER TABLE [dbo].[FedExProfile] ADD
[ThirdPartyConsignee] [bit] NULL
GO