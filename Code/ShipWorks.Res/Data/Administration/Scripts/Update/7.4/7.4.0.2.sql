PRINT N'Creating [dbo].[BestRateExcludedAccount]'
GO
IF OBJECT_ID(N'[dbo].[BestRateExcludedAccount]', 'U') IS NULL
CREATE TABLE [dbo].[BestRateExcludedAccount](
 [AccountID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_AccountID] on [dbo].[BestRateExcludedAccount]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'PK_AccountID' AND object_id = OBJECT_ID(N'[dbo].[BestRateExcludedAccount]'))
ALTER TABLE [dbo].[BestRateExcludedAccount] ADD CONSTRAINT [PK_AccountID] PRIMARY KEY CLUSTERED ([AccountID])
GO
