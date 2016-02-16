UPDATE       GenericModuleStore
SET                ModuleUrl = REPLACE(ModuleUrl, 'nomorerack', 'choxi')
WHERE        (ModuleUrl LIKE '%nomorerack%') AND (ModulePlatform = 'Nomorerack')
GO