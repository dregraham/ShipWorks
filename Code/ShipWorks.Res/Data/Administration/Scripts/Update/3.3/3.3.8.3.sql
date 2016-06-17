
-- DDL TO CREATE SCAN FORM BATCH TABLE 
SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Creating [dbo].[ScanFormBatch]'
GO
CREATE TABLE [dbo].[ScanFormBatch]
(
[ScanFormBatchID] [bigint] NOT NULL IDENTITY(1095, 1000),
[ShipmentType] [int] NOT NULL,
[CreatedDate] [datetime] NOT NULL,
[ShipmentCount] [int] NOT NULL
)
GO
PRINT N'Creating primary key [PK_ScanFormBatch] on [dbo].[ScanFormBatch]'
GO
ALTER TABLE [dbo].[ScanFormBatch] ADD CONSTRAINT [PK_ScanFormBatch] PRIMARY KEY CLUSTERED  ([ScanFormBatchID])
GO


-- Begin backfilling the scan form batch table with scan form records
DECLARE @EndiciaShipmentType INT = 2,
		@Express1ShipmentType INT = 9,
		@StampsShipmentType INT = 3
		

-- Insert all Express 1 scan form records into the batch table
-- (!Express1 accounts are GUID values (length 36)
INSERT INTO ScanFormBatch
(
	ShipmentType, CreatedDate, ShipmentCount
)
SELECT @Express1ShipmentType, CreatedDate, ShipmentCount
FROM EndiciaScanForm WITH (NOLOCK)
WHERE LEN(EndiciaAccountNumber) = 36



-- Insert all Endicia scan form records into the batch table
-- (any account less than 36 characters)
INSERT INTO ScanFormBatch
(
	ShipmentType, CreatedDate, ShipmentCount
)
SELECT @EndiciaShipmentType, CreatedDate, ShipmentCount
FROM EndiciaScanForm WITH (NOLOCK)
WHERE LEN(EndiciaAccountNumber) < 36


-- Insert all Stamps.com scan form records into the batch table
INSERT INTO ScanFormBatch
(
	ShipmentType, CreatedDate, ShipmentCount
)
SELECT @StampsShipmentType, CreatedDate, ShipmentCount
FROM StampsScanForm WITH (NOLOCK)




-- CODE TO ADD THE BATCH SCAN FORM ID AND DESCRIPTION TO THE INDIVIDUAL SCAN FORM TABLES
-- SCAN FORM BATCH ID AND DESCRIPTION FIELDS SHOULD BE NULLABLE AT FIRST
SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Dropping constraints from [dbo].[EndiciaScanForm]'
GO
ALTER TABLE [dbo].[EndiciaScanForm] DROP CONSTRAINT [PK_EndiciaScanForm]
GO
PRINT N'Dropping constraints from [dbo].[StampsScanForm]'
GO
ALTER TABLE [dbo].[StampsScanForm] DROP CONSTRAINT [PK_StampsScanForm]
GO
PRINT N'Rebuilding [dbo].[EndiciaScanForm]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_EndiciaScanForm]
(
[EndiciaScanFormID] [bigint] NOT NULL IDENTITY(1067, 1000),
[EndiciaAccountID] [bigint] NOT NULL,
[EndiciaAccountNumber] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SubmissionID] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CreatedDate] [datetime] NOT NULL,
[ShipmentCount] [int] NULL,
[ScanFormBatchID] [bigint] NULL,
[Description] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_EndiciaScanForm] ON
GO
INSERT INTO [dbo].[tmp_rg_xx_EndiciaScanForm]([EndiciaScanFormID], [EndiciaAccountID], [EndiciaAccountNumber], [SubmissionID], [CreatedDate], [ShipmentCount]) SELECT [EndiciaScanFormID], [EndiciaAccountID], [EndiciaAccountNumber], [SubmissionID], [CreatedDate], [ShipmentCount] FROM [dbo].[EndiciaScanForm]
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_EndiciaScanForm] OFF
GO
DECLARE @idVal BIGINT
SELECT @idVal = IDENT_CURRENT(N'[dbo].[EndiciaScanForm]')
IF @idVal IS NOT NULL
    DBCC CHECKIDENT(N'[dbo].[tmp_rg_xx_EndiciaScanForm]', RESEED, @idVal)
GO
DROP TABLE [dbo].[EndiciaScanForm]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_EndiciaScanForm]', N'EndiciaScanForm'
GO
PRINT N'Creating primary key [PK_EndiciaScanForm] on [dbo].[EndiciaScanForm]'
GO
ALTER TABLE [dbo].[EndiciaScanForm] ADD CONSTRAINT [PK_EndiciaScanForm] PRIMARY KEY CLUSTERED  ([EndiciaScanFormID])
GO
PRINT N'Rebuilding [dbo].[StampsScanForm]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_StampsScanForm]
(
[StampsScanFormID] [bigint] NOT NULL IDENTITY(1072, 1000),
[StampsAccountID] [bigint] NOT NULL,
[ScanFormTransactionID] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ScanFormUrl] [varchar] (2048) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipmentCount] [int] NOT NULL,
[CreatedDate] [datetime] NOT NULL,
[ScanFormBatchID] [bigint] NULL,
[Description] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_StampsScanForm] ON
GO
INSERT INTO [dbo].[tmp_rg_xx_StampsScanForm]([StampsScanFormID], [StampsAccountID], [ScanFormTransactionID], [ScanFormUrl], [ShipmentCount], [CreatedDate]) SELECT [StampsScanFormID], [StampsAccountID], [ScanFormTransactionID], [ScanFormUrl], [ShipmentCount], [CreatedDate] FROM [dbo].[StampsScanForm]
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_StampsScanForm] OFF
GO
DECLARE @idVal BIGINT
SELECT @idVal = IDENT_CURRENT(N'[dbo].[StampsScanForm]')
IF @idVal IS NOT NULL
    DBCC CHECKIDENT(N'[dbo].[tmp_rg_xx_StampsScanForm]', RESEED, @idVal)
