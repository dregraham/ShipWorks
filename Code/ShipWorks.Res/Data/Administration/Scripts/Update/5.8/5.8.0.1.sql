﻿SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Updating [dbo].[GenericModuleStore]'
UPDATE GenericModuleStore
     SET ModuleUrl = REPLACE(ModuleUrl, '/rest/V1/shipworks', '')
WHERE ModulePlatform = 'Magento'