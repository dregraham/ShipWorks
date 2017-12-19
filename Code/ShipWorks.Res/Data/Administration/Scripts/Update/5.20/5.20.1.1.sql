PRINT N'Altering [dbo].[MivaStore]'
GO
ALTER TABLE [dbo].[MivaStore] ADD
[AddendumCheckoutDataEnabled] [bit] NOT NULL CONSTRAINT [DF_MivaStore_AddendumCheckoutDataEnabled] DEFAULT ((1))
GO
PRINT N'Dropping constraints from [dbo].[MivaStore]'
GO
ALTER TABLE [dbo].[MivaStore] DROP CONSTRAINT [DF_MivaStore_AddendumCheckoutDataEnabled]
GO