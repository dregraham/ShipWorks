﻿SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Altering [dbo].[FilterNodeUpdatePending]'
GO
ALTER TABLE [dbo].[FilterNodeUpdatePending] ALTER COLUMN [ColumnMask] [varbinary] (100) NOT NULL
GO
