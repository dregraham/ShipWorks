PRINT N'Altering [dbo].[EndiciaShipment]'
GO
ALTER TABLE [dbo].[EndiciaShipment] ADD
[ReferenceID2] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_EndiciaShipment_ReferenceID2] DEFAULT (''),
[ReferenceID3] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_EndiciaShipment_ReferenceID3] DEFAULT (''),
[ReferenceID4] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_EndiciaShipment_ReferenceID4] DEFAULT (''),
[GroupCode] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_EndiciaShipment_GroupCode] DEFAULT ('')
GO
PRINT N'Altering [dbo].[EndiciaProfile]'
GO
ALTER TABLE [dbo].[EndiciaProfile] ADD
[ReferenceID2] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ReferenceID3] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ReferenceID4] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[GroupCode] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
GO