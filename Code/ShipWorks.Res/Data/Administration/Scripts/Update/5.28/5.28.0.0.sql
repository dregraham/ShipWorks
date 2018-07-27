PRINT N'Adding purge email history and print job history to purge database task.'
GO
UPDATE [ActionTask] 
	SET TaskSettings = REPLACE(CONVERT(NVARCHAR(MAX), TaskSettings), '</Settings>', '<PurgeEmailHistory value="False" /><PurgePrintJobHistory value="False" /></Settings>')
	WHERE TaskIdentifier = 'PurgeDatabase' AND CONVERT(NVARCHAR(MAX), TaskSettings) NOT LIKE '%PurgeEmailHistory%'
GO

PRINT N'Adding FK_AuditChangeDetail_Audit.'
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AuditChangeDetail_Audit]') AND parent_object_id = OBJECT_ID(N'[dbo].[AuditChangeDetail]'))
	ALTER TABLE [dbo].[AuditChangeDetail]  WITH CHECK ADD  CONSTRAINT [FK_AuditChangeDetail_Audit] FOREIGN KEY([AuditID])
	REFERENCES [dbo].[Audit] ([AuditID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AuditChangeDetail_Audit]') AND parent_object_id = OBJECT_ID(N'[dbo].[AuditChangeDetail]'))
	ALTER TABLE [dbo].[AuditChangeDetail] CHECK CONSTRAINT [FK_AuditChangeDetail_Audit]
GO

