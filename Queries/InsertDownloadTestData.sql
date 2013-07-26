IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertDownloadTestData]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[InsertDownloadTestData]
GO
CREATE PROC InsertDownloadTestData (@NumberToAdd INT) as

SET NOCOUNT ON

DECLARE @Message NVARCHAR(500)
DECLARE @ProcessingStart DATETIME = GETDATE()
DECLARE @ProcessingEnd DATETIME
DECLARE @Count INT = 0
DECLARE @StartDate DATETIME = GETDATE() - 28
DECLARE @EndDate DATETIME = GETDATE()
DECLARE @NumberOfDays int = DATEDIFF(d, @StartDate, @enddate)
DECLARE @NumberPerDay INT = @NumberToAdd / @NumberOfDays
DECLARE @DownloadID BIGINT
DECLARE @DownloadDetailID BIGINT
DECLARE @UserID BIGINT = 1002
DECLARE @ComputerId BIGINT = 1001
DECLARE @OrderID BIGINT 
DECLARE @StoreID BIGINT 

SELECT @OrderID = OrderID, @StoreID = StoreID 
FROM dbo.[Order] WHERE OrderID IN (SELECT TOP 1 OrderID FROM dbo.[Order] ORDER BY OrderID DESC)

--SELECT COUNT(*) FROM dbo.Download d
--SELECT COUNT(*) FROM dbo.DownloadDetail dd

DECLARE @PercentComplete int = 0
DECLARE @PercentCompleteBreak INT = @NumberToAdd / 10

--In long running loops, Print doesn't send output to the client.  So using RaiseError with severity of no error and NoWait 
--so that we can always get timely status updates.
RAISERROR('Starting Download Test Data Inserts', 10, 1) WITH NOWAIT

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

		INSERT INTO dbo.Download (StoreID, ComputerID, UserID, InitiatedBy, Started, Ended, QuantityTotal, QuantityNew, Result, ErrorMessage)
		VALUES  ( @StoreID , -- StoreID - bigint
		          @ComputerId , -- ComputerID - bigint
		          @UserID , -- UserID - bigint
		          0 , -- InitiatedBy - int
		          DATEADD(d,  @count % @NumberOfDays, @StartDate) , -- Started - datetime
		          DATEADD(d,  @count % @NumberOfDays, @StartDate) + 1 , -- Ended - datetime
		          0 , -- QuantityTotal - int
		          0 , -- QuantityNew - int
		          0 , -- Result - int
		          N''  -- ErrorMessage - nvarchar(max)
		        )
		SET @DownloadID = SCOPE_IDENTITY()
		
		INSERT INTO dbo.DownloadDetail (DownloadID, OrderID, InitialDownload, OrderNumber, ExtraBigIntData1, ExtraBigIntData2, ExtraBigIntData3, ExtraStringData1)
		VALUES  ( @DownloadID , -- DownloadID - bigint
		          @OrderID , -- OrderID - bigint
		          0 , -- InitialDownload - bit
		          NULL , -- OrderNumber - bigint
		          NULL , -- ExtraBigIntData1 - bigint
		          NULL , -- ExtraBigIntData2 - bigint
		          NULL , -- ExtraBigIntData3 - bigint
		          N''  -- ExtraStringData1 - nvarchar(50)
		        )
	END
SET @Message = '100 Percent Complete.  ' + CAST(@Count AS NVARCHAR(10)) + ' Rows Inserted.'
RAISERROR(@Message, 10, 1) WITH NOWAIT
  
--SELECT COUNT(*) FROM dbo.Download a
--SELECT COUNT(*) FROM dbo.Download ac
--SELECT COUNT(*) FROM dbo.DownloadDetail acd
    
SET @ProcessingEnd = GETDATE()

SELECT DATEDIFF(second, @ProcessingStart, @ProcessingEnd) AS 'Time To Process'

SELECT Started, COUNT(Started) 
FROM Download 
WHERE Downloadid IN (SELECT TOP (@NumberToAdd) Downloadid FROM Download ORDER BY Downloadid DESC)
GROUP BY Started 
ORDER BY Started

GO

