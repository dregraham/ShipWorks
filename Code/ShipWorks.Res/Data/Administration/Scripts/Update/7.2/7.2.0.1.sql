PRINT N'Altering [dbo].[Order]'
GO
IF COL_LENGTH(N'[dbo].[Order]', N'Verified') IS NULL
    ALTER TABLE [dbo].[Order] ADD [Verified] [bit] NOT NULL CONSTRAINT [DF_Order_Verified] DEFAULT (0)
GO
IF COL_LENGTH(N'[dbo].[Order]', N'VerifiedBy') IS NULL
    ALTER TABLE [dbo].[Order] ADD [VerifiedBy] [bigint] NULL
GO
IF COL_LENGTH(N'[dbo].[Order]', N'VerifiedDate') IS NULL
    ALTER TABLE [dbo].[Order] ADD [VerifiedDate] [datetime] NULL
GO

