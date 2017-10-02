PRINT N'Creating [dbo].[AmazonServiceType]'
GO
CREATE TABLE [dbo].[AmazonServiceType]
(
[AmazonServiceTypeID] [int] NOT NULL IDENTITY(1, 1),
[ApiValue] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Description] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_AmazonServiceTypeID] on [dbo].[AmazonServiceType]'
GO
ALTER TABLE [dbo].[AmazonServiceType] ADD CONSTRAINT [PK_AmazonServiceTypeID] PRIMARY KEY CLUSTERED  ([AmazonServiceTypeID])
GO


PRINT (N'Add Initial AmazonServiceTypes')
GO
INSERT INTO dbo.AmazonServiceType
(ApiValue, Description)
VALUES
('USPS_PTP_FC', 'USPS First Class'),
('USPS_PTP_PRI', 'USPS Priority Mail®'),
('USPS_PTP_PRI_SFRB', 'USPS Priority Mail® Small Flat Rate Box'),
('USPS_PTP_PRI_MFRB', 'USPS Priority Mail® Flat Rate Box'),
('USPS_PTP_PRI_LFRB', 'USPS Priority Mail® Large Flat Rate Box'),
('USPS_PTP_PRI_FRE', 'USPS Priority Mail® Flat Rate Envelope'),
('USPS_PTP_PRI_LFRE', 'USPS Priority Mail Legal Flat Rate Envelope'),
('USPS_PTP_PRI_PFRE', 'USPS Priority Mail Padded Flat Rate Envelope'),
('USPS_PTP_PRI_RA', 'USPS Priority Mail Regional Rate Box A'),
('USPS_PTP_PRI_RB', 'USPS Priority Mail Regional Rate Box B'),
('USPS_PTP_EXP', 'USPS Priority Mail Express®'),
('USPS_PTP_EXP_FRE', 'USPS Priority Mail Express® Flat Rate Envelope'),
('USPS_PTP_EXP_LFRE', 'USPS Priority Mail Express Legal Flat Rate Envelope'),
('USPS_PTP_EXP_PFRE', 'USPS Priority Mail Express Padded Flat Rate Envelope'),
('USPS_PTP_PSBN', 'USPS Parcel Select'),
('FEDEX_PTP_GROUND', 'FedEx Ground®'),
('FEDEX_PTP_SECOND_DAY', 'FedEx 2Day®'),
('FEDEX_PTP_SECOND_DAY_AM', 'FedEx 2Day®A.M.'),
('FEDEX_PTP_EXPRESS_SAVER', 'FedEx Express Saver®'),
('FEDEX_PTP_STANDARD_OVERNIGHT', 'FedEx Standard Overnight®'),
('FEDEX_PTP_PRIORITY_OVERNIGHT', 'FedEx Priority Overnight®'),
('UPS_PTP_GND', 'UPS Ground'),
('UPS_PTP_3DAY_SELECT', 'UPS 3 Day Select'),
('UPS_PTP_2ND_DAY_AIR', 'UPS 2nd Day Air'),
('UPS_PTP_NEXT_DAY_AIR_SAVER', 'UPS Next Day Air Saver'),
('UPS_PTP_NEXT_DAY_AIR', 'UPS Next Day Air');
GO

PRINT N'Creating index [IX_AmazonServiceType_ApiValue] on [dbo].[AmazonServiceType]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_AmazonServiceType_ApiValue] ON [dbo].[AmazonServiceType] ([ApiValue])
GO