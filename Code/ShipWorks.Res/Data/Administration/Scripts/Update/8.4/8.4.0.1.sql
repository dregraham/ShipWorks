PRINT N'Altering [dbo].[GenericModuleOrder]'
GO
IF COL_LENGTH(N'[dbo].[GenericModuleOrder]', N'Marketplace') IS NULL
ALTER TABLE [dbo].[GenericModuleOrder] ADD
    [Marketplace] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_GenericModuleOrder_Marketplace] DEFAULT ('')
GO
PRINT N'Dropping constraints from [dbo].[GenericModuleOrder]'
GO
IF EXISTS (SELECT 1 FROM sys.columns WHERE name = N'Marketplace' AND object_id = OBJECT_ID(N'[dbo].[GenericModuleOrder]', 'U') AND default_object_id = OBJECT_ID(N'[dbo].[DF_GenericModuleOrder_Marketplace]', 'D'))
ALTER TABLE [dbo].[GenericModuleOrder] DROP CONSTRAINT [DF_GenericModuleOrder_Marketplace]
GO
PRINT N'Creating index for [dbo].[GenericModuleOrder][Marketplace]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[GenericModuleOrder]')
                                 AND name = N'IX_SWDefault_GenericModuleOrder_Marketplace')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_SWDefault_GenericModuleOrder_Marketplace] ON [dbo].[GenericModuleOrder] ([Marketplace])
END
GO