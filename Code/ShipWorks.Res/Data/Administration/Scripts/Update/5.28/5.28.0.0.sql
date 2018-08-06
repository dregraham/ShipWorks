PRINT N'Adding purge email history and print job history to purge database task.'
GO
UPDATE [ActionTask] 
	SET TaskSettings = REPLACE(CONVERT(NVARCHAR(MAX), TaskSettings), '</Settings>', '<PurgeEmailHistory value="False" /><PurgePrintJobHistory value="False" /></Settings>')
	WHERE TaskIdentifier = 'PurgeDatabase' AND CONVERT(NVARCHAR(MAX), TaskSettings) NOT LIKE '%PurgeEmailHistory%'
GO

PRINT N'Adding FK_AuditChangeDetail_Audit.'
GO
/* Remove any AuditChangeDetail rows that do not point to an existing AuditID*/
DELETE dbo.AuditChangeDetail  
FROM dbo.AuditChangeDetail 
WHERE AuditID NOT IN 
(
	SELECT AuditID FROM dbo.[Audit]
)

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AuditChangeDetail_Audit]') AND parent_object_id = OBJECT_ID(N'[dbo].[AuditChangeDetail]'))
	ALTER TABLE [dbo].[AuditChangeDetail]  WITH CHECK ADD  CONSTRAINT [FK_AuditChangeDetail_Audit] FOREIGN KEY([AuditID])
	REFERENCES [dbo].[Audit] ([AuditID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AuditChangeDetail_Audit]') AND parent_object_id = OBJECT_ID(N'[dbo].[AuditChangeDetail]'))
	ALTER TABLE [dbo].[AuditChangeDetail] CHECK CONSTRAINT [FK_AuditChangeDetail_Audit]
GO

PRINT N'Dropping index [IX_SWDefault_ObjectReference_ConsumerIDReferenceKey] from [dbo].[ObjectReference]'
GO
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_SWDefault_ObjectReference_ConsumerIDReferenceKey' AND object_id = OBJECT_ID(N'[dbo].[ObjectReference]'))
	DROP INDEX [IX_SWDefault_ObjectReference_ConsumerIDReferenceKey] ON [dbo].[ObjectReference]
GO
PRINT N'Creating index [IX_SWDefault_ObjectReference_ConsumerIDReferenceKey] on [dbo].[ObjectReference]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_SWDefault_ObjectReference_ConsumerIDReferenceKey' AND object_id = OBJECT_ID(N'[dbo].[ObjectReference]'))
	CREATE UNIQUE NONCLUSTERED INDEX [IX_SWDefault_ObjectReference_ConsumerIDReferenceKey] ON [dbo].[ObjectReference] ([ConsumerID], [ReferenceKey]) INCLUDE ([ObjectID])
GO

PRINT N'Dropping index [IX_SWDefault_PrintResult_PrintDateRelatedObjectID] from [dbo].[PrintResult]'
GO
IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[PrintResult]') AND name = N'IX_SWDefault_PrintResult_PrintDateRelatedObjectID')
	DROP INDEX [IX_SWDefault_PrintResult_PrintDateRelatedObjectID] ON [dbo].[PrintResult]
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[PrintResult]') AND name = N'IX_SWDefault_PrintResult_PrintDateRelatedObjectID')
	CREATE NONCLUSTERED INDEX [IX_SWDefault_PrintResult_PrintDateRelatedObjectID] ON [dbo].[PrintResult] ([PrintDate] ASC, [RelatedObjectID] ASC) INCLUDE ([TemplateType], [ContentResourceID])  
GO