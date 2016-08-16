ALTER TABLE [dbo].[Configuration] ADD
[CustomerKey] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Configuration_CustomerKey] DEFAULT ('')
GO
ALTER TABLE [dbo].[Configuration] DROP CONSTRAINT [DF_Configuration_CustomerKey]
GO