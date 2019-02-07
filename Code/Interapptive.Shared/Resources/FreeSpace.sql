-- Get the drives the database files are in along with their physical sizes
DECLARE @DBFiles TABLE (FilePath varchar(252), Size INT)

INSERT INTO @DBFiles
    SELECT DISTINCT Physical_Name, mf.size as Size
    FROM sys.master_files mf
    INNER JOIN sys.databases db ON db.database_id = mf.database_id
    WHERE db.name = DB_NAME()

-- Get the freespace on the drives
DECLARE @FreeSpace TABLE(Drive CHAR(1), Free INT)
INSERT INTO @FreeSpace 
	exec xp_fixeddrives 

-- Return results
SELECT files.FilePath, files.size AS TotalSize, fs.Free AS Free
    FROM @DBFiles files
    LEFT OUTER JOIN @FreeSpace fs ON SUBSTRING(files.FilePath,1,1) = fs.drive