SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
IF EXISTS (SELECT * FROM tempdb..sysobjects WHERE id=OBJECT_ID('tempdb..#tmpErrors')) DROP TABLE #tmpErrors
GO
CREATE TABLE #tmpErrors (Error int)
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
PRINT N'Dropping constraints from [dbo].[Configuration]'
GO
ALTER TABLE [dbo].[Configuration] DROP CONSTRAINT [PK_Configuration]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Rebuilding [dbo].[Configuration]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_Configuration]
(
[ConfigurationID] [bit] NOT NULL,
[RowVersion] [timestamp] NOT NULL,
[LogOnMethod] [int] NOT NULL,
[AddressCasing] [bit] NOT NULL,
[CustomerCompareEmail] [bit] NOT NULL,
[CustomerCompareAddress] [bit] NOT NULL,
[CustomerUpdateBilling] [bit] NOT NULL,
[CustomerUpdateShipping] [bit] NOT NULL,
[CustomerUpdateModifiedBilling] [int] NOT NULL,
[CustomerUpdateModifiedShipping] [int] NOT NULL,
[AuditNewOrders] [bit] NOT NULL,
[AuditDeletedOrders] [bit] NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
INSERT INTO [dbo].[tmp_rg_xx_Configuration]([ConfigurationID], [LogOnMethod], [AddressCasing], [CustomerCompareEmail], [CustomerCompareAddress], [CustomerUpdateBilling], [CustomerUpdateShipping], [AuditNewOrders], [AuditDeletedOrders], [CustomerUpdateModifiedBilling], [CustomerUpdateModifiedShipping]) SELECT [ConfigurationID], [LogOnMethod], [AddressCasing], [CustomerCompareEmail], [CustomerCompareAddress], [CustomerUpdateBilling], [CustomerUpdateShipping], [AuditNewOrders], [AuditDeletedOrders], 0, 0 FROM [dbo].[Configuration]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DROP TABLE [dbo].[Configuration]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_Configuration]', N'Configuration'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_Configuration] on [dbo].[Configuration]'
GO
ALTER TABLE [dbo].[Configuration] ADD CONSTRAINT [PK_Configuration] PRIMARY KEY CLUSTERED  ([ConfigurationID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
IF EXISTS (SELECT * FROM #tmpErrors) ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT>0 BEGIN
PRINT 'The database update succeeded'
COMMIT TRANSACTION
END
ELSE PRINT 'The database update failed'
GO
DROP TABLE #tmpErrors
GO
