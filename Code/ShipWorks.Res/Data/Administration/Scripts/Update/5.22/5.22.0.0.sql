PRINT N'Creating [dbo].[PackageProfile]'
GO
CREATE TABLE [dbo].[PackageProfile]
(
[PackageProfileID] [bigint] NOT NULL IDENTITY(1104, 1000),
[ShippingProfileID] [bigint] NOT NULL,
[Weight] [float] NULL,
[DimsProfileID] [bigint] NULL,
[DimsLength] [float] NULL,
[DimsWidth] [float] NULL,
[DimsHeight] [float] NULL,
[DimsWeight] [float] NULL,
[DimsAddWeight] [bit] NULL,
[UpsProfilePackageID] [bigint] NULL
)
GO
PRINT N'Creating primary key [PK_PackageProfile] on [dbo].[PackageProfile]'
GO
ALTER TABLE [dbo].[PackageProfile] ADD CONSTRAINT [PK_PackageProfile] PRIMARY KEY CLUSTERED  ([PackageProfileID])
GO
PRINT N'Adding foreign keys to [dbo].[PackageProfile]'
GO
ALTER TABLE [dbo].[PackageProfile] ADD CONSTRAINT [FK_PackageProfile_ShippingProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[ShippingProfile] ([ShippingProfileID]) ON DELETE CASCADE
GO

-- AMAZON
PRINT N'Transfer Amazon profile Dimensions and Weight to PackageProfile'
GO
INSERT INTO [PackageProfile] (ShippingProfileID, [Weight], DimsProfileID, DimsLength, DimsWidth, DimsHeight, DimsWeight, DimsAddWeight) 
SELECT ShippingProfileID, [Weight], DimsProfileID, DimsLength, DimsWidth, DimsHeight, DimsWeight, DimsAddWeight
FROM [AmazonProfile]

PRINT N'Drop AmazonProfile dimension and weight columns'
GO
ALTER TABLE AmazonProfile
DROP COLUMN [Weight], DimsProfileID, DimsLength, DimsWidth, DimsHeight, DimsWeight, DimsAddWeight
GO
-- AMAZON

-- ASENDIA
PRINT N'Inserting from [AsendiaProfile] into [PackageProfile]'
GO
INSERT INTO [PackageProfile] ([ShippingProfileID], [Weight], [DimsProfileID], [DimsLength], [DimsWidth], [DimsHeight], [DimsWeight], [DimsAddWeight]) 
SELECT [ShippingProfileID], [Weight], [DimsProfileID], [DimsLength], [DimsWidth], [DimsHeight], [DimsWeight], [DimsAddWeight]
FROM [AsendiaProfile]

PRINT N'Dropping Weight and Dimensions columns for [AsendiaProfile]'
GO
ALTER TABLE [AsendiaProfile] 
DROP COLUMN [Weight], [DimsProfileID], [DimsLength], [DimsWidth], [DimsHeight], [DimsWeight], [DimsAddWeight]
GO
-- ASENDIA

-- Best Rate
PRINT N'Transfer BestRate profile Dimensions and Weight to PackageProfile'
GO
INSERT INTO PackageProfile (ShippingProfileID, [Weight], DimsProfileID, DimsLength, DimsWidth, DimsHeight, DimsWeight, DimsAddWeight)
SELECT ShippingProfileID, [Weight], DimsProfileID, DimsLength, DimsWidth, DimsHeight, DimsWeight, DimsAddWeight
FROM BestRateProfile
GO

PRINT N'Drop BestRateProfile dimension and weight columns'
GO
ALTER TABLE BestRateProfile
DROP COLUMN [Weight], DimsProfileID, DimsLength, DimsWidth, DimsHeight, DimsWeight, DimsAddWeight
GO
--Best Rate

-- DHL Express
PRINT N'Transfer DhlExpress profile Dimensions and Weight to PackageProfile'
GO
INSERT INTO PackageProfile (ShippingProfileID, [Weight], DimsProfileID, DimsLength, DimsWidth, DimsHeight, DimsWeight, DimsAddWeight)
SELECT ShippingProfileID, [Weight], DimsProfileID, DimsLength, DimsWidth, DimsHeight, DimsWeight, DimsAddWeight
FROM DhlExpressProfilePackage
GO

PRINT N'Drop DhlExpressProfilePackage'
GO
DROP TABLE DhlExpressProfilePackage
GO
-- DHL Express

-- iParcelProfilePackage
PRINT N'Transfer iParcel profile Dimensions and Weight to PackageProfile'
GO
INSERT INTO PackageProfile (ShippingProfileID, [Weight], DimsProfileID, DimsLength, DimsWidth, DimsHeight, DimsWeight, DimsAddWeight)
SELECT ShippingProfileID, [Weight], DimsProfileID, DimsLength, DimsWidth, DimsHeight, DimsWeight, DimsAddWeight
FROM iParcelProfilePackage
GO

PRINT N'Drop iParcelProfilePackage'
GO
DROP TABLE iParcelProfilePackage
GO
-- iParcelProfilePackage

-- OnTrac
PRINT N'Transfer OnTrac profile Dimensions and Weight to PackageProfile'
GO
INSERT INTO PackageProfile (ShippingProfileID, [Weight], DimsProfileID, DimsLength, DimsWidth, DimsHeight, DimsWeight, DimsAddWeight)
SELECT ShippingProfileID, [Weight], DimsProfileID, DimsLength, DimsWidth, DimsHeight, DimsWeight, DimsAddWeight
FROM OnTracProfile
GO
PRINT N'Drop OnTracProfile dimension and weight columns'
GO
ALTER TABLE OnTracProfile
DROP COLUMN [Weight], DimsProfileID, DimsLength, DimsWidth, DimsHeight, DimsWeight, DimsAddWeight
GO
--OnTrac

-- UPS
PRINT N'Adding [PackageProfileID] to [dbo].[PackageProfile]' 
GO 
ALTER TABLE [UpsProfilePackage] ADD [PackageProfileID] [bigint] NULL
GO
-- Copy package data to the PackageProfile table
PRINT N'Transfer Ups profile Dimensions and Weight to PackageProfile'
GO
INSERT INTO [PackageProfile] (ShippingProfileID, [Weight], DimsProfileID, DimsLength, DimsWidth, DimsHeight, DimsWeight, DimsAddWeight, UpsProfilePackageID) 
SELECT ShippingProfileID, [Weight], DimsProfileID, DimsLength, DimsWidth, DimsHeight, DimsWeight, DimsAddWeight, UpsProfilePackageID
FROM [UpsProfilePackage]
GO
UPDATE upp
SET upp.PackageProfileID = pp.PackageProfileID
FROM [UpsProfilePackage] AS upp
INNER JOIN [PackageProfile] AS pp
    ON upp.UpsProfilePackageID = pp.UpsProfilePackageID
GO
PRINT N'Dropping [UpsProfilePackageID] from [ProfilePackage]' 
GO 
ALTER TABLE [PackageProfile] DROP COLUMN [UpsProfilePackageID]
GO
PRINT N'Dropping Weight and Dimensions columns for [UpsProfilePackage]' 
GO 
ALTER TABLE [UpsProfilePackage] 
DROP COLUMN [Weight], [DimsProfileID], [DimsLength], [DimsWidth], [DimsHeight], [DimsWeight], [DimsAddWeight]
GO
ALTER TABLE [UpsProfilePackage] ALTER COLUMN [PackageProfileID] [bigint] NOT NULL
GO
PRINT N'Adding foreign key to [UpsProfilePackage]' 
ALTER TABLE [dbo].[UpsProfilePackage]  WITH CHECK ADD  CONSTRAINT [FK_UpsProfilePackage_PackageProfile] FOREIGN KEY([PackageProfileID])
REFERENCES [dbo].[PackageProfile] ([PackageProfileID])
GO
-- UPS