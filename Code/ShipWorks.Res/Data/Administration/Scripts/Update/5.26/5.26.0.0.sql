﻿PRINT N'Altering [dbo].[GrouponOrderItem]'
GO
ALTER TABLE [dbo].[GrouponOrderItem] ADD
[GrouponPO] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_GrouponOrderItem_GrouponPO] DEFAULT ('')
GO
PRINT N'Dropping constraints from [dbo].[GrouponOrderItem]'
GO
ALTER TABLE [dbo].[GrouponOrderItem] DROP CONSTRAINT [DF_GrouponOrderItem_GrouponPO]
GO