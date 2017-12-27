
PRINT N'Altering TABLE [dbo].[UspsAccount]'
GO
ALTER TABLE [dbo].[UspsAccount]
Add [AcceptedFCMILetterWarning] [bit] NOT NULL CONSTRAINT [DF_UspsAccount_AcceptedFCMILetterWarning] DEFAULT ((0))
GO
PRINT N'Dropping constraints from [dbo].[UspsAccount]'
GO
ALTER TABLE [dbo].[UspsAccount] DROP CONSTRAINT [DF_UspsAccount_AcceptedFCMILetterWarning]
GO