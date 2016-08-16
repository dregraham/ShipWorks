
ALTER TABLE [dbo].[WorldShipPackage] ADD
[MIDeliveryConfirmation] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[QvnOption] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[QvnFrom] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[QvnSubjectLine] [nvarchar] (18) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[QvnMemo] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Qvn1ShipNotify] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Qvn1ContactName] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Qvn1Email] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Qvn2ShipNotify] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Qvn2ContactName] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Qvn2Email] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Qvn3ShipNotify] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Qvn3ContactName] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Qvn3Email] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
GO

ALTER TABLE [dbo].[WorldShipShipment] ADD
[ShipmentProcessedOnComputerID] [bigint] NULL
GO
ALTER TABLE [dbo].[WorldShipShipment] ALTER COLUMN [QvnFrom] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
ALTER TABLE [dbo].[WorldShipShipment] ALTER COLUMN [QvnSubjectLine] [nvarchar] (18) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
ALTER TABLE [dbo].[WorldShipShipment] ALTER COLUMN [QvnMemo] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
GO

ALTER TABLE [dbo].[WorldShipProcessed] ALTER COLUMN [ShipmentID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
GO
