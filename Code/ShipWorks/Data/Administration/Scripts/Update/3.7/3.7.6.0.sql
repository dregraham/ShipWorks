/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Store SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_AmazonStore
	(
	StoreID bigint NOT NULL,
	AmazonApi int NOT NULL,
	AmazonApiRegion char(2) NOT NULL,
	SellerCentralUsername nvarchar(50) NOT NULL,
	SellerCentralPassword nvarchar(50) NOT NULL,
	MerchantName varchar(64) NOT NULL,
	MerchantToken varchar(32) NOT NULL,
	AccessKeyID varchar(32) NOT NULL,
	Cookie text NOT NULL,
	CookieExpires datetime NOT NULL,
	CookieWaitUntil datetime NOT NULL,
	Certificate varbinary(2048) NULL,
	WeightDownloads text NOT NULL,
	MerchantID nvarchar(50) NOT NULL,
	MarketplaceID nvarchar(50) NOT NULL,
	ExcludeFBA bit NOT NULL,
	DomainName nvarchar(50) NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_AmazonStore SET (LOCK_ESCALATION = TABLE)
GO
IF EXISTS(SELECT * FROM dbo.AmazonStore)
	 EXEC('INSERT INTO dbo.Tmp_AmazonStore (StoreID, AmazonApi, AmazonApiRegion, SellerCentralUsername, SellerCentralPassword, MerchantName, MerchantToken, AccessKeyID, Cookie, CookieExpires, CookieWaitUntil, Certificate, WeightDownloads, MerchantID, MarketplaceID, ExcludeFBA, DomainName)
		SELECT StoreID, AmazonApi, ''US'', SellerCentralUsername, SellerCentralPassword, MerchantName, MerchantToken, AccessKeyID, Cookie, CookieExpires, CookieWaitUntil, Certificate, WeightDownloads, MerchantID, MarketplaceID, ExcludeFBA, DomainName FROM dbo.AmazonStore WITH (HOLDLOCK TABLOCKX)')
GO
ALTER TABLE dbo.AmazonASIN
	DROP CONSTRAINT FK_AmazonASIN_AmazonStore
GO
DROP TABLE dbo.AmazonStore
GO
EXECUTE sp_rename N'dbo.Tmp_AmazonStore', N'AmazonStore', 'OBJECT' 
GO
ALTER TABLE dbo.AmazonStore ADD CONSTRAINT
	PK_AmazonStore PRIMARY KEY CLUSTERED 
	(
	StoreID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.AmazonStore ADD CONSTRAINT
	FK_AmazonStore_Store FOREIGN KEY
	(
	StoreID
	) REFERENCES dbo.Store
	(
	StoreID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.AmazonASIN ADD CONSTRAINT
	FK_AmazonASIN_AmazonStore FOREIGN KEY
	(
	StoreID
	) REFERENCES dbo.AmazonStore
	(
	StoreID
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.AmazonASIN SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
