-- Get the drives the database files are in along with their physical sizes
DECLARE @drive TABLE (drive CHAR, size INT)

INSERT INTO @drive
	SELECT DISTINCT SUBSTRING(Physical_Name,1,1), mf.size
	FROM
		sys.master_files mf
	INNER JOIN 
		sys.databases db ON db.database_id = mf.database_id
	WHERE db.name = DB_NAME() AND SUBSTRING(Physical_Name,2,1) = ':'

-- Get the freespace on the drives
DECLARE @FreeSpace TABLE(Drive CHAR(1), Free INT)
INSERT INTO @FreeSpace exec xp_fixeddrives

-- Return results
SELECT SUM(size) AS TotalSize, MIN(Free) AS Free
	FROM @drive d
	INNER JOIN @FreeSpace f ON d.drive=f.drive
