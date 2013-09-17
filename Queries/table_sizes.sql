CREATE TABLE #sp_spaceused_output 
(
    Name varchar(255),
    [rows] int,
    reserved varchar(255),
    data varchar(255),
    index_size varchar(255),
    unused varchar(255)
)
    
CREATE TABLE #TableSizes 
(
    Name varchar(255),
    Records int,
    DataMB int,
    IndexMB int,
    UnusedMB int
)

EXEC sp_MSforeachtable @command1="insert into #sp_spaceused_output EXEC sp_spaceused '?'"

INSERT INTO #TableSizes (Name, Records, DataMB, IndexMB, UnusedMB)
	SELECT 
		name, 
		[rows], 
		SUBSTRING(data, 0, LEN(data)-2) / 1000, 
		SUBSTRING(index_size, 0, LEN(index_size)-2) / 1000, 
		SUBSTRING(unused, 0, LEN(unused)-2) / 1000
	FROM #sp_spaceused_output

SELECT * FROM #TableSizes
ORDER BY DataMB desc

DROP TABLE #sp_spaceused_output
DROP TABLE #TableSizes