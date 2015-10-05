SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Altering [dbo].[UpsAccount]'
GO
ALTER TABLE [dbo].[UpsAccount] ADD
[PromoStatus] [tinyint] NOT NULL CONSTRAINT [DF_UpsAccount_PromoStatus] DEFAULT ((0))
GO
PRINT N'Dropping constraints from [dbo].[UpsAccount]'
GO
ALTER TABLE [dbo].[UpsAccount] DROP CONSTRAINT [DF_UpsAccount_PromoStatus]
GO