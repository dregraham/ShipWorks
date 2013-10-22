use master;

select 
	(select name from sys.databases where database_id = r.database_id) as 'DatabaseName',
	r.session_ID, 
	r.start_time, 
	datediff(ms, r.start_time, getdate())/1000.0/60.0 as 'Elapsed Minutes', 
	r.command, 
	r.status,  
	t.text, 
	r.blocking_session_id, 
	r.last_wait_type, 
	r.wait_resource, 
	r.percent_complete, 
	r.total_elapsed_time, 
	r.reads, 
	r.writes, 
	r.logical_reads, 
	r.transaction_isolation_level, 
	r.row_count
FROM sys.dm_exec_requests AS r
		CROSS APPLY sys.dm_exec_sql_text(r.sql_handle) AS t
