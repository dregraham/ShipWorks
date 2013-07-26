IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertPrintResultTestData]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[InsertPrintResultTestData]
GO
CREATE PROC InsertPrintResultTestData (@NumberToAdd INT) as

SET NOCOUNT ON

DECLARE @Message NVARCHAR(500)
DECLARE @ProcessingStart DATETIME = GETUTCDATE()
DECLARE @ProcessingEnd DATETIME
DECLARE @Count INT = 0
DECLARE @StartDate DATETIME = GETUTCDATE() - 28
DECLARE @EndDate DATETIME = GETUTCDATE()
DECLARE @NumberOfDays int = DATEDIFF(d, @StartDate, @enddate)
DECLARE @NumberPerDay INT = @NumberToAdd / @NumberOfDays

DECLARE @PrintResultID BIGINT 
DECLARE @ObjectReferenceID BIGINT
DECLARE @AdditionalObjectReferenceID BIGINT
DECLARE @ResourceID BIGINT
DECLARE @UserID BIGINT = 1002
DECLARE @ComputerId BIGINT = 1001

--SELECT COUNT(*) AS 'PrintResults' FROM dbo.PrintResult
--SELECT COUNT(*) AS 'Resources' FROM dbo.Resource 
--SELECT COUNT(*) AS 'ObjectReferences' FROM dbo.ObjectReference

DECLARE @PercentComplete int = 0
DECLARE @PercentCompleteBreak INT = @NumberToAdd / 10

--In long running loops, Print doesn't send output to the client.  So using RaiseError with severity of no error and NoWait 
--so that we can always get timely status updates.
RAISERROR('Starting Print Result Test Data Inserts', 10, 1) WITH NOWAIT
        
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
  
		-- Create a fake resource to use
		INSERT INTO dbo.Resource ( Data ,Checksum ,Compressed ,Filename)
		VALUES  ( NEWID(), -- Data - varbinary(max)
		          NEWID()  , -- Checksum - binary
		          0 , -- Compressed - bit
		          N'' + SUBSTRING(CAST(NEWID() AS NVARCHAR(50)), 0, 30)  -- Filename - nvarchar(30)
		        )
		SET @ResourceID = SCOPE_IDENTITY()    
        
		-- Add PrintResult
		INSERT INTO dbo.PrintResult (JobIdentifier, RelatedObjectID, ContextObjectID, TemplateID, TemplateType, OutputFormat, LabelSheetID, ComputerID, ContentResourceID, PrintDate, PrinterName, PaperSource, PaperSourceName, Copies, Collated, PageMarginLeft, PageMarginRight, PageMarginBottom, PageMarginTop, PageWidth, PageHeight )
		VALUES  ( NEWID() , -- JobIdentifier - uniqueidentifier
		          0 , -- RelatedObjectID - bigint
		          0 , -- ContextObjectID - bigint
		          0 , -- TemplateID - bigint
		          0 , -- TemplateType - int
		          0 , -- OutputFormat - int
		          0 , -- LabelSheetID - bigint
		          @ComputerId , -- ComputerID - bigint
		          0 , -- ContentResourceID - bigint
		          DATEADD(d,  @count % @NumberOfDays, @StartDate) , -- PrintDate - datetime
		          N'' , -- PrinterName - nvarchar(350)
		          0 , -- PaperSource - int
		          N'' , -- PaperSourceName - nvarchar(100)
		          0 , -- Copies - int
		          0 , -- Collated - bit
		          0.0 , -- PageMarginLeft - float
		          0.0 , -- PageMarginRight - float
		          0.0 , -- PageMarginBottom - float
		          0.0 , -- PageMarginTop - float
		          0.0 , -- PageWidth - float
		          0.0  -- PageHeight - float
		        )
		SELECT @PrintResultID = SCOPE_IDENTITY()

		INSERT INTO dbo.ObjectReference
		        ( ConsumerID , ReferenceKey ,ObjectID ,Reason
		        )
		VALUES  ( @PrintResultID , -- ConsumerID - bigint
		          N'' + SUBSTRING(CAST(NEWID() AS NVARCHAR(50)), 0, 30) , -- ReferenceKey - varchar(250)
		          @ResourceID , -- ObjectID - bigint
		          N''  -- Reason - nvarchar(250)
		        )
		SET @ObjectReferenceID = SCOPE_IDENTITY()  

		-- Additional ObjectReference for PrintResults to point to, so that the purge script has something to delete
		INSERT INTO dbo.ObjectReference
				( ConsumerID , ReferenceKey ,ObjectID ,Reason
				)
		VALUES  ( @PrintResultID , -- ConsumerID - bigint
					N'' + SUBSTRING(CAST(NEWID() AS NVARCHAR(50)), 0, 30) , -- ReferenceKey - varchar(250)
					@ResourceID , -- ObjectID - bigint
					N''  -- Reason - nvarchar(250)
				)

		UPDATE dbo.PrintResult SET ContentResourceID = @ObjectReferenceID WHERE PrintResultID = @PrintResultID
	END
SET @Message = '100 Percent Complete.  ' + CAST(@Count AS NVARCHAR(10)) + ' Rows Inserted.'
RAISERROR(@Message, 10, 1) WITH NOWAIT

SET @ProcessingEnd = GETUTCDATE()

SELECT DATEDIFF(second, @ProcessingStart, @ProcessingEnd) AS 'Time To Process'

SELECT PrintDate, COUNT(PrintDate) 
FROM dbo.PrintResult 
WHERE PrintResultID IN (SELECT TOP (@NumberToAdd) PrintResultID FROM PrintResult ORDER BY PrintResultID DESC)
GROUP BY PrintDate 
ORDER BY PrintDate

-- 43 minutes per 1 million records

--SELECT COUNT(*) AS 'PrintResults' FROM dbo.PrintResult
--SELECT COUNT(*) AS 'Resources' FROM dbo.Resource 
--SELECT COUNT(*) AS 'ObjectReferences' FROM dbo.ObjectReference

GO
