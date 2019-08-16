PRINT N'Deleting extra auto archive tasks'
GO

DECLARE @maxAutoArchiveActionQueueId BIGINT = -1

SELECT TOP 1 @maxAutoArchiveActionQueueId = ActionQueueId 
FROM ActionQueue
WHERE ActionName = 'Auto archive action' AND [Status] = 0
ORDER BY [Status], TriggerDate DESC

DELETE FROM ActionQueue
WHERE ActionQueueID != @maxAutoArchiveActionQueueId
  AND ActionName = 'Auto archive action'
GO
