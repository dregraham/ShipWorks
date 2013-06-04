select OBJECT_NAME(p.major_id),c.name, p.name, p.value 
   FROM sys.extended_properties p INNER JOIN sys.columns c ON p.major_id = c.object_id AND p.minor_id = c.column_id 
   WHERE p.name like 'Audit%'