GO
DROP TABLE [dbo].[StampsScanForm]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_StampsScanForm]', N'StampsScanForm'
GO
PRINT N'Creating primary key [PK_StampsScanForm] on [dbo].[StampsScanForm]'
GO
ALTER TABLE [dbo].[StampsScanForm] ADD CONSTRAINT [PK_StampsScanForm] PRIMARY KEY CLUSTERED  ([StampsScanFormID])
GO
PRINT N'Adding foreign keys to [dbo].[EndiciaScanForm]'
GO
ALTER TABLE [dbo].[EndiciaScanForm] ADD CONSTRAINT [FK_EndiciaScanForm_EndiciaScanForm] FOREIGN KEY ([ScanFormBatchID]) REFERENCES [dbo].[ScanFormBatch] ([ScanFormBatchID])
GO
PRINT N'Adding foreign keys to [dbo].[StampsScanForm]'
GO
ALTER TABLE [dbo].[StampsScanForm] ADD CONSTRAINT [FK_StampsScanForm_ScanFormBatch] FOREIGN KEY ([ScanFormBatchID]) REFERENCES [dbo].[ScanFormBatch] ([ScanFormBatchID])
GO


-- Populate the Endicia/Express 1 scan form records with the corresponding batch ID
UPDATE EndiciaScanForm
	SET 
		EndiciaScanForm.ScanFormBatchId = ScanFormBatch.ScanFormBatchID,
		EndiciaScanForm.Description = ''
FROM EndiciaScanForm 
INNER JOIN ScanFormBatch 
ON ScanFormBatch.CreatedDate = EndiciaScanForm.CreatedDate
	AND ScanFormBatch.ShipmentCount = EndiciaScanForm.ShipmentCount


UPDATE StampsScanForm
	SET 
		StampsScanForm.ScanFormBatchId = ScanFormBatch.ScanFormBatchID,
		StampsScanForm.Description = ''
FROM StampsScanForm 
INNER JOIN ScanFormBatch 
ON ScanFormBatch.CreatedDate = StampsScanForm.CreatedDate
	AND ScanFormBatch.ShipmentCount = StampsScanForm.ShipmentCount



-- DDL TO UPDATE THE SCAN FORM TABLES SO THE BATCH ID AND DESCRIPTION
-- FIELDS ARE NOT NULLABLE
SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Dropping foreign keys from [dbo].[EndiciaScanForm]'
GO
ALTER TABLE [dbo].[EndiciaScanForm] DROP CONSTRAINT[FK_EndiciaScanForm_EndiciaScanForm]
GO
PRINT N'Dropping foreign keys from [dbo].[StampsScanForm]'
GO
ALTER TABLE [dbo].[StampsScanForm] DROP CONSTRAINT[FK_StampsScanForm_ScanFormBatch]
GO
PRINT N'Altering [dbo].[EndiciaScanForm]'
GO
ALTER TABLE [dbo].[EndiciaScanForm] ALTER COLUMN [ShipmentCount] [int] NOT NULL
ALTER TABLE [dbo].[EndiciaScanForm] ALTER COLUMN [ScanFormBatchID] [bigint] NOT NULL
ALTER TABLE [dbo].[EndiciaScanForm] ALTER COLUMN [Description] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
GO
PRINT N'Altering [dbo].[StampsScanForm]'
GO
ALTER TABLE [dbo].[StampsScanForm] ALTER COLUMN [ScanFormBatchID] [bigint] NOT NULL
ALTER TABLE [dbo].[StampsScanForm] ALTER COLUMN [Description] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
GO
PRINT N'Adding foreign keys to [dbo].[EndiciaScanForm]'
GO
ALTER TABLE [dbo].[EndiciaScanForm] ADD CONSTRAINT [FK_EndiciaScanForm_EndiciaScanForm] FOREIGN KEY ([ScanFormBatchID]) REFERENCES [dbo].[ScanFormBatch] ([ScanFormBatchID])
GO
PRINT N'Adding foreign keys to [dbo].[StampsScanForm]'
GO
ALTER TABLE [dbo].[StampsScanForm] ADD CONSTRAINT [FK_StampsScanForm_ScanFormBatch] FOREIGN KEY ([ScanFormBatchID]) REFERENCES [dbo].[ScanFormBatch] ([ScanFormBatchID])
GO
