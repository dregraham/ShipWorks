-- Cart systems in V2 that are no longer in V3.

-- Updating the StoreType after the fact because the correct storetypecode is needed during the 
-- actual migration process.
UPDATE {MASTERDATABASE}.dbo.Store
SET TypeCode = 28
WHERE 
	TypeCode IN (14, 15) -- CartKeeper and eTailComplete