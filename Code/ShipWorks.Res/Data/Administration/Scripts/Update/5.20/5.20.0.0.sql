PRINT N'Altering [dbo].[ThreeDCartStore]'
GO
ALTER TABLE [dbo].[ThreeDCartStore] ADD
[OrderIDUpgradeFixDate] [datetime] NULL
GO
PRINT N'Setting date of OrderIDUpgradeFixDate'
GO
UPDATE ThreeDCartStore 
SET OrderIDUpgradeFixDate = GetUtcDate()
GO