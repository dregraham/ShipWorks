IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertAuditTestData]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[InsertAuditTestData]
GO
CREATE PROC InsertAuditTestData (@NumberToAdd INT) as

SET NOCOUNT ON

DECLARE @Message NVARCHAR(500)
DECLARE @ProcessingStart DATETIME = GETUTCDATE()
DECLARE @ProcessingEnd DATETIME
DECLARE @Count INT = 0
DECLARE @StartDate DATETIME = GETUTCDATE() - 28
DECLARE @EndDate DATETIME = GETUTCDATE()
DECLARE @NumberOfDays int = DATEDIFF(d, @StartDate, @enddate)
DECLARE @NumberPerDay INT = @NumberToAdd / @NumberOfDays
DECLARE @AuditID BIGINT
DECLARE @AuditChangeID BIGINT
DECLARE @AuditChangeDetailID BIGINT
DECLARE @UserID BIGINT = 1002
DECLARE @ComputerId BIGINT = 1001
DECLARE @TransactionID BIGINT 

SELECT @NumberOfDays
SELECT @NumberPerDay

SELECT @TransactionID = MAX(TransactionID) FROM Audit

--SELECT COUNT(*) FROM dbo.Audit a
--SELECT COUNT(*) FROM dbo.AuditChange ac
--SELECT COUNT(*) FROM dbo.AuditChangeDetail acd

DECLARE @PercentComplete int = 0
DECLARE @PercentCompleteBreak INT = @NumberToAdd / 10

--In long running loops, Print doesn't send output to the client.  So using RaiseError with severity of no error and NoWait 
--so that we can always get timely status updates.
RAISERROR('Starting Audit Test Data Inserts', 10, 1) WITH NOWAIT

WHILE (@Count < @NumberToAdd)
	BEGIN
		SET @Count = @Count + 1
		
		IF (@PercentComplete < (@Count / @PercentCompleteBreak) % 10)
		--PRINT @PercentComplete
		BEGIN
			SET @PercentComplete = (@Count / @PercentCompleteBreak) % 10
			SET @Message = CAST(@PercentComplete * 10 AS NVARCHAR(10)) + ' Percent Complete.  ' + CAST(@Count AS NVARCHAR(10)) + ' Rows Inserted.'
			RAISERROR(@Message, 10, 1) WITH NOWAIT
		END
		
		SET @TransactionID = @TransactionID + 1
		INSERT INTO dbo.Audit (TransactionID, UserID ,ComputerID ,Reason ,ReasonDetail ,Date ,Action ,ObjectID , HasEvents)
		VALUES  ( @TransactionID + 1 , -- TransactionID - bigint
		          @UserID , -- UserID - bigint
		          @ComputerId , -- ComputerID - bigint
		          0 , -- Reason - int
		          'ReasonDetail' , -- ReasonDetail - varchar(100)
		          DATEADD(d,  @count % @NumberOfDays, @StartDate) , -- Date - datetime
		          0 , -- Action - int
		          0 , -- ObjectID - bigint
		          0  -- HasEvents - bit
		        )
		SET @AuditID = SCOPE_IDENTITY()
		
		INSERT INTO dbo.AuditChange ( AuditID, ChangeType, ObjectID )
		VALUES  ( @AuditID, -- AuditID - bigint
		          0, -- ChangeType - int
		          0  -- ObjectID - bigint
		          )
		SET @AuditChangeID = SCOPE_IDENTITY()

		INSERT INTO dbo.AuditChangeDetail ( AuditChangeID ,AuditID ,DisplayName ,DisplayFormat ,DataType ,
		          TextOld ,TextNew ,VariantOld ,VariantNew
		        )
		VALUES  ( @AuditChangeID , -- AuditChangeID - bigint
		          @AuditID , -- AuditID - bigint
		          '' , -- DisplayName - varchar(50)
		          0 , -- DisplayFormat - tinyint
		          0 , -- DataType - tinyint
		          N'' , -- TextOld - nvarchar(max)
		          N'' , -- TextNew - nvarchar(max)
		          NULL , -- VariantOld - sql_variant
		          NULL  -- VariantNew - sql_variant
		        )

	END

SET @Message = 'Done. ' + CAST(@Count AS NVARCHAR(10)) + ' Rows Inserted.'
RAISERROR(@Message, 10, 1) WITH NOWAIT
SET @ProcessingEnd = GETUTCDATE()

--SELECT COUNT(*) FROM dbo.Audit a
--SELECT COUNT(*) FROM dbo.AuditChange ac
--SELECT COUNT(*) FROM dbo.AuditChangeDetail acd
    
SET @ProcessingEnd = GETUTCDATE()

SELECT DATEDIFF(second, @ProcessingStart, @ProcessingEnd) AS 'Time To Process'

SELECT date, COUNT(date) 
FROM audit 
WHERE auditid IN (SELECT TOP (@NumberToAdd) auditid FROM audit ORDER BY auditid DESC)
GROUP BY date 
ORDER BY Date

GO

EXEC InsertAuditTestData 10