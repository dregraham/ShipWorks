PRINT N'Altering [dbo].[FedExPackage]'
GO
IF COL_LENGTH(N' [dbo].[FedExPackage]', N'DangerousGoodsAuthorization') IS NULL
ALTER TABLE [dbo].[FedExPackage] ADD
[DangerousGoodsAuthorization] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
GO
PRINT N'Altering [dbo].[FedExProfilePackage]'
GO
IF COL_LENGTH(N' [dbo].[FedExProfilePackage]', N'DangerousGoodsAuthorization') IS NULL
ALTER TABLE [dbo].[FedExProfilePackage] ADD
[DangerousGoodsAuthorization] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
GO