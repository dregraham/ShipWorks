DECLARE @name varchar(100)
DECLARE @newname varchar(100)

DECLARE TableCursor CURSOR FOR
SELECT name from sys.tables where name not like 'v2m_%' AND [type] = 'U'

OPEN TableCursor;

FETCH NEXT FROM TableCursor INTO @name
WHILE @@FETCH_STATUS = 0
BEGIN
	IF @name IN 
	(
		'Actions',
		'AmazonInventory',
		'Clients',
		'ClientStoreSettings',
		'Customers',
		'CustomLabelSheets',
		'DhlPreferences',
		'DhlShipments',
		'DhlShippers',
		'Downloaded',
		'DownloadLog',
		'EmailAccounts',
		'EmailLog',
		'EndiciaPreferences',
		'EndiciaShippers',
		'EndiciaScanForms',
		'FedexClosings',
		'FedexPackages',
		'FedexPreferences',
		'FedexShipments',
		'FedexShippers',
		'FeedbackPresets',
		'Filters',
		'MivaBatches',
		'MivaSebenzaMsgs',
		'Notifications',
		'OrderCharges',
		'OrderItemAttributes',
		'OrderItems',
		'Orders',
		'PaymentDetails',
		'ShipmentCommodities',
		'Shipments',
		'Stores',
		'UpsPackages',
		'UpsPreferences',
		'UpsShipments',
		'UpsShippers',
		'UspsPackages',
		'UspsShipments',
		'YahooInventory'
	)
	BEGIN
		SET @newname = 'v2m_' + @name
		print 'changing ' + @name + ' to ' + @newname
		EXECUTE sp_rename @name, @newname , 'OBJECT' 
    END
    FETCH NEXT FROM TableCursor INTO @name
END;

CLOSE TableCursor;
DEALLOCATE TableCursor;
