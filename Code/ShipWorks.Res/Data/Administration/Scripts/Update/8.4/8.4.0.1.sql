PRINT N'Altering [dbo].[GenericModuleOrder]'
GO
IF COL_LENGTH(N'[dbo].[GenericModuleOrder]', N'OrderSource') IS NULL
ALTER TABLE [dbo].[GenericModuleOrder] ADD
    [OrderSource] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_GenericModuleOrder_OrderSource] DEFAULT ('')
GO
PRINT N'Dropping constraints from [dbo].[GenericModuleOrder]'
GO
IF EXISTS (SELECT 1 FROM sys.columns WHERE name = N'OrderSource' AND object_id = OBJECT_ID(N'[dbo].[GenericModuleOrder]', 'U') AND default_object_id = OBJECT_ID(N'[dbo].[DF_GenericModuleOrder_OrderSource]', 'D'))
ALTER TABLE [dbo].[GenericModuleOrder] DROP CONSTRAINT [DF_GenericModuleOrder_OrderSource]
GO