PRINT N'Altering [dbo].[Configuration]'
GO
IF COL_LENGTH(N'[dbo].[Configuration]', N'LegacyCustomerKey') IS NULL
ALTER TABLE [dbo].[Configuration] ADD
    [LegacyCustomerKey] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Configuration_LegacyCustomerKey] DEFAULT ('')
GO