SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Altering [dbo].[ChannelAdvisorOrder]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrder] ADD
[IsPrime] [int] NULL
GO

PRINT N'Populating RequestedShipping'
GO
UPDATE cao
SET cao.IsPrime	= 
	CASE 
		WHEN o.RequestedShipping LIKE '%Amazon%' AND o.RequestedShipping LIKE '%Prime%' THEN 1
		WHEN o.RequestedShipping = '' THEN 0
		ELSE 2
	END
 FROM dbo.ChannelAdvisorOrder cao	
INNER JOIN dbo.[Order] o ON o.OrderID = cao.OrderID	

GO
PRINT N'Altering [dbo].[ChannelAdvisorOrder]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrder] ALTER COLUMN [IsPrime] [int] NOT NULL
GO
