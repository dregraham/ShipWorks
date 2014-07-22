ALTER TABLE dbo.Shipment
	DROP CONSTRAINT FK_Shipment_ProcessedComputer
GO
ALTER TABLE dbo.Shipment
	DROP CONSTRAINT FK_Shipment_VoidedComputer
GO
ALTER TABLE dbo.Computer SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Shipment
	DROP CONSTRAINT FK_Shipment_ProcessedUser
GO
ALTER TABLE dbo.Shipment
	DROP CONSTRAINT FK_Shipment_VoidedUser
GO
ALTER TABLE dbo.[User] SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Shipment
	DROP CONSTRAINT FK_Shipment_Order
GO
ALTER TABLE dbo.[Order] SET (LOCK_ESCALATION = TABLE)
GO
CREATE TABLE dbo.Tmp_Shipment
	(
	ShipmentID bigint NOT NULL IDENTITY (1031, 1000),
	RowVersion timestamp NOT NULL,
	OrderID bigint NOT NULL,
	ShipmentType int NOT NULL,
	ContentWeight float(53) NOT NULL,
	TotalWeight float(53) NOT NULL,
	Processed bit NOT NULL,
	ProcessedDate datetime NULL,
	ProcessedUserID bigint NULL,
	ProcessedComputerID bigint NULL,
	ShipDate datetime NOT NULL,
	ShipmentCost money NOT NULL,
	Voided bit NOT NULL,
	VoidedDate datetime NULL,
	VoidedUserID bigint NULL,
	VoidedComputerID bigint NULL,
	TrackingNumber nvarchar(50) NOT NULL,
	CustomsGenerated bit NOT NULL,
	CustomsValue money NOT NULL,
	ThermalType int NULL,
	ShipFirstName nvarchar(30) NOT NULL,
	ShipMiddleName nvarchar(30) NOT NULL,
	ShipLastName nvarchar(30) NOT NULL,
	ShipCompany nvarchar(60) NOT NULL,
	ShipStreet1 nvarchar(60) NOT NULL,
	ShipStreet2 nvarchar(60) NOT NULL,
	ShipStreet3 nvarchar(60) NOT NULL,
	ShipCity nvarchar(50) NOT NULL,
	ShipStateProvCode nvarchar(50) NOT NULL,
	ShipPostalCode nvarchar(20) NOT NULL,
	ShipCountryCode nvarchar(50) NOT NULL,
	ShipPhone nvarchar(25) NOT NULL,
	ShipEmail nvarchar(100) NOT NULL,
	ResidentialDetermination int NOT NULL,
	ResidentialResult bit NOT NULL,
	OriginOriginID bigint NOT NULL,
	OriginFirstName nvarchar(30) NOT NULL,
	OriginMiddleName nvarchar(30) NOT NULL,
	OriginLastName nvarchar(30) NOT NULL,
	OriginCompany nvarchar(60) NOT NULL,
	OriginStreet1 nvarchar(60) NOT NULL,
	OriginStreet2 nvarchar(60) NOT NULL,
	OriginStreet3 nvarchar(60) NOT NULL,
	OriginCity nvarchar(50) NOT NULL,
	OriginStateProvCode nvarchar(50) NOT NULL,
	OriginPostalCode nvarchar(20) NOT NULL,
	OriginCountryCode nvarchar(50) NOT NULL,
	OriginPhone nvarchar(25) NOT NULL,
	OriginFax nvarchar(35) NOT NULL,
	OriginEmail nvarchar(100) NOT NULL,
	OriginWebsite nvarchar(50) NOT NULL,
	ReturnShipment bit NOT NULL,
	Insurance bit NOT NULL,
	InsuranceProvider int NOT NULL,
	InsuredWith int NOT NULL,
	ShipNameParseStatus int NOT NULL,
	ShipUnparsedName nvarchar(100) NOT NULL,
	OriginNameParseStatus int NOT NULL,
	OriginUnparsedName nvarchar(100) NOT NULL,
	BestRateEvents tinyint NOT NULL,
	ShipSenseStatus int NOT NULL,
	ShipSenseChangeSets xml NOT NULL,
	ShipSenseEntry varbinary(MAX) NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Shipment SET (LOCK_ESCALATION = TABLE)
GO
DECLARE @v sql_variant 
SET @v = N'103'
EXECUTE sp_addextendedproperty N'AuditFormat', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_Shipment', N'COLUMN', N'ShipmentType'
GO
DECLARE @v sql_variant 
SET @v = N'1'
EXECUTE sp_addextendedproperty N'AuditFormat', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_Shipment', N'COLUMN', N'ContentWeight'
GO
DECLARE @v sql_variant 
SET @v = N'3'
EXECUTE sp_addextendedproperty N'AuditFormat', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_Shipment', N'COLUMN', N'TotalWeight'
GO
DECLARE @v sql_variant 
SET @v = N'7'
EXECUTE sp_addextendedproperty N'AuditFormat', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_Shipment', N'COLUMN', N'ShipDate'
GO
DECLARE @v sql_variant 
SET @v = N'2'
EXECUTE sp_addextendedproperty N'AuditFormat', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_Shipment', N'COLUMN', N'ShipmentCost'
GO
DECLARE @v sql_variant 
SET @v = N'1'
EXECUTE sp_addextendedproperty N'AuditFormat', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_Shipment', N'COLUMN', N'CustomsGenerated'
GO
DECLARE @v sql_variant 
SET @v = N'2'
EXECUTE sp_addextendedproperty N'AuditFormat', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_Shipment', N'COLUMN', N'CustomsValue'
GO
DECLARE @v sql_variant 
SET @v = N'5'
EXECUTE sp_addextendedproperty N'AuditFormat', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_Shipment', N'COLUMN', N'ShipStateProvCode'
SET @v = N'ShipState'
EXECUTE sp_addextendedproperty N'AuditName', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_Shipment', N'COLUMN', N'ShipStateProvCode'
GO
DECLARE @v sql_variant 
SET @v = N'6'
EXECUTE sp_addextendedproperty N'AuditFormat', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_Shipment', N'COLUMN', N'ShipCountryCode'
SET @v = N'ShipCountry'
EXECUTE sp_addextendedproperty N'AuditName', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_Shipment', N'COLUMN', N'ShipCountryCode'
GO
DECLARE @v sql_variant 
SET @v = N'111'
EXECUTE sp_addextendedproperty N'AuditFormat', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_Shipment', N'COLUMN', N'ResidentialDetermination'
SET @v = N'Residential \ Commercial'
EXECUTE sp_addextendedproperty N'AuditName', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_Shipment', N'COLUMN', N'ResidentialDetermination'
GO
DECLARE @v sql_variant 
SET @v = N'1'
EXECUTE sp_addextendedproperty N'AuditFormat', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_Shipment', N'COLUMN', N'ResidentialResult'
GO
DECLARE @v sql_variant 
SET @v = N'4'
EXECUTE sp_addextendedproperty N'AuditFormat', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_Shipment', N'COLUMN', N'OriginOriginID'
GO
DECLARE @v sql_variant 
SET @v = N'5'
EXECUTE sp_addextendedproperty N'AuditFormat', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_Shipment', N'COLUMN', N'OriginStateProvCode'
SET @v = N'OriginState'
EXECUTE sp_addextendedproperty N'AuditName', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_Shipment', N'COLUMN', N'OriginStateProvCode'
GO
DECLARE @v sql_variant 
SET @v = N'6'
EXECUTE sp_addextendedproperty N'AuditFormat', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_Shipment', N'COLUMN', N'OriginCountryCode'
SET @v = N'OriginCountry'
EXECUTE sp_addextendedproperty N'AuditName', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_Shipment', N'COLUMN', N'OriginCountryCode'
GO
DECLARE @v sql_variant 
SET @v = N'112'
EXECUTE sp_addextendedproperty N'AuditFormat', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_Shipment', N'COLUMN', N'InsuranceProvider'
SET @v = N'Insurance'
EXECUTE sp_addextendedproperty N'AuditName', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_Shipment', N'COLUMN', N'InsuranceProvider'
GO
DECLARE @v sql_variant 
SET @v = N'1'
EXECUTE sp_addextendedproperty N'AuditFormat', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_Shipment', N'COLUMN', N'ShipNameParseStatus'
GO
DECLARE @v sql_variant 
SET @v = N'1'
EXECUTE sp_addextendedproperty N'AuditFormat', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_Shipment', N'COLUMN', N'OriginNameParseStatus'
GO
ALTER TABLE dbo.Tmp_Shipment ADD CONSTRAINT
	DF_Shipment_InsuredWith DEFAULT 0 FOR InsuredWith
GO
SET IDENTITY_INSERT dbo.Tmp_Shipment ON
GO
IF EXISTS(SELECT * FROM dbo.Shipment)
	 EXEC('INSERT INTO dbo.Tmp_Shipment (ShipmentID, OrderID, ShipmentType, ContentWeight, TotalWeight, Processed, ProcessedDate, ProcessedUserID, ProcessedComputerID, ShipDate, ShipmentCost, Voided, VoidedDate, VoidedUserID, VoidedComputerID, TrackingNumber, CustomsGenerated, CustomsValue, ThermalType, ShipFirstName, ShipMiddleName, ShipLastName, ShipCompany, ShipStreet1, ShipStreet2, ShipStreet3, ShipCity, ShipStateProvCode, ShipPostalCode, ShipCountryCode, ShipPhone, ShipEmail, ResidentialDetermination, ResidentialResult, OriginOriginID, OriginFirstName, OriginMiddleName, OriginLastName, OriginCompany, OriginStreet1, OriginStreet2, OriginStreet3, OriginCity, OriginStateProvCode, OriginPostalCode, OriginCountryCode, OriginPhone, OriginFax, OriginEmail, OriginWebsite, ReturnShipment, Insurance, InsuranceProvider, ShipNameParseStatus, ShipUnparsedName, OriginNameParseStatus, OriginUnparsedName, BestRateEvents, ShipSenseStatus, ShipSenseChangeSets, ShipSenseEntry)
		SELECT ShipmentID, OrderID, ShipmentType, ContentWeight, TotalWeight, Processed, ProcessedDate, ProcessedUserID, ProcessedComputerID, ShipDate, ShipmentCost, Voided, VoidedDate, VoidedUserID, VoidedComputerID, TrackingNumber, CustomsGenerated, CustomsValue, ThermalType, ShipFirstName, ShipMiddleName, ShipLastName, ShipCompany, ShipStreet1, ShipStreet2, ShipStreet3, ShipCity, ShipStateProvCode, ShipPostalCode, ShipCountryCode, ShipPhone, ShipEmail, ResidentialDetermination, ResidentialResult, OriginOriginID, OriginFirstName, OriginMiddleName, OriginLastName, OriginCompany, OriginStreet1, OriginStreet2, OriginStreet3, OriginCity, OriginStateProvCode, OriginPostalCode, OriginCountryCode, OriginPhone, OriginFax, OriginEmail, OriginWebsite, ReturnShipment, Insurance, InsuranceProvider, ShipNameParseStatus, ShipUnparsedName, OriginNameParseStatus, OriginUnparsedName, BestRateEvents, ShipSenseStatus, ShipSenseChangeSets, ShipSenseEntry FROM dbo.Shipment WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Shipment OFF
GO
ALTER TABLE dbo.BestRateShipment
	DROP CONSTRAINT FK_BestRateShipment_Shipment
GO
ALTER TABLE dbo.EquaShipShipment
	DROP CONSTRAINT FK_EquashipShipment_Shipment
GO
ALTER TABLE dbo.iParcelShipment
	DROP CONSTRAINT FK_iParcelShipment_Shipment
GO
ALTER TABLE dbo.OnTracShipment
	DROP CONSTRAINT FK_OnTracShipment_Shipment
GO
ALTER TABLE dbo.OtherShipment
	DROP CONSTRAINT FK_OtherShipment_Shipment
GO
ALTER TABLE dbo.PostalShipment
	DROP CONSTRAINT FK_PostalShipment_Shipment
GO
ALTER TABLE dbo.ShipmentCustomsItem
	DROP CONSTRAINT FK_ShipmentCustomsItem_Shipment
GO
ALTER TABLE dbo.UpsShipment
	DROP CONSTRAINT FK_UpsShipment_Shipment
GO
ALTER TABLE dbo.FedExShipment
	DROP CONSTRAINT FK_FedExShipment_Shipment
GO
DROP TABLE dbo.Shipment
GO
EXECUTE sp_rename N'dbo.Tmp_Shipment', N'Shipment', 'OBJECT' 
GO
ALTER TABLE dbo.Shipment ADD CONSTRAINT
	PK_Shipment PRIMARY KEY CLUSTERED 
	(
	ShipmentID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE NONCLUSTERED INDEX IX_Shipment_ProcessedOrderID ON dbo.Shipment
	(
	Processed DESC
	) INCLUDE (OrderID) 
 WITH( PAD_INDEX = OFF, FILLFACTOR = 75, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_Shipment_OrderID ON dbo.Shipment
	(
	OrderID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_Shipment_OrderID_ShipSenseStatus ON dbo.Shipment
	(
	OrderID,
	Processed,
	ShipSenseStatus
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE dbo.Shipment ADD CONSTRAINT
	FK_Shipment_Order FOREIGN KEY
	(
	OrderID
	) REFERENCES dbo.[Order]
	(
	OrderID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Shipment ADD CONSTRAINT
	FK_Shipment_ProcessedUser FOREIGN KEY
	(
	ProcessedUserID
	) REFERENCES dbo.[User]
	(
	UserID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Shipment ADD CONSTRAINT
	FK_Shipment_ProcessedComputer FOREIGN KEY
	(
	ProcessedComputerID
	) REFERENCES dbo.Computer
	(
	ComputerID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Shipment ADD CONSTRAINT
	FK_Shipment_VoidedUser FOREIGN KEY
	(
	VoidedUserID
	) REFERENCES dbo.[User]
	(
	UserID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Shipment ADD CONSTRAINT
	FK_Shipment_VoidedComputer FOREIGN KEY
	(
	VoidedComputerID
	) REFERENCES dbo.Computer
	(
	ComputerID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER dbo.FilterDirtyShipment ON dbo.Shipment 
AFTER UPDATE , DELETE , INSERT 
AS 
 EXTERNAL NAME [ShipWorks.SqlServer].Triggers.FilterDirtyShipment
GO
CREATE TRIGGER dbo.ShipmentDeleteTrigger ON dbo.Shipment 
AFTER DELETE 
AS 
 EXTERNAL NAME [ShipWorks.SqlServer].Triggers.ShipmentDeleteTrigger
GO
CREATE TRIGGER dbo.ShipmentAuditTrigger ON dbo.Shipment 
AFTER UPDATE , DELETE , INSERT 
AS 
 EXTERNAL NAME [ShipWorks.SqlServer].Triggers.ShipmentAuditTrigger
GO
CREATE TRIGGER dbo.ShipmentLabelTrigger ON dbo.Shipment 
AFTER UPDATE , DELETE , INSERT 
AS 
 EXTERNAL NAME [ShipWorks.SqlServer].Triggers.ShipmentLabelTrigger
GO
ALTER TABLE dbo.Shipment ENABLE CHANGE_TRACKING
GO
ALTER TABLE dbo.FedExShipment ADD CONSTRAINT
	FK_FedExShipment_Shipment FOREIGN KEY
	(
	ShipmentID
	) REFERENCES dbo.Shipment
	(
	ShipmentID
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.FedExShipment SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.UpsShipment ADD CONSTRAINT
	FK_UpsShipment_Shipment FOREIGN KEY
	(
	ShipmentID
	) REFERENCES dbo.Shipment
	(
	ShipmentID
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.UpsShipment SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.ShipmentCustomsItem ADD CONSTRAINT
	FK_ShipmentCustomsItem_Shipment FOREIGN KEY
	(
	ShipmentID
	) REFERENCES dbo.Shipment
	(
	ShipmentID
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.ShipmentCustomsItem SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.PostalShipment ADD CONSTRAINT
	FK_PostalShipment_Shipment FOREIGN KEY
	(
	ShipmentID
	) REFERENCES dbo.Shipment
	(
	ShipmentID
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.PostalShipment SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.OtherShipment ADD CONSTRAINT
	FK_OtherShipment_Shipment FOREIGN KEY
	(
	ShipmentID
	) REFERENCES dbo.Shipment
	(
	ShipmentID
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.OtherShipment SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.OnTracShipment ADD CONSTRAINT
	FK_OnTracShipment_Shipment FOREIGN KEY
	(
	ShipmentID
	) REFERENCES dbo.Shipment
	(
	ShipmentID
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.OnTracShipment SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.iParcelShipment ADD CONSTRAINT
	FK_iParcelShipment_Shipment FOREIGN KEY
	(
	ShipmentID
	) REFERENCES dbo.Shipment
	(
	ShipmentID
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.iParcelShipment SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.EquaShipShipment ADD CONSTRAINT
	FK_EquashipShipment_Shipment FOREIGN KEY
	(
	ShipmentID
	) REFERENCES dbo.Shipment
	(
	ShipmentID
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.EquaShipShipment SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.BestRateShipment ADD CONSTRAINT
	FK_BestRateShipment_Shipment FOREIGN KEY
	(
	ShipmentID
	) REFERENCES dbo.Shipment
	(
	ShipmentID
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.BestRateShipment SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Shipment
	DROP CONSTRAINT DF_Shipment_InsuredWith
GO
ALTER TABLE dbo.Shipment SET (LOCK_ESCALATION = TABLE)
GO
