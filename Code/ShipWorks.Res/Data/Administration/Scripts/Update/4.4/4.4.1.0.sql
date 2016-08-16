SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Altering [dbo].[ShippingSettings]'
GO
ALTER TABLE [dbo].[ShippingSettings] ADD
[FedExFimsEnabled] [bit] NOT NULL CONSTRAINT [DF_ShippingSettings_FedExFimsEnabled] DEFAULT ((0)),
[FedExFimsUsername] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_ShippingSettings_FedExFimsUsername] DEFAULT (''),
[FedExFimsPassword] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_ShippingSettings_FedExFimsPassword] DEFAULT ('')
GO