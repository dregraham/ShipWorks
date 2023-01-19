UPDATE a
SET a.TaskIdentifier = 'AmazonShipmentUploadTask'
FROM dbo.ActionTask AS a
	OUTER APPLY a.TaskSettings.nodes('Settings/StoreID') AS m(c)
	INNER JOIN dbo.Store 
		s ON s.StoreID=m.c.value('@value', 'bigint') 
WHERE a.TaskIdentifier = 'ApiShipmentUploadTask' AND s.TypeCode = 10