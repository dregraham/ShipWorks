PRINT N'Altering [dbo].[Configuration]'
GO
ALTER TABLE [dbo].[Configuration] ADD
[AuditEnabled] [bit] NOT NULL CONSTRAINT [DF_Configuration_AuditEnabled] DEFAULT ((1))
GO

