/****************************************************************

IF THIS VERSION NUMBER CHANGES, CHANGE ConfigurationData.IsArchive
	TOO!!!!!!!!!

*****************************************************************/

PRINT N'Altering [dbo].[Configuration]'
GO
ALTER TABLE [dbo].[Configuration] ADD
[ArchivalSettingsXml] [xml] NOT NULL CONSTRAINT [DF_Configuration_ArchivalSettingsXml] DEFAULT ('<ArchivalSettings/>')
GO
