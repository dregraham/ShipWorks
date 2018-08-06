PRINT N'Altering Rebuild Index Action'
GO
	DECLARE @actionID BIGINT
	SELECT @actionID = ActionID FROM [Action] WHERE [Name] = 'Reindex Data' and InternalOwner = 'ReIndex'

	UPDATE [Action] SET [Name] = 'Database Maintenance', TaskSummary = 'DatabaseMaintenance', InternalOwner = 'DatabaseMaintenance' WHERE ActionID = @actionID
	UPDATE ActionTask SET TaskIdentifier = 'DatabaseMaintenance' WHERE ActionID = @actionID
GO


