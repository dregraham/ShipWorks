SELECT SUM(DataLength(r2.data) + DataLength(r2.checksum)) FROM 
	(SELECT DISTINCT res.resourceid FROM
		PrintResult r JOIN ObjectReference o ON o.ConsumerID = r.PrintResultID
		JOIN [Resource] res ON res.ResourceID = o.ObjectID WHERE r.PrintDate < '{CUTOFFDATE}' ) AS R1 join [Resource] R2
		ON R1.resourceID = R2.ResourceID