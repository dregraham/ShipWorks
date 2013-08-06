IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertEmailTestData]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[InsertEmailTestData]
GO
CREATE PROC InsertEmailTestData (@NumberToAdd INT) as

SET NOCOUNT ON

DECLARE @Message NVARCHAR(500)
DECLARE @ProcessingStart DATETIME = GETUTCDATE()
DECLARE @ProcessingEnd DATETIME
DECLARE @Count INT = 0
DECLARE @StartDate DATETIME = GETUTCDATE() - 28
DECLARE @EndDate DATETIME = GETUTCDATE()
DECLARE @NumberOfDays int = DATEDIFF(d, @StartDate, @enddate)
DECLARE @NumberPerDay INT = @NumberToAdd / @NumberOfDays
DECLARE @EmailOutboundID BIGINT
DECLARE @ObjectReferenceID BIGINT
DECLARE @HtmlPartResourceID BIGINT = NULL
DECLARE @PlainPartResourceID BIGINT = NULL
DECLARE @ResourceID BIGINT
DECLARE @UserID BIGINT = 1002
DECLARE @ComputerId BIGINT = 1001

--SELECT COUNT(eo.EmailOutboundID)
--FROM EmailOutbound eo
--SELECT COUNT(o.ObjectReferenceID)
--FROM dbo.ObjectReference o

DECLARE @PercentComplete int = 0
DECLARE @PercentCompleteBreak INT = @NumberToAdd / 10

--In long running loops, Print doesn't send output to the client.  So using RaiseError with severity of no error and NoWait 
--so that we can always get timely status updates.
RAISERROR('Starting Email Test Data Inserts', 10, 1) WITH NOWAIT

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

		INSERT INTO dbo.ObjectReference
		        ( ConsumerID , ReferenceKey ,ObjectID ,Reason
		        )
		VALUES  ( 0 , -- ConsumerID - bigint
		          N'' + SUBSTRING(CAST(NEWID() AS NVARCHAR(50)), 0, 30) , -- ReferenceKey - varchar(250)
		          0 , -- ObjectID - bigint
		          N''  -- Reason - nvarchar(250)
		        )
		SET @PlainPartResourceID = SCOPE_IDENTITY()

		INSERT INTO dbo.EmailOutbound
		        ( ContextID , ContextType ,TemplateID ,AccountID ,Visibility ,FromAddress ,ToList ,CcList ,BccList ,Subject ,
		          HtmlPartResourceID ,PlainPartResourceID ,Encoding ,ComposedDate ,
		          SentDate ,DontSendBefore ,SendStatus ,SendAttemptCount ,SendAttemptLastError
		        )
		VALUES  ( 0 , -- ContextID - bigint
		          0 , -- ContextType - int
		          0 , -- TemplateID - bigint
		          0 , -- AccountID - bigint
		          0 , -- Visibility - int
		          N'' , -- FromAddress - nvarchar(200)
		          N'' , -- ToList - nvarchar(max)
		          N'' , -- CcList - nvarchar(max)
		          N'' , -- BccList - nvarchar(max)
		          N'' , -- Subject - nvarchar(300)
		          @HtmlPartResourceID , -- HtmlPartResourceID - bigint
		          @PlainPartResourceID , -- PlainPartResourceID - bigint
		          '' , -- Encoding - varchar(20)
		          '2013-01-24 18:57:21' , -- ComposedDate - datetime
		          DATEADD(d,  @count % @NumberOfDays, @StartDate) , -- SentDate - datetime
		          NULL , -- DontSendBefore - datetime
		          0 , -- SendStatus - int
		          0 , -- SendAttemptCount - int
		          N''  -- SendAttemptLastError - nvarchar(300)
		        )
		SET @EmailOutboundID = SCOPE_IDENTITY()

		SELECT @ResourceID = resourceid FROM dbo.Resource WHERE resourceid IN (SELECT TOP 1 ResourceID FROM dbo.Resource)

		UPDATE dbo.ObjectReference SET ConsumerID = @EmailOutboundID, ObjectID = @ResourceID WHERE ObjectReferenceID = @PlainPartResourceID

		INSERT INTO dbo.ObjectReference
		        ( ConsumerID , ReferenceKey ,ObjectID ,Reason
		        )
		VALUES  ( @EmailOutboundID , -- ConsumerID - bigint
		          N'' + SUBSTRING(CAST(NEWID() AS NVARCHAR(50)), 0, 30) , -- ReferenceKey - varchar(250)
		          @ResourceID , -- ObjectID - bigint
		          N''  -- Reason - nvarchar(250)
		        )
		SET @HtmlPartResourceID = SCOPE_IDENTITY()

		UPDATE dbo.EmailOutbound SET HtmlPartResourceID = @HtmlPartResourceID WHERE EmailOutboundID = @EmailOutboundID

	END
SET @Message = '100 Percent Complete.  ' + CAST(@Count AS NVARCHAR(10)) + ' Rows Inserted.'
RAISERROR(@Message, 10, 1) WITH NOWAIT

SET @ProcessingEnd = GETUTCDATE()

SELECT DATEDIFF(second, @ProcessingStart, @ProcessingEnd) AS 'Time To Process'

SELECT SentDate, COUNT(SentDate) 
FROM dbo.EmailOutbound 
WHERE EmailOutboundid IN (SELECT TOP (@NumberToAdd) EmailOutboundID FROM EmailOutbound ORDER BY EmailOutboundID DESC)
GROUP BY SentDate 
ORDER BY SentDate


SELECT COUNT(eo.EmailOutboundID)
FROM EmailOutbound eo
SELECT COUNT(o.ObjectReferenceID)
FROM dbo.ObjectReference o

GO

