/* Drop the assembly */
IF EXISTS (SELECT name FROM sys.assemblies WHERE name = N'{AssemblyName}')
BEGIN
	/* Drop the assembly user defined aggregates, triggers, functions and procedures */
	DECLARE @moduleId sysname
	DECLARE @moduleName sysname
	DECLARE @moduleType char(2)
	DECLARE @moduleClass tinyint
	DECLARE assemblyModules CURSOR FAST_FORWARD FOR
		SELECT t.object_id, t.name, t.type, t.parent_class as class
			FROM sys.triggers t
			INNER JOIN sys.assembly_modules m ON t.object_id = m.object_id
			INNER JOIN sys.assemblies a ON m.assembly_id = a.assembly_id
			WHERE a.name = N'{AssemblyName}'
		UNION
		SELECT o.object_id, o.name, o.type, NULL as class
			FROM sys.objects o
			INNER JOIN sys.assembly_modules m ON o.object_id = m.object_id
			INNER JOIN sys.assemblies a ON m.assembly_id = a.assembly_id
			WHERE a.name = N'{AssemblyName}'
	OPEN assemblyModules
	FETCH NEXT FROM assemblyModules INTO @moduleId, @moduleName, @moduleType, @moduleClass
	WHILE (@@FETCH_STATUS = 0)
	BEGIN
		DECLARE @dropModuleString nvarchar(256)
		IF (@moduleType = 'AF') SET @dropModuleString = N'AGGREGATE'
		IF (@moduleType = 'TA') SET @dropModuleString = N'TRIGGER'
		IF (@moduleType = 'FT' OR @moduleType = 'FS') SET @dropModuleString = N'FUNCTION'
		IF (@moduleType = 'PC') SET @dropModuleString = N'PROCEDURE'
		SET @dropModuleString = N'DROP ' + @dropModuleString + ' [' + REPLACE(@moduleName, ']', ']]') + ']'
		IF (@moduleType = 'TA' AND @moduleClass = 0)
		BEGIN
			SET @dropModuleString = @dropModuleString + N' ON DATABASE'
		END

		EXEC sp_executesql @dropModuleString
		FETCH NEXT FROM assemblyModules INTO @moduleId, @moduleName, @moduleType, @moduleClass
	END
	CLOSE assemblyModules
	DEALLOCATE assemblyModules

	DROP ASSEMBLY [{AssemblyName}] WITH NO DEPENDENTS
END
