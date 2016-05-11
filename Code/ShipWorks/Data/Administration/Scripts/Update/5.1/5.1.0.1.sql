SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Altering [dbo].[SearsStore]'
GO
ALTER TABLE [dbo].[SearsStore] ADD
[SecretKey] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_SearsStore_SecretKey] DEFAULT (''),
[SellerID] [nvarchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_SearsStore_SellerID] DEFAULT ('')
GO
PRINT N'Dropping constraints from [dbo].[SearsStore]'
GO
ALTER TABLE [dbo].[SearsStore] DROP CONSTRAINT [DF_SearsStore_SecretKey]
GO