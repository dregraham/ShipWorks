UPDATE       GenericModuleStore
SET                ModuleUrl = REPLACE(ModuleUrl, 'vendor-api.nomorerack.com', 'vendor-api.choxi.com')
WHERE        (ModuleUrl LIKE '%vendor-api.nomorerack.com%') AND (ModulePlatform = 'Nomorerack')
GO