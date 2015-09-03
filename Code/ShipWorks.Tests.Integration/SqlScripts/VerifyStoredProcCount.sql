--select '''' + name + ''','  from sys.procedures

declare @count int
select @count = count(*) from sys.procedures where name not in
(
'GetSchemaVersion',
'GetAssemblySchemaVersion',
'ValidateGridLayouts',
'CalculateUpdateFilterCounts',
'CalculateInitialFilterCounts',
'DeleteAbandonedFilterCounts',
'ValidateFilterLayouts',
'PurgeAudit',
'PurgeLabels',
'PurgePrintResult',
'PurgeEmailOutbound',
'PurgeAbandonedResources',
'ResetShipSense',
'ShipmentShipSenseProcedure',
'RebuildTableIndex'
)

if @count > 0
begin
	RAISERROR('FAILED: Stored proc count is wrong', 10, 1, '')
end

