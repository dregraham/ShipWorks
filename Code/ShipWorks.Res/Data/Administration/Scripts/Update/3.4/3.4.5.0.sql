BEGIN transaction

	UPDATE ActionTask
	SET TaskSettings = '<Settings><StoreID value="'+TaskSettings.value('(/Settings/StoreID/@value)[1]', 'varchar(max)')+'" /></Settings>',
	TaskIdentifier = 'EtsyShipmentUploadTask'
	where TaskSettings.value('(/Settings/Comment/@value)[1]', 'varchar(max)') = '{//ServiceUsed} - {//TrackingNumber}'
	AND TaskSettings.exist('(/Settings/StoreID/@value)[1]') = 1
	AND ActionTask.TaskIdentifier = 'EtsyUploadTask'
	
	UPDATE a
	SET a.TaskSummary = 'EtsyShipmentUploadTask'
	from Action a
	inner JOIN ActionTask at ON a.ActionID = at.ActionID
	where at.TaskIdentifier = 'EtsyShipmentUploadTask'

COMMIT
