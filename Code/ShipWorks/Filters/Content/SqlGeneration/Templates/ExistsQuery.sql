SELECT TOP 1 CONVERT(BIGINT, <SwFilterNodeID />) as [FilterNodeID] 
	   FROM {2}
	   {3}
	   AND {0}.{1} = @ExistsQueryObjectID