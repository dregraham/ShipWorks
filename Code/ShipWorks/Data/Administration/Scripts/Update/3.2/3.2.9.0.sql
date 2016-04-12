-- Convert 3dcart GenericStoreOrderUpdate action tasks to ThreeDCartOrderUpdateTask
begin tran

	declare @storeId nvarchar(20)

	DECLARE updateStores CURSOR
		FOR 
			SELECT distinct s.StoreID FROM Store s
			where s.TypeCode = 29
			
	OPEN updateStores
	FETCH NEXT FROM updateStores into @storeId

	WHILE @@FETCH_STATUS = 0
	BEGIN

		update [Action] set TaskSummary = 'ThreeDCartOrderUpdate' where ActionID in 
		(
			select ActionID from ActionTask 
			where TaskIdentifier = 'GenericStoreOrderUpdate' and TaskSettings.exist('//Settings/StoreID[@value=sql:variable("@storeId")]') = 1
		)

		update ActionTask set TaskIdentifier='ThreeDCartOrderUpdate', TaskSettings.modify('delete //Settings/Comment')
		where TaskIdentifier = 'GenericStoreOrderUpdate' and TaskSettings.exist('//Settings/StoreID[@value=sql:variable("@storeId")]') = 1

		FETCH NEXT FROM updateStores into @storeId
	END

	CLOSE updateStores;
	DEALLOCATE updateStores;

commit tran